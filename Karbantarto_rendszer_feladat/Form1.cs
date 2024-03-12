using Karbantarto_rendszer_feladat;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Karbantarto_rendszer
{
    public partial class Form1 : Form
    {

        HttpClient _httpClient = new HttpClient();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button_getalldata_Click(object sender, EventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://retoolapi.dev/Kc6xuH/data");
            HttpResponseMessage response = client.GetAsync(client.BaseAddress).Result;
            var dolgozo = response.Content.ReadAsStringAsync().Result;
            dataGridView1.DataSource = Dolgozo.FromJson(dolgozo);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void button_delete_Click(object sender, EventArgs e)
        {
            try
            {
                string idToDelete = textBox_id_delete.Text.Trim();
                if (string.IsNullOrEmpty(idToDelete))
                {
                    MessageBox.Show("Please enter an ID to delete.");
                    return;
                }

                string apiUrl = $"https://retoolapi.dev/Kc6xuH/data/{idToDelete}";
                var response = await _httpClient.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Item deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to delete item from the API.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void button_add_Click(object sender, EventArgs e)
        {
            try
            {
                // Get user input from textboxes
                string name = textBox_name.Text.Trim();
                string salary = textBox_salary.Text.Trim();
                string position = textBox_position.Text.Trim();

                // Validate input (you may add further validation as needed)
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(salary) || string.IsNullOrEmpty(position))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                // Create a new Dolgozo object with user input
                var dolgozo = new Dolgozo
                {
                    DolgozoName = name,
                    DolgozoSalary =long.Parse(salary),
                    DolgozoPosition = position
                };

                // Serialize Dolgozo object to JSON
                string jsonDolgozo = Newtonsoft.Json.JsonConvert.SerializeObject(dolgozo);

                // Send POST request to the API endpoint to add the new Dolgozo
                var response = await _httpClient.PostAsync("https://retoolapi.dev/Kc6xuH/data", new StringContent(jsonDolgozo, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Dolgozo added successfully.");
                    // Clear textboxes after successful addition
                    ClearTextBoxes();
                }
                else
                {
                    MessageBox.Show("Failed to add Dolgozo to the API.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void ClearTextBoxes()
        {
            // Clear textboxes
            textBox_name.Clear();
            textBox_salary.Clear();
            textBox_position.Clear();
        }

        private void textBox_id_delete_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_id_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_name_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_salary_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox_position_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
