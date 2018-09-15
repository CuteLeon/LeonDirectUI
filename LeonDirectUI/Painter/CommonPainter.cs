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
        /// 绘图对象
        /// </summary>
        public Graphics Graphics { get; set; }

        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="control"></param>
        public void Paint(ControlBase control)
        {
            if (Graphics == null) throw new Exception("绘制器使用了空的 Graphics");
            if (control == null) throw new Exception("绘制器绘制空的 ControlBase");

            //绘制文本对齐和换行：https://www.cnblogs.com/dannyqiu/articles/2837515.html
            //TODO: 实现绘制方法逻辑
            Console.WriteLine($"{this.ToString()} 绘制 {control.Name} ...");
            using (SolidBrush brush = new SolidBrush(control.BackColor))
            {
                Graphics.FillRectangle(brush, control.Rectangle);
                Graphics.DrawString(control.Name, SystemFonts.DefaultFont, Brushes.Red, control.Location);
            }
        }

        /// <summary>
        /// 注入绘制对象
        /// </summary>
        /// <param name="graphics"></param>
        public void SetGraphics(Graphics graphics)
        {
            Graphics = graphics ?? throw new Exception("向绘制器注入空的 Graphics");
        }
    }
}
