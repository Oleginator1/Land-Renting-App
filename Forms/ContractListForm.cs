using LandRentManagementApp.Config;
using LandRentManagementApp.Data;
using LandRentManagementApp.Forms;
using LandRentManagementApp.Models;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ArendaManagement.Forms;

public class ContractListForm : FormBase
{
    private readonly Farmer? _fermierFilter;
    private DataGridView grid = new();
    private Button btnAdd = new();
    private Button btnEdit = new();
    private Button btnDelete = new();
    private Label lblCount = new();

    
    public ContractListForm(Farmer? fermierFilter = null)
    {
        _fermierFilter = fermierFilter;
        InitializeComponent();
        IncarcaDate();
    }

    private void InitializeComponent()
    {
        string titlu = _fermierFilter != null
            ? $"📄 Contracte — {_fermierFilter.FullName}"
            : "📄 Gestionare Contracte de Arendă";

        Text = titlu;
        BackColor = UITheme.BackgroundLight;

        if (_fermierFilter != null)
        {
            // Mod dialog
            Size = new Size(900, 580);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.Sizable;
        }
        else
        {
            Dock = DockStyle.Fill;
        }

        // Toolbar
        var toolbar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = UITheme.BackgroundWhite,
            Padding = new Padding(10, 10, 10, 5)
        };

        var lblTitlu = new Label
        {
            Text = titlu,
            Font = UITheme.FontTitle,
            ForeColor = UITheme.PrimaryGreen,
            AutoSize = true,
            Top = 12,
            Left = 10
        };

        btnAdd = CBtn("➕ Adaugă", 500, false);
        btnEdit = CBtn("✏ Modifică", 620, false);
        btnDelete = CBtn("🗑 Anulează", 740, true);
        var btnRefresh = CBtn("🔄 Reîncarcă", 860, false);

        lblCount = new Label
        {
            Text = "",
            AutoSize = true,
            Top = 20,
            Left = 990,
            Font = UITheme.FontSmall,
            ForeColor = UITheme.TextGray
        };

        toolbar.Controls.AddRange(new Control[]
            { lblTitlu, btnAdd, btnEdit, btnDelete, btnRefresh, lblCount });

        // Grid
        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        UITheme.ApplyGridStyle(grid);
        grid.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) DeschideEditare(); };

        Controls.Add(grid);
        Controls.Add(toolbar);

        btnAdd.Click += (s, e) => DeschideAdaugare();
        btnEdit.Click += (s, e) => DeschideEditare();
        btnDelete.Click += (s, e) => AnuleazaSelectat();
        btnRefresh.Click += (s, e) => IncarcaDate();
    }

    private Button CBtn(string t, int left, bool d)
    {
        var btn = new Button { Text = t, Left = left, Top = 12, Width = 110, Height = 36 };
        UITheme.ApplyButtonStyle(btn, d);
        return btn;
    }

    private void IncarcaDate()
    {
        try
        {
            List<Contract> lista = _fermierFilter != null
                ? ServiceLocator.ContractRepo.GetByFarmer(_fermierFilter.FarmerId)
                : ServiceLocator.ContractRepo.GetAll();

            grid.DataSource = lista.Select(c => new
            {
                Fermier = c.NumeCompletFermier,
                Teren = $"{c.LandCategory} — {c.LandLocation}",
                DataSemnare = c.ContractSignDate.ToString("dd.MM.yyyy"),
                AniAchitati = c.YearsPayed,
                PretAnual = $"{c.AnnualRentPrice:N2} Lei",
                SumaTotala = $"{c.TotalSum:N2} Lei"
            }).ToList();

            if (grid.Columns.Count > 0)
            {
                grid.Columns["Fermier"].HeaderText = "Fermier";
                grid.Columns["Teren"].HeaderText = "Teren";
                grid.Columns["DataSemnare"].HeaderText = "Data Semnare";
                grid.Columns["AniAchitati"].HeaderText = "Ani Achitați";
                grid.Columns["PretAnual"].HeaderText = "Preț/An";
                grid.Columns["SumaTotala"].HeaderText = "Sumă Totală";
            }

            grid.Tag = lista;
            lblCount.Text = $"Total: {lista.Count}";
        }
        catch (Exception ex) { TrateazaExceptie(ex, "încărcare contracte"); }
    }

    private Contract? GetContractSelectat()
    {
        if (grid.SelectedRows.Count == 0) return null;
        return (grid.Tag as List<Contract>)?[grid.SelectedRows[0].Index];
    }

    private void DeschideAdaugare()
    {
        var form = new ContractEditForm(null, _fermierFilter);
        if (form.ShowDialog(this) == DialogResult.OK) IncarcaDate();
    }

    private void DeschideEditare()
    {
        var contract = GetContractSelectat();
        if (contract == null) { AfiseazaInfo("Selectați un contract!"); return; }
        if (new ContractEditForm(contract, null).ShowDialog(this) == DialogResult.OK)
            IncarcaDate();
    }

    private void AnuleazaSelectat()
    {
        var contract = GetContractSelectat();
        if (contract == null) { AfiseazaInfo("Selectați un contract!"); return; }

        if (!ConfirmaActiune(
            $"Sigur doriți să anulați contractul:\n" +
            $"Fermier: {contract.NumeCompletFermier}\n" +
            $"Teren: {contract.LandCategory} — {contract.LandCategory}\n" +
            $"Data: {contract.ContractSignDate:dd.MM.yyyy}\n\n" +
            "Această acțiune nu poate fi anulată!",
            "Confirmare anulare contract"))
            return;

        try
        {
            ServiceLocator.ContractRepo.Delete(contract.ContractId);
            AfiseazaInfo("Contractul a fost anulat cu succes!");
            IncarcaDate();
        }
        catch (Exception ex) { TrateazaExceptie(ex, "anulare contract"); }
    }
}