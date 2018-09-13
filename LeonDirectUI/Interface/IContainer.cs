using LeonDirectUI.DUIControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeonDirectUI.Interface
{
    public interface IContainer
    {
        /// <summary>
        /// 控件列表
        /// </summary>
        List<ControlBase> Controls { get; }

        /// <summary>
        /// 物理容器
        /// </summary>
        Control TargetContainer { get; set; }

        /// <summary>
        /// 注入物理容器
        /// </summary>
        /// <param name="container"></param>
        void SetContainer(Control container);

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="m"></param>
        int MessageReceived(IntPtr hWnd, int Msg, int wParam, int lParam);

        /// <summary>
        /// 初始化布局
        /// </summary>
        void InitializeLayout();

        /// <summary>
        /// 绘制全部可见虚拟控件
        /// </summary>
        void PaintAll();
    }
}
