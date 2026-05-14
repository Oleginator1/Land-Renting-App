using LandRentManagementApp.Config;
using LandRentManagementApp.Data;
using LandRentManagementApp.Forms;
using LandRentManagementApp.Models;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace LandRentManagementApp.Forms;

public class FarmerEditForm : FormBase
{
    private readonly Farmer? _fermier;
    private readonly bool _isEdit;

    private TextBox txtNume = new();
    private TextBox txtPrenume = new();
    private TextBox txtIDNP = new();
    private TextBox txtLocalitate = new();
    private TextBox txtTelefon = new();
    private TextBox txtEmail = new();
    private Button btnSalveaza = new();
    private Button btnAnuleaza = new();

    public FarmerEditForm(Farmer? fermier)
    {
        _fermier = fermier;
        _isEdit = fermier != null;
        InitializeComponent();
        if (_isEdit) CompleteazaFormular();
    }

    private void InitializeComponent()
    {
        Text = _isEdit ? "✏ Modificare Fermier" : "➕ Adăugare Fermier";
        Size = new Size(460, 420);
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
            Text = _isEdit ? "✏ Modificare Fermier" : "➕ Adăugare Fermier",
            Font = UITheme.FontTitle,
            ForeColor = UITheme.PrimaryGreen,
            AutoSize = true,
            Margin = new Padding(0, 0, 0, 15)
        };
        layout.Controls.Add(lblTitlu);
        layout.SetColumnSpan(lblTitlu, 2);

        AdaugaCamp(layout, "Nume *:", txtNume, 1);
        AdaugaCamp(layout, "Prenume *:", txtPrenume, 2);
        AdaugaCamp(layout, "IDNP *:", txtIDNP, 3);
        AdaugaCamp(layout, "Localitate *:", txtLocalitate, 4);
        AdaugaCamp(layout, "Telefon:", txtTelefon, 5);
        AdaugaCamp(layout, "Email:", txtEmail, 6);

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

    private void AdaugaCamp(TableLayoutPanel layout,
        string eticheta, TextBox tb, int rand)
    {
        layout.Controls.Add(new Label
        {
            Text = eticheta,
            Font = UITheme.FontNormal,
            ForeColor = UITheme.TextDark,
            Anchor = AnchorStyles.Left | AnchorStyles.Top,
            Margin = new Padding(0, 8, 5, 0)
        });
        tb.Dock = DockStyle.Fill;
        tb.Font = UITheme.FontNormal;
        tb.Margin = new Padding(0, 4, 0, 4);
        layout.Controls.Add(tb);
    }

    private void CompleteazaFormular()
    {
        txtNume.Text = _fermier!.Name;
        txtPrenume.Text = _fermier.Surname;
        txtIDNP.Text = _fermier.IDNP;
        txtLocalitate.Text = _fermier.Residence;
        txtTelefon.Text = _fermier.Phone ?? "";
        txtEmail.Text = _fermier.Email ?? "";
    }

    private void BtnSalveaza_Click(object? sender, EventArgs e)
    {
        ResetareErori();
        bool valid = true;

        if (string.IsNullOrWhiteSpace(txtNume.Text))
        { SetCampEroare(txtNume, true); valid = false; }
        if (string.IsNullOrWhiteSpace(txtPrenume.Text))
        { SetCampEroare(txtPrenume, true); valid = false; }
        if (txtIDNP.Text.Trim().Length != 13 || !txtIDNP.Text.Trim().All(char.IsDigit))
        { SetCampEroare(txtIDNP, true); valid = false; }
        if (string.IsNullOrWhiteSpace(txtLocalitate.Text))
        { SetCampEroare(txtLocalitate, true); valid = false; }
        if (!ValidareEmail(txtEmail.Text))
        { SetCampEroare(txtEmail, true); valid = false; }
        if (!ValidareTelefon(txtTelefon.Text))
        { SetCampEroare(txtTelefon, true); valid = false; }

        if (!valid)
        {
            AfiseazaEroare("Verificați câmpurile marcate cu roșu!\n" +
                "• Câmpurile cu * sunt obligatorii\n" +
                "• IDNP trebuie să aibă 13 cifre\n" +
                "• Email trebuie să fie valid (ex: name@domain.com)\n" +
                "• Telefonul trebuie să aibă minim 9 cifre");
            return;
        }

        try
        {
            int excludeId = _isEdit ? _fermier!.FarmerId : 0;
            if (ServiceLocator.FarmerRepo.ExistsIdnp(txtIDNP.Text.Trim(), excludeId))
            { AfiseazaEroare("Există deja un fermier cu acest IDNP!"); txtIDNP.Focus(); return; }

            var fermier = new Farmer
            {
                FarmerId = _isEdit ? _fermier!.FarmerId : 0,
                Name = txtNume.Text.Trim(),
                Surname = txtPrenume.Text.Trim(),
                IDNP = txtIDNP.Text.Trim(),
                Residence = txtLocalitate.Text.Trim(),
                Phone = string.IsNullOrWhiteSpace(txtTelefon.Text) ? null : txtTelefon.Text.Trim(),
                Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim()
            };

            if (_isEdit)
                ServiceLocator.FarmerRepo.Update(fermier);
            else
                ServiceLocator.FarmerRepo.Add(fermier);

            AfiseazaInfo(_isEdit ? "Fermierul a fost actualizat!" : "Fermierul a fost adăugat!");
            DialogResult = DialogResult.OK;
            Close();
        }
        catch (Exception ex) { TrateazaExceptie(ex, "salvare fermier"); }
    }

    private bool ValidareEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return true; 
        return email.Contains('@') && email.Contains('.') && email.Length > 5;
    }

    private bool ValidareTelefon(string telefon)
    {
        if (string.IsNullOrWhiteSpace(telefon)) return true; 
        var cifre = new string(telefon.Where(char.IsDigit).ToArray());
        return cifre.Length >= 9;
    }

    private void SetCampEroare(TextBox tb, bool eroare)
    {
        tb.BackColor = eroare
            ? Color.FromArgb(255, 220, 220)
            : Color.White;
    }

    private void ResetareErori()
    {
        foreach (var tb in new[]{txtNume,txtPrenume,txtIDNP,
                              txtLocalitate,txtTelefon,txtEmail})
            SetCampEroare(tb, false);
    }
}