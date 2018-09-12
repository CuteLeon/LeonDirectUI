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
        /// 绘制器
        /// </summary>
        IPaint painter = new CommonPainter();

        /// <summary>
        /// DUI控件
        /// </summary>
        ControlBase control_0 = new ControlBase();

        /// <summary>
        /// 物理容器
        /// </summary>
        public Control TargetContainer { get; set; }

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
