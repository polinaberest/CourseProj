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
using System.Data;
using System.Data.SqlClient;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class Statistics : Window
    {
        DataTable table = new DataTable();


        public Statistics()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter adapterD = new SqlDataAdapter($"SELECT TOP 10 WITH TIES first_name + ' ' + surname AS 'ПІ', badge_num AS 'Номер значка', a.affair_type AS 'Спеціалізація', dep.department_name AS 'Відділ міста', FORMAT(last_visit_date, 'dd.MM.yyyy    hh:mm:ss') AS 'Останній вхід' FROM Detectives d, Departments dep, Affair_Types a WHERE d.department_id = dep.department_id AND d.type_id = a.type_id ORDER BY last_visit_date DESC; ", PoliceCardIndex.GetSqlConnection());
            adapterD.Fill(table);

            Det_LastVisit.ItemsSource = table.AsDataView();
            Det_LastVisit.AutoGenerateColumns = true;
        }

        private void BackInMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void Refresh_Det_Click(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter adapterD = new SqlDataAdapter($"SELECT TOP 10 WITH TIES first_name + ' ' + surname AS 'ПІ', badge_num AS 'Номер значка', a.affair_type AS 'Спеціалізація', dep.department_name AS 'Відділ міста', FORMAT(last_visit_date, 'dd.MM.yyyy    hh:mm:ss') AS 'Останній вхід' FROM Detectives d, Departments dep, Affair_Types a WHERE d.department_id = dep.department_id AND d.type_id = a.type_id ORDER BY last_visit_date DESC; ", PoliceCardIndex.GetSqlConnection());
            table.Clear();
            adapterD.Fill(table);

            Det_LastVisit.ItemsSource = table.AsDataView();
        }
    }
}
