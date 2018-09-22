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
        #region 高频绘制阻止器

        //TODO: [提醒] [容器子类开发] 大量绘制任务 {开始前使用 SuspendPaint() 挂起绘制、结束后使用 ResumePaint() 恢复绘制} 以避免频繁绘制造成的性能损耗；

        /// <summary>
        /// 阻止器计数
        /// </summary>
        [ThreadStatic]
        private volatile byte PaintSuspendCount;

        /// <summary>
        /// 挂起绘制
        /// </summary>
        public void SuspendPaint()
        {
            this.PaintSuspendCount += 1;
        }

        /// <summary>
        /// 恢复绘制挂起
        /// </summary>
        public void ResumePaint()
        {
            this.PaintSuspendCount -= 1;

            if (this.PaintSuspendCount == 0)
                this.PaintAll();
        }

        #endregion

        #region 事件

        /// <summary>
        /// 有控件加入
        /// </summary>
        public new event EventHandler<ControlBase> ControlAdded;

        /// <summary>
        /// 有控件移除
        /// </summary>
        public new event EventHandler<ControlBase> ControlRemoved;

        #endregion

        #region 基础属性

        private readonly List<ControlBase> _controls = new List<ControlBase>();
        /// <summary>
        /// 控件列表
        /// </summary>
        [Browsable(false)]
        public new ControlBase[] Controls => this._controls.ToArray();
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

        #region 基础方法

        /// <summary>
        /// 构造容器基类
        /// </summary>
        public ContainerBase() : base()
        {
            base.DoubleBuffered = true;
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.ContainerControl, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, false);
            base.SetStyle(ControlStyles.UserPaint, true);

            //初始化布局
            this.InitializeLayout();
        }

        /// <summary>
        /// 控件列表索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ControlBase this[int index] => this.Controls[index];

        /// <summary>
        /// 注入全局绘制器
        /// </summary>
        /// <param name="painter"></param>
        public void SetPainter(IPainter painter)
        {
            this.Painter = painter ?? throw new Exception("注入绘制器为空");
            //切换绘制器后重绘
            this.Invalidate();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public new void Dispose()
        {
            //清空控件列表
            this.Clear();
            base.Dispose();
        }

        /// <summary>
        /// 注册控件
        /// </summary>
        /// <param name="control"></param>
        protected virtual void RegisterControl(ControlBase control)
        {
            //TODO: [提醒] [容器扩展] 新增控件时需要的初始化操作放这里
            //TODO: [提醒] [容器扩展] 这里的操作需要兼容克隆容器
            if (control == null) throw new Exception("注册的虚拟控件为空");

            try
            {
                control.Disposing += this.Control_Dispoing;
                control.PaintRequired += this.Control_PaintRequired;
            }
            catch { }
            finally { }
        }

        /// <summary>
        /// 注销控件
        /// </summary>
        /// <param name="control"></param>
        protected virtual void DisregisterControl(ControlBase control)
        {
            //TODO: [提醒] [容器扩展] 移除控件时需要的初始化操作放这里
            //TODO: [提醒] [容器扩展] 这里的操作需要兼容克隆容器
            if (control == null) return;

            try
            {
                control.Disposing -= this.Control_Dispoing;
                control.PaintRequired -= this.Control_PaintRequired;
            }
            catch { }
            finally { }
        }

        #endregion

        #region Controls公开方法

        #region 增加

        /// <summary>
        /// 添加控件
        /// </summary>
        /// <param name="control">虚拟控件</param>
        public virtual void Add(ControlBase control)
        {
            this.RegisterControl(control);

            this._controls.Add(control);

            this.OnControlAdded(control);
        }

        /// <summary>
        /// 添加控件数组
        /// </summary>
        /// <param name="controls">虚拟控件</param>
        public virtual void AddRange(IEnumerable<ControlBase> controls)
        {
            foreach (var control in controls)
                this.Add(control);

            //this._controls.AddRange(controls);
        }

        /// <summary>
        /// 插入虚拟控件
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="control">虚拟控件</param>
        public virtual void Insert(int index, ControlBase control)
        {
            this.RegisterControl(control);

            this._controls.Insert(index, control);

            this.OnControlAdded(control);
        }

        /// <summary>
        /// 插入虚拟控件数组
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="controls">虚拟控件数组</param>
        public virtual void InsertRange(int index, IEnumerable<ControlBase> controls)
        {
            this._controls.ForEach(
                (control) =>
                {
                    this.Insert(index, control);
                    index++;
                });
        }

        #endregion

        #region 移除

        /// <summary>
        /// 移除虚拟控件
        /// </summary>
        /// <param name="control">虚拟控件</param>
        /// <returns></returns>
        public virtual bool Remove(ControlBase control)
        {
            this.DisregisterControl(control);

            var result = this._controls.Remove(control);

            this.OnControlRemoved(control);
            return result;
        }

        /// <summary>
        /// 移除对应标识的虚拟控件
        /// </summary>
        /// <param name="index">标识</param>
        public virtual void RemoveAt(int index)
        {
            this.Remove(this._controls[index]);
        }

        /// <summary>
        /// 移除指定区间的虚拟控件
        /// </summary>
        /// <param name="index">开始位置</param>
        /// <param name="count">移除长度</param>
        public virtual void RemoveRange(int index, int count)
        {
            foreach (var control in this._controls.Skip(index).Take(count))
                this.Remove(control);
        }

        /// <summary>
        /// 移除所有符合条件谓词的虚拟控件
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns></returns>
        public virtual int RemoveAll(Predicate<ControlBase> predicate)
        {
            List<ControlBase> controls = this._controls.FindAll(predicate);

            foreach (var control in controls)
            {
                this.Remove(control);
            }

            return controls.Count;
        }

        /// <summary>
        /// 清空列表
        /// </summary>
        public virtual void Clear()
        {
            while (this._controls.Count != 0)
                this.RemoveAt(this._controls.Count - 1);
        }

        #endregion

        /// <summary>
        /// 是否存在符合条件谓词的元素
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns></returns>
        public virtual bool Exists(Predicate<ControlBase> predicate)
            => this._controls.Exists(predicate);

        /// <summary>
        /// 获取符合条件谓词的元素标识
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns></returns>
        public virtual int FindIndex(Predicate<ControlBase> predicate)
            => this._controls.FindIndex(predicate);

        /// <summary>
        /// 查找符合条件谓词的最一个元素
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns></returns>
        public virtual ControlBase FindLast(Predicate<ControlBase> predicate)
            => this._controls.FindLast(predicate);

        /// <summary>
        /// 查找符合条件谓词的最一个元素的标识
        /// </summary>
        /// <param name="predicate">条件谓词</param>
        /// <returns></returns>
        public virtual int FindLastIndex(Predicate<ControlBase> predicate)
            => this._controls.FindLastIndex(predicate);

        /// <summary>
        /// 对每个元素执行动作
        /// </summary>
        /// <param name="action"></param>
        public virtual void ForEach(Action<ControlBase> action)
            => this._controls.ForEach(action);

        /// <summary>
        /// 获取元素的标识
        /// </summary>
        /// <param name="control">虚拟控件</param>
        /// <returns></returns>
        public virtual int IndexOf(ControlBase control)
            => this._controls.IndexOf(control);

        /// <summary>
        /// 倒序查找虚拟控件的标识
        /// </summary>
        /// <param name="control">虚拟控件</param>
        /// <returns></returns>
        public virtual int LastIndexOf(ControlBase control)
            => this._controls.LastIndexOf(control);

        /// <summary>
        /// 使用默认比较器排序
        /// </summary>
        public virtual void Sort()
            => this._controls.Sort();

        /// <summary>
        /// 使用指定比较器排序
        /// </summary>
        /// <param name="comparison">比较器</param>
        public virtual void Sort(Comparison<ControlBase> comparison)
            => this._controls.Sort(comparison);

        /// <summary>
        /// 使用指定比较器排序
        /// </summary>
        /// <param name="comparer">比较器</param>
        public virtual void Sort(IComparer<ControlBase> comparer)
            => this._controls.Sort(comparer);

        /// <summary>
        /// 使用指定比较器
        /// </summary>
        /// <param name="index">排序开始标识</param>
        /// <param name="count">排序区域长度</param>
        /// <param name="comparer">比较器</param>
        public virtual void Sort(int index, int count, IComparer<ControlBase> comparer)
            => this._controls.Sort(index, count, comparer);

        #endregion

        #region 消息处理层

        protected override void WndProc(ref Message m)
        {
            //TODO: [提醒] [容器基类开发] 处理消息放在这里
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
            this.PaintAll(e.Graphics, e.ClipRectangle);
            base.OnPaint(e);
        }

        /// <summary>
        /// 绘制全部可见虚拟控件
        /// </summary>
        public void PaintAll(Graphics graphics)
        {
            if (graphics == null) throw new Exception("容器绘制时使用的 Graphics 为空");
            if (this.Painter == null) throw new Exception("容器绘制时使用的 Painter 为空");

            foreach (var control in this.Controls.Where(c => c.Visible))
            {
                this.Painter.Paint(graphics, control);
            }
            //Console.WriteLine("——< 绘制完成>——————");
        }

        /// <summary>
        /// 绘制与目标区域相交的可见虚拟控件
        /// </summary>
        //TODO: [提醒] [容器子类开发]/[虚拟控件绘制] 虚拟控件使用透明背景时切换图像造成图像重叠，需要在切换前 this.Invalidate(control.Rectangle); 刷新此区域。爱你呦，么么哒；
        public void PaintAll(Graphics graphics, Rectangle rectangle)
        {
            if (graphics == null) throw new Exception("容器绘制时使用的 Graphics 为空");
            if (this.Painter == null) throw new Exception("容器绘制时使用的 Painter 为空");
            if (rectangle.Width <= 0 || rectangle.Height <= 0) return;

            foreach (var control in this.Controls.Where(
                control =>
                control.Visible &&
                control.IntersectsWith(rectangle)
                ))
            {
                this.Painter.Paint(graphics, control);
            }
            //Console.WriteLine("——< 绘制完成>——————");
        }

        /// <summary>
        /// 绘制全部可见虚拟控件
        /// </summary>
        public void PaintAll()
        {
            if (this.Disposing || this.IsDisposed) return;

            this.PaintAll(this.CreateGraphics());
        }

        /// <summary>
        /// 子虚拟控件绘制请求事件
        /// </summary>
        /// <param name="sender">请求绘制的子虚拟控件</param>
        /// <param name="rectangle">涉及的区域</param>
        protected void Control_PaintRequired(ControlBase sender, Rectangle rectangle)
        {
            //绘制被挂起，请求也不绘制你，哼！(つД`)
            if (this.PaintSuspendCount > 0) return;

            if (this.Disposing || this.IsDisposed)
            {
                Console.WriteLine("使用释放的对象绘制UI；");
                return;
            }
            if (sender == null) throw new Exception("空的虚拟控件请求了绘制");
            if ((rectangle.Width <= 0 || rectangle.Height <= 0) &&
                (sender.Width <= 0 || sender.Height <= 0)) return;

            //绘制请求绘制的虚拟控件和与绘制区域有交集的可见虚拟控件
            using (Graphics graphics = this.CreateGraphics())
            {
                foreach (var control in this.Controls.Where(
                    ctl => ctl.Visible &&
                    ctl != sender &&
                    ctl.IntersectsWith(rectangle))
                )
                    this.Painter?.Paint(graphics, control);

                //最后绘制发起请求的虚拟控件
                if (sender.Visible)
                    this.Painter?.Paint(graphics, sender);
            }
        }

        /// <summary>
        /// 虚拟控件正在释放事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Control_Dispoing(object sender, EventArgs e)
        {
            this.Remove(sender as ControlBase);
        }

        /// <summary>
        /// 覆写触发尺寸改变事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            this.ResetSize(this.Width, this.Height);
            base.OnSizeChanged(e);
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
        /// 弃用基类 OnControlAdded(ControlEventArgs) 方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlAdded(ControlEventArgs e)
            => throw new Exception("亲爱的子类，我是你的爸爸——ContainerBase，请不要继续使用此方法，移步 OnControlAdded(ControlBase control)");

        /// <summary>
        /// 弃用基类 OnControlRemoved(ControlEventArgs) 方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnControlRemoved(ControlEventArgs e)
            => throw new Exception("亲爱的子类，我是你的爸爸——ContainerBase，请不要继续使用此方法，移步 OnControlAdded(ControlBase control)");

        /// <summary>
        /// 触发添加虚拟控件事件
        /// </summary>
        /// <param name="control">添加的虚拟控件</param>
        protected virtual void OnControlAdded(ControlBase control)
        {
            this.ControlAdded?.Invoke(this, control);
        }

        /// <summary>
        /// 触发移除虚拟控件事件
        /// </summary>
        /// <param name="control">移除的虚拟空间</param>
        protected virtual void OnControlRemoved(ControlBase control)
        {
            this.ControlRemoved?.Invoke(this, control);
        }

        /// <summary>
        /// 点击
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e)
        {
            Point mousePoint = this.PointToClient(MousePosition);
            ControlBase control = this.Controls.FirstOrDefault(
                ctl => ctl.Visible &&
                ctl.Enabled &&
                ctl.Mouseable &&
                ctl.Contains(mousePoint));

            if (control != null)
                control.OnClick(e);

            base.OnClick(e);
        }

        /// <summary>
        /// 双击
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDoubleClick(EventArgs e)
        {
            Point mousePoint = this.PointToClient(MousePosition);
            ControlBase control = this.Controls.FirstOrDefault(
                ctl => ctl.Visible &&
                ctl.Enabled &&
                ctl.Mouseable &&
                ctl.Contains(mousePoint));

            if (control != null)
                control.OnDoubleClick(e);

            base.OnDoubleClick(e);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            ControlBase control = this.Controls.FirstOrDefault(
                ctl => ctl.Visible &&
                ctl.Enabled &&
                ctl.Mouseable &&
                ctl.Contains(e.Location));

            //激活控件发生变化
            if (this.ActiveControl != control)
            {
                if (this.ActiveControl != null)
                {
                    //通知之前的激活控件"鼠标离开"
                    this.ActiveControl.OnMouseLeave(EventArgs.Empty);
                }
                //更新激活控件
                this.ActiveControl = control;
                if (this.ActiveControl != null)
                {
                    //通知新的激活控件"鼠标进入"和"鼠标移动"
                    this.ActiveControl.OnMouseEnter(EventArgs.Empty);
                    this.ActiveControl.OnMouseMove(e);
                }
            }
            else
            {
                if (this.ActiveControl != null)
                    this.ActiveControl.OnMouseMove(e);

                base.OnMouseMove(e);
            }
        }

        /// <summary>
        /// 鼠标悬停
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseHover(EventArgs e)
        {
            Point mousePoint = this.PointToClient(MousePosition);
            ControlBase control = this.Controls.FirstOrDefault(
                ctl => ctl.Visible &&
                ctl.Enabled &&
                ctl.Mouseable &&
                ctl.Contains(mousePoint));

            if (control != null)
                control.OnMouseHover(e);

            base.OnMouseHover(e);
        }

        /// <summary>
        /// 鼠标按压
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            ControlBase control = this.Controls.FirstOrDefault(
                ctl => ctl.Visible &&
                ctl.Enabled &&
                ctl.Mouseable &&
                ctl.Contains(e.Location));

            if (control != null)
                control.OnMouseDown(e);

            base.OnMouseDown(e);
        }

        /// <summary>
        /// 鼠标抬起
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            ControlBase control = Controls.FirstOrDefault(
                ctl => ctl.Visible &&
                ctl.Enabled &&
                ctl.Mouseable &&
                ctl.Contains(e.Location));

            if (control != null)
                control.OnMouseUp(e);

            base.OnMouseUp(e);
        }

        /// <summary>
        /// 鼠标进入
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            Point mousePoint = this.PointToClient(MousePosition);
            ControlBase control = Controls.FirstOrDefault(
                ctl => ctl.Visible &&
                ctl.Enabled &&
                ctl.Mouseable &&
                ctl.Contains(mousePoint));

            if (control != null)
            {
                this.ActiveControl = control;
                this.ActiveControl.OnMouseEnter(e);
            }

            base.OnMouseEnter(e);
        }

        /// <summary>
        /// 鼠标离开
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            if (this.ActiveControl != null)
            {
                this.ActiveControl.OnMouseLeave(e);
                this.ActiveControl = null;
            }

            base.OnMouseLeave(e);
        }

        #endregion

    }
}
