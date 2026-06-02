namespace ArendaManagement.Config;

public static class Validators
{

    public static bool ValidareIDNP(string cnp)
    {
        if (string.IsNullOrWhiteSpace(cnp)) return false;
        if (cnp.Length != 13) return false;
        if (!cnp.All(char.IsDigit)) return false;


        int[] ponderi = { 2, 7, 9, 1, 4, 6, 3, 5, 8, 2, 7, 9 };
        int suma = 0;
        for (int i = 0; i < 12; i++)
            suma += (cnp[i] - '0') * ponderi[i];

        int rest = suma % 11;
        int cifraControl = rest == 10 ? 1 : rest;

        return cifraControl == (cnp[12] - '0');
    }


    public static bool ValidareEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return true;
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch { return false; }
    }


    public static bool ValidareTelefon(string telefon)
    {
        if (string.IsNullOrWhiteSpace(telefon)) return true;
        var cifre = new string(telefon.Where(char.IsDigit).ToArray());
        return cifre.Length >= 9 && cifre.Length <= 13;
    }


    public static (bool isValid, string error) ValidareSuprafata(string text)
    {
        if (!decimal.TryParse(text.Replace(',', '.'),
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture,
            out decimal val))
            return (false, "Suprafața trebuie să fie un număr valid (ex: 5.50)!");
        if (val <= 0)
            return (false, "Suprafața trebuie să fie mai mare decât 0!");
        if (val > 10000)
            return (false, "Suprafața nu poate depăși 10.000 ha!");
        return (true, "");
    }


    public static (bool isValid, string error) ValidareAniAchitati(string text)
    {
        if (!int.TryParse(text, out int val))
            return (false, "Numărul de ani trebuie să fie un număr întreg!");
        if (val <= 0)
            return (false, "Numărul de ani achitați trebuie să fie mai mare decât 0!");
        if (val > 99)
            return (false, "Numărul de ani nu poate depăși 99!");
        return (true, "");
    }
}