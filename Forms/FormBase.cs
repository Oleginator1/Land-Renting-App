namespace LandRentManagementApp.Forms;

public partial class FormBase : Form
{
    protected void AfiseazaEroare(string mesaj, string titlu = "Eroare")
    {
        MessageBox.Show(mesaj, titlu,
            MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    protected void AfiseazaInfo(string mesaj, string titlu = "Informație")
    {
        MessageBox.Show(mesaj, titlu,
            MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    protected bool ConfirmaActiune(string mesaj, string titlu = "Confirmare")
    {
        return MessageBox.Show(mesaj, titlu,
            MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            == DialogResult.Yes;
    }

    protected bool ValidareCampObligatoriu(TextBox tb, string numeCamp)
    {
        if (string.IsNullOrWhiteSpace(tb.Text))
        {
            AfiseazaEroare($"Câmpul '{numeCamp}' este obligatoriu!");
            tb.Focus();
            return false;
        }
        return true;
    }

    protected bool ValidareDecimal(TextBox tb, string numeCamp,
        out decimal valoare, decimal minim = 0)
    {
        valoare = 0;
        if (!decimal.TryParse(tb.Text.Replace(',', '.'),
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out valoare))
        {
            AfiseazaEroare($"'{numeCamp}' trebuie să fie un număr valid!");
            tb.Focus();
            return false;
        }
        if (valoare <= minim)
        {
            AfiseazaEroare($"'{numeCamp}' trebuie să fie mai mare decât {minim}!");
            tb.Focus();
            return false;
        }
        return true;
    }

    protected bool ValidareInteger(TextBox tb, string numeCamp,
        out int valoare, int minim = 0)
    {
        valoare = 0;
        if (!int.TryParse(tb.Text, out valoare))
        {
            AfiseazaEroare($"'{numeCamp}' trebuie să fie un număr întreg!");
            tb.Focus();
            return false;
        }
        if (valoare <= minim)
        {
            AfiseazaEroare($"'{numeCamp}' trebuie să fie mai mare decât {minim}!");
            tb.Focus();
            return false;
        }
        return true;
    }

    protected void TrateazaExceptie(Exception ex, string operatie = "operație")
    {
        AfiseazaEroare(
            $"Eroare la {operatie}:\n{ex.Message}",
            "Eroare neașteptată");
    }
}