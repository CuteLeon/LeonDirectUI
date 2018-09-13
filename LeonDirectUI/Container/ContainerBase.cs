using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeonDirectUI.DUIControl;
using LeonDirectUI.Interface;
using LeonDirectUI.Painter;

namespace LeonDirectUI.Container
{
    //TODO: 订阅虚拟控件的鼠标事件等，以允许控件回调
    //TODO: 需要显示或刷新时， 调用 虚拟空间.Paint(this.CreateGraphics)；
    //TODO: 覆写 WndProc 方法，收到鼠标移动、按压、抬起等消息时判断鼠标坐标所在控件区域，(通过接口或直接) 调用控件 OnMouseXXX，由控件再回调鼠标事件，由外部容器刚订阅的事件调用外部容器的方法；
    //TODO: 在注入物理容器时使用 HOOK API 向物理容器句柄下钩子，将消息引到 MessageReceived 方法
    //TODO: 实现 IAccessible 接口

    /// <summary>
    /// 容器基类
    /// </summary>
    public class ContainerBase : IContainer
    {
        /// <summary>
        /// 控件列表
        /// </summary>
        public List<ControlBase> Controls { get; protected set; } = new List<ControlBase>();

        /// <summary>
        /// 控件列表索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ControlBase this[int index] => Controls[index];
        
        /// <summary>
        /// 物理容器
        /// </summary>
        public Control TargetContainer { get; set; }

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="m"></param>
        public virtual void MessageReceived(ref Message m)
        {
            switch (m.Msg)
            {
                //case 
            }
        }

        /// <summary>
        /// 注入物理容器
        /// </summary>
        /// <param name="container"></param>
        public void SetContainer(Control container)
        {
            TargetContainer = container ?? throw new Exception("注入物理容器为空");

        }

    }
}
