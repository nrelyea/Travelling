using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Travelling
{
    public partial class Form1 : Form
    {
        public List<List<int>> pointsList = new List<List<int>> { };

        public Form1(List<List<int>> intListList)
        {
            InitializeComponent();
            pointsList = intListList;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(Form1_Paint);

        }

        public void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            
            drawPath(e, pointsList);
 
        }

        private void tnrAppTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        public void drawPath(System.Windows.Forms.PaintEventArgs e, List<List<int>> pointsList)
        {
            TextFormatFlags flags = TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis;

            int pointSize = 10;
             
            for(int i=0; i < pointsList.Count; i++)
            {
                Rectangle pt = new Rectangle(pointsList[i][0], pointsList[i][1], pointSize, pointSize);
                System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);               
                e.Graphics.FillEllipse(redBrush, pt);
                e.Graphics.DrawEllipse(Pens.Black, pt);

                //TextRenderer.DrawText(e.Graphics, "This is some text that will be clipped at the end.", this.Font,
                //new Rectangle(10, 10, 100, 50), SystemColors.ControlText, flags);

                if (i > 0)
                {
                    Point previous = new Point(pointsList[i - 1][0]+(pointSize/2), pointsList[i - 1][1]+(pointSize/2));
                    Point current = new Point(pointsList[i][0] + (pointSize / 2), pointsList[i][1] + (pointSize / 2));
                    e.Graphics.DrawLine(Pens.Black, previous, current);
                }
            }

            Point last = new Point(pointsList[pointsList.Count-1][0] + (pointSize / 2), pointsList[pointsList.Count - 1][1] + (pointSize / 2));
            Point first = new Point(pointsList[0][0] + (pointSize / 2), pointsList[0][1] + (pointSize / 2));
            e.Graphics.DrawLine(Pens.Black, last, first);

            
            

        }
     
    }
}
