namespace ClearMine.Framework.Controls
{
    using System;
    using System.Diagnostics;
    using ClearMine.Common.ComponentModel;
    using ClearMine.Common.Utilities;

    /// <summary>
    /// 
    /// </summary>
    public class SingleNumber : BindableObject, IComparable
    {
        private int number;
        private bool isPoint;

        public int Number
        {
            get { return number; }
            set { SetProperty(ref number, value, "Number"); }
        }

        public bool IsPoint
        {
            get { return isPoint; }
            set { SetProperty(ref isPoint, value, "IsPoint"); }
        }

        public SingleNumber(int value)
        {
            number = value;
        }

        public override bool Equals(object obj)
        {
            return this.CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            return number.GetHashCode();
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
                Trace.TraceWarning(ResourceHelper.FindText("CompareSingleNumberFailed", this, obj));
                return 0;
            }
        }
    }
}
