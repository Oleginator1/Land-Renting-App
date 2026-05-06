using LandRentManagementApp.Config;
using LandRentManagementApp.Data;
using LandRentManagementApp.Models;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LandRentManagementApp.Forms;

public class FarmerListForm : FormBase
{
    private DataGridView grid = new();
    private TextBox txtSearch = new();
    private Button btnAdd = new();
    private Button btnEdit = new();
    private Button btnDelete = new();
    private Button btnRefresh = new();
    private Label lblCount = new();

    public FarmerListForm()
    {
        InitializeComponent();
        IncarcaDate();
    }

    private void InitializeComponent()
    {
        Text = "Gestionare Fermieri";
        Dock = DockStyle.Fill;
        BackColor = UITheme.BackgroundLight;


        var toolbar = new Panel
        {
            Dock = DockStyle.Top,
            Height = 60,
            BackColor = UITheme.BackgroundWhite,
            Padding = new Padding(10, 10, 10, 5)
        };

        var lblTitlu = new Label
        {
            Text = "👨‍🌾 Gestionare Fermieri",
            Font = UITheme.FontTitle,
            ForeColor = UITheme.PrimaryGreen,
            AutoSize = true,
            Top = 12,
            Left = 10
        };

        // Butoane
        btnAdd = CreazaButon("➕ Adaugă", false);
        btnEdit = CreazaButon("✏ Modifică", false);
        btnDelete = CreazaButon("🗑 Șterge", true);
        btnRefresh = CreazaButon("🔄 Reîncarcă", false);

        btnAdd.Left = 500; btnEdit.Left = 620;
        btnDelete.Left = 740; btnRefresh.Left = 860;
        foreach (var btn in new[] { btnAdd, btnEdit, btnDelete, btnRefresh })
        { btn.Top = 12; btn.Width = 110; toolbar.Controls.Add(btn); }

        toolbar.Controls.Add(lblTitlu);


        var panelSearch = new Panel
        {
            Dock = DockStyle.Top,
            Height = 45,
            BackColor = Color.FromArgb(240, 248, 240),
            Padding = new Padding(10, 8, 10, 5)
        };
        var lblSearch = new Label
        {
            Text = "🔍 Caută:",
            AutoSize = true,
            Top = 12,
            Left = 10,
            Font = UITheme.FontNormal
        };
        txtSearch = new TextBox
        { Top = 9, Left = 80, Width = 300, Font = UITheme.FontNormal };
        var btnSearch = new Button
        { Text = "Caută", Top = 8, Left = 390, Width = 80, Height = 28 };
        var btnClear = new Button
        { Text = "✖ Șterge", Top = 8, Left = 475, Width = 80, Height = 28 };
        UITheme.ApplyButtonStyle(btnSearch);
        UITheme.ApplyButtonStyle(btnClear);

        lblCount = new Label
        {
            Text = "",
            AutoSize = true,
            Top = 14,
            Left = 570,
            Font = UITheme.FontSmall,
            ForeColor = UITheme.TextGray
        };

        panelSearch.Controls.AddRange(new Control[]
            { lblSearch, txtSearch, btnSearch, btnClear, lblCount });


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
        Controls.Add(panelSearch);
        Controls.Add(toolbar);

        // Events
        btnAdd.Click += (s, e) => DeschideAdaugare();
        btnEdit.Click += (s, e) => DeschideEditare();
        btnDelete.Click += (s, e) => StergeSelectat();
        btnRefresh.Click += (s, e) => IncarcaDate();
        btnSearch.Click += (s, e) => CautaFermieri();
        btnClear.Click += (s, e) => { txtSearch.Clear(); IncarcaDate(); };
        txtSearch.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) CautaFermieri(); };
    }

    private Button CreazaButon(string text, bool isDanger)
    {
        var btn = new Button { Text = text, Height = 36 };
        UITheme.ApplyButtonStyle(btn, isDanger);
        return btn;
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
                f.Phone,
                f.Email
            }).ToList();


            if (grid.Columns.Count > 0)
            {
                grid.Columns["Nume"].HeaderText = "Nume";
                grid.Columns["Prenume"].HeaderText = "Prenume";
                grid.Columns["CNP"].HeaderText = "CNP";
                grid.Columns["Localitate"].HeaderText = "Localitate";
                grid.Columns["Telefon"].HeaderText = "Telefon";
                grid.Columns["Email"].HeaderText = "Email";
            }

            // Stocăm ID-urile ascuns
            grid.Tag = lista;
            lblCount.Text = $"Total: {lista.Count} fermieri";
        }
        catch (Exception ex) { TrateazaExceptie(ex, "încărcare fermieri"); }
    }

    private Farmer? GetFermierSelectat()
    {
        if (grid.SelectedRows.Count == 0) return null;
        var lista = grid.Tag as List<Farmer>;
        return lista?[grid.SelectedRows[0].Index];
    }

    private void DeschideAdaugare()
    {
        var form = new FarmerEditForm(null);
        if (form.ShowDialog(this) == DialogResult.OK)
            IncarcaDate();
    }

    private void DeschideEditare()
    {
        var fermier = GetFermierSelectat();
        if (fermier == null)
        { AfiseazaInfo("Selectați un fermier din listă!"); return; }

        var form = new FarmerEditForm(fermier);
        if (form.ShowDialog(this) == DialogResult.OK)
            IncarcaDate();
    }

    private void StergeSelectat()
    {
        var fermier = GetFermierSelectat();
        if (fermier == null)
        { AfiseazaInfo("Selectați un fermier din listă!"); return; }

        if (!ConfirmaActiune(
            $"Sigur doriți să ștergeți fermierul\n" +
            $"'{fermier.FullName}'?\n\n" +
            "Toate contractele asociate vor fi șterse!",
            "Confirmare ștergere"))
            return;

        try
        {
            ServiceLocator.FarmerRepo.Delete(fermier.FarmerId);
            AfiseazaInfo("Fermierul a fost șters cu succes!");
            IncarcaDate();
        }
        catch (Exception ex) { TrateazaExceptie(ex, "ștergere fermier"); }
    }

    private void CautaFermieri()
    {
        var termen = txtSearch.Text.Trim();
        if (string.IsNullOrEmpty(termen)) { IncarcaDate(); return; }
        try
        {
            var rezultate = ServiceLocator.FarmerRepo.Search(termen);
            IncarcaDate(rezultate);
        }
        catch (Exception ex) { TrateazaExceptie(ex, "căutare"); }
    }
}