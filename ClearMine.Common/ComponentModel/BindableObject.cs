namespace ClearMine.Common.ComponentModel
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;

    using ClearMine.Common.Utilities;

    public class BindableObject : INotifyPropertyChanged
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [SuppressMessage("Microsoft.Design", "CA1045",
            Justification = "Use ref to change the value of a field in a sub class easily.")]
        public bool SetProperty<T>(ref T property, T newValue, string propertyName)
        {
            if (!Object.Equals(property, newValue))
            {
                property = newValue;
                TriggerPropertyChanged(propertyName);

                return true;
            }

            return false;
        }

        /// <summary>
        /// expression parameter sample: () => PropertyName
        /// If you can use it properly, the formance is almost the same as the string version.
        /// Tips: Cache the expression.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public bool SetProperty<T>(ref T property, T newValue, LambdaExpression expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            if (expression.Body is MemberExpression)
            {
                return SetProperty<T>(ref property, newValue, (expression.Body as MemberExpression).Member.Name);
            }
            else
            {
                throw new InvalidProgramException(LocalizationHelper.FindText("ExpressionMustBeMember"));
            }
        }

        public void TriggerPropertyChanged(string propertyName)
        {
            var temp = PropertyChanged;
            if (temp != null)
            {
                temp(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
