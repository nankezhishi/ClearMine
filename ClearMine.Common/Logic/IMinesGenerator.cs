namespace ClearMine.Common.Logic
{
    using System.Collections.Generic;
    using System.Windows;

    public interface IMinesGenerator
    {
        /// <summary>
        /// Fills mines randomly to the given cell list.
        /// </summary>
        /// <param name="size">The size of the mines area. </param>
        /// <param name="grid">The cell list where to put generated mines.</param>
        /// <param name="mines">The count of the mines to generate.</param>
        /// <remarks>
        /// The size.Width * size.Height must less than cellList.Count.
        /// If the cell list is cachable, it probrably larger than the size.
        /// </remarks>
        void Fill(Size size, IList<MineCell> cellList, int mines);
    }
}
