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
        DataTable participants;

        int count = 0;

        public DetectiveCrimes()
        {
            InitializeComponent();

            crimesTable = new DataTable();
            participants = new DataTable();
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
            var idx = Crimes.SelectedIndex;

            int crime_id = (int)crimesTable.Rows[idx]["ID злочину"];

            SqlCommand commandP = new SqlCommand($"DELETE FROM Participants WHERE crime_id = {crime_id};", PoliceCardIndex.GetSqlConnection());

            SqlCommand commandC = new SqlCommand($"DELETE FROM Crimes WHERE crime_id = {crime_id};", PoliceCardIndex.GetSqlConnection());

            PoliceCardIndex.OpenConnection();
            commandP.ExecuteNonQuery();
            commandC.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();

            crimesTable.Rows[idx].Delete();
            Crimes.ItemsSource = crimesTable.AsDataView();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var idx = Crimes.SelectedIndex;

            DateTime date;
            int type_id;

            if (idx == -1 || idx > (int)crimesTable.Rows.Count - 1)
                return;

            DataRowView item = Crimes.Items[idx] as DataRowView;

            if (item == null)
                return;
            try
            {
                int idC = (int)crimesTable.Rows[idx]["ID злочину"];

                if (Convert.ToString(item.Row.ItemArray[1]) != "" && Convert.ToString(item.Row.ItemArray[0]) != ""
                    && Convert.ToString(item.Row.ItemArray[2]) != ""
                    && Convert.ToString(item.Row.ItemArray[3]) != ""
                    && DateTime.TryParse((Convert.ToString(item.Row.ItemArray[2])), out date))
                {
                    ExtensionsToCheckInput.InsertIfUnique(Convert.ToString(item.Row.ItemArray[3]).Trim(), "Affair_Types", "affair_type");
                    type_id = ExtensionsToCheckInput.GetIdForTextItems("Affair_Types", "type_id", "affair_type", Convert.ToString(item.Row.ItemArray[3]).Trim());

                    string sqlFormattedDate = date.ToString("yyyy-MM-dd hh:mm:ss.fff");

                    SqlCommand commandC = new SqlCommand($"UPDATE Crimes SET title = '{(string)item.Row.ItemArray[1]}', type_id = {type_id}, commit_date =  '{sqlFormattedDate}' WHERE crime_id = {idC};", PoliceCardIndex.GetSqlConnection());
                    PoliceCardIndex.OpenConnection();
                    commandC.ExecuteNonQuery();
                    PoliceCardIndex.CloseConnection();
                    // MessageBox.Show("ok");
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void Dismiss_Click(object sender, RoutedEventArgs e)
        {
            var idx = Crimes.SelectedIndex;

            int crime_id = (int)crimesTable.Rows[idx]["ID злочину"];

            SqlCommand commandC = new SqlCommand($"UPDATE Crimes SET detective_id = NULL WHERE crime_id = {crime_id};", PoliceCardIndex.GetSqlConnection());

            PoliceCardIndex.OpenConnection();
            commandC.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();

            crimesTable.Rows[idx].Delete();
            Crimes.ItemsSource = crimesTable.AsDataView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT c.crime_id AS 'ID злочину', c.title AS 'Назва злочину', c.commit_date AS 'Дата скоєння',  a.affair_type AS 'Тип злочину', COUNT(p.participant_id) AS 'Кількість учасників' FROM Crimes c, Participants p, Affair_Types a WHERE c.crime_id = p.crime_id AND c.type_id = a.type_id AND c.detective_id = {PoliceCardIndex.DetectiveID} GROUP BY c.crime_id, c.commit_date, c.title, a.affair_type", PoliceCardIndex.GetSqlConnection());

            adapter.Fill(crimesTable);

            Crimes.ItemsSource = crimesTable.AsDataView();
            Crimes.AutoGenerateColumns = true;

            Crimes.Columns[0].IsReadOnly = true;
            Crimes.Columns[4].IsReadOnly = true;

            Crimes.CanUserDeleteRows = true;

            NothingText.Visibility = Visibility.Collapsed;
            participantsGrid.Visibility = Visibility.Collapsed;

            if (crimesTable.Rows.Count == 0)
            {
                Crimes.Visibility = Visibility.Collapsed;
                Delete.Visibility = Visibility.Collapsed;
                Dismiss.Visibility = Visibility.Collapsed;
                Save.Visibility = Visibility.Collapsed;
                NothingText.Visibility = Visibility.Visible;
            }
        }

        private void See_Click(object sender, RoutedEventArgs e)
        {
            count++;
            string selectQuery;

            if (count % 2 == 1)
            {
                var idx = Crimes.SelectedIndex;
                int crime_id = (int)crimesTable.Rows[idx]["ID злочину"];

                participants.Clear();

                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT c.criminal_id AS 'ID злочинця', CONCAT(c.first_name, ' ', c.surname, ' (', c.nickname, ') ') AS 'Злочинець', p.crime_role AS 'Роль у злочині', is_archived AS 'Затримано', add_date AS 'Дата початку розшуку', CONCAT(height, ' см зростом, очі: ', eye_color, ', волосся: ', hair_color, '. Особливість: ', special_feature ) AS 'Прикмети', citizenship AS 'Громадянство', last_accomodation AS 'Останнє місце проживання',  criminal_job AS 'Злочинна професія', b.band_name AS 'Член злочинного угруповання' FROM Criminals c, Participants p, Bands b WHERE p.crime_id = {crime_id} AND p.criminal_id = c.criminal_id AND c.band_id = b.band_id UNION SELECT c.criminal_id AS 'ID злочинця', CONCAT(c.first_name, ' ', c.surname, ' (', c.nickname, ') ') AS 'Злочинець', p.crime_role AS 'Роль у злочині', is_archived AS 'Затримано', add_date AS 'Дата початку розшуку', CONCAT(height, ' см зростом, очі: ', eye_color, ', волосся: ', hair_color, '. Особливість: ', special_feature ) AS 'Прикмети', citizenship AS 'Громадянство', last_accomodation AS 'Останнє місце проживання',  criminal_job AS 'Злочинна професія', '-' AS 'Член злочинного угруповання' FROM Criminals c, Participants p WHERE p.crime_id = {crime_id} AND p.criminal_id = c.criminal_id AND is_in_band = '{false}'", PoliceCardIndex.GetSqlConnection());

                adapter.Fill(participants);

                participantsGrid.ItemsSource = participants.AsDataView();
                participantsGrid.AutoGenerateColumns = true;
                participantsGrid.Visibility = Visibility.Visible;
            }
            else { 
                participants.Clear();
                participantsGrid.Visibility = Visibility.Collapsed;
            }

            
        }
    }
}
