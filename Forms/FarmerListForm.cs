using LandRentManagementApp.Config;
using LandRentManagementApp.Data;
using LandRentManagementApp.Forms;
using LandRentManagementApp.Models;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LandRentManagementApp.Forms;

public class FarmerListForm : FormBase
{
    private DataGridView grid;
    private TextBox txtSearch;
    private ComboBox cmbLocalitate;
    private Button btnAdd;
    private Button btnEdit;
    private Button btnDelete;
    private Button btnRefresh;
    private Label lblCount;
    private System.Windows.Forms.Timer searchTimer;

    public FarmerListForm()
    {
        grid = new DataGridView();
        txtSearch = new TextBox();
        cmbLocalitate = new ComboBox();
        btnAdd = new Button();
        btnEdit = new Button();
        btnDelete = new Button();
        btnRefresh = new Button();
        lblCount = new Label();
        searchTimer = new System.Windows.Forms.Timer();

        InitializeComponent();
        IncarcaLocalitati();
        IncarcaDate();
    }

    private void InitializeComponent()
    {
        Text = "Gestionare Fermieri";
        Dock = DockStyle.Fill;
        BackColor = UITheme.BackgroundLight;


        var toolbar = new Panel();
        toolbar.Dock = DockStyle.Top;
        toolbar.Height = 60;
        toolbar.BackColor = UITheme.BackgroundWhite;
        toolbar.Padding = new Padding(10, 10, 10, 5);

        var lblTitlu = new Label();
        lblTitlu.Text = "👨‍🌾 Gestionare Fermieri";
        lblTitlu.Font = UITheme.FontTitle;
        lblTitlu.ForeColor = UITheme.PrimaryGreen;
        lblTitlu.AutoSize = true;
        lblTitlu.Top = 12;
        lblTitlu.Left = 10;

        btnAdd.Text = "➕ Adaugă";
        btnAdd.Left = 500;
        btnAdd.Top = 12;
        btnAdd.Width = 110;
        btnAdd.Height = 36;
        UITheme.ApplyButtonStyle(btnAdd, false);

        btnEdit.Text = "✏ Modifică";
        btnEdit.Left = 620;
        btnEdit.Top = 12;
        btnEdit.Width = 110;
        btnEdit.Height = 36;
        UITheme.ApplyButtonStyle(btnEdit, false);

        btnDelete.Text = "🗑 Șterge";
        btnDelete.Left = 740;
        btnDelete.Top = 12;
        btnDelete.Width = 110;
        btnDelete.Height = 36;
        UITheme.ApplyButtonStyle(btnDelete, true);

        btnRefresh.Text = "🔄 Reîncarcă";
        btnRefresh.Left = 860;
        btnRefresh.Top = 12;
        btnRefresh.Width = 110;
        btnRefresh.Height = 36;
        UITheme.ApplyButtonStyle(btnRefresh, false);

        toolbar.Controls.Add(lblTitlu);
        toolbar.Controls.Add(btnAdd);
        toolbar.Controls.Add(btnEdit);
        toolbar.Controls.Add(btnDelete);
        toolbar.Controls.Add(btnRefresh);


        var panelSearch = new Panel();
        panelSearch.Dock = DockStyle.Top;
        panelSearch.Height = 45;
        panelSearch.BackColor = Color.FromArgb(240, 248, 240);
        panelSearch.Padding = new Padding(10, 8, 10, 5);

        var lblSearch = new Label();
        lblSearch.Text = "🔍 Caută:";
        lblSearch.AutoSize = true;
        lblSearch.Top = 12;
        lblSearch.Left = 10;
        lblSearch.Font = UITheme.FontNormal;

        txtSearch.Top = 9;
        txtSearch.Left = 80;
        txtSearch.Width = 250;
        txtSearch.Font = UITheme.FontNormal;

        var btnClear = new Button();
        btnClear.Text = "✖";
        btnClear.Top = 8;
        btnClear.Left = 338;
        btnClear.Width = 30;
        btnClear.Height = 28;
        UITheme.ApplyButtonStyle(btnClear, false);

        var lblLocalitate = new Label();
        lblLocalitate.Text = "Localitate:";
        lblLocalitate.AutoSize = true;
        lblLocalitate.Top = 12;
        lblLocalitate.Left = 385;
        lblLocalitate.Font = UITheme.FontNormal;

        cmbLocalitate.Top = 9;
        cmbLocalitate.Left = 465;
        cmbLocalitate.Width = 200;
        cmbLocalitate.Font = UITheme.FontNormal;
        cmbLocalitate.DropDownStyle = ComboBoxStyle.DropDownList;

        var btnResetFiltre = new Button();
        btnResetFiltre.Text = "✖ Resetează";
        btnResetFiltre.Top = 8;
        btnResetFiltre.Left = 675;
        btnResetFiltre.Width = 110;
        btnResetFiltre.Height = 28;
        UITheme.ApplyButtonStyle(btnResetFiltre, false);

        lblCount.Text = "";
        lblCount.AutoSize = true;
        lblCount.Top = 14;
        lblCount.Left = 800;
        lblCount.Font = UITheme.FontSmall;
        lblCount.ForeColor = UITheme.TextGray;

        panelSearch.Controls.Add(lblSearch);
        panelSearch.Controls.Add(txtSearch);
        panelSearch.Controls.Add(btnClear);
        panelSearch.Controls.Add(lblLocalitate);
        panelSearch.Controls.Add(cmbLocalitate);
        panelSearch.Controls.Add(btnResetFiltre);
        panelSearch.Controls.Add(lblCount);


        grid.Dock = DockStyle.Fill;
        grid.ReadOnly = true;
        grid.MultiSelect = false;
        grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        grid.AllowUserToAddRows = false;
        grid.AllowUserToDeleteRows = false;
        UITheme.ApplyGridStyle(grid);

        Controls.Add(grid);
        Controls.Add(panelSearch);
        Controls.Add(toolbar);

        searchTimer.Interval = 400;
        searchTimer.Tick += (s, e) =>
        {
            searchTimer.Stop();
            AplicaFiltreCombinate();
        };

        btnAdd.Click += (s, e) => DeschideAdaugare();
        btnEdit.Click += (s, e) => DeschideEditare();
        btnDelete.Click += (s, e) => StergeSelectat();
        btnRefresh.Click += (s, e) =>
        {
            txtSearch.Clear();
            cmbLocalitate.SelectedIndex = 0;
            IncarcaLocalitati();
            IncarcaDate();
        };

        btnClear.Click += (s, e) =>
        {
            txtSearch.Clear();
            AplicaFiltreCombinate();
        };

        btnResetFiltre.Click += (s, e) =>
        {
            txtSearch.Clear();
            cmbLocalitate.SelectedIndex = 0;
            IncarcaDate();
        };


        txtSearch.TextChanged += (s, e) =>
        {
            searchTimer.Stop();
            searchTimer.Start();
        };


        cmbLocalitate.SelectedIndexChanged += (s, e) =>
            AplicaFiltreCombinate();

        grid.CellDoubleClick += (s, e) =>
        {
            if (e.RowIndex >= 0) DeschideEditare();
        };
    }

    private void IncarcaLocalitati()
    {
        try
        {
            var toti = ServiceLocator.FarmerRepo.GetAll();
            var localitati = toti
                .Select(f => f.Residence)
                .Distinct()
                .OrderBy(l => l)
                .ToList();

            cmbLocalitate.Items.Clear();
            cmbLocalitate.Items.Add("— Toate localitățile —");
            foreach (var loc in localitati)
                cmbLocalitate.Items.Add(loc);

            cmbLocalitate.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            TrateazaExceptie(ex, "încărcare localități");
        }
    }

    private void IncarcaDate(List<Farmer>? lista = null)
    {
        try
        {
            lista ??= ServiceLocator.FarmerRepo.GetAll();

            grid.DataSource = lista.Select(f => new
            {
                f.Name,
                f.Surname,
                f.IDNP,
                f.Residence,
                Telefon = f.Phone ?? "—",
                Email = f.Email ?? "—"
            }).ToList();

            if (grid.Columns.Count > 0)
            {
                grid.Columns["Name"].HeaderText = "Nume";
                grid.Columns["Surname"].HeaderText = "Prenume";
                grid.Columns["IDNP"].HeaderText = "IDNP";
                grid.Columns["Residence"].HeaderText = "Localitate";
                grid.Columns["Telefon"].HeaderText = "Telefon";
                grid.Columns["Email"].HeaderText = "Email";
            }

            grid.Tag = lista;
            lblCount.Text = $"Total: {lista.Count} fermieri";
        }
        catch (Exception ex)
        {
            TrateazaExceptie(ex, "încărcare fermieri");
        }
    }

    private void AplicaFiltreCombinate()
    {
        try
        {
            var toti = ServiceLocator.FarmerRepo.GetAll();


            string termen = txtSearch.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(termen))
            {
                toti = toti.Where(f =>
                    f.Name.ToLower().Contains(termen) ||
                    f.Surname.ToLower().Contains(termen) ||
                    f.IDNP.Contains(termen) ||
                    f.Residence.ToLower().Contains(termen) ||
                    (f.Email != null && f.Email.ToLower().Contains(termen)) ||
                    (f.Phone != null && f.Phone.Contains(termen))
                ).ToList();
            }

            // Filtru după localitate
            string? locSel = cmbLocalitate.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(locSel) && !locSel.StartsWith("—"))
            {
                toti = toti.Where(f => f.Residence == locSel).ToList();
            }

            IncarcaDate(toti);
        }
        catch (Exception ex)
        {
            TrateazaExceptie(ex, "filtrare fermieri");
        }
    }

    private Farmer? GetFermierSelectat()
    {
        if (grid.SelectedRows.Count == 0) return null;
        var lista = grid.Tag as List<Farmer>;
        if (lista == null || grid.SelectedRows[0].Index >= lista.Count)
            return null;
        return lista[grid.SelectedRows[0].Index];
    }

    private void DeschideAdaugare()
    {
        var form = new FarmerEditForm(null);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            IncarcaLocalitati();
            IncarcaDate();
        }
    }

    private void DeschideEditare()
    {
        var fermier = GetFermierSelectat();
        if (fermier == null)
        {
            AfiseazaInfo("Selectați un fermier din listă!");
            return;
        }

        var form = new FarmerEditForm(fermier);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            IncarcaLocalitati();
            IncarcaDate();
        }
    }

    private void StergeSelectat()
    {
        var fermier = GetFermierSelectat();
        if (fermier == null)
        {
            AfiseazaInfo("Selectați un fermier din listă!");
            return;
        }


        var contracte = ServiceLocator.ContractRepo.GetByFarmer(fermier.FarmerId);
        string mesaj = $"Sigur doriți să ștergeți fermierul\n'{fermier.FullName}'?";

        if (contracte.Count > 0)
            mesaj += $"\n\n⚠ ATENȚIE: Fermierul are {contracte.Count} contract(e) activ(e)!" +
                     "\nAcestea vor fi șterse automat!";

        if (!ConfirmaActiune(mesaj, "Confirmare ștergere fermier"))
            return;

        try
        {
            ServiceLocator.FarmerRepo.Delete(fermier.FarmerId);
            AfiseazaInfo("Fermierul a fost șters cu succes!");
            IncarcaLocalitati();
            IncarcaDate();
        }
        catch (Exception ex)
        {
            TrateazaExceptie(ex, "ștergere fermier");
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            searchTimer?.Stop();
            searchTimer?.Dispose();
        }
        base.Dispose(disposing);
    }
}