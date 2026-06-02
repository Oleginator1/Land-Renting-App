using LandRentManagementApp.Config;
using LandRentManagementApp.Data;
using LandRentManagementApp.Forms;
using LandRentManagementApp.Models;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ArendaManagement.Forms;

public class ContractEditForm : FormBase
{
    private readonly Contract? _contract;
    private readonly Farmer? _fermierPreSelectat;
    private readonly bool _isEdit;

    private ComboBox cmbFermier = new();
    private ComboBox cmbTeren = new();
    private DateTimePicker dtpData = new();
    private TextBox txtAni = new();
    private Label lblSumaCalc = new();
    private Button btnSalveaza = new();
    private Button btnAnuleaza = new();

    private List<Farmer> _fermieri = new();
    private List<Land> _terenuri = new();

    public ContractEditForm(Contract? contract, Farmer? fermierPreSelectat)
    {
        _contract = contract;
        _fermierPreSelectat = fermierPreSelectat;
        _isEdit = contract != null;
        InitializeComponent();
        IncarcaDropDowns();
        if (_isEdit) CompleteazaFormular();
        else if (_fermierPreSelectat != null) PreSelecteazaFermier();
        ActualizeazaSuma();
    }

    private void InitializeComponent()
    {
        Text = _isEdit ? "✏ Modificare Contract" : "➕ Adăugare Contract";
        Size = new Size(500, 430);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        BackColor = UITheme.BackgroundLight;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 8,
            Padding = new Padding(20),
            ColumnStyles = { new ColumnStyle(SizeType.Absolute, 130f),
                             new ColumnStyle(SizeType.Percent, 100f) }
        };

        var lblTitlu = new Label
        {
            Text = _isEdit ? "✏ Modificare Contract" : "➕ Contract nou",
            Font = UITheme.FontTitle,
            ForeColor = UITheme.PrimaryGreen,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 15)
        };
        layout.Controls.Add(lblTitlu);
        layout.SetColumnSpan(lblTitlu, 2);

        cmbFermier.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTeren.DropDownStyle = ComboBoxStyle.DropDownList;
        dtpData.Format = DateTimePickerFormat.Short;
        dtpData.Value = DateTime.Today;

        AdaugaCamp(layout, "Fermier *:", cmbFermier, 1);
        AdaugaCamp(layout, "Teren *:", cmbTeren, 2);
        AdaugaCamp(layout, "Data Semnare *:", dtpData, 3);
        AdaugaCamp(layout, "Ani Achitați *:", txtAni, 4);

        lblSumaCalc = new Label
        {
            Text = "Sumă totală: —",
            Font = UITheme.FontSubtitle,
            ForeColor = UITheme.PrimaryGreen,
            AutoSize = true,
            Margin = new Padding(0, 5, 0, 5)
        };
        layout.Controls.Add(new Label());
        layout.Controls.Add(lblSumaCalc);

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

        cmbTeren.SelectedIndexChanged += (s, e) => ActualizeazaSuma();
        txtAni.TextChanged += (s, e) => ActualizeazaSuma();
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
        c.Dock = DockStyle.Fill; c.Font = UITheme.FontNormal;
        c.Margin = new Padding(0, 4, 0, 4); l.Controls.Add(c);
    }

    private void IncarcaDropDowns()
    {
        try
        {
            _fermieri = ServiceLocator.FarmerRepo.GetAll();
            _terenuri = ServiceLocator.LandRepo.GetAll();
            cmbFermier.Items.AddRange(_fermieri.Select(f => (object)f.FullName).ToArray());
            cmbTeren.Items.AddRange(_terenuri.Select(t => (object)t.LandName).ToArray());
        }
        catch (Exception ex) { TrateazaExceptie(ex, "încărcare date"); }
    }

    private void CompleteazaFormular()
    {
        var idxF = _fermieri.FindIndex(f => f.FarmerId == _contract!.FarmerId);
        var idxT = _terenuri.FindIndex(t => t.LandId == _contract!.LandId);
        if (idxF >= 0) cmbFermier.SelectedIndex = idxF;
        if (idxT >= 0) cmbTeren.SelectedIndex = idxT;
        dtpData.Value = _contract!.ContractSignDate;
        txtAni.Text = _contract.YearsPayed.ToString();
    }

    private void PreSelecteazaFermier()
    {
        var idx = _fermieri.FindIndex(f => f.FarmerId == _fermierPreSelectat!.FarmerId);
        if (idx >= 0) cmbFermier.SelectedIndex = idx;
    }

    private void ActualizeazaSuma()
    {
        try
        {
            var teren = cmbTeren.SelectedIndex >= 0
                ? _terenuri[cmbTeren.SelectedIndex] : null;
            if (teren == null || !int.TryParse(txtAni.Text, out int ani) || ani <= 0)
            { lblSumaCalc.Text = "Sumă totală: —"; return; }
            var suma = teren.AnnualRentPrice * ani;
            lblSumaCalc.Text = $"💰 Sumă totală: {suma:N2} Lei";
        }
        catch { lblSumaCalc.Text = "Sumă totală: —"; }
    }

    private void BtnSalveaza_Click(object? sender, EventArgs e)
    {
        if (cmbFermier.SelectedIndex < 0)
        { AfiseazaEroare("Selectați fermierul!"); return; }
        if (cmbTeren.SelectedIndex < 0)
        { AfiseazaEroare("Selectați terenul!"); return; }
        if (!ValidareInteger(txtAni, "Ani achitați", out int ani, 0))
            return;

        var fermier = _fermieri[cmbFermier.SelectedIndex];
        var teren = _terenuri[cmbTeren.SelectedIndex];
        int excludeId = _isEdit ? _contract!.ContractId : 0;

        try
        {
            if (ServiceLocator.ContractRepo.TerenOcupat(
               teren.LandId, excludeId))
            {
                if (!ConfirmaActiune(
                    $"Terenul '{teren.LandName}' are deja un contract activ.\n" +
                    "Doriți totuși să continuați?",
                    "Atenție — Teren ocupat"))
                    return;
            }

            var contract = new Contract
            {
                ContractId = _isEdit ? _contract!.ContractId : 0,
                FarmerId = fermier.FarmerId,
                LandId = teren.LandId,
                ContractSignDate = dtpData.Value.Date,
                YearsPayed = ani
            };

            if (_isEdit) ServiceLocator.ContractRepo.Update(contract);
            else ServiceLocator.ContractRepo.Add(contract);

            AfiseazaInfo(_isEdit
                ? "Contractul a fost actualizat!"
                : $"Contract adăugat!\nSumă totală: {teren.AnnualRentPrice * ani:N2} Lei");
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { TrateazaExceptie(ex, "salvare contract"); }
    }
}