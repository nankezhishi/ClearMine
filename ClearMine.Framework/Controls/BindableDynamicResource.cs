namespace ClearMine.Framework.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// 
    /// </summary>
    [MarkupExtensionReturnType(typeof(object))]
    [TypeConverter(typeof(DynamicResourceExtensionConverter))]
    public class BindableDynamicResource : StaticResourceExtension
    {
        private static readonly DependencyProperty ValueHostProperty;

        static BindableDynamicResource()
        {
            ValueHostProperty = DependencyProperty.RegisterAttached("ValueHost", typeof(Object), typeof(DependencyObject), new UIPropertyMetadata(null));
        }

        [ConstructorArgument("binding")]
        public Binding Binding { get; set; }

        public BindableDynamicResource()
        {
        }

        public BindableDynamicResource(Binding binding)
        {
            Binding = binding;
        }

        public new object ResourceKey
        {
            get { return base.ResourceKey; }
            set
            {
                if (value != null)
                    base.ResourceKey = value;
            }
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var target = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
            var targetObject = (FrameworkElement)target.TargetObject;

            Binding.Source = targetObject.DataContext;
            var DummyDO = new DependencyObject();
            BindingOperations.SetBinding(DummyDO, ValueHostProperty, Binding);

            ResourceKey = DummyDO.GetValue(ValueHostProperty);

            if (ResourceKey != null)
            {
                return base.ProvideValue(serviceProvider);
            }
            else
            {
                return null;
            }
        }
    }
}
