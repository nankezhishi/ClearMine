namespace ClearMine.Framework.Controls
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows;

    using ClearMine.Common.ComponentModel.UI;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public static class CommandsHelper
    {
        public static bool GetLoadBindingsFromVM(DependencyObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return (bool)obj.GetValue(LoadBindingsFromVMProperty);
        }

        public static void SetLoadBindingsFromVM(DependencyObject obj, bool value)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            obj.SetValue(LoadBindingsFromVMProperty, value);
        }

        // Using a DependencyProperty as the backing store for LoadBindingsFromVM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadBindingsFromVMProperty =
            DependencyProperty.RegisterAttached("LoadBindingsFromVM", typeof(bool), typeof(CommandsHelper), new UIPropertyMetadata(new PropertyChangedCallback(OnLoadBindingsFromVMChanged)));

        private static void OnLoadBindingsFromVMChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var host = sender as Window ?? Window.GetWindow(sender as DependencyObject);
            if (host == null)
            {
                throw new InvalidOperationException(ResourceHelper.FindText("LoadBindingsFromVMReqireWindow"));
            }

            if ((bool)e.NewValue)
            {
                if (host.IsLoaded)
                {
                    LoadBindings(host);
                }
                else
                {
                    host.Loaded += new RoutedEventHandler(OnLoaded);
                }
            }
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var frameElement = sender as FrameworkElement;
            if (frameElement != null)
            {
                frameElement.Loaded -= new RoutedEventHandler(OnLoaded);
                LoadBindings(frameElement);
            }
        }

        private static void LoadBindings(FrameworkElement host)
        {
            var vm = host.DataContext as IViewModel;
            if (vm != null)
            {
                host.CommandBindings.AddRange(vm.CommandBindings.ToList());
            }
            else
            {
                Trace.TraceError(ResourceHelper.FindText("CannotFindVM", host));
            }
        }
    }
}
