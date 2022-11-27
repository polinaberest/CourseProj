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
    /// Interaction logic for Archived.xaml
    /// </summary>
    public partial class Archived : Window
    {
        string query = $"SELECT c.criminal_id, first_name, surname, nickname, add_date, height, eye_color, hair_color, special_feature, citizenship, birth_date, birth_place, last_accomodation, criminal_job, is_in_band, band_name, c.band_id, crime_role, cr.crime_id, cr.type_id, t.affair_type, title, commit_date, detective_id INTO Archive FROM (((Criminals c LEFT JOIN Bands b ON (c.band_id = b.band_id)) LEFT JOIN Participants p ON (c.criminal_id = p.criminal_id)) LEFT JOIN Crimes cr ON (cr.crime_id = p.crime_id)) LEFT JOIN Affair_Types t ON (cr.type_id = t.type_id) WHERE is_archived = 1;";

        string delQuery = "DROP TABLE Archive";

        string initQuery;

        string ids = "";

        int dateClick = 0;
        int nameClick = 0;
        int specClick = 0;

        // конструктор класу Archived
        public Archived()
        {
            InitializeComponent();

            SqlCommand command = new SqlCommand(query, PoliceCardIndex.GetSqlConnection());
            SqlCommand delCommand = new SqlCommand(delQuery, PoliceCardIndex.GetSqlConnection());

            PoliceCardIndex.OpenConnection();
            delCommand.ExecuteNonQuery();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();

            PoliceCardIndex.IdxsArchived.Clear();
            FormIdxList();
            if (PoliceCardIndex.IdxsArchived.Count > 0)
            {
                foreach (int i in PoliceCardIndex.IdxsArchived)
                {
                    if (i == PoliceCardIndex.IdxsArchived.Last())
                    {
                        ids += i.ToString();
                        break;
                    }
                    ids += i.ToString() + ", ";
                }

                initQuery = $"SELECT c.criminal_id, CONCAT(first_name, ' ', surname, ' (', nickname, ')  ', ' Злочинів: ') as description, COUNT(p.criminal_id) as count ,  ' Архівовано: ' as date_text,  c.archivation_date as date INTO FoundA  FROM Criminals c, Participants p WHERE c.criminal_id = p.criminal_id AND c.criminal_id IN ({ids}) GROUP BY c.criminal_id, first_name, surname, nickname, c.archivation_date UNION SELECT c.criminal_id, CONCAT(first_name, ' ', surname, ' (', nickname, ')  ', '  Злочинів: ') as description, 0 as count , '   Архівовано: ' as date_text,  c.archivation_date as date FROM Criminals c  WHERE c.criminal_id NOT IN (SELECT DISTINCT criminal_id FROM Participants) AND c.criminal_id IN ({ids});";
            }
            

            DisplayArchive("description");
        }

        private void DisplayArchive(string sortBy)
        {
            if (PoliceCardIndex.IdxsArchived.Count == 0)
            {
                NothingFound.Visibility = Visibility.Visible;
                return;
            }
            int i = 1;

            //зачистка з попереднього разу
            foreach (object el in ArchivedList.Children)
            {
                if (el is Button)
                {
                    ((Button)el).Visibility = Visibility.Collapsed;
                }
            }

            SqlCommand command = new SqlCommand(initQuery, PoliceCardIndex.GetSqlConnection());
            SqlCommand delCommand = new SqlCommand("DROP TABLE FoundA;", PoliceCardIndex.GetSqlConnection());


            PoliceCardIndex.OpenConnection();
            delCommand.ExecuteNonQuery();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM FoundA ORDER BY {sortBy};", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);

            List<int> idsSorted = new List<int>();

            for (int k = 0; k < table.Rows.Count; k++)
            {
                idsSorted.Add((int)table.Rows[k]["criminal_id"]);
            }


            foreach (int idx in idsSorted)
            {
                Button button = new Button();
                button.Content = i + ". " + ShowDetails(idx);
                button.FontSize = 20;
                button.HorizontalContentAlignment = HorizontalAlignment.Left;
                button.Margin = new Thickness(3);
                Color color = Color.FromRgb(255, 165, 0);
                button.Background = new SolidColorBrush(color);
                button.MaxWidth = 1100;
                button.Tag = idx;
                button.ToolTip = "Переглянути, редагувати, архівувати, видалити анкету - натисніть ліву клавішу миші.";
                button.Click += Button_Click;
                ArchivedList.Children.Add(button);
                i++;
            }
        }

        private void FormIdxList()
        {
            PoliceCardIndex.IdxsArchived.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT criminal_id FROM Archive;", PoliceCardIndex.GetSqlConnection());
            DataTable tableID = new DataTable();
            adapter.Fill(tableID);

            for (int i = 0; i < tableID.Rows.Count; i++)
            {
                PoliceCardIndex.IdxsArchived.Add((int)tableID.Rows[i]["criminal_id"]);

            }

            PoliceCardIndex.IdxsArchived = PoliceCardIndex.IdxsArchived.Distinct().ToList();
        }

        private string ShowDetails(int id)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM FoundA WHERE criminal_id = {id};", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            string description = table.Rows[0]["description"].ToString() + table.Rows[0]["count"].ToString() + table.Rows[0]["date_text"].ToString() + ((DateTime)table.Rows[0]["date"]).ToString("dd.MM.yyyy");

            return description;
        }

        private void ShowDetSpecializations()
        {
            List<int> crIDs = new List<int>();
            int type_id = 0;
            SqlDataAdapter adapterA = new SqlDataAdapter($"SELECT type_id FROM Detectives WHERE detective_id = {PoliceCardIndex.DetectiveID};", PoliceCardIndex.GetSqlConnection());
            DataTable tableAffairID = new DataTable();
            adapterA.Fill(tableAffairID);
            var di = tableAffairID.Rows[0]["type_id"];
            try
            {
                type_id = (int)di;
            }
            catch (Exception ex)
            {
                type_id = 0;
                MessageBox.Show("Неможливо визначити вашу спеціалізацію");
            }

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT criminal_id FROM Archive WHERE type_id = {type_id};", PoliceCardIndex.GetSqlConnection());
            DataTable tableID = new DataTable();
            adapter.Fill(tableID);

            for (int f = 0; f < tableID.Rows.Count; f++)
            {
                crIDs.Add((int)tableID.Rows[f]["criminal_id"]);
            }

            crIDs = crIDs.Distinct().ToList();

            if (crIDs.Count < 1)
                return;

            string iQuery;
            string idss = "";

            foreach (int k in crIDs)
                {
                    if (k == crIDs.Last())
                    {
                        idss += k.ToString();
                        break;
                    }
                    idss += k.ToString() + ", ";
                }

             iQuery = $"SELECT c.criminal_id, CONCAT(first_name, ' ', surname, ' (', nickname, ')  ', ' Злочинів: ') as description, COUNT(p.criminal_id) as count ,  ' Архівовано: ' as date_text,  c.archivation_date as date INTO FoundA  FROM Criminals c, Participants p WHERE c.criminal_id = p.criminal_id AND c.criminal_id IN ({idss}) GROUP BY c.criminal_id, first_name, surname, nickname, c.archivation_date UNION SELECT c.criminal_id, CONCAT(first_name, ' ', surname, ' (', nickname, ')  ', '  Злочинів: ') as description, 0 as count , '   Архівовано: ' as date_text,  c.archivation_date as date FROM Criminals c  WHERE c.criminal_id NOT IN (SELECT DISTINCT criminal_id FROM Participants) AND c.criminal_id IN ({idss});";
            

            int i = 1;

            //зачистка з попереднього разу
            foreach (object el in ArchivedList.Children)
            {
                if (el is Button)
                {
                    ((Button)el).Visibility = Visibility.Collapsed;
                }
            }

            SqlCommand command = new SqlCommand(iQuery, PoliceCardIndex.GetSqlConnection());
            SqlCommand delCommand = new SqlCommand("DROP TABLE FoundA;", PoliceCardIndex.GetSqlConnection());


            PoliceCardIndex.OpenConnection();
            delCommand.ExecuteNonQuery();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();

            SqlDataAdapter adapterF = new SqlDataAdapter($"SELECT * FROM FoundA;", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapterF.Fill(table);

            List<int> idsSorted = new List<int>();

            for (int k = 0; k < table.Rows.Count; k++)
            {
                idsSorted.Add((int)table.Rows[k]["criminal_id"]);
            }


            foreach (int idx in idsSorted)
            {
                Button button = new Button();
                button.Content = i + ". " + ShowDetails(idx);
                button.FontSize = 20;
                button.HorizontalContentAlignment = HorizontalAlignment.Left;
                button.Margin = new Thickness(3);
                Color color = Color.FromRgb(255, 165, 0);
                button.Background = new SolidColorBrush(color);
                button.MaxWidth = 1100;
                button.Tag = idx;
                button.ToolTip = "Переглянути, редагувати, архівувати, видалити анкету - натисніть ліву клавішу миші.";
                button.Click += Button_Click;
                ArchivedList.Children.Add(button);
                i++;
            }
        }

        // обробники подій
        private void BackInMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = (int)((Control)sender).Tag;
            MessageBoxResult result = MessageBox.Show(
                "Ви впевнені, що хочете перемістити анкету з архіву до основної картотеки ?\n",
                "Підтверждення деархівації", MessageBoxButton.OKCancel);
            
            if (result == MessageBoxResult.OK)
            {
                PoliceCardIndex.Unarchive(id);
                MessageBox.Show("Справу деархівовано");
                Archived arch = new Archived();
                arch.Show();
                this.Close();
            }            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
          /*  PoliceCardIndex.WriteToFile("criminals.txt");
            PoliceCardIndex.WriteToFile("archived.txt");*/
        }

        private void bySpecialization_Click(object sender, RoutedEventArgs e)
        {
            
            specClick += 1;
            if (specClick % 2 == 1)
            {
                byDate.Visibility = Visibility.Hidden;
                byName.Visibility = Visibility.Hidden;
                bySpecialization.Content = "Усі";
                ShowDetSpecializations();
            }
            else
            {
                byName.Visibility = Visibility.Visible;
                byDate.Visibility = Visibility.Visible;
                bySpecialization.Content = "Моя спеціалізація";
                DisplayArchive("description");
            }
        }

        private void byName_Click(object sender, RoutedEventArgs e)
        {
            nameClick += 1;
            if (nameClick % 2 == 1)
                DisplayArchive("description");
            else
            {
                DisplayArchive("description DESC");
            }
        }

        private void byDate_Click(object sender, RoutedEventArgs e)
        {
            dateClick += 1;
            if (dateClick % 2 == 1)
                DisplayArchive("date");
            else
            {
                DisplayArchive("date DESC");
            }
        }
    }
}
