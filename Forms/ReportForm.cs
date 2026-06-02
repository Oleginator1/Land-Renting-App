using LandRentManagementApp.Config;
using LandRentManagementApp.Data;
using LandRentManagementApp.Forms;
using System.Windows.Forms;


namespace LandRentManagementApp.Forms;

public class ReportForm : FormBase
{
    private DataGridView gridRaport = new();
    private Label lblTotal = new();
    private Label lblMedia = new();
    private Label lblTopTeren = new();
    private Label lblTotalFerm = new();
    private Button btnInchide = new();
    private Button btnPrint = new();
    private Panel panelGrafic = new();
    private Button btnExport = new();

    public ReportForm()
    {
        InitializeComponent();
        GenereazaRaport();
    }

    private void InitializeComponent()
    {
        Text = "📊 Raport Financiar — Arendă";
        Size = new Size(900, 650);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.Sizable;
        BackColor = UITheme.BackgroundLight;

        // Header
        var panelHeader = new Panel
        {
            Dock = DockStyle.Top,
            Height = 70,
            BackColor = UITheme.PrimaryGreen,
            Padding = new Padding(20, 10, 20, 5)
        };
        var lblTitlu = new Label
        {
            Text = "📊 RAPORT FINANCIAR — SISTEM EVIDENȚĂ ARENDĂ",
            Font = new Font("Segoe UI", 14f, FontStyle.Bold),
            ForeColor = Color.White,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleCenter
        };
        var lblData = new Label
        {
            Text = $"Generat la: {DateTime.Now:dd.MM.yyyy HH:mm}",
            Font = UITheme.FontSmall,
            ForeColor = Color.LightGreen,
            Dock = DockStyle.Bottom,
            TextAlign = ContentAlignment.MiddleRight,
            Height = 20
        };
        panelHeader.Controls.Add(lblTitlu);
        panelHeader.Controls.Add(lblData);


        var panelStats = new Panel
        {
            Dock = DockStyle.Bottom,
            Height = 160,
            BackColor = Color.White,
            Padding = new Padding(20)
        };

        var lblStatTitlu = new Label
        {
            Text = "📈 STATISTICI GENERALE",
            Font = UITheme.FontSubtitle,
            ForeColor = UITheme.PrimaryGreen,
            Dock = DockStyle.Top,
            Height = 28
        };

        var tableStats = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            ColumnStyles = {
                new ColumnStyle(SizeType.Percent, 50f),
                new ColumnStyle(SizeType.Percent, 50f)
            }
        };

        lblTotal = CrLabel("💰 Total încasat: —");
        lblMedia = CrLabel("📊 Media plăților: —");
        lblTopTeren = CrLabel("🏆 Teren cel mai solicitat: —");
        lblTotalFerm = CrLabel("👨‍🌾 Total fermieri cu contracte: —");

        tableStats.Controls.AddRange(new Control[]
            { lblTotal, lblMedia, lblTopTeren, lblTotalFerm });


        var panelBtns = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            Height = 55,
            FlowDirection = FlowDirection.RightToLeft,
            Padding = new Padding(10, 10, 10, 5),
            BackColor = UITheme.BackgroundWhite
        };

        btnInchide = new Button { Text = "✖ Închide", Width = 110, Height = 36 };
        btnPrint = new Button { Text = "🖨 Printează", Width = 120, Height = 36 };
        UITheme.ApplyButtonStyle(btnInchide, true);
        UITheme.ApplyButtonStyle(btnPrint);

        panelBtns.Controls.Add(btnInchide);
        panelBtns.Controls.Add(btnPrint);


        gridRaport.Dock = DockStyle.Fill;
        gridRaport.ReadOnly = true;
        gridRaport.MultiSelect = false;
        gridRaport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridRaport.AllowUserToAddRows = false;
        gridRaport.AllowUserToDeleteRows = false;
        UITheme.ApplyGridStyle(gridRaport);

        panelStats.Controls.Add(tableStats);
        panelStats.Controls.Add(lblStatTitlu);

        Controls.Add(gridRaport);
        Controls.Add(panelStats);
        Controls.Add(panelHeader);
        Controls.Add(panelBtns);

        btnInchide.Click += (s, e) => Close();
        btnPrint.Click += BtnPrint_Click;
    }

    private Label CrLabel(string text) => new Label
    {
        Text = text,
        Font = UITheme.FontNormal,
        ForeColor = UITheme.TextDark,
        AutoSize = true,
        Margin = new Padding(5)
    };

    private void GenereazaRaport()
    {
        try
        {
            var contracte = ServiceLocator.ContractRepo.GetAll();
            var fermieri = ServiceLocator.FarmerRepo.GetAll();

            
            var raport = fermieri
                .Select(f => new
                {
                    Fermier = f.FullName,
                    Localitate = f.Residence,
                    NrContracte = contracte.Count(c => c.FarmerId == f.FarmerId),
                    SumaTotala = contracte
                        .Where(c => c.FarmerId == f.FarmerId)
                        .Sum(c => c.TotalSum),
                })
                .Where(x => x.NrContracte > 0)
                .OrderByDescending(x => x.SumaTotala)
                .ToList();

            gridRaport.DataSource = raport.Select(r => new
            {
                r.Fermier,
                r.Localitate,
                NrContracte = r.NrContracte,
                SumaTotala = $"{r.SumaTotala:N2} Lei"
            }).ToList();

            if (gridRaport.Columns.Count > 0)
            {
                gridRaport.Columns["Fermier"].HeaderText = "Fermier";
                gridRaport.Columns["Localitate"].HeaderText = "Localitate";
                gridRaport.Columns["NrContracte"].HeaderText = "Nr. Contracte";
                gridRaport.Columns["SumaTotala"].HeaderText = "Sumă Totală (Lei)";
            }

            // Statistici generale
            decimal totalIncasat = contracte.Sum(c => c.TotalSum);
            decimal media = raport.Count > 0
                ? raport.Average(r => r.SumaTotala) : 0;

            var topTeren = ServiceLocator.ContractRepo
                .GetTerenCeleMaiMulteContracte();

            lblTotal.Text = $"💰 Total încasat:  {totalIncasat:N2} Lei";
            lblMedia.Text = $"📊 Media plăților: {media:N2} Lei";
            lblTopTeren.Text = $"🏆 Top teren: {topTeren.Teren} ({topTeren.NrContracte} contracte)";
            lblTotalFerm.Text = $"👨‍🌾 Fermieri cu contracte: {raport.Count}";
        }
        catch (Exception ex) { TrateazaExceptie(ex, "generare raport"); }
    }

    private void BtnPrint_Click(object? sender, EventArgs e)
    {
        var printDoc = new System.Drawing.Printing.PrintDocument();
        printDoc.PrintPage += PrintDoc_PrintPage;

        var preview = new System.Windows.Forms.PrintPreviewDialog
        {
            Document = printDoc,
            WindowState = FormWindowState.Maximized
        };
        preview.ShowDialog(this);
    }

    private void AdaugaGraficSimple(List<(string Fermier, decimal Suma)> date)
    {
        panelGrafic.Controls.Clear();
        if (!date.Any()) return;

        decimal maxSuma = date.Max(x => x.Suma);
        int yOffset = 10;

        foreach (var (fermier, suma) in date.Take(8))
        {
            var lblNume = new Label
            {
                Text = fermier,
                Left = 10,
                Top = yOffset,
                Width = 180,
                Height = 22,
                Font = UITheme.FontSmall,
                TextAlign = ContentAlignment.MiddleRight
            };

            int barWidth = maxSuma > 0
                ? (int)(350 * suma / maxSuma) : 0;

            var bar = new Panel
            {
                Left = 200,
                Top = yOffset + 3,
                Width = barWidth,
                Height = 18,
                BackColor = UITheme.LightGreen
            };

            var lblVal = new Label
            {
                Text = $"{suma:N0} Lei",
                Left = 205 + barWidth,
                Top = yOffset,
                Width = 120,
                Height = 22,
                Font = UITheme.FontSmall,
                ForeColor = UITheme.TextGray
            };

            panelGrafic.Controls.AddRange(new Control[] { lblNume, bar, lblVal });
            yOffset += 28;
        }
    }


    private void ExportRaport(object? sender, EventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Filter = "Text Files (*.txt)|*.txt",
            FileName = $"Raport_Arenda_{DateTime.Now:yyyyMMdd_HHmm}.txt"
        };

        if (dialog.ShowDialog() != DialogResult.OK) return;

        try
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("═══════════════════════════════════════════════════════");
            sb.AppendLine("         RAPORT FINANCIAR — SISTEM EVIDENȚĂ ARENDĂ");
            sb.AppendLine($"         Generat la: {DateTime.Now:dd.MM.yyyy HH:mm:ss}");
            sb.AppendLine("═══════════════════════════════════════════════════════");
            sb.AppendLine();
            sb.AppendLine("LISTA FERMIERI ȘI SUMELE ACHITATE:");
            sb.AppendLine(new string('─', 60));
            sb.AppendLine($"{"Fermier",-30} {"Nr.Contr",8} {"Sumă Totală",15}");
            sb.AppendLine(new string('─', 60));

            var contracte = ServiceLocator.ContractRepo.GetAll();
            var fermieri = ServiceLocator.FarmerRepo.GetAll();
            decimal total = 0;

            foreach (var f in fermieri)
            {
                var cF = contracte.Where(c => c.FarmerId == f.FarmerId).ToList();
                if (!cF.Any()) continue;
                decimal suma = cF.Sum(c => c.TotalSum);
                total += suma;
                sb.AppendLine($"{f.FullName,-30} {cF.Count,8} {suma,14:N2} Lei");
            }

            sb.AppendLine(new string('─', 60));
            sb.AppendLine($"{"TOTAL GENERAL:",-30} {"",8} {total,14:N2} Lei");
            sb.AppendLine();
            sb.AppendLine("STATISTICI GENERALE:");
            sb.AppendLine($"  • Suma totală încasată: {total:N2} Lei");
            sb.AppendLine($"  • Media per fermier:    {(fermieri.Count > 0 ? total / fermieri.Count : 0):N2} Lei");
            var top = ServiceLocator.ContractRepo.GetTerenCeleMaiMulteContracte();
            sb.AppendLine($"  • Terenul cu mai multe contracte: {top.Teren}");
            sb.AppendLine();
            sb.AppendLine("═══════════════════════════════════════════════════════");

            File.WriteAllText(dialog.FileName, sb.ToString(), System.Text.Encoding.UTF8);
            AfiseazaInfo($"Raportul a fost exportat în:\n{dialog.FileName}");
        }
        catch (Exception ex) { TrateazaExceptie(ex, "export raport"); }
    }

    
    private void PrintDoc_PrintPage(object sender,
        System.Drawing.Printing.PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        float y = 40f;
        float x = 50f;
        float pageW = e.PageBounds.Width - 100f;

        var fTitle = new Font("Segoe UI", 16f, FontStyle.Bold);
        var fHeader = new Font("Segoe UI", 11f, FontStyle.Bold);
        var fNormal = new Font("Segoe UI", 10f);
        var fSmall = new Font("Segoe UI", 8f);
        var brushDark = new SolidBrush(Color.FromArgb(30, 30, 30));
        var brushGreen = new SolidBrush(UITheme.PrimaryGreen);
        var penLine = new Pen(UITheme.PrimaryGreen, 1.5f);

       
        g.DrawString("RAPORT FINANCIAR — SISTEM EVIDENȚĂ ARENDĂ",
            fTitle, brushGreen, x, y);
        y += 30f;
        g.DrawString($"Generat la: {DateTime.Now:dd.MM.yyyy HH:mm}",
            fSmall, Brushes.Gray, x, y);
        y += 15f;
        g.DrawLine(penLine, x, y, x + pageW, y);
        y += 15f;

      
        g.DrawString("Fermier", fHeader, brushDark, x, y);
        g.DrawString("Nr. Contracte", fHeader, brushDark, x + 280, y);
        g.DrawString("Sumă Totală", fHeader, brushDark, x + 400, y);
        y += 20f;
        g.DrawLine(penLine, x, y, x + pageW, y);
        y += 10f;

      
        var contracte = ServiceLocator.ContractRepo.GetAll();
        var fermieri = ServiceLocator.FarmerRepo.GetAll();
        decimal total = 0;

        foreach (var f in fermieri)
        {
            var cF = contracte.Where(c => c.FarmerId == f.FarmerId).ToList();
            if (!cF.Any()) continue;
            decimal suma = cF.Sum(c => c.TotalSum);
            total += suma;
            g.DrawString(f.FullName, fNormal, brushDark, x, y);
            g.DrawString(cF.Count.ToString(), fNormal, brushDark, x + 280, y);
            g.DrawString($"{suma:N2} Lei", fNormal, brushDark, x + 400, y);
            y += 22f;
        }

        y += 10f;
        g.DrawLine(penLine, x, y, x + pageW, y);
        y += 10f;
        g.DrawString($"TOTAL: {total:N2} Lei",
            fHeader, brushGreen, x + 300, y);
    }
}