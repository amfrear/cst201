using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SteinerTreeVisualization
{
    public partial class SteinerForm : Form
    {
        // Define the four village vertices (unit square)
        private readonly PointF A = new PointF(0f, 0f);
        private readonly PointF B = new PointF(1f, 0f);
        private readonly PointF C = new PointF(1f, 1f);
        private readonly PointF D = new PointF(0f, 1f);

        // Define the two Steiner points (pre-computed for the optimal configuration)
        private readonly PointF S1 = new PointF(0.5f, 0.288675f);
        private readonly PointF S2 = new PointF(0.5f, 0.711325f);

        // Scale factor and offset for drawing (to convert unit coordinates to screen coordinates)
        private readonly float scale = 300f;  // 1 unit = 300 pixels
        private readonly float offset = 50f;  // margin from the window edge

        public SteinerForm()
        {
            this.Text = "Steiner Tree Visualization";
            this.ClientSize = new Size(400, 400);
            this.BackColor = Color.White;
        }

        // Transform a point from unit coordinates to drawing coordinates.
        private PointF Transform(PointF p)
        {
            // Flip the y-axis so that (0,0) is at the bottom-left.
            return new PointF(offset + p.X * scale, offset + (1 - p.Y) * scale);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw the villages as blue circles
            float villageRadius = 5f;
            Brush villageBrush = Brushes.Blue;
            foreach (var village in new PointF[] { A, B, C, D })
            {
                PointF p = Transform(village);
                g.FillEllipse(villageBrush, p.X - villageRadius, p.Y - villageRadius, villageRadius * 2, villageRadius * 2);
            }

            // Draw the Steiner points as red circles
            float steinerRadius = 5f;
            Brush steinerBrush = Brushes.Red;
            foreach (var sp in new PointF[] { S1, S2 })
            {
                PointF p = Transform(sp);
                g.FillEllipse(steinerBrush, p.X - steinerRadius, p.Y - steinerRadius, steinerRadius * 2, steinerRadius * 2);
            }

            // Draw roads (edges) in black
            Pen roadPen = new Pen(Color.Black, 2);
            g.DrawLine(roadPen, Transform(A), Transform(S1));
            g.DrawLine(roadPen, Transform(B), Transform(S1));
            g.DrawLine(roadPen, Transform(C), Transform(S2));
            g.DrawLine(roadPen, Transform(D), Transform(S2));
            g.DrawLine(roadPen, Transform(S1), Transform(S2));

            // Optionally, draw the boundary of the unit square in dashed gray
            Pen squarePen = new Pen(Color.Gray, 1)
            {
                DashStyle = DashStyle.Dash
            };
            PointF[] squarePoints = {
                Transform(A), Transform(B), Transform(C), Transform(D), Transform(A)
            };
            g.DrawLines(squarePen, squarePoints);
        }
    }
}
