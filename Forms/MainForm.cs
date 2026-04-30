using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LandRentManagementApp.Data;

namespace LandRentManagementApp.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            TestConnection();
            
        }

        private void TestConnection()
        {
            if (DatabaseHelper.TestConnection())
            {
                statusLabel.Text = "✔ Conectat la baza de date";
                statusLabel.ForeColor = Color.Green;
            }
            else
            {
                statusLabel.Text = "✘ Eroare conexiune baza de date";
                statusLabel.ForeColor = Color.Red;
                MessageBox.Show(
                    "Nu s-a putut conecta la baza de date!\n" +
                    "Verificați că SQL Server este pornit și că stringul de conexiune este corect.",
                    "Eroare conexiune",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        
        private void VerificaConexiune()
        {
            if (DatabaseHelper.TestConnection())
            {
                statusLabel.Text = "✔ Conectat la baza de date";
                statusLabel.ForeColor = Color.Green;
            }
            else
            {
                statusLabel.Text = "✘ Eroare conexiune baza de date";
                statusLabel.ForeColor = Color.Red;
                MessageBox.Show(
                    "Nu s-a putut conecta la baza de date!\n" +
                    "Verificați că SQL Server este pornit și că stringul de conexiune este corect.",
                    "Eroare conexiune",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void AfiseazaFormular(Form form)
        {
            panelContent.Controls.Clear();
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            panelContent.Controls.Add(form);
            form.Show();
        }


        private void menuFermieri_Click(object sender, EventArgs e)
            => AfiseazaFormular(new FermierListForm());

        private void menuTerenuri_Click(object sender, EventArgs e)
            => AfiseazaFormular(new LandListForm());

        private void menuContracte_Click(object sender, EventArgs e)
            => AfiseazaFormular(new ContractListForm());

        private void menuRaport_Click(object sender, EventArgs e)
            => new ReportForm().ShowDialog(this);

        private void menuIesire_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Doriți să închideți aplicația?",
                "Confirmare", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
