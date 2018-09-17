using LeonDirectUI.Container;
using LeonDirectUI.DUIControl;
using LeonDirectUI.Interface;
using LeonDirectUI.Painter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeonDirectUIDemo
{
    public partial class DemoForm : Form
    {
        ContainerBase container = new CustomContainer();

        public DemoForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ControlBase control = new ControlBase();
            control.MaxSize = new Size(-100,100);
            control.MinSize = new Size(-1,-1);
            control.SetSize(100,120);
            Console.WriteLine(control.Size);

            control.MinSize = new Size(100,100);
            control.MaxSize = new Size(50,50);
            control.SetSize(75,75);
            Console.WriteLine(control.Size);

            control.MaxSize = new Size(50, 50);
            control.MinSize = new Size(100,100);
            control.SetSize(75, 75);
            Console.WriteLine(control.Size);
        }

    }
}
