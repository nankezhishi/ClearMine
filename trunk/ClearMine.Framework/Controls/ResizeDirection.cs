namespace ClearMine.Framework.Controls
{
    using System.ComponentModel;

    /// <summary>
    /// 
    /// </summary>
    public enum ResizeDirection
    {
        [Description("Horizontal Right")]
        HorizontalRight = 1,

        [Description("Horizontal Left")]
        HorizontalLeft = 2,

        [Description("Vertical Down")]
        VerticalDown = 3,

        [Description("Vertical Up")]
        VerticalUp = 4,

        [Description("Horizontal and Vertial")]
        RightDown = 5,

        [Description("Right Up")]
        RightUp = 6,

        [Description("Left Down")]
        LeftDown = 7,

        [Description("Left Up")]
        LeftUp = 8
    }
}
