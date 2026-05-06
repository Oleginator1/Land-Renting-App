using LandRentManagementApp.Data;

namespace ArendaManagement.Forms;

public class WelcomePanel : UserControl
{
    public WelcomePanel()
    {
        InitializeComponent();
        IncarcaStatistici();
    }

    private Label lblWelcome = new();
    private Label lblStatFermieri = new();
    private Label lblStatTerenuri = new();
    private Label lblStatContracte = new();

    private void InitializeComponent()
    {
        BackColor = Color.WhiteSmoke;
        Dock = DockStyle.Fill;

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            RowCount = 5,
            ColumnCount = 1,
            Padding = new Padding(40)
        };

        lblWelcome.Text = "🌾 Bun venit în Sistemul de Evidență Arendă";
        lblWelcome.Font = new Font("Segoe UI", 18f, FontStyle.Bold);
        lblWelcome.ForeColor = Color.FromArgb(34, 85, 34);
        lblWelcome.AutoSize = true;
        lblWelcome.Margin = new Padding(0, 0, 0, 30);

        foreach (var lbl in new[] { lblStatFermieri, lblStatTerenuri, lblStatContracte })
        {
            lbl.AutoSize = true;
            lbl.Font = new Font("Segoe UI", 13f);
            lbl.ForeColor = Color.FromArgb(60, 60, 60);
            lbl.Margin = new Padding(0, 5, 0, 5);
        }

        panel.Controls.Add(lblWelcome);
        panel.Controls.Add(lblStatFermieri);
        panel.Controls.Add(lblStatTerenuri);
        panel.Controls.Add(lblStatContracte);
        Controls.Add(panel);
    }

    private void IncarcaStatistici()
    {
        try
        {
            var nrF = ServiceLocator.FarmerRepo.GetAll().Count;
            var nrT = ServiceLocator.LandRepo.GetAll().Count;
            var nrC = ServiceLocator.ContractRepo.GetAll().Count;

            lblStatFermieri.Text = $"👨‍🌾  Fermieri înregistrați:  {nrF}";
            lblStatTerenuri.Text = $"🌾  Terenuri înregistrate: {nrT}";
            lblStatContracte.Text = $"📄  Contracte active:       {nrC}";
        }
        catch (Exception ex)
        {
            lblStatFermieri.Text = "⚠ Nu s-au putut încărca statisticile.";
            lblStatFermieri.ForeColor = Color.Red;
        }
    }
}