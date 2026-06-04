using System;
using System.Drawing;
using System.Windows.Forms;

namespace RealEstateAdmin.WinForms
{
    public partial class MainForm : Form
    {
        private Button _activeButton;

        public MainForm()
        {
            InitializeComponent();
            ResetButtonStyle(btnGradovi);
            ResetButtonStyle(btnProjekti);
            ResetButtonStyle(btnKategorije);
            ResetButtonStyle(btnStrukture);
            ResetButtonStyle(btnNekretnine);
            ResetButtonStyle(btnCene);
        }

        private void btnGradovi_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            LoadForm(new GradForm(), "Gradovi");
        }

        private void btnProjekti_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            LoadForm(new ProjekatForm(), "Projekti");
        }

        private void btnKategorije_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            LoadForm(new KategorijaForm(), "Kategorije");
        }

        private void btnStrukture_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            LoadForm(new StrukturaForm(), "Strukture");
        }

        private void btnNekretnine_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            LoadForm(new NekretninaForm(), "Nekretnine");
        }

        private void btnCene_Click(object sender, EventArgs e)
        {
            ActivateButton(sender as Button);
            LoadForm(new CenaForm(), "Cene");
        }

        private void btnIzlaz_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ActivateButton(Button button)
        {
            if (button == null)
            {
                return;
            }

            if (_activeButton != null && _activeButton != button)
            {
                ResetButtonStyle(_activeButton);
            }

            _activeButton = button;
            _activeButton.BackColor = Color.FromArgb(59, 130, 246);
            _activeButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        }

        private void ResetButtonStyle(Button button)
        {
            if (button == null)
            {
                return;
            }

            button.BackColor = Color.FromArgb(31, 41, 55);
            button.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
        }

        private void LoadForm(Form form, string title)
        {
            while (pnlContent.Controls.Count > 0)
            {
                var control = pnlContent.Controls[0];
                pnlContent.Controls.RemoveAt(0);
                control.Dispose();
            }

            lblSectionTitle.Text = title;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(form);
            pnlContent.Tag = form;
            form.Show();
        }
    }
}

