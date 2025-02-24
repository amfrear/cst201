using System;
using System.Drawing;
using System.Windows.Forms;

namespace SteinerTreeUseCases
{
    public partial class UseCasesForm : Form
    {
        private ComboBox comboBox;
        private RichTextBox richTextBox;
        private Label label;

        public UseCasesForm()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Form properties
            this.Text = "Steiner Tree Use Cases";
            this.ClientSize = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Create a Label
            label = new Label();
            label.Text = "Select a use case:";
            label.Location = new Point(20, 20);
            label.AutoSize = true;
            this.Controls.Add(label);

            // Create a ComboBox for use case selection
            comboBox = new ComboBox();
            comboBox.Location = new Point(20, 50);
            comboBox.Width = 300;
            comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox.Items.Add("Telecommunications & Network Design");
            comboBox.Items.Add("VLSI Circuit Layout");
            comboBox.Items.Add("Transportation & Road Networks");
            comboBox.Items.Add("Computational Biology (Phylogenetics)");
            comboBox.Items.Add("Logistics & Supply Chain Optimization");
            comboBox.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            this.Controls.Add(comboBox);

            // Create a RichTextBox to display the use case details
            richTextBox = new RichTextBox();
            richTextBox.Location = new Point(20, 90);
            richTextBox.Size = new Size(550, 280);
            richTextBox.ReadOnly = true;
            richTextBox.Font = new Font("Arial", 10);
            this.Controls.Add(richTextBox);

            // Set the default selection
            comboBox.SelectedIndex = 0;
        }

        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedCase = comboBox.SelectedItem.ToString();
            switch (selectedCase)
            {
                case "Telecommunications & Network Design":
                    richTextBox.Text = "Example: In designing communication networks (such as fiber-optic or telephone systems), Steiner trees are used to connect multiple nodes (like cities or distribution centers) with the minimal total cable length. This minimizes costs and reduces signal loss.";
                    break;
                case "VLSI Circuit Layout":
                    richTextBox.Text = "Example: In VLSI (Very Large Scale Integration) circuit design, connecting various components on a chip with the minimal wiring length is crucial. Steiner trees help minimize delays and reduce power consumption by optimizing wire routing.";
                    break;
                case "Transportation & Road Networks":
                    richTextBox.Text = "Example: When planning road networks or pipelines, adding strategic junctions (Steiner points) can reduce the total distance required to connect multiple locations. This results in lower construction and maintenance costs.";
                    break;
                case "Computational Biology (Phylogenetics)":
                    richTextBox.Text = "Example: In phylogenetics, Steiner trees are used to reconstruct evolutionary trees by connecting species with hypothetical common ancestors. This approach aims to minimize the total genetic changes required to explain the observed data.";
                    break;
                case "Logistics & Supply Chain Optimization":
                    richTextBox.Text = "Example: In logistics, determining optimal hub locations using Steiner trees can reduce overall travel distances. This leads to more efficient distribution networks, lowering transportation costs and improving delivery times.";
                    break;
                default:
                    richTextBox.Text = "";
                    break;
            }
        }
    }
}
