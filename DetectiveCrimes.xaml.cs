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
    /// Interaction logic for DetectiveCrimes.xaml
    /// </summary>
    public partial class DetectiveCrimes : Window
    {
        DataTable crimesTable; 
        public DetectiveCrimes()
        {
            InitializeComponent();

            crimesTable = new DataTable();
        }

        private void BackInDetAffair_Click(object sender, RoutedEventArgs e)
        {
            DetAffair detAffair = new DetAffair();
            detAffair.Show();
            this.Close();
        }

        private void Crimes_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Dismiss_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"  SELECT c.crime_id AS 'ID злочину', c.title AS 'Назва злочину', c.commit_date AS 'Дата скоєння',  a.affair_type AS 'Тип злочину', COUNT(p.participant_id) AS 'Кількість учасників' FROM Crimes c, Participants p, Affair_Types a WHERE c.crime_id = p.crime_id AND c.type_id = a.type_id AND c.detective_id = {PoliceCardIndex.DetectiveID} GROUP BY c.crime_id, c.commit_date, c.title, a.affair_type", PoliceCardIndex.GetSqlConnection());

            adapter.Fill(crimesTable);

            Crimes.ItemsSource = crimesTable.AsDataView();
            Crimes.AutoGenerateColumns = true;

            Crimes.Columns[0].IsReadOnly = true;
            Crimes.Columns[4].IsReadOnly = true;

            Crimes.CanUserDeleteRows = true;

            if (crimesTable.Rows.Count == 0)
            {
                Crimes.Visibility = Visibility.Collapsed;
                Delete.Visibility = Visibility.Collapsed;
                Dismiss.Visibility = Visibility.Collapsed;
                Save.Visibility = Visibility.Collapsed;
                NothingText.Visibility = Visibility.Visible;
            }
        }
    }
}
