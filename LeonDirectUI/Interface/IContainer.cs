using LeonDirectUI.DUIControl;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeonDirectUI.Interface
{
    public interface IContainer
    {

        /// <summary>
        /// 注入全局绘制器
        /// </summary>
        /// <param name="painter"></param>
        void SetPainter(IPainter painter);

        /// <summary>
        /// 初始化布局
        /// </summary>
        void InitializeLayout();

        /// <summary>
        /// 根据尺寸响应布局
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        void ResetSize(int width, int height);

        /// <summary>
        /// 绘制全部可见虚拟控件
        /// </summary>
        void PaintAll(Graphics graphics);

        /// <summary>
        /// 绘制全部可见虚拟控件
        /// </summary>
        void PaintAll();
    }
}
