using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeonDirectUI.Container
{
    public class CloneContainerBase : ContainerBase
    {
        #region 属性-克隆目标容器

        ContainerBase _targetContainer = null;
        /// <summary>
        /// 克隆目标容器
        /// </summary>
        public ContainerBase TargetContainer
        {
            get => _targetContainer;
            set
            {
                if (_targetContainer != value)
                {
                    if (_targetContainer != null)
                    {
                        //解除绑定旧容器
                        DiscloneContainer(_targetContainer);
                    }
                    _targetContainer = value;
                    if (_targetContainer != null)
                    {
                        //绑定新目标容器
                        CloneContainer(_targetContainer);
                    }
                }
            }
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 无参构造函数
        /// </summary>
        private CloneContainerBase() { }

        /// <summary>
        /// 构造绑定到目标容器的克隆容器
        /// </summary>
        /// <param name="container"></param>
        public CloneContainerBase(ContainerBase container) : base()
            => TargetContainer = container;

        #endregion

        #region 方法-克隆/解除目标容器

        /// <summary>
        /// 绑定克隆目标容器
        /// </summary>
        /// <param name="container">待绑定的容器</param>
        public virtual void CloneContainer(ContainerBase container)
        {
            if (container == null) throw new Exception("克隆的目标容器为空");

            //初始化克隆容器尺寸
            this.Size = container.Size;

            //订阅目标容器事件以跟随
            container.HandleDestroyed += TargetContainer_HandleDestroyed;
            container.SizeChanged += TargetContainer_SizeChanged;

            //重复注册虚拟控件
            container.ForEach(control => this.Add(control));
        }

        /// <summary>
        /// 解除绑定
        /// </summary>
        public virtual void DiscloneContainer()
            => DiscloneContainer(_targetContainer);

        /// <summary>
        /// 解除绑定克隆目标容器
        /// </summary>
        /// <param name="container">待解除的容器</param>
        protected virtual void DiscloneContainer(ContainerBase container)
        {
            if (container == null || container.Disposing || container.IsDisposed) return; 

            //解除目标容器绑定
            Clear();
        }

        #endregion

        #region 绑定到克隆容器的方法

        /// <summary>
        /// 跟随容器尺寸改变调整尺寸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetContainer_SizeChanged(object sender, EventArgs e) => this.Size = (sender as ContainerBase).Size;

        /// <summary>
        /// 跟随容器销毁解除克隆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TargetContainer_HandleDestroyed(object sender, EventArgs e)
        {
            //仅解除克隆绑定
            DiscloneContainer((sender as ContainerBase));

            this.Invalidate();
        }

        #endregion

        #region 克隆时允许忽略的方法

        public override void InitializeLayout() { }

        public override void ResetSize(int width, int height) { }

        #endregion
    }
}
