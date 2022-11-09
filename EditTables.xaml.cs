using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for EditTables.xaml
    /// </summary>
    public partial class EditTables : Window
    {
        string t;

        public EditTables(string table)
        {
            InitializeComponent();
            t = table;
        }

        private void BackInDetAffair_Click(object sender, RoutedEventArgs e)
        {
            DetAffair d = new DetAffair();
            d.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (t == "dep")
            {
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT department_id AS 'ID', department_name AS 'Назва відділу', post_index AS 'Поштовий індекс' FROM Departments", PoliceCardIndex.GetSqlConnection());
                DataTable table = new DataTable();
                adapter.Fill(table);

                Departments.ItemsSource = table.AsDataView();
                Departments.AutoGenerateColumns = true;
            }
            else if (t == "spec")
            {
                TopText.Text = "Інформація про спеціалзації";
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT type_id AS 'ID', affair_type AS 'Тип злочину-звинувачення / спеціалізація' FROM Affair_Types", PoliceCardIndex.GetSqlConnection());
                DataTable table = new DataTable();
                adapter.Fill(table);

                Departments.ItemsSource = table.AsDataView();
                Departments.AutoGenerateColumns = true;
            }

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Departments_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

        }

        private void Departments_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }
    }
}
