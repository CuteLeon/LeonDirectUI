﻿using LeonDirectUI.DUIControl;
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
    public class CommonPainter : PainterBase, IPainter
    {
        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="control"></param>
        public override void Paint(Graphics graphics, ControlBase control)
        {
            if (!control.Visible) return;
            if (control.Width <= 0 || control.Height <= 0) return;

            if (graphics == null) throw new Exception("绘制器使用了空的 Graphics");
            if (control == null) throw new Exception("绘制器绘制空的 ControlBase");
            //Console.WriteLine(control.Name);

            //绘制背景色和图像
            if(control.BackgroundImage!=null || control.BackColor !=Color.Transparent)
                DrawBackground(
                    graphics,
                    control.BackgroundImage,
                    control.BackColor,
                    control.BackgroundImageLayout,
                    control.Rectangle);

            //绘制前置图像
            if(control.Image !=null)
                DrawImage(
                graphics,
                control.Image,
                control.ImageAlign,
                control.Rectangle,
                control.Padding,
                control.Enabled);

            if (!string.IsNullOrEmpty(control.Text))
                DrawText(
                    graphics,
                    control.Text,
                    control.Font,
                    control.ForeColor,
                    control.Rectangle,
                    control.Enabled,
                    control.BackColor,
                    control.TextAlign);
            /*
            control.Font;
            control.ForeColor;
            control.Text;
            control.TextAlign;
             */

            graphics.DrawRectangle(Pens.Red, control.Left, control.Top, control.Width-1, control.Height-1);
        }

    }
}
