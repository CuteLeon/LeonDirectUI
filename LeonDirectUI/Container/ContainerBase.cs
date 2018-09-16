using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
    public abstract class ContainerBase : Control, Interface.IContainer
    {

        #region 基础字段

        /// <summary>
        /// 控件列表
        /// </summary>
        [Browsable(false)]
        public new List<ControlBase> Controls { get; } = new List<ControlBase>();

        /// <summary>
        /// 绘制器
        /// </summary>
        [Browsable(false)]
        public IPainter Painter { get; protected set; } = new CommonPainter();

        #endregion

        #region 基础属性

        #endregion

        #region 基础方法

        /// <summary>
        /// 构造容器基类
        /// </summary>
        public ContainerBase() : base()
        {
            base.DoubleBuffered = true;
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, true);
            base.SetStyle(ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// 控件列表索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ControlBase this[int index] => Controls[index];

        /// <summary>
        /// 注入全局绘制器
        /// </summary>
        /// <param name="painter"></param>
        public void SetPainter(IPainter painter)
        {
            Painter = painter ?? throw new Exception("注入绘制器为空");
            //切换绘制器后重绘
            this.Invalidate();
        }

        #endregion

        #region 消息处理层

        protected override void WndProc(ref Message m)
        {
            //TODO: 处理消息放在这里
            base.WndProc(ref m);
        }

        #endregion

        #region 布局和绘制

        /// <summary>
        /// 页面布局 (初始化布局)
        /// </summary>
        public abstract void InitializeLayout();

        /// <summary>
        /// 覆写触发绘制事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            PaintAll(e.Graphics);
            base.OnPaint(e);
        }

        /// <summary>
        /// 绘制全部可见虚拟控件
        /// </summary>
        public void PaintAll(Graphics graphics)
        {
            if (graphics == null) throw new Exception("容器绘制时使用的 Graphics 为空");
            if (Painter == null) throw new Exception("容器绘制时使用的 Painter 为空");

            foreach (var control in Controls.Where(c => c.Visible))
            {
                Painter.Paint(graphics, control);
            }
            //Console.WriteLine("——< 绘制完成>——————");
        }

        /// <summary>
        /// 绘制全部可见虚拟控件
        /// </summary>
        public void PaintAll()
        {
            PaintAll(this.CreateGraphics());
        }

        /// <summary>
        /// 覆写触发尺寸改变事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ResetSize(this.Width, this.Height);
        }

        /// <summary>
        /// 响应式布局
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public abstract void ResetSize(int width, int height);

        #endregion

    }
}
