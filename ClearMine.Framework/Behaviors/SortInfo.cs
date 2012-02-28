namespace ClearMine.Framework.Behaviors
{
    using System.Windows.Controls;

    using ClearMine.Framework.Controls;

    /// <summary>
    /// 
    /// </summary>
    public class SortInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public GridViewColumnHeader LastSortColumn { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public VisualAdorner CurrentAdorner { get; set; }
    }
}
