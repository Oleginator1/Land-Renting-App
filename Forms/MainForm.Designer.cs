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
            lblTitlu = new Label();
            panelContent = new Panel();
            statusStrip = new StatusStrip();
            statusLabel = new ToolStripStatusLabel();
            menuStrip.SuspendLayout();
            panelLeft.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.BackColor = Color.FromArgb(34, 85, 34);
            menuStrip.Font = new Font("Segoe UI", 10F);
            menuStrip.ForeColor = Color.White;
            menuStrip.Items.AddRange(new ToolStripItem[] { menuItemDate, menuItemRapoarte, menuItemAplicatie });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1084, 27);
            menuStrip.TabIndex = 2;
            // 
            // menuItemDate
            // 
            menuItemDate.DropDownItems.AddRange(new ToolStripItem[] { menuFermieri, menuTerenuri, menuContracte });
            menuItemDate.ForeColor = Color.White;
            menuItemDate.Name = "menuItemDate";
            menuItemDate.Size = new Size(70, 23);
            menuItemDate.Text = "📋 Date";
            // 
            // menuFermieri
            // 
            menuFermieri.Name = "menuFermieri";
            menuFermieri.Size = new Size(225, 24);
            menuFermieri.Text = "👨‍🌾 Fermieri";
            menuFermieri.Click += menuFermieri_Click;
            // 
            // menuTerenuri
            // 
            menuTerenuri.Name = "menuTerenuri";
            menuTerenuri.Size = new Size(225, 24);
            menuTerenuri.Text = "🌾 Terenuri";
            menuTerenuri.Click += menuTerenuri_Click;
            // 
            // menuContracte
            // 
            menuContracte.Name = "menuContracte";
            menuContracte.Size = new Size(225, 24);
            menuContracte.Text = "📄 Contracte de Arendă";
            menuContracte.Click += menuContracte_Click;
            // 
            // menuItemRapoarte
            // 
            menuItemRapoarte.DropDownItems.AddRange(new ToolStripItem[] { menuRaport });
            menuItemRapoarte.ForeColor = Color.White;
            menuItemRapoarte.Name = "menuItemRapoarte";
            menuItemRapoarte.Size = new Size(98, 23);
            menuItemRapoarte.Text = "📊 Rapoarte";
            // 
            // menuRaport
            // 
            menuRaport.Name = "menuRaport";
            menuRaport.Size = new Size(199, 24);
            menuRaport.Text = "📈 Raport Financiar";
            menuRaport.Click += menuRaport_Click;
            // 
            // menuItemAplicatie
            // 
            menuItemAplicatie.DropDownItems.AddRange(new ToolStripItem[] { menuIesire });
            menuItemAplicatie.ForeColor = Color.White;
            menuItemAplicatie.Name = "menuItemAplicatie";
            menuItemAplicatie.Size = new Size(95, 23);
            menuItemAplicatie.Text = "⚙ Aplicație";
            // 
            // menuIesire
            // 
            menuIesire.Name = "menuIesire";
            menuIesire.Size = new Size(129, 24);
            menuIesire.Text = "🚪 Ieșire";
            menuIesire.Click += menuIesire_Click;
            // 
            // panelLeft
            // 
            panelLeft.BackColor = Color.FromArgb(45, 100, 45);
            panelLeft.Controls.Add(lblTitlu);
            panelLeft.Dock = DockStyle.Left;
            panelLeft.Location = new Point(0, 27);
            panelLeft.Name = "panelLeft";
            panelLeft.Size = new Size(200, 592);
            panelLeft.TabIndex = 1;
            // 
            // lblTitlu
            // 
            lblTitlu.Dock = DockStyle.Top;
            lblTitlu.Font = new Font("Segoe UI", 13F, FontStyle.Bold);
            lblTitlu.ForeColor = Color.White;
            lblTitlu.Location = new Point(0, 0);
            lblTitlu.Name = "lblTitlu";
            lblTitlu.Padding = new Padding(0, 10, 0, 0);
            lblTitlu.Size = new Size(200, 80);
            lblTitlu.TabIndex = 0;
            lblTitlu.Text = "🌾 Arendă\nManagement";
            lblTitlu.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panelContent
            // 
            panelContent.BackColor = Color.WhiteSmoke;
            panelContent.Dock = DockStyle.Fill;
            panelContent.Location = new Point(200, 27);
            panelContent.Name = "panelContent";
            panelContent.Padding = new Padding(5);
            panelContent.Size = new Size(884, 592);
            panelContent.TabIndex = 0;
            panelContent.Paint += panelContent_Paint;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
            statusStrip.Location = new Point(0, 619);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1084, 22);
            statusStrip.TabIndex = 3;
            // 
            // statusLabel
            // 
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(91, 17);
            statusLabel.Text = "Se inițializează...";
            // 
            // MainForm
            // 
            ClientSize = new Size(1084, 641);
            Controls.Add(panelContent);
            Controls.Add(panelLeft);
            Controls.Add(menuStrip);
            Controls.Add(statusStrip);
            MainMenuStrip = menuStrip;
            MinimumSize = new Size(900, 600);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Sistem de Evidență Arendă";
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            panelLeft.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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