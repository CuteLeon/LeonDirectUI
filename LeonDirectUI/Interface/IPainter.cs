using LeonDirectUI.DUIControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="control"></param>
        void Paint(ControlBase control);

        /// <summary>
        /// 注入绘图对象
        /// </summary>
        /// <param name="graphics"></param>
        void SetGraphics(Graphics graphics);
    }
}
