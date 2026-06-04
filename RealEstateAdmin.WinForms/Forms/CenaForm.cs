using System;
using System.Windows.Forms;
using RealEstateAdmin.Business;
using RealEstateAdmin.DTO;

namespace RealEstateAdmin.WinForms
{
    public partial class CenaForm : Form
    {
        private readonly CenaService _cenaService = new CenaService();

        public CenaForm()
        {
            InitializeComponent();
        }

        private void CenaForm_Load(object sender, EventArgs e)
        {
            LoadCene();
        }

        private void LoadCene()
        {
            try
            {
                dgvCene.DataSource = _cenaService.GetAll();
                dgvCene.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvCene.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvCene.MultiSelect = false;

                if (dgvCene.Columns["CenaID"] != null)
                {
                    dgvCene.Columns["CenaID"].Visible = false;
                }

                if (dgvCene.Columns["NekretninaID"] != null)
                {
                    dgvCene.Columns["NekretninaID"].Visible = false;
                }

                if (dgvCene.Columns["NekretninaNaziv"] != null)
                {
                    dgvCene.Columns["NekretninaNaziv"].HeaderText = "Nekretnina";
                    dgvCene.Columns["NekretninaNaziv"].DisplayIndex = 1;
                }

                if (dgvCene.Columns["Iznos"] != null)
                {
                    dgvCene.Columns["Iznos"].DefaultCellStyle.Format = "N2";
                }

                if (dgvCene.Columns["DatumOd"] != null)
                {
                    dgvCene.Columns["DatumOd"].HeaderText = "Datum Od";
                    dgvCene.Columns["DatumOd"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }

                if (dgvCene.Columns["DatumDo"] != null)
                {
                    dgvCene.Columns["DatumDo"].HeaderText = "Datum Do";
                    dgvCene.Columns["DatumDo"].DefaultCellStyle.Format = "dd.MM.yyyy";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri učitavanju cena: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNovi_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new CenaEditForm())
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadCene();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri otvaranju forme za unos cene: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIzmeni_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCene.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Molimo izaberite cenu koju želite izmeniti.", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dgvCene.SelectedRows[0];
                var cena = new CenaDTO
                {
                    CenaID = (int)selectedRow.Cells["CenaID"].Value,
                    NekretninaID = (int)selectedRow.Cells["NekretninaID"].Value,
                    Iznos = (decimal)selectedRow.Cells["Iznos"].Value,
                    DatumOd = (DateTime)selectedRow.Cells["DatumOd"].Value,
                    DatumDo = (DateTime)selectedRow.Cells["DatumDo"].Value,
                    Aktivna = (bool)selectedRow.Cells["Aktivna"].Value
                };

                using (var form = new CenaEditForm(cena))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        LoadCene();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri otvaranju forme za izmenu cene: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnObrisi_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCene.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Molimo izaberite cenu koju želite obrisati.", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int cenaId = (int)dgvCene.SelectedRows[0].Cells["CenaID"].Value;

                DialogResult result = MessageBox.Show(
                    "Da li ste sigurni da želite obrisati izabranu cenu?",
                    "Potvrda brisanja",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _cenaService.Delete(cenaId);
                    LoadCene();
                    MessageBox.Show("Cena je uspešno obrisana.", "Uspeh", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri brisanju cene: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
