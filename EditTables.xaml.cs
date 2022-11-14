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
        DataTable table = new DataTable();

        public EditTables(string tab)
        {
            InitializeComponent();
            t = tab;
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
                
                adapter.Fill(table);
                
                Departments.ItemsSource = table.AsDataView();
                Departments.AutoGenerateColumns = true;

                Departments.Columns[0].IsReadOnly = true;
                Departments.CanUserDeleteRows = true;
            }
            else if (t == "spec")
            {
                TopText.Text = "Інформація про спеціалізації - типи злочинів";
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT type_id AS 'ID', affair_type AS 'Тип злочину-звинувачення' FROM Affair_Types", PoliceCardIndex.GetSqlConnection());
                adapter.Fill(table);

                Departments.ItemsSource = table.AsDataView();
                Departments.AutoGenerateColumns = true;
                Departments.Columns[0].IsReadOnly = true;
                Departments.CanUserDeleteRows = true;
            }

        }

        private void Save_Click(object sender, RoutedEventArgs e) //add
        {
            int count = 0;
            if (t == "dep")
            {
                string query = "INSERT INTO Departments(department_name, post_index) VALUES ";
                //for (DataRowView item in Departments.Items)
                for (int i = 0; i < Departments.Items.Count; i++)
                {
                    DataRowView item = Departments.Items[i] as DataRowView;
                    if (item == null)
                    {
                        query = query.Remove(query.Length - 2);
                        break;
                    }

                    if ((item.Row.ItemArray[0] == null || Convert.ToString(item.Row.ItemArray[0]) == "") && Convert.ToString(item.Row.ItemArray[1]) != "")
                    {
                        count = count + 1;
                        query += $"('{(string)item.Row.ItemArray[1]}', {((item.Row.ItemArray[2] is int) ? item.Row.ItemArray[2] : 0)}), ";
                    }

                }
                if (count > 0)
                {
                    SqlCommand command = new SqlCommand(query, PoliceCardIndex.GetSqlConnection());
                    PoliceCardIndex.OpenConnection();
                    command.ExecuteNonQuery();
                    PoliceCardIndex.CloseConnection();

                    table.Clear();
                    SqlDataAdapter adapter = new SqlDataAdapter($"SELECT department_id AS 'ID', department_name AS 'Назва відділу', post_index AS 'Поштовий індекс' FROM Departments", PoliceCardIndex.GetSqlConnection());
                    adapter.Fill(table);
                    Departments.ItemsSource = table.AsDataView();
                }
            }
            else if (t == "spec")
            {
                string query = "INSERT INTO Affair_Types(affair_type) VALUES ";
                
                for (int i = 0; i < Departments.Items.Count; i++)
                {
                    DataRowView item = Departments.Items[i] as DataRowView;
                    if (item == null)
                    {
                        query = query.Remove(query.Length - 2);
                        break;
                    }

                    if ((item.Row.ItemArray[0] == null || Convert.ToString(item.Row.ItemArray[0]) == "") && Convert.ToString(item.Row.ItemArray[1]) != "")
                    {
                        count = count + 1;
                        query += $"('{(string)item.Row.ItemArray[1]}'), ";
                    }

                }
                if (count > 0)
                {
                    SqlCommand command = new SqlCommand(query, PoliceCardIndex.GetSqlConnection());
                    PoliceCardIndex.OpenConnection();
                    command.ExecuteNonQuery();
                    PoliceCardIndex.CloseConnection();

                    table.Clear();
                    SqlDataAdapter adapter = new SqlDataAdapter($"SELECT type_id AS 'ID', affair_type AS 'Тип злочину-звинувачення' FROM Affair_Types", PoliceCardIndex.GetSqlConnection());
                    adapter.Fill(table);
                    Departments.ItemsSource = table.AsDataView();
                }
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var idx = Departments.SelectedIndex;

           int id = (int)table.Rows[idx]["ID"];
            if (t == "dep")
            {
                SqlCommand command = new SqlCommand($"UPDATE Detectives SET department_id = NULL WHERE department_id = {id};", PoliceCardIndex.GetSqlConnection());
                PoliceCardIndex.OpenConnection();
                command.ExecuteNonQuery();

                command = new SqlCommand($"DELETE FROM Departments WHERE department_id = {id};", PoliceCardIndex.GetSqlConnection());
                command.ExecuteNonQuery();
                PoliceCardIndex.CloseConnection();

                table.Rows[idx].Delete();
                Departments.ItemsSource = table.AsDataView();
            }
            else if (t == "spec")
            {
                SqlCommand command = new SqlCommand($"UPDATE Detectives SET type_id = NULL WHERE type_id = {id};", PoliceCardIndex.GetSqlConnection());
                PoliceCardIndex.OpenConnection();
                command.ExecuteNonQuery();

                command = new SqlCommand($"DELETE FROM Affair_Types WHERE type_id = {id};", PoliceCardIndex.GetSqlConnection());
                command.ExecuteNonQuery();
                PoliceCardIndex.CloseConnection();

                table.Rows[idx].Delete();
                Departments.ItemsSource = table.AsDataView();
            }

        }


        private void Departments_LostFocus(object sender, RoutedEventArgs e)
        {
            if (t == "dep")
            {
                var idx = Departments.SelectedIndex;

                if (idx == -1 || idx == 0 || idx > (int)table.Rows.Count - 1)
                    return;

                DataRowView item = Departments.Items[idx] as DataRowView;

                if (item == null)
                    return;
                try
                {
                    int id = (int)table.Rows[idx]["ID"];

                    if (Convert.ToString(item.Row.ItemArray[1]) != "" && Convert.ToString(item.Row.ItemArray[0]) != "")
                    {
                        SqlCommand command = new SqlCommand($"UPDATE Departments SET department_name = '{(string)item.Row.ItemArray[1]}', post_index =  '{((item.Row.ItemArray[2] is int) ? item.Row.ItemArray[2] : 0)}' WHERE department_id = {id};", PoliceCardIndex.GetSqlConnection());
                        PoliceCardIndex.OpenConnection();
                        command.ExecuteNonQuery();
                        PoliceCardIndex.CloseConnection();
                        // MessageBox.Show("ok");
                    }
                }
                catch (Exception ex)
                {
                    return;
                }

            }

            else if (t == "spec")
            {
                var idx = Departments.SelectedIndex;

                if (idx == -1 || idx == 0 || idx > (int)table.Rows.Count - 1)
                    return;

                DataRowView item = Departments.Items[idx] as DataRowView;

                if (item == null)
                    return;
                try
                {
                    int id = (int)table.Rows[idx]["ID"];

                    if (Convert.ToString(item.Row.ItemArray[1]) != "" && Convert.ToString(item.Row.ItemArray[0]) != "")
                    {
                        SqlCommand command = new SqlCommand($"UPDATE Affair_Types SET affair_type = '{(string)item.Row.ItemArray[1]}' WHERE type_id = {id};", PoliceCardIndex.GetSqlConnection());
                        PoliceCardIndex.OpenConnection();
                        command.ExecuteNonQuery();
                        PoliceCardIndex.CloseConnection();
                        // MessageBox.Show("ok");
                    }
                }
                catch (Exception ex)
                {
                    return;
                }

            }

        }
    }
}
