using LandRentManagementApp.Config;
using LandRentManagementApp.Data;
using LandRentManagementApp.Forms;
using LandRentManagementApp.Models;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LandRentManagementApp.Forms;

public class LandEditForm : FormBase
{
    private readonly Land? _teren;
    private readonly bool _isEdit;

    private TextBox txtSuprafata = new();
    private ComboBox cmbCategorie = new();
    private ComboBox cmbZona = new();
    private TextBox txtPret = new();
    private TextBox txtDescriere = new();
    private Button btnSalveaza = new();
    private Button btnAnuleaza = new();

    private static readonly string[] Categorii =
        { "Arabil", "Pășune", "Vie", "Livadă", "Pădure", "Fânețe", "Grădini" };
    private static readonly string[] Zone =
        { "Zona Nord", "Zona Sud", "Zona Est", "Zona Vest", "Zona Centrală" };

    public LandEditForm(Land? teren)
    {
        _teren = teren;
        _isEdit = teren != null;
        InitializeComponent();
        if (_isEdit) CompleteazaFormular();
    }

    private void InitializeComponent()
    {
        Text = _isEdit ? "✏ Modificare Teren" : "➕ Adăugare Teren";
        Size = new Size(480, 400);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        BackColor = UITheme.BackgroundLight;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 7,
            Padding = new Padding(20),
            ColumnStyles = { new ColumnStyle(SizeType.Absolute, 140f),
                             new ColumnStyle(SizeType.Percent, 100f) }
        };

        var lblTitlu = new Label
        {
            Text = _isEdit ? "✏ Modificare Teren" : "➕ Adăugare Teren",
            Font = UITheme.FontTitle,
            ForeColor = UITheme.PrimaryGreen,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 15)
        };
        layout.Controls.Add(lblTitlu);
        layout.SetColumnSpan(lblTitlu, 2);

        cmbCategorie.Items.AddRange(Categorii);
        cmbCategorie.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbZona.Items.AddRange(Zone);
        cmbZona.DropDownStyle = ComboBoxStyle.DropDownList;

        txtDescriere.Multiline = true;
        txtDescriere.Height = 60;

        AdaugaCamp(layout, "Suprafață (ha) *:", txtSuprafata, 1);
        AdaugaCombo(layout, "Categorie *:", cmbCategorie, 2);
        AdaugaCombo(layout, "Zonă *:", cmbZona, 3);
        AdaugaCamp(layout, "Preț Arendă/An *:", txtPret, 4);
        AdaugaCamp(layout, "Descriere:", txtDescriere, 5);

        var panelBtns = new FlowLayoutPanel
        {
            FlowDirection = FlowDirection.RightToLeft,
            Dock = DockStyle.Fill,
            Padding = new Padding(0, 10, 0, 0)
        };
        btnSalveaza = new Button { Text = "💾 Salvează", Width = 120, Height = 36 };
        btnAnuleaza = new Button { Text = "✖ Anulează", Width = 100, Height = 36 };
        UITheme.ApplyButtonStyle(btnSalveaza);
        UITheme.ApplyButtonStyle(btnAnuleaza, true);
        panelBtns.Controls.Add(btnAnuleaza);
        panelBtns.Controls.Add(btnSalveaza);
        layout.Controls.Add(panelBtns);
        layout.SetColumnSpan(panelBtns, 2);

        Controls.Add(layout);
        btnSalveaza.Click += BtnSalveaza_Click;
        btnAnuleaza.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
    }

    private void AdaugaCamp(TableLayoutPanel l, string et, Control c, int r)
    {
        l.Controls.Add(new Label
        {
            Text = et,
            Font = UITheme.FontNormal,
            ForeColor = UITheme.TextDark,
            AutoSize = true,
            Margin = new Padding(0, 8, 5, 0)
        });
        c.Dock = DockStyle.Fill; ((Control)c).Font = UITheme.FontNormal;
        c.Margin = new Padding(0, 4, 0, 4); l.Controls.Add(c);
    }

    private void AdaugaCombo(TableLayoutPanel l, string et, ComboBox c, int r)
        => AdaugaCamp(l, et, c, r);

    private void CompleteazaFormular()
    {
        txtSuprafata.Text = _teren!.Area.ToString("F2");
        cmbCategorie.SelectedItem = _teren.Category;
        cmbZona.SelectedItem = _teren.LandLocation;
        txtPret.Text = _teren.AnnualRentPrice.ToString("F2");
        txtDescriere.Text = _teren.LandDescription ?? "";
    }

    private void BtnSalveaza_Click(object? sender, EventArgs e)
    {
        if (!ValidareDecimal(txtSuprafata, "Suprafața", out decimal sup)) return;
        if (cmbCategorie.SelectedItem == null)
        { AfiseazaEroare("Selectați categoria terenului!"); return; }
        if (cmbZona.SelectedItem == null)
        { AfiseazaEroare("Selectați zona terenului!"); return; }
        if (!ValidareDecimal(txtPret, "Prețul arendei", out decimal pret)) return;

        try
        {
            var teren = new Land
            {
                LandId = _isEdit ? _teren!.LandId : 0,
                Area = sup,
                Category = cmbCategorie.SelectedItem.ToString()!,
                LandLocation = cmbZona.SelectedItem.ToString()!,
                AnnualRentPrice = pret,
                LandDescription = string.IsNullOrWhiteSpace(txtDescriere.Text)
                                  ? null : txtDescriere.Text.Trim()
            };

            if (_isEdit) ServiceLocator.LandRepo.Update(teren);
            else ServiceLocator.LandRepo.Add(teren);

            AfiseazaInfo(_isEdit ? "Terenul a fost actualizat!" : "Terenul a fost adăugat!");
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { TrateazaExceptie(ex, "salvare teren"); }
    }
}