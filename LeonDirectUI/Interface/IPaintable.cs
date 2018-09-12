using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeonDirectUI.Interface
{
    /// <summary>
    /// 可绘制接口
    /// </summary>
    public interface IPaintable
    {
        /// <summary>
        /// 注入绘制器
        /// </summary>
        /// <param name="painter"></param>
        void SetPainter(IPaint painter);

        /// <summary>
        /// 调用绘制器绘制
        /// </summary>
        void Paint(Graphics graphics);

    }
}
