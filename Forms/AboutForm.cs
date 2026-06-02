using LandRentManagementApp.Config;
using LandRentManagementApp.Forms;
using System.Windows.Forms;


namespace ArendaManagement.Forms;

public class AboutForm : FormBase
{
    public AboutForm()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Text = "ℹ Despre aplicație";
        Size = new Size(420, 340);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        BackColor = Color.White;

        var layout = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            Padding = new Padding(30),
            AutoSize = true
        };

        void Add(string text, Font font, Color color,
                 int marginBottom = 5)
        {
            layout.Controls.Add(new Label
            {
                Text = text,
                Font = font,
                ForeColor = color,
                AutoSize = true,
                Margin = new Padding(0, 0, 0, marginBottom)
            });
        }

        Add("🌾 Sistem de Evidență Arendă",
            new Font("Segoe UI", 16f, FontStyle.Bold),
            Color.FromArgb(34, 85, 34), 10);

        Add("Versiunea 1.0.0",
            new Font("Segoe UI", 11f), Color.Gray, 20);

        Add("Descriere:", new Font("Segoe UI", 10f, FontStyle.Bold),
            Color.FromArgb(30, 30, 30));
        Add("Aplicație desktop pentru gestionarea fermierilor,\n" +
            "terenurilor agricole și contractelor de arendă.",
            new Font("Segoe UI", 10f), Color.FromArgb(60, 60, 60), 15);

        Add("Tehnologii utilizate:",
            new Font("Segoe UI", 10f, FontStyle.Bold),
            Color.FromArgb(30, 30, 30));
        Add("  • C# .NET 6 / Windows Forms\n" +
            "  • Microsoft SQL Server\n" +
            "  • ADO.NET",
            new Font("Segoe UI", 10f), Color.FromArgb(60, 60, 60), 15);

        Add($"© {DateTime.Now.Year} — Toate drepturile rezervate",
            new Font("Segoe UI", 9f), Color.Gray);

        var btnOk = new Button
        {
            Text = "OK",
            Width = 100,
            Height = 36,
            Margin = new Padding(120, 10, 0, 0)
        };
        UITheme.ApplyButtonStyle(btnOk);
        btnOk.Click += (s, e) => Close();
        layout.Controls.Add(btnOk);

        Controls.Add(layout);
    }
}