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
        /// 物理容器
        /// </summary>
        Control TargetContainer { get; set; }

        /// <summary>
        /// 注入物理容器
        /// </summary>
        /// <param name="container"></param>
        void SetContainer(Control container);
    }
}
