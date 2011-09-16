namespace ClearMine.Logic.Test
{
    using System;
    using System.Linq.Expressions;
    using ClearMine.Common.ComponentModel;

    internal class SampleObject : BindableObject
    {
        private int sampleProperty;

        public int SampleProperty
        {
            get { return sampleProperty; }
            set { SetProperty(ref sampleProperty, value, "SampleProperty"); }
        }

        private int property;
        private Expression<Func<int>> propertyName;

        public int Property
        {
            get { return property; }
            set
            {
                // Here is the correct way to use the expression version of Property Setter.
                SetProperty(ref property, value, propertyName ?? (propertyName = () => Property));
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private decimal key;

        public decimal Key
        {
            get { return key; }
            set { key = value; }
        }        
    }
}
