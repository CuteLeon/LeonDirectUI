using System.Drawing;

using LeonDirectUI.DUIControl;

namespace LeonDirectUI.Interface
{
    /// <summary>
    /// 绘制接口
    /// </summary>
    public interface IPainter
    {
        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="control"></param>
        void Paint(Graphics graphics, ControlBase control);

    }
}
