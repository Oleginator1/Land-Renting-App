namespace LandRentManagementApp.Config;

public static class UITheme
{
    
    public static Color PrimaryGreen = Color.FromArgb(34, 85, 34);
    public static Color LightGreen = Color.FromArgb(55, 120, 55);
    public static Color AccentGreen = Color.FromArgb(80, 160, 80);
    public static Color BackgroundLight = Color.WhiteSmoke;
    public static Color BackgroundWhite = Color.White;
    public static Color TextDark = Color.FromArgb(30, 30, 30);
    public static Color TextGray = Color.FromArgb(100, 100, 100);
    public static Color DangerRed = Color.FromArgb(180, 30, 30);
    public static Color WarningOrange = Color.FromArgb(200, 100, 0);

    public static Font FontTitle = new Font("Segoe UI", 14f, FontStyle.Bold);
    public static Font FontSubtitle = new Font("Segoe UI", 11f, FontStyle.Bold);
    public static Font FontNormal = new Font("Segoe UI", 10f);
    public static Font FontSmall = new Font("Segoe UI", 9f);


    public static void ApplyButtonStyle(Button btn, bool isDanger = false)
    {
        btn.FlatStyle = FlatStyle.Flat;
        btn.BackColor = isDanger ? DangerRed : LightGreen;
        btn.ForeColor = Color.White;
        btn.Font = FontNormal;
        btn.Cursor = Cursors.Hand;
        btn.Height = 36;
        btn.FlatAppearance.BorderColor = isDanger
            ? Color.FromArgb(140, 20, 20)
            : AccentGreen;
    }


    public static void ApplyGridStyle(DataGridView grid)
    {
        grid.BackgroundColor = BackgroundWhite;
        grid.BorderStyle = BorderStyle.None;
        grid.ColumnHeadersDefaultCellStyle.BackColor = PrimaryGreen;
        grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        grid.ColumnHeadersDefaultCellStyle.Font = FontSubtitle;
        grid.ColumnHeadersHeight = 40;
        grid.RowTemplate.Height = 32;
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 248, 240);
        grid.DefaultCellStyle.Font = FontNormal;
        grid.DefaultCellStyle.SelectionBackColor = AccentGreen;
        grid.DefaultCellStyle.SelectionForeColor = Color.White;
        grid.EnableHeadersVisualStyles = false;
        grid.GridColor = Color.FromArgb(200, 220, 200);
    }
}