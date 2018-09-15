using LeonDirectUI.DUIControl;
using LeonDirectUI.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeonDirectUI.Painter
{
    /// <summary>
    /// 通用绘制器
    /// </summary>
    public class CommonPainter : IPainter
    {
        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="control"></param>
        public void Paint(Graphics graphics, ControlBase control)
        {
            if (graphics == null) throw new Exception("绘制器使用了空的 Graphics");
            if (control == null) throw new Exception("绘制器绘制空的 ControlBase");

            //绘制文本对齐和换行：https://www.cnblogs.com/dannyqiu/articles/2837515.html
            //TODO: 实现绘制方法逻辑
            Console.WriteLine($"{this.ToString()} 绘制 {control.Name} ...");
            using (SolidBrush brush = new SolidBrush(control.BackColor))
            {
                graphics.FillRectangle(brush, control.Rectangle);
                graphics.DrawString(control.Name, SystemFonts.DefaultFont, Brushes.Red, control.Location);
            }
        }

    }
}
