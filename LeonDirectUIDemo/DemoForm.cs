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
        IPaint painter = new CommonPainter();
        ContainerBase container = new ContainerBase();
        ControlBase control_0 = new ControlBase();
        ControlBase control_1 = new ControlBase();
        ControlBase control_2 = new ControlBase();

        public DemoForm()
        {
            InitializeComponent();
        }
        protected override void WndProc(ref Message m)
        {
            container.MessageReceived(ref m);
            base.WndProc(ref m);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            container.SetContainer(this);
            painter.SetGraphics(this.CreateGraphics());
            control_0.SetPainter(painter);
            control_1.SetPainter(painter);
            control_2.SetPainter(painter);
            
            control_0.Paint();
            control_1.Paint();
            control_2.Paint();
        }
    }
}
