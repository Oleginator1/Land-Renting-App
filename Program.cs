using ArendaManagement.Forms;
using LandRentManagementApp;
using LandRentManagementApp.Data;
using LandRentManagementApp.Forms;

namespace ArendaManagement;

internal static class Program
{
    [STAThread]
    static void Main()
    {        
        Application.ThreadException += Application_ThreadException;
        AppDomain.CurrentDomain.UnhandledException +=
            CurrentDomain_UnhandledException;
        Application.SetUnhandledExceptionMode(
            UnhandledExceptionMode.CatchException);

        ApplicationConfiguration.Initialize();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        
        if (!DatabaseHelper.TestConnection())
        {
            var result = MessageBox.Show(
                "Nu s-a putut stabili conexiunea cu baza de date!\n\n" +
                "Verificați că:\n" +
                "  • SQL Server este pornit\n" +
                "  • Baza de date 'ArendaDB' există\n" +
                "  • Stringul de conexiune este corect\n\n" +
                "Doriți să continuați oricum?",
                "Eroare Conexiune",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Error);

            if (result == DialogResult.No) return;
        }

        Application.Run(new MainForm());
    }

    private static void Application_ThreadException(
        object sender, System.Threading.ThreadExceptionEventArgs e)
    {
        LogExceptie(e.Exception);
        MessageBox.Show(
            $"A apărut o eroare neașteptată:\n\n{e.Exception.Message}\n\n" +
            "Aplicația va continua să funcționeze.\n" +
            "Eroarea a fost înregistrată în fișierul de log.",
            "Eroare Neașteptată",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }

    private static void CurrentDomain_UnhandledException(
        object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception ex)
            LogExceptie(ex);
    }

    private static void LogExceptie(Exception ex)
    {
        try
        {
            string logPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "error.log");
            string mesaj =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] " +
                $"{ex.GetType().Name}: {ex.Message}\n" +
                $"StackTrace: {ex.StackTrace}\n" +
                new string('─', 80) + "\n";
            File.AppendAllText(logPath, mesaj,
                System.Text.Encoding.UTF8);
        }
        catch { /* */ }
    }
}