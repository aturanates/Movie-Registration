using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MovieRegistiration.DataAccesLayer.Context;
using MovieRegistiration.DataAccesLayer.Entities;

namespace MovieRegistiration
{
    public partial class MovieForm : Form
    {
        MovieContext context = new MovieContext();

        public MovieForm()
        {
            InitializeComponent();
        }

        private void MovieForm_Load(object sender, EventArgs e)
        {
            LoadMovies();
            LoadCategories();
        }

        private void LoadMovies()
        {
            var values = context.Movies.ToList();
            dgvMovies.DataSource = values;

            // Category sütunu görünürse, gizle
            if (dgvMovies.Columns.Contains("Category"))
                dgvMovies.Columns["Category"].Visible = false;
        }

        private void LoadCategories()
        {
            var categories = context.Categories.ToList();
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            LoadMovies();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Film başlığı boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDuration.Text) || !int.TryParse(txtDuration.Text, out int duration))
                {
                    MessageBox.Show("Süre sayısal bir değer olmalıdır!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Movie movie = new Movie();
                movie.Title = txtTitle.Text;
                movie.Duration = int.Parse(txtDuration.Text);
                movie.Description = txtDescription.Text;
                movie.ReleaseDate = dtpReleaseDate.Value;
                movie.CategoryId = (int)cmbCategory.SelectedValue;

                context.Movies.Add(movie);
                context.SaveChanges();
                MessageBox.Show("Film eklendi");

                ClearForm();
                LoadMovies();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMovieId.Text))
                {
                    MessageBox.Show("Lütfen güncellenecek filmi seçin!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Film başlığı boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDuration.Text) || !int.TryParse(txtDuration.Text, out int duration))
                {
                    MessageBox.Show("Süre sayısal bir değer olmalıdır!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = int.Parse(txtMovieId.Text);
                var movie = context.Movies.Find(id);

                if (movie == null)
                {
                    MessageBox.Show("Film bulunamadı!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                movie.Title = txtTitle.Text;
                movie.Duration = int.Parse(txtDuration.Text);
                movie.Description = txtDescription.Text;
                movie.ReleaseDate = dtpReleaseDate.Value;
                movie.CategoryId = (int)cmbCategory.SelectedValue;

                context.SaveChanges();
                MessageBox.Show("Film güncellendi");

                ClearForm();
                LoadMovies();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtMovieId.Text))
                {
                    MessageBox.Show("Lütfen silinecek filmi seçin!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = int.Parse(txtMovieId.Text);
                var movie = context.Movies.Find(id);

                if (movie == null)
                {
                    MessageBox.Show("Film bulunamadı!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Filmi silmek istediğinize emin misiniz?",
                    "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    context.Movies.Remove(movie);
                    context.SaveChanges();
                    MessageBox.Show("Film silindi");

                    ClearForm();
                    LoadMovies();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    LoadMovies();
                    return;
                }

                var movies = context.Movies
                    .Where(x => x.Title.Contains(txtTitle.Text))
                    .ToList();

                dgvMovies.DataSource = movies;

                // Category sütunu görünürse, gizle
                if (dgvMovies.Columns.Contains("Category"))
                    dgvMovies.Columns["Category"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtMovieId.Clear();
            txtTitle.Clear();
            txtDuration.Clear();
            txtDescription.Clear();
            dtpReleaseDate.Value = DateTime.Now;
            if (cmbCategory.Items.Count > 0)
                cmbCategory.SelectedIndex = 0;
        }

        private void dgvMovies_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvMovies.Rows[e.RowIndex];
                txtMovieId.Text = row.Cells["MovieId"].Value.ToString();
                txtTitle.Text = row.Cells["Title"].Value.ToString();
                txtDuration.Text = row.Cells["Duration"].Value.ToString();
                txtDescription.Text = row.Cells["Description"].Value?.ToString() ?? "";

                if (row.Cells["ReleaseDate"].Value != null && row.Cells["ReleaseDate"].Value != DBNull.Value)
                {
                    dtpReleaseDate.Value = Convert.ToDateTime(row.Cells["ReleaseDate"].Value);
                }

                cmbCategory.SelectedValue = row.Cells["CategoryId"].Value;
            }
        }
    }
}