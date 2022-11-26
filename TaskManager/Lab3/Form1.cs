using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Lab3
{
    public partial class Form1 : Form
    {
        private SqlConnection _sqlConnection = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            _sqlConnection.Open();

            DataUpdate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand command = new SqlCommand($"INSERT INTO [Project] (ProjectName, CastomerCompany, ContractorCompany, ProjectManager, ProjectExecutors, StartDate, EndDate, Priority) VALUES (@ProjectName, @CastomerCompany, @ContractorCompany, @ProjectManager, @ProjectExecutors, @StartDate, @EndDate, @Priority)", _sqlConnection);

            DateTime startDate = DateTime.Parse(StartDate.Text);
            DateTime endDate = DateTime.Parse(EndDate.Text);

            command.Parameters.AddWithValue("ProjectName", ProjectName.Text);
            command.Parameters.AddWithValue("CastomerCompany", CastomerCompany.Text);
            command.Parameters.AddWithValue("ContractorCompany", ContractorCompany.Text);
            command.Parameters.AddWithValue("ProjectManager", ProjectManager.Text);
            command.Parameters.AddWithValue("ProjectExecutors", ProjectExecutors.Text);
            command.Parameters.AddWithValue("StartDate", $"{startDate.Month}/{startDate.Day}/{startDate.Year}");
            command.Parameters.AddWithValue("EndDate", $"{endDate.Month}/{endDate.Day}/{endDate.Year}");
            command.Parameters.AddWithValue("Priority", Priority.Text);

            MessageBox.Show(command.ExecuteNonQuery().ToString());
            DataUpdate();
        }

        private void DataUpdate()
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Project", _sqlConnection);
            FillView(dataAdapter);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter($"SELECT * FROM Project WHERE Priority = {int.Parse(PriorityFilter.Text)}", _sqlConnection);
            PriorityFilter.Clear();
            FillView(dataAdapter);
        }

        private void StartDateFilterButton_Click(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.Parse(StartDateFilter.Text);
            SqlDataAdapter dataAdapter = new SqlDataAdapter($"SELECT * FROM Project WHERE StartDate = '{startDate.Month}/{startDate.Day}/{startDate.Year}'", _sqlConnection);
            StartDateFilter.Clear();
            FillView(dataAdapter);
        }

        private void FillView(SqlDataAdapter dataAdapter)
        {
            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet);

            dataGridView1.DataSource = dataSet.Tables[0];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DataUpdate();
        }

        private void EndDateFilterButton_Click(object sender, EventArgs e)
        {
            DateTime startDate = DateTime.Parse(StartDateFilter.Text);
            SqlDataAdapter dataAdapter = new SqlDataAdapter($"SELECT * FROM Project WHERE EndDate = '{startDate.Month}/{startDate.Day}/{startDate.Year}'", _sqlConnection);
            EndDateFilter.Clear();
            FillView(dataAdapter);
        }

        private void SortPriorityDecrease_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Project ORDER BY Priority DESC", _sqlConnection);
            FillView(dataAdapter);
        }

        private void SortPriorityIncrease_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Project ORDER BY Priority ASC", _sqlConnection);
            FillView(dataAdapter);
        }

        private void NameProjectSort_Click(object sender, EventArgs e)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM Project ORDER BY ProjectName", _sqlConnection);
            FillView(dataAdapter);
        }
    }
}
