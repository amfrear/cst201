using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SteinerTreeUseCaseDemo
{
    public partial class NetworkForm : Form
    {
        // ComboBox to switch between Steiner Tree and MST
        private ComboBox modeComboBox;
        // Label to display total length
        private Label infoLabel;

        // Four city locations (unit square corners)
        private readonly PointF A = new PointF(0f, 0f);
        private readonly PointF B = new PointF(1f, 0f);
        private readonly PointF C = new PointF(1f, 1f);
        private readonly PointF D = new PointF(0f, 1f);

        // Steiner points (bow-tie configuration)
        private readonly PointF S1 = new PointF(0.5f, 0.288675f);
        private readonly PointF S2 = new PointF(0.5f, 0.711325f);

        // Scaling + offset to convert unit coords to screen coords
        private readonly float scale = 300f;
        private readonly float offset = 50f;

        // Current display mode: "Steiner" or "MST"
        private string currentMode = "Steiner";

        public NetworkForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Form properties
            this.Text = "Telecommunications Network (Steiner vs MST)";
            this.ClientSize = new Size(700, 600);
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;

            // ComboBox
            modeComboBox = new ComboBox();
            modeComboBox.Location = new Point(20, 20);
            modeComboBox.Width = 250;
            modeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            modeComboBox.Items.Add("Steiner Tree (Optimal)");
            modeComboBox.Items.Add("Minimum Spanning Tree (MST)");
            modeComboBox.SelectedIndex = 0;
            modeComboBox.SelectedIndexChanged += ModeComboBox_SelectedIndexChanged;
            this.Controls.Add(modeComboBox);

            // Label for total length
            infoLabel = new Label();
            infoLabel.Location = new Point(20, 50);
            infoLabel.AutoSize = true;
            infoLabel.Font = new Font("Arial", 10);
            this.Controls.Add(infoLabel);
        }

        private void ModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modeComboBox.SelectedIndex == 0)
                currentMode = "Steiner";
            else
                currentMode = "MST";

            // Force redraw
            this.Invalidate();
        }

        // Convert (0..1) coordinates to screen coordinates, flipping Y
        private PointF Transform(PointF p)
        {
            return new PointF(offset + p.X * scale, offset + (1 - p.Y) * scale);
        }

        // Draw a city circle + label with custom offset
        private void DrawCity(Graphics g, PointF point, string label, float radius, Brush brush, Font font, Point offset)
        {
            PointF screenPt = Transform(point);
            g.FillEllipse(brush, screenPt.X - radius, screenPt.Y - radius, radius * 2, radius * 2);

            // Offset the label so it doesn't overlap the circle
            float labelX = screenPt.X + offset.X;
            float labelY = screenPt.Y + offset.Y;
            g.DrawString(label, font, Brushes.Black, labelX, labelY);
        }

        // Draw a line between two points + label the distance near its midpoint
        private void DrawEdgeWithDistance(Graphics g, Pen pen, Font font, Brush textBrush, PointF p1, PointF p2)
        {
            PointF t1 = Transform(p1);
            PointF t2 = Transform(p2);

            // Draw the line
            g.DrawLine(pen, t1, t2);

            // Compute the midpoint in world coords, then transform
            PointF midWorld = new PointF((p1.X + p2.X) / 2f, (p1.Y + p2.Y) / 2f);
            PointF midScreen = Transform(midWorld);

            // Distance in unit coords
            double distance = Dist(p1, p2);

            // Draw the distance slightly offset from the midpoint
            g.DrawString(distance.ToString("F3"), font, textBrush, midScreen.X + 5, midScreen.Y - 10);
        }

        // Euclidean distance in world (unit) coords
        private double Dist(PointF p1, PointF p2)
        {
            double dx = p1.X - p2.X;
            double dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw cities (with label offsets so they don't overlap)
            float cityRadius = 6f;
            Brush cityBrush = Brushes.Blue;
            Font cityFont = new Font("Arial", 10);

            // For corners, we offset labels in different directions
            DrawCity(g, A, "City A", cityRadius, cityBrush, cityFont, new Point(5, 5));
            DrawCity(g, B, "City B", cityRadius, cityBrush, cityFont, new Point(-40, 5));
            DrawCity(g, C, "City C", cityRadius, cityBrush, cityFont, new Point(-40, -25));
            DrawCity(g, D, "City D", cityRadius, cityBrush, cityFont, new Point(5, -25));

            // We'll reuse a small font for edge distances
            Font distFont = new Font("Arial", 8);
            Brush distBrush = Brushes.Black;

            if (currentMode == "Steiner")
            {
                // Draw Steiner edges in red
                Pen steinerPen = new Pen(Color.Red, 2);

                // Edges: A-S1, B-S1, S1-S2, C-S2, D-S2
                DrawEdgeWithDistance(g, steinerPen, distFont, distBrush, A, S1);
                DrawEdgeWithDistance(g, steinerPen, distFont, distBrush, B, S1);
                DrawEdgeWithDistance(g, steinerPen, distFont, distBrush, S1, S2);
                DrawEdgeWithDistance(g, steinerPen, distFont, distBrush, C, S2);
                DrawEdgeWithDistance(g, steinerPen, distFont, distBrush, D, S2);

                // Draw Steiner points (junctions)
                float steinerRadius = 6f;
                Brush steinerBrush = Brushes.DarkRed;
                PointF s1Screen = Transform(S1);
                PointF s2Screen = Transform(S2);
                g.FillEllipse(steinerBrush, s1Screen.X - steinerRadius, s1Screen.Y - steinerRadius, steinerRadius * 2, steinerRadius * 2);
                g.FillEllipse(steinerBrush, s2Screen.X - steinerRadius, s2Screen.Y - steinerRadius, steinerRadius * 2, steinerRadius * 2);

                // Compute total Steiner length
                double steinerLength = Dist(A, S1) + Dist(B, S1) + Dist(S1, S2) + Dist(C, S2) + Dist(D, S2);
                infoLabel.Text = $"Steiner Tree Total Length: {steinerLength:F4}";
            }
            else
            {
                // Draw MST edges in green
                Pen mstPen = new Pen(Color.Green, 2);

                // MST edges: A-B, B-C, C-D
                DrawEdgeWithDistance(g, mstPen, distFont, distBrush, A, B);
                DrawEdgeWithDistance(g, mstPen, distFont, distBrush, B, C);
                DrawEdgeWithDistance(g, mstPen, distFont, distBrush, C, D);

                // Compute total MST length
                double mstLength = Dist(A, B) + Dist(B, C) + Dist(C, D);
                infoLabel.Text = $"MST Total Length: {mstLength:F4}";
            }
        }
    }
}
