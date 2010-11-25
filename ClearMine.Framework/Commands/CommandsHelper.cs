namespace ClearMine.Framework.Commands
{
    using System.Diagnostics;
    using System.Windows;
    using ClearMine.Common.ComponentModel;

    internal class CommandsHelper
    {
        public static bool GetLoadBindingsFromVM(DependencyObject obj)
        {
            return (bool)obj.GetValue(LoadBindingsFromVMProperty);
        }

        public static void SetLoadBindingsFromVM(DependencyObject obj, bool value)
        {
            obj.SetValue(LoadBindingsFromVMProperty, value);
        }

        // Using a DependencyProperty as the backing store for LoadBindingsFromVM.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LoadBindingsFromVMProperty =
            DependencyProperty.RegisterAttached("LoadBindingsFromVM", typeof(bool), typeof(CommandsHelper), new UIPropertyMetadata(new PropertyChangedCallback(OnLoadBindingsFromVMChanged)));

        private static void OnLoadBindingsFromVMChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var host = sender as FrameworkElement;
            if (host != null && (bool) e.NewValue)
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
            ViewModelBase vm = host.DataContext as ViewModelBase;
            if (vm != null)
            {
                foreach (var binding in vm.GetCommandBindings())
                {
                    host.CommandBindings.Add(binding);
                }
            }
            else
            {
                Trace.TraceError("Cannot find a VM from {0}", host);
            }
        }
    }
}
