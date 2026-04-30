namespace LandRentManagementApp.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        private MenuStrip menuStrip;
        private ToolStripMenuItem menuItemDate;
        private ToolStripMenuItem menuFermieri;
        private ToolStripMenuItem menuTerenuri;
        private ToolStripMenuItem menuContracte;
        private ToolStripMenuItem menuItemRapoarte;
        private ToolStripMenuItem menuRaport;
        private ToolStripMenuItem menuItemAplicatie;
        private ToolStripMenuItem menuIesire;
        private Panel panelLeft;
        private Panel panelContent;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private Label lblTitlu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            menuStrip = new MenuStrip();
            menuItemDate = new ToolStripMenuItem();
            menuFermieri = new ToolStripMenuItem();
            menuTerenuri = new ToolStripMenuItem();
            menuContracte = new ToolStripMenuItem();
            menuItemRapoarte = new ToolStripMenuItem();
            menuRaport = new ToolStripMenuItem();
            menuItemAplicatie = new ToolStripMenuItem();
            menuIesire = new ToolStripMenuItem();
            panelLeft = new Panel();
            panelContent = new Panel();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            lblTitlu = new Label();


            menuStrip.BackColor = Color.FromArgb(34, 85, 34);
            menuStrip.ForeColor = Color.White;
            menuStrip.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            menuStrip.Items.AddRange(new ToolStripItem[]
                { menuItemDate, menuItemRapoarte, menuItemAplicatie });

            menuItemDate.Text = "📋 Date";
            menuItemDate.ForeColor = Color.White;
            menuItemDate.DropDownItems.AddRange(new ToolStripItem[]
                { menuFermieri, menuTerenuri, menuContracte });

            menuFermieri.Text = "👨‍🌾 Fermieri";
            menuTerenuri.Text = "🌾 Terenuri";
            menuContracte.Text = "📄 Contracte de Arendă";
            menuFermieri.Click += menuFermieri_Click;
            menuTerenuri.Click += menuTerenuri_Click;
            menuContracte.Click += menuContracte_Click;

            menuItemRapoarte.Text = "📊 Rapoarte";
            menuItemRapoarte.ForeColor = Color.White;
            menuItemRapoarte.DropDownItems.Add(menuRaport);
            menuRaport.Text = "📈 Raport Financiar";
            menuRaport.Click += menuRaport_Click;

            menuItemAplicatie.Text = "⚙ Aplicație";
            menuItemAplicatie.ForeColor = Color.White;
            menuItemAplicatie.DropDownItems.Add(menuIesire);
            menuIesire.Text = "🚪 Ieșire";
            menuIesire.Click += menuIesire_Click;


            panelLeft.BackColor = Color.FromArgb(45, 100, 45);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Width = 200;

            lblTitlu.Text = "🌾 Arendă\nManagement";
            lblTitlu.ForeColor = Color.White;
            lblTitlu.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblTitlu.TextAlign = ContentAlignment.MiddleCenter;
            lblTitlu.Dock = DockStyle.Top;
            lblTitlu.Height = 80;
            lblTitlu.Padding = new Padding(0, 10, 0, 0);
            panelLeft.Controls.Add(lblTitlu);

         
            AddNavButton(panelLeft, "👨‍🌾 Fermieri", menuFermieri_Click, 80);
            AddNavButton(panelLeft, "🌾 Terenuri", menuTerenuri_Click, 130);
            AddNavButton(panelLeft, "📄 Contracte", menuContracte_Click, 180);
            AddNavButton(panelLeft, "📊 Raport", menuRaport_Click, 230);

     
            panelContent.Dock = DockStyle.Fill;
            panelContent.BackColor = Color.WhiteSmoke;
            panelContent.Padding = new Padding(5);

            statusStrip.Items.Add(statusLabel);
            statusLabel.Text = "Se inițializează...";


            Text = "Sistem de Evidență Arendă";
            Size = new Size(1100, 680);
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(900, 600);
            MainMenuStrip = menuStrip;

            Controls.Add(panelContent);
            Controls.Add(panelLeft);
            Controls.Add(menuStrip);
            Controls.Add(statusStrip);
        }

        private void AddNavButton(Panel parent, string text,
            EventHandler handler, int top)
        {
            var btn = new Button
            {
                Text = text,
                Top = top,
                Left = 10,
                Width = 180,
                Height = 40,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(55, 120, 55),
                Font = new Font("Segoe UI", 10f),
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(80, 150, 80);
            btn.Click += handler;
            parent.Controls.Add(btn);
        }
    }
}