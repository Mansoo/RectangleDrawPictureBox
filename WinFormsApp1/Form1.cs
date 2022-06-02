using System.Diagnostics;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Point? startPoint;
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastRect = null;
            startPoint = e.Location;
            //Capture = true;
            Debug.Write("start");
        }

        Rectangle? lastRect;
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            lastRect = new Rectangle(startPoint.Value.X, startPoint.Value.Y, e.Location.X - startPoint.Value.X + 1, e.Location.Y - startPoint.Value.Y + 1);
            startPoint = null;
            // Capture = false;
            Debug.Write("end");
            pictureBox1.Invalidate();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (startPoint != null)
            {
                Debug.WriteLine(startPoint + "~" + e.Location + ", capture = " + Capture);
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            //base.Paint(sender, e);
            if (lastRect != null)
            {
                Debug.WriteLine(lastRect);
                e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), lastRect.Value);
            }
        }
    }
}