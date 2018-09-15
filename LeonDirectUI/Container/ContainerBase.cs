using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeonDirectUI.DUIControl;
using LeonDirectUI.Interface;
using LeonDirectUI.Painter;
using LeonDirectUI.Win32;

namespace LeonDirectUI.Container
{
    /// <summary>
    /// 容器基类
    /// </summary>
    public abstract class ContainerBase : IContainer
    {

        #region 基础字段

        /// <summary>
        /// 控件列表
        /// </summary>
        public List<ControlBase> Controls { get; } = new List<ControlBase>();

        /// <summary>
        /// 控件列表索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ControlBase this[int index] => Controls[index];

        /// <summary>
        /// 物理容器
        /// </summary>
        public Control TargetContainer { get; protected set; }

        #endregion

        #region 基础方法
        
        /// <summary>
        /// 构造容器基类
        /// </summary>
        public ContainerBase()
        {
            CustomWndProc = new SubWndProc((w, x, y, z) => MessageReceived(w, x, y, z));
        }

        /// <summary>
        /// 注入物理容器
        /// </summary>
        /// <param name="container"></param>
        public void SetContainer(Control container)
        {
            TargetContainer = container ?? throw new Exception("注入物理容器为空");
            
            //如果已经有钩子需要注销掉
            if (Hooked) UnHook();

            //TODO: 不提供自动等待句柄创建后挂载钩子的能力，注入的物理容器不可用直接报错
            /*
            base.DoubleBuffered = true;
            base.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            base.SetStyle(ControlStyles.ResizeRedraw, false);
            base.SetStyle(ControlStyles.UserPaint, true);
             */

            if (!container.IsHandleCreated)
            {
                //等待控件句柄创建后挂载钩子
                container.HandleCreated += (s, v) => 
                {
                    SetHook(container);
                    ResetSize(container.DisplayRectangle.Width, container.DisplayRectangle.Height);
                };
            }
            else
            {
                //立即挂载钩子
                SetHook(container);
                ResetSize(container.DisplayRectangle.Width, container.DisplayRectangle.Height);
            }
        }

        /// <summary>
        /// 注入全局绘制器
        /// </summary>
        /// <param name="painter"></param>
        public void SetPainter(IPainter painter)
        {
            if (painter==null) throw new Exception("注入绘制器为空");

            foreach (var control in Controls)
            {
                control.SetPainter(painter);
                control.Paint();
            }
        }

        #endregion

        #region 消息处理层

        /// <summary>
        /// 接收到消息
        /// </summary>
        /// <param name="m"></param>
        public virtual int MessageReceived(IntPtr hWnd, int Msg, int wParam, int lParam)
        {
            //TODO: 燃烧吧，大脑！参考：Systen.Windwos.Forms.Control.WndProc(ref Message m) 方法；
            //TODO: 鼠标移动：判断当前响应的控件；鼠标按压、抬起、点击、双击等，调用控件 OnMouseXXX（注意鼠标按压后仍移动，并移动到别处抬起的情况），由控件再回调鼠标事件；此容器刚再向外部触发封装好的自定义事件；
            //TODO: 劫持到尺寸改变的消息时调用 ResetSize(int, int) 调整虚拟控件位置和大小以刷新布局；

            //Console.WriteLine($"{DateTime.Now.ToString()} - {hWnd} : {Msg} ({wParam}, {lParam})");
            switch (Msg)
            {
                //case 
            }

            //放行消息
            return Win32API.CallWindowProc(this.OldWndProcHandle, hWnd, Msg, wParam, lParam);
        }

        #endregion

        #region 钩子

        /// <summary>
        /// 钩子挂载结果
        /// </summary>
        private bool Hooked = false;

        /// <summary>
        /// 物理容器控件原有消息处理函数地址
        /// </summary>
        private IntPtr OldWndProcHandle = IntPtr.Zero;
        /// <summary>
        /// 此容器劫持到的新消息处理函数地址
        /// </summary>
        private IntPtr NewWndProcHandle = IntPtr.Zero;

        /// <summary>
        /// 消息处理函数委托
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public delegate int SubWndProc(IntPtr hWnd, int msg, int wParam, int lParam);
        /// <summary>
        /// 自定义消息处理函数
        /// </summary>
        SubWndProc CustomWndProc;

        /// <summary>
        /// 挂载钩子
        /// </summary>
        /// <param name="container"></param>
        private void SetHook(Control container)
        {
            if (Hooked) UnHook();

            //记录物理控件原消息处理函数句柄
            OldWndProcHandle = Win32API.GetWindowLong(container.Handle, Win32API.GWL_WNDPROC);
            if (OldWndProcHandle == IntPtr.Zero) Console.WriteLine("挂载钩子前获取原消息处理方法句柄出错");
            
            //劫持消息
            NewWndProcHandle = Marshal.GetFunctionPointerForDelegate(CustomWndProc);
            IntPtr result = Win32API.SetWindowLong(container.Handle, Win32API.GWL_WNDPROC, NewWndProcHandle);

            //检查劫持结果
            if (result == IntPtr.Zero && NewWndProcHandle != IntPtr.Zero)
            {
                Hooked = false;
                Console.WriteLine($"挂载物理控件消息钩子失败：NewWndProcHandle = {NewWndProcHandle}");
                //throw new Exception($"挂载物理控件消息钩子失败：NewWndProcHandle = {NewWndProcHandle}");
            }
            else
            {
                Hooked = true;
                //TODO: 钩子挂载成功后立即刷新一次布局并绘制；
            }
        }

        /// <summary>
        /// 注销钩子
        /// </summary>
        private void UnHook()
        {
            if (TargetContainer != null && TargetContainer.IsHandleCreated && OldWndProcHandle != IntPtr.Zero)
            {
                //把物理控件的消息处理函数恢复到原方法
                Win32API.SetWindowLong(TargetContainer.Handle,Win32API.GWL_WNDPROC, OldWndProcHandle);
                //清楚变量记录
                OldWndProcHandle = IntPtr.Zero;
            }
            Hooked = false;
        }

        #endregion

        #region 布局和绘制

        /// <summary>
        /// 页面布局 (初始化布局)
        /// </summary>
        public abstract void InitializeLayout();

        /// <summary>
        /// 绘制全部可见虚拟控件
        /// </summary>
        public void PaintAll()
        {
            foreach (var control in Controls.Where(c => c.Visible))
            {
                if (control.Painter != null)
                    control.Paint();
            }
            Console.WriteLine("——< 绘制完成>——————");
        }

        /// <summary>
        /// 根据尺寸响应布局
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public abstract void ResetSize(int width, int height);

        #endregion

    }
}
