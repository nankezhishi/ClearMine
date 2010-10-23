﻿namespace ClearMine.Framework.Dialogs
{
    using System.Collections;
    using System.Windows;
    using System.Windows.Media;

    public class OptionDialog : Window
    {
        static OptionDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OptionDialog), new FrameworkPropertyMetadata(typeof(OptionDialog)));
        }

        #region ImageSource Property
        /// <summary>
        /// Gets or sets the ImageSource property of current instance of DetailedOptionDialog
        /// </summary>
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(OptionDialog), new UIPropertyMetadata(new PropertyChangedCallback(OnImageSourcePropertyChanged)));

        private static void OnImageSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            OptionDialog instance = sender as OptionDialog;
            if (instance != null)
            {
                instance.OnImageSourceChanged(e);
            }
        }

        protected virtual void OnImageSourceChanged(DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
        #region Message Property
        /// <summary>
        /// Gets or sets the Message property of current instance of DetailedOptionDialog
        /// </summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(OptionDialog), new UIPropertyMetadata(new PropertyChangedCallback(OnMessagePropertyChanged)));

        private static void OnMessagePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            OptionDialog instance = sender as OptionDialog;
            if (instance != null)
            {
                instance.OnMessageChanged(e);
            }
        }

        protected virtual void OnMessageChanged(DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
        #region Options Property
        /// <summary>
        /// Gets or sets the Options property of current instance of DetailedOptionDialog
        /// </summary>
        public IEnumerable Options
        {
            get { return (IEnumerable)GetValue(OptionsProperty); }
            set { SetValue(OptionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Options.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OptionsProperty =
            DependencyProperty.Register("Options", typeof(IEnumerable), typeof(OptionDialog), new UIPropertyMetadata(new PropertyChangedCallback(OnOptionsPropertyChanged)));

        private static void OnOptionsPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            OptionDialog instance = sender as OptionDialog;
            if (instance != null)
            {
                instance.OnOptionsChanged(e);
            }
        }

        protected virtual void OnOptionsChanged(DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion
    }
}
