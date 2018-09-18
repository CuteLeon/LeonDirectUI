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
    public abstract class ContainerBase : Control, Interface.IContainer, IDisposable
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

        /// <summary>
        /// 当前鼠标进入的虚拟控件
        /// </summary>
        [Browsable(false)]
        public ControlBase ActiveControl { get; protected set; } = null;

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
            base.SetStyle(ControlStyles.ResizeRedraw, false);
            base.SetStyle(ControlStyles.UserPaint, true);

            //初始化布局
            InitializeLayout();

            //监听子虚拟控件绘制请求
            foreach (var control in Controls)
            {
                control.PaintRequired += Control_PaintRequired;
            }
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

        /// <summary>
        /// 释放资源
        /// </summary>
        public new void Dispose()
        {
            //取消订阅控件列表内的虚拟控件的请求订阅并移除虚拟控件
            while (Controls.Count>0)
            {
                try
                {
                    Controls[0].PaintRequired -= Control_PaintRequired;
                    Controls.RemoveAt(0);
                }
                catch { }
            }
            base.Dispose();
        }

        #endregion

        #region 消息处理层

        protected override void WndProc(ref Message m)
        {
            //TODO: [提醒] 处理消息放在这里
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
        /// 子虚拟控件绘制请求事件
        /// </summary>
        /// <param name="sender">请求绘制的子虚拟控件</param>
        /// <param name="rectangle">涉及的区域</param>
        //TODO: [提醒] 虚拟控件动态注册到控件容器后需要订阅 PaintRequired 事件到此方法以接收绘制请求；
        public void Control_PaintRequired(ControlBase sender, Rectangle rectangle)
        {
            if (sender == null) throw new Exception("空的虚拟控件请求了绘制");
            if ((sender.Width <= 0 || rectangle.Height <= 0) &&
                (sender.Width <= 0 || sender.Height <= 0)) return;

            //绘制请求绘制的虚拟控件和与绘制区域有交集的可见虚拟控件
            using (Graphics graphics = this.CreateGraphics())
            {
                foreach (var control in Controls.Where(ctl => ctl.Visible && ctl!=sender && ctl.IntersectsWith(rectangle)))
                    Painter?.Paint(graphics, control);
                //最后绘制发起请求的虚拟控件
                Painter?.Paint(graphics, sender);
            }
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

        #region 触发事件

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            Point mousePoint= this.PointToClient(MousePosition);
            ControlBase control = Controls.FirstOrDefault(ctl => ctl.Visible && ctl.Enabled && ctl.Contains(mousePoint));
            
            if (control == null)
                base.OnClick(e);
            else
                control.OnClick(e);
        }

        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDoubleClick(EventArgs e)
        {
            Point mousePoint = this.PointToClient(MousePosition);
            ControlBase control = Controls.FirstOrDefault(ctl => ctl.Visible && ctl.Enabled && ctl.Contains(mousePoint));

            if (control == null)
                base.OnDoubleClick(e);
            else
                control.OnDoubleClick(e);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            ControlBase control = Controls.FirstOrDefault(ctl => ctl.Visible && ctl.Enabled && ctl.Contains(e.Location));

            //激活控件发生变化
            if (ActiveControl != control)
            {
                if (ActiveControl != null)
                {
                    //通知之前的激活控件"鼠标离开"
                    ActiveControl.OnMouseLeave(EventArgs.Empty);
                }
                //更新激活控件
                ActiveControl = control;
                if (ActiveControl != null)
                {
                    //通知新的激活控件"鼠标进入"和"鼠标移动"
                    ActiveControl.OnMouseEnter(EventArgs.Empty);
                    ActiveControl.OnMouseMove(e);
                }
            }
            else
            {
                if (ActiveControl != null)
                {
                    ActiveControl.OnMouseMove(e);
                }
                else
                {
                    base.OnMouseMove(e);
                }
            }
        }

        /// <summary>
        /// 鼠标悬停
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseHover(EventArgs e)
        {
            Point mousePoint = this.PointToClient(MousePosition);
            ControlBase control = Controls.FirstOrDefault(ctl => ctl.Visible && ctl.Enabled && ctl.Contains(mousePoint));

            if (control == null)
                base.OnMouseHover(e);
            else
                control.OnMouseHover(e);
        }

        /// <summary>
        /// 鼠标按压
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            ControlBase control = Controls.FirstOrDefault(ctl => ctl.Visible && ctl.Enabled && ctl.Contains(e.Location));

            if (control == null)
                base.OnMouseDown(e);
            else
                control.OnMouseDown(e);
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            ControlBase control = Controls.FirstOrDefault(ctl => ctl.Visible && ctl.Enabled && ctl.Contains(e.Location));

            if (control == null)
                base.OnMouseUp(e);
            else
                control.OnMouseUp(e);
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            Point mousePoint = this.PointToClient(MousePosition);
            ControlBase control = Controls.FirstOrDefault(ctl => ctl.Visible && ctl.Enabled && ctl.Contains(mousePoint));

            if (control == null)
            {
                base.OnMouseEnter(e);
            }
            else
            {
                ActiveControl = control;
                ActiveControl.OnMouseEnter(e);
            }
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (ActiveControl != null)
            {
                ActiveControl.OnMouseLeave(e);
                ActiveControl = null;
            }
            else
            {
                base.OnMouseLeave(e);
            }
        }

        #endregion

    }
}
