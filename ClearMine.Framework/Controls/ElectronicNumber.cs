namespace ClearMine.Framework.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    using ClearMine.Common.ComponentModel;

    public class SingleNumber : BindableObject, IComparable
    {
        private int number;

        public int Number
        {
            get { return number; }
            set { SetProperty(ref number, value, "Number"); }
        }

        public SingleNumber(int value)
        {
            number = value;
        }

        public int CompareTo(object obj)
        {
            if (obj is SingleNumber)
            {
                return this.number.CompareTo((obj as SingleNumber).number);
            }
            else if (obj is IComparable)
            {
                return (this.number as IComparable).CompareTo(obj);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }

    public class ElectronicNumber : Control
    {
        static ElectronicNumber()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElectronicNumber), new FrameworkPropertyMetadata(typeof(ElectronicNumber)));
        }

        public ElectronicNumber()
        {
            Numbers = new ObservableCollection<SingleNumber>();
        }

        #region Numbers Property - ReadOnly
        /// <summary>
        /// Gets the Numbers property of current instance of ElectronicNumber
        /// </summary>
        public ObservableCollection<SingleNumber> Numbers
        {
            get { return (ObservableCollection<SingleNumber>)GetValue(NumbersPropertyKey.DependencyProperty); }
            protected set { SetValue(NumbersPropertyKey, value); }
        }

        // Using a DependencyProperty as the backing store for Numbers.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey NumbersPropertyKey =
            DependencyProperty.RegisterReadOnly("Numbers", typeof(ObservableCollection<SingleNumber>), typeof(ElectronicNumber), new UIPropertyMetadata(null));

        public static readonly DependencyProperty NumbersProperty = NumbersPropertyKey.DependencyProperty;
        #endregion
        #region DisplayNumber Property
        /// <summary>
        /// Gets or sets the DisplayNumber property of current instance of ElectronicNumber
        /// </summary>
        public string DisplayNumber
        {
            get { return (string)GetValue(DisplayNumberProperty); }
            set { SetValue(DisplayNumberProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayNumber.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayNumberProperty =
            DependencyProperty.Register("DisplayNumber", typeof(string), typeof(ElectronicNumber), new UIPropertyMetadata(new PropertyChangedCallback(OnDisplayNumberPropertyChanged)));

        private static void OnDisplayNumberPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ElectronicNumber instance = sender as ElectronicNumber;
            if (instance != null)
            {
                instance.OnDisplayNumberChanged(e);
            }
        }

        protected virtual void OnDisplayNumberChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue != null)
            {
                int i = 0;
                string newNumber = e.NewValue as string;
                int startOffset = Numbers.Count - newNumber.Length;
                if (startOffset < 0)
                {
                    startOffset = 0;
                }
                for (int z = 0; z < startOffset; z++)
                {
                    Numbers[z].Number = '0';
                }
                foreach (var item in newNumber)
                {
                    int number = Convert.ToInt32(item);
                    if (Numbers.Count > i)
                    {
                        Numbers[i + startOffset].Number = number;
                    }
                    else
                    {
                        Numbers.Insert(i, new SingleNumber(number));
                    }
                    i++;
                }
            }
            else
            {
                Numbers.Clear();
            }
        }
        #endregion
    }
}
