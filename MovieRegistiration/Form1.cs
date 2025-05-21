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
    public partial class Form1 : Form
    {
        MovieContext context = new MovieContext();

        public Form1()
        {
            InitializeComponent();
            ApplySimpleStyle();
        }

        private void ApplySimpleStyle()
        {
            // Ana form rengi
            this.BackColor = System.Drawing.Color.WhiteSmoke;

            // Butonlar için
            btnList.BackColor = System.Drawing.Color.FromArgb(52, 152, 219); // Mavi
            btnAdd.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnUpdate.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnDelete.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            btnSearch.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);

            // Tüm butonlar için metin rengi
            btnList.ForeColor = System.Drawing.Color.White;
            btnAdd.ForeColor = System.Drawing.Color.White;
            btnUpdate.ForeColor = System.Drawing.Color.White;
            btnDelete.ForeColor = System.Drawing.Color.White;
            btnSearch.ForeColor = System.Drawing.Color.White;

            // DataGridView'i güzelleştir
            dgvCategory.BackgroundColor = System.Drawing.Color.White;
            dgvCategory.BorderStyle = BorderStyle.None;
            dgvCategory.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(52, 152, 219);
            dgvCategory.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            var values = context.Categories.ToList();
            dgvCategory.DataSource = values;

            // Eğer Movies sütunu görünürse, gizle
            if (dgvCategory.Columns.Contains("Movies"))
                dgvCategory.Columns["Movies"].Visible = false;
        }

        private void buttonList_Click(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                {
                    MessageBox.Show("Kategori adı boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Category category = new Category();
                category.CategoryName = txtCategoryName.Text;
                context.Categories.Add(category);
                context.SaveChanges();
                MessageBox.Show("Kategori eklendi");

                ClearForm();
                LoadCategories();
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
                if (string.IsNullOrWhiteSpace(txtCategoryId.Text))
                {
                    MessageBox.Show("Lütfen güncellenecek kategoriyi seçin!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                {
                    MessageBox.Show("Kategori adı boş olamaz!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = int.Parse(txtCategoryId.Text);
                var value = context.Categories.Find(id);

                if (value == null)
                {
                    MessageBox.Show("Kategori bulunamadı!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                value.CategoryName = txtCategoryName.Text;
                context.SaveChanges();
                MessageBox.Show("Kategori güncellendi");

                ClearForm();
                LoadCategories();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }
        private void btnOpenMovieForm_Click(object sender, EventArgs e)
        {
            MovieForm movieForm = new MovieForm();
            movieForm.ShowDialog();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtCategoryId.Text))
                {
                    MessageBox.Show("Lütfen silinecek kategoriyi seçin!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int id = int.Parse(txtCategoryId.Text);
                var value = context.Categories.Find(id);

                if (value == null)
                {
                    MessageBox.Show("Kategori bulunamadı!", "Uyarı",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // İlişkili filmler kontrolü
                var relatedMovies = context.Movies.Where(m => m.CategoryId == id).ToList();
                if (relatedMovies.Count > 0)
                {
                    MessageBox.Show("Bu kategoriye ait filmler bulunmaktadır. Önce ilişkili filmleri silmelisiniz.",
                        "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult result = MessageBox.Show("Kategoriyi silmek istediğinize emin misiniz?",
                    "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    context.Categories.Remove(value);
                    context.SaveChanges();
                    MessageBox.Show("Kategori silindi");

                    ClearForm();
                    LoadCategories();
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
                if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
                {
                    LoadCategories();
                    return;
                }

                var values = context.Categories
                    .Where(x => x.CategoryName.Contains(txtCategoryName.Text))
                    .ToList();

                dgvCategory.DataSource = values;

                // Eğer Movies sütunu görünürse, gizle
                if (dgvCategory.Columns.Contains("Movies"))
                    dgvCategory.Columns["Movies"].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void ClearForm()
        {
            txtCategoryId.Clear();
            txtCategoryName.Clear();
            txtCategoryName.Focus();
        }

        private void dgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvCategory.Rows[e.RowIndex];
                txtCategoryId.Text = row.Cells["CategoryId"].Value.ToString();
                txtCategoryName.Text = row.Cells["CategoryName"].Value.ToString();
            }
        }
    }
}