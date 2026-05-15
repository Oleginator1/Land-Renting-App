using LandRentManagementApp.Config;
using LandRentManagementApp.Data;
using LandRentManagementApp.Forms;
using LandRentManagementApp.Models;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LandRentManagementApp.Forms;

public class LandListForm : FormBase
{
    private DataGridView grid = new();
    private ComboBox cmbZona = new();
    private ComboBox cmbCat = new();
    private Button btnAdd = new();
    private Button btnEdit = new();
    private Button btnDelete = new();
    private Label lblCount = new();

    public LandListForm()
    {
        InitializeComponent();
        IncarcaFiltreDropDown();
        IncarcaDate();
    }

    private void InitializeComponent()
    {
        Text = "Gestionare Terenuri";
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
            Text = "🌾 Gestionare Terenuri",
            Font = UITheme.FontTitle,
            ForeColor = UITheme.PrimaryGreen,
            AutoSize = true,
            Top = 12,
            Left = 10
        };

        btnAdd = CBtn("➕ Adaugă", 500, false);
        btnEdit = CBtn("✏ Modifică", 620, false);
        btnDelete = CBtn("🗑 Șterge", 740, true);
        var btnRefresh = CBtn("🔄 Reîncarcă", 860, false);

        toolbar.Controls.AddRange(new Control[]
            { lblTitlu, btnAdd, btnEdit, btnDelete, btnRefresh });

  
        var panelFilter = new Panel
        {
            Dock = DockStyle.Top,
            Height = 45,
            BackColor = Color.FromArgb(240, 248, 240),
            Padding = new Padding(10, 8, 10, 5)
        };

        var lblZona = new Label
        {
            Text = "Zonă:",
            AutoSize = true,
            Top = 12,
            Left = 10,
            Font = UITheme.FontNormal
        };
        cmbZona = new ComboBox
        {
            Top = 9,
            Left = 60,
            Width = 160,
            Font = UITheme.FontNormal,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        var lblCat = new Label
        {
            Text = "Categorie:",
            AutoSize = true,
            Top = 12,
            Left = 235,
            Font = UITheme.FontNormal
        };
        cmbCat = new ComboBox
        {
            Top = 9,
            Left = 310,
            Width = 160,
            Font = UITheme.FontNormal,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        var btnFilter = new Button
        {
            Text = "🔍 Filtrează",
            Top = 8,
            Left = 485,
            Width = 110,
            Height = 28
        };
        var btnClear = new Button
        {
            Text = "✖ Resetează",
            Top = 8,
            Left = 600,
            Width = 100,
            Height = 28
        };
        UITheme.ApplyButtonStyle(btnFilter);
        UITheme.ApplyButtonStyle(btnClear);

        lblCount = new Label
        {
            Text = "",
            AutoSize = true,
            Top = 14,
            Left = 720,
            Font = UITheme.FontSmall,
            ForeColor = UITheme.TextGray
        };

        panelFilter.Controls.AddRange(new Control[]
            { lblZona, cmbZona, lblCat, cmbCat, btnFilter, btnClear, lblCount });

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
        Controls.Add(panelFilter);
        Controls.Add(toolbar);

        btnAdd.Click += (s, e) => DeschideAdaugare();
        btnEdit.Click += (s, e) => DeschideEditare();
        btnDelete.Click += (s, e) => StergeSelectat();
        btnRefresh.Click += (s, e) => IncarcaDate();
        btnFilter.Click += (s, e) => AplicaFiltre();
        btnClear.Click += (s, e) => {
            cmbZona.SelectedIndex = 0; cmbCat.SelectedIndex = 0;
            IncarcaDate();
        };
    }

    private Button CBtn(string text, int left, bool danger)
    {
        var btn = new Button { Text = text, Left = left, Top = 12, Width = 110, Height = 36 };
        UITheme.ApplyButtonStyle(btn, danger);
        return btn;
    }

    private void IncarcaFiltreDropDown()
    {
        try
        {
            cmbZona.Items.Clear();
            cmbZona.Items.Add("— Toate zonele —");
            ServiceLocator.LandRepo.GetLocation().ForEach(z => cmbZona.Items.Add(z));
            cmbZona.SelectedIndex = 0;

            cmbCat.Items.Clear();
            cmbCat.Items.Add("— Toate categoriile —");
            ServiceLocator.LandRepo.GetCategory().ForEach(c => cmbCat.Items.Add(c));
            cmbCat.SelectedIndex = 0;
        }
        catch (Exception ex) { TrateazaExceptie(ex, "încărcare filtre"); }
    }

    private void IncarcaDate(List<Land>? lista = null)
    {
        try
        {
            lista ??= ServiceLocator.LandRepo.GetAll();
            grid.DataSource = lista.Select(t => new
            {
                t.Area,
                t.Category,
                t.LandLocation,
                PretArendaAnual = $"{t.AnnualRentPrice:N2} Lei",
                t.LandDescription
            }).ToList();

            if (grid.Columns.Count > 0)
            {
                grid.Columns["Suprafata"].HeaderText = "Suprafață (ha)";
                grid.Columns["Categoria"].HeaderText = "Categorie";
                grid.Columns["Zona"].HeaderText = "Zonă";
                grid.Columns["PretArendaAnual"].HeaderText = "Preț Arendă / An";
                grid.Columns["Descriere"].HeaderText = "Descriere";
            }
            grid.Tag = lista;
            lblCount.Text = $"Total: {lista.Count} terenuri";
        }
        catch (Exception ex) { TrateazaExceptie(ex, "încărcare terenuri"); }
    }

    private void AplicaFiltre()
    {
        var zona = cmbZona.SelectedItem?.ToString();
        var cat = cmbCat.SelectedItem?.ToString();
        if (zona?.StartsWith("—") == true) zona = null;
        if (cat?.StartsWith("—") == true) cat = null;

        try
        {
            var rezultate = ServiceLocator.LandRepo.Filter(zona, cat);
            IncarcaDate(rezultate);
        }
        catch (Exception ex) { TrateazaExceptie(ex, "filtrare terenuri"); }
    }

    private Land? GetTerenSelectat()
    {
        if (grid.SelectedRows.Count == 0) return null;
        return (grid.Tag as List<Land>)?[grid.SelectedRows[0].Index];
    }

    private void DeschideAdaugare()
    {
        if (new LandEditForm(null).ShowDialog(this) == DialogResult.OK)
        { IncarcaFiltreDropDown(); IncarcaDate(); }
    }

    private void DeschideEditare()
    {
        var teren = GetTerenSelectat();
        if (teren == null) { AfiseazaInfo("Selectați un teren!"); return; }
        if (new LandEditForm(teren).ShowDialog(this) == DialogResult.OK)
        { IncarcaFiltreDropDown(); IncarcaDate(); }
    }

    private void StergeSelectat()
    {
        var teren = GetTerenSelectat();
        if (teren == null) { AfiseazaInfo("Selectați un teren!"); return; }

        if (!ConfirmaActiune(
            $"Sigur doriți să ștergeți terenul:\n" +
            $"'{teren.LandName}'?\n\nToate contractele asociate vor fi șterse!",
            "Confirmare ștergere teren")) return;

        try
        {
            ServiceLocator.LandRepo.Delete(teren.LandId);
            AfiseazaInfo("Terenul a fost șters!");
            IncarcaFiltreDropDown(); IncarcaDate();
        }
        catch (Exception ex) { TrateazaExceptie(ex, "ștergere teren"); }
    }
}