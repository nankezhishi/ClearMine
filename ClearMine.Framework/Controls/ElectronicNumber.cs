namespace ClearMine.Framework.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;

    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class ElectronicNumber : Control
    {
        [SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline")]
        static ElectronicNumber()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElectronicNumber), new FrameworkPropertyMetadata(typeof(ElectronicNumber)));
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
        #region MinLength Property
        /// <summary>
        /// Gets or sets the MinLength property of current instance of ElectronicNumber
        /// </summary>
        public int MinLength
        {
            get { return (int)GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MinLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinLengthProperty =
            DependencyProperty.Register("MinLength", typeof(int), typeof(ElectronicNumber), new UIPropertyMetadata(2, new PropertyChangedCallback(OnMinLengthPropertyChanged)));

        private static void OnMinLengthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ElectronicNumber instance = sender as ElectronicNumber;
            if (instance != null)
            {
                instance.OnMinLengthChanged(e);
            }
        }

        protected virtual void OnMinLengthChanged(DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue < 1)
            {
                throw new InvalidOperationException(ResourceHelper.FindText("MinLengthMustPositive"));
            }

            InitializeNumbersCollection();
        }
        #endregion

        #region LengthAfterPoint Property
        /// <summary>
        /// Gets or sets the LengthAfterPoint property of current instance of ElectronicNumber
        /// </summary>
        public int LengthAfterPoint
        {
            get { return (int)GetValue(LengthAfterPointProperty); }
            set { SetValue(LengthAfterPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LengthAfterPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LengthAfterPointProperty =
            DependencyProperty.Register("LengthAfterPoint", typeof(int), typeof(ElectronicNumber), new UIPropertyMetadata(new PropertyChangedCallback(OnLengthAfterPointPropertyChanged)));

        private static void OnLengthAfterPointPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ElectronicNumber instance = sender as ElectronicNumber;
            if (instance != null)
            {
                instance.OnLengthAfterPointChanged(e);
            }
        }

        protected virtual void OnLengthAfterPointChanged(DependencyPropertyChangedEventArgs e)
        {
            InitializeNumbersCollection();
        }
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
            if (Numbers == null)
            {
                InitializeNumbersCollection();
            }

            if (DisplayNumber != null)
            {
                string newNumber = DisplayNumber as string;

                if (LengthAfterPoint > 0)
                {
                    newNumber = Double.Parse(newNumber, CultureInfo.InvariantCulture).ToString("0." + new String('0', LengthAfterPoint), CultureInfo.InvariantCulture);
                }
                else
                {
                    newNumber = ((int)Double.Parse(newNumber, CultureInfo.InvariantCulture)).ToString(CultureInfo.InvariantCulture);
                }

                int current = newNumber.Length - 1;
                int updateIndex = Numbers.Count - 1;

                while (current >= 0)
                {
                    if (updateIndex >= 0)
                    {
                        if (newNumber[current].Equals('.'))
                        {
                            Numbers[updateIndex].IsPoint = true;
                        }
                        else
                        {
                            Numbers[updateIndex].IsPoint = false;
                            Numbers[updateIndex].Number = newNumber[current];
                        }
                    }
                    else
                    {
                        Numbers.Insert(0, new SingleNumber(newNumber[current]));
                    }

                    current--;
                    updateIndex--;
                }

                while (updateIndex >= 0)
                {
                    if (Numbers.Count > MinLength)
                    {
                        Numbers.RemoveAt(updateIndex);
                    }
                    else
                    {
                        Numbers[updateIndex].IsPoint = false;
                        Numbers[updateIndex].Number = 0;
                    }
                    updateIndex--;
                }
            }
            else
            {
                InitializeNumbersCollection();
            }
        }
        #endregion

        private void InitializeNumbersCollection()
        {
            if (Numbers == null)
            {
                Numbers = new ObservableCollection<SingleNumber>();
            }

            int targetLength = LengthAfterPoint > 0 ? MinLength + LengthAfterPoint + 1 : MinLength;
            while (Numbers.Count > targetLength)
            {
                Numbers.RemoveAt(0);
            }

            while (Numbers.Count < targetLength)
            {
                Numbers.Add(new SingleNumber(0));
            }
        }
    }
}
