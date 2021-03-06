﻿namespace ClearMine.VM.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Input;

    using ClearMine.Common.Properties;
    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Controls;
    using ClearMine.Framework.Interactivity;
    using ClearMine.GameDefinition;
    using ClearMine.GameDefinition.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class MainInteractionBehavior : UIElementBehavior<FrameworkElement>
    {
        private bool expanding = false;
        private ClearMineViewModel vm;
        private DateTime? lastStateChangedTime;
        private Window attachedWindow;

        static MainInteractionBehavior()
        {
            EventManager.RegisterClassHandler(typeof(MineCellControl), UIElement.MouseLeaveEvent, new MouseEventHandler(OnCellMouseLeave));
            EventManager.RegisterClassHandler(typeof(MineCellControl), UIElement.MouseEnterEvent, new MouseEventHandler(OnCellMouseEnter));
        }

        protected override void OnAttached()
        {
            if (!Infrastructure.IsInDesignMode)
            {
                AutoDetach = true;
                vm = AttachedObject.DataContext as ClearMineViewModel;
                AttachedObject.Loaded += OnAttatchedObjectLoaded;
                AttachedObject.MouseUp += OnMineGroudMouseUp;
                AttachedObject.MouseDown += OnMineGroudMouseDown;
                AttachedObject.MouseLeave += OnMineGroudMouseLeave;
                AttachedObject.MouseEnter += OnMineGroudMouseEnter;
                attachedWindow = Window.GetWindow(AttachedObject);
                if (attachedWindow != null)
                {
                    attachedWindow.Closing += OnMainWindowClosing;
                    attachedWindow.StateChanged += OnMainWindowStateChanged;
                    attachedWindow.SizeChanged += OnMainWindowSizeChanged;
                }
            }
        }

        protected override void OnDetaching()
        {
            AttachedObject.Loaded -= OnAttatchedObjectLoaded;
            AttachedObject.MouseUp -= OnMineGroudMouseUp;
            AttachedObject.MouseDown -= OnMineGroudMouseDown;
            AttachedObject.MouseLeave -= OnMineGroudMouseLeave;
            AttachedObject.MouseEnter -= OnMineGroudMouseEnter;
            if (attachedWindow != null)
            {
                attachedWindow.Closing -= OnMainWindowClosing;
                attachedWindow.StateChanged -= OnMainWindowStateChanged;
                attachedWindow.SizeChanged -= OnMainWindowSizeChanged;
            }
        }

        private void OnMineGroudMouseDown(object sender, MouseButtonEventArgs e)
        {
            var cell = e.ExtractDataContext<MineCell>();
            if (cell == null)
            {
                return;
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed &&
                Mouse.RightButton == MouseButtonState.Pressed)
            {
                if (cell != null && (vm.Game.GameState == GameState.Initialized || vm.Game.GameState == GameState.Started))
                {
                    ThreadPool.QueueUserWorkItem(o => vm.Game.TryExpandAt(cell));
                }
                expanding = true;
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                cell.PressState = PressState.Pressed;
            }

            vm.IsMousePressed = true;
        }

        private void OnMineGroudMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Maximim the window trigger a mouse up within the playground.
            // We need to block it here.
            if (lastStateChangedTime.HasValue && (DateTime.Now - lastStateChangedTime.Value).TotalMilliseconds < Math.Min(NativeMethods.DoubleClickInterval, 300))
            {
                return;
            }

            var cell = e.ExtractDataContext<MineCell>();
            if (cell == null)
            {
                return;
            }

            if (e.ChangedButton == MouseButton.Left &&
                Mouse.RightButton == MouseButtonState.Released)
            {
                expanding = false;
                if (cell != null && (vm.Game.GameState == GameState.Initialized || vm.Game.GameState == GameState.Started))
                {
                    ThreadPool.QueueUserWorkItem(o => vm.Game.TryDigAt(cell));
                }
            }
            else if (e.ChangedButton == MouseButton.Right &&
                Mouse.LeftButton == MouseButtonState.Released)
            {
                if (expanding)
                {
                    expanding = false;
                }
                else
                {
                    if (vm.Game.AutoMarkAt(cell))
                    {
                        vm.TriggerPropertyChanged("RemainedMines");
                        vm.TriggerPropertyChanged("Flags");
                    }
                }
            }

            foreach (var c in vm.Cells)
            {
                c.PressState = PressState.Released;
            }

            if (Mouse.LeftButton == MouseButtonState.Released &&
                Mouse.RightButton == MouseButtonState.Released)
            {
                vm.IsMousePressed = false;
            }
        }

        private void OnMineGroudMouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                vm.IsMousePressed = true;
            }
        }

        private void OnMineGroudMouseLeave(object sender, MouseEventArgs e)
        {
            vm.IsMousePressed = false;
        }

        private static void OnCellMouseEnter(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {
                var cell = e.ExtractDataContext<MineCell>();
                if (cell != null)
                {
                    if (Mouse.RightButton == MouseButtonState.Pressed)
                    {
                        cell.PressState = PressState.DoublePressed;
                        var vm = e.ExtractDataContext<ClearMineViewModel>();
                        foreach (var c in vm.Cells)
                        {
                            if (c.Near(cell) && (c.State == CellState.Normal || c.State == CellState.Question))
                            {
                                c.PressState = PressState.Pressed;
                            }
                        }
                    }
                    else
                    {
                        cell.PressState = PressState.Pressed;
                    }
                }
            }
        }

        private static void OnCellMouseLeave(object sender, MouseEventArgs e)
        {
            var vm = e.ExtractDataContext<ClearMineViewModel>();
            if (vm != null)
            {
                foreach (var c in vm.Cells)
                {
                    c.PressState = PressState.Released;
                }
            }
        }

        private void OnMainWindowStateChanged(object sender, EventArgs e)
        {
            if (Window.GetWindow(AttachedObject).WindowState == WindowState.Maximized)
            {
                lastStateChangedTime = DateTime.Now;
            }
        }

        private void OnAttatchedObjectLoaded(object sender, RoutedEventArgs e)
        {
            // Swith theme also trigger this load event, but we don't want to start a new game here.
            if (vm.State == GameState.Initialized)
            {
                vm.StartNewGame();
                vm.RefreshUI();
            }
        }

        private void OnMainWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            vm.Width = e.NewSize.Width;
            var window = sender as Window;
            if (window != null)
            {
                window.SetBinding(Window.WidthProperty, new Binding("Width") { Mode = BindingMode.TwoWay });
            }
        }

        private void OnMainWindowClosing(object sender, CancelEventArgs e)
        {
            var game = vm.Game;
            if (game.GameState == GameState.Started)
            {
                if (Settings.Default.SaveOnExit)
                {
                    Infrastructure.Container.GetExportedValue<IGameSerializer>().SaveGame(game, Settings.Default.UnfinishedGameFileName);
                }
                else
                {
                    // Game shouldn't be paused here
                    var result = MessageBox.Show(ResourceHelper.FindText("AskingSaveGameMessage"), ResourceHelper.FindText("AskingSaveGameTitle"),
                        MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                    else if (result == MessageBoxResult.Yes)
                    {
                        Infrastructure.Container.GetExportedValue<IGameSerializer>().SaveGame(game, Settings.Default.UnfinishedGameFileName);
                    }
                    else
                    {
                        game.UpdateStatistics();
                    }
                }
            }
        }
    }
}
