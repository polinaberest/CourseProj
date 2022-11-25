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
using System.IO;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Data;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResults : Window
    {
        string ids = "";

        string initQuery;

        string delQuery = "DROP TABLE Found;";

        int dateClick = 0;
        int nameClick = 0;
        int dnClick = 0;

        // конструктор класу
        public SearchResults()
        {
            InitializeComponent();

            foreach (int i in PoliceCardIndex.IdxsFoundByRequest)
            {
                if (i == PoliceCardIndex.IdxsFoundByRequest.Last())
                { 
                    ids += i.ToString();
                    break;
                }
                ids += i.ToString() + ", ";
            }

            initQuery = $"SELECT c.criminal_id, CONCAT(first_name, ' ', surname, ' (', nickname, ')  ', ' Злочинів: ') as description, COUNT(p.criminal_id) as count ,  ' Анкету додано: ' as date_text,  c.add_date as date INTO Found  FROM Criminals c, Participants p WHERE c.criminal_id = p.criminal_id AND c.criminal_id IN ({ids}) GROUP BY c.criminal_id, first_name, surname, nickname, c.add_date UNION SELECT c.criminal_id, CONCAT(first_name, ' ', surname, ' (', nickname, ')  ', '  Злочинів: ') as description, 0 as count , '   Анкету додано: ' as date_text,  c.add_date as date FROM Criminals c  WHERE c.criminal_id NOT IN (SELECT DISTINCT criminal_id FROM Participants) AND c.criminal_id IN ({ids});";

            DisplayFound("description"); 
        }

        private void DisplayFound(string sortBy)
        {
            if (PoliceCardIndex.IdxsFoundByRequest.Count == 0)
            {
                NothingFound.Visibility = Visibility.Visible;
                TextHint.Visibility = Visibility.Hidden;
                SortParams.Visibility = Visibility.Hidden;
                Write.Visibility = Visibility.Collapsed;
                return;
            }
            int i = 1;

            //зачистка з попереднього разу
            foreach (object el in ResultsInner.Children)
            {
                if (el is Button)
                {
                    ((Button)el).Visibility = Visibility.Collapsed;
                }
            }

            SqlCommand command = new SqlCommand(initQuery, PoliceCardIndex.GetSqlConnection());
            SqlCommand delCommand = new SqlCommand(delQuery, PoliceCardIndex.GetSqlConnection());


            PoliceCardIndex.OpenConnection();
            delCommand.ExecuteNonQuery();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM Found ORDER BY {sortBy};", PoliceCardIndex.GetSqlConnection());
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
                Color color = Color.FromRgb(0, 118, 214);
                button.Background = new SolidColorBrush(color);
                button.MaxWidth = 1100;
                button.Tag = idx;
                button.ToolTip = "Переглянути, редагувати, архівувати, видалити анкету - натисніть ліву клавішу миші.";
                button.MouseRightButtonDown += Button_RMBdown;
                button.Click += Button_Click;
                ResultsInner.Children.Add(button);
                i++;
            }
        }

        private void HighlightAdded(bool isIn, Button button)
        {
            Color initColor = Color.FromRgb(0, 118, 214);
            Color newColor = Color.FromRgb(34, 139, 34);
            if (isIn)
                button.Background = new SolidColorBrush(newColor);
            else
                button.Background = new SolidColorBrush(initColor);
        }

        private string ShowDetails(int id)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM Found WHERE criminal_id = {id};", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            string description = table.Rows[0]["description"].ToString() + table.Rows[0]["count"].ToString() + table.Rows[0]["date_text"].ToString() + ((DateTime)table.Rows[0]["date"]).ToString("dd.MM.yyyy") ;

            return description;
        }

        private void BackInSearchResults_Click(object sender, RoutedEventArgs e)
        {
            SearchAffair search = new SearchAffair();
            search.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (PoliceCardIndex.IsDetective)
            {
                int idx = (int)((Control)sender).Tag;
                EditAffair edit = new EditAffair(idx);

                edit.Show();
                this.Close();
            }
            else {
                MessageBox.Show("Для перегляду повної інформації \nта редагування відомостей в картотеці \nнеобхідно увійти або \nзареєструватися як детектив");
            }
        }

        private void Button_RMBdown(object sender, RoutedEventArgs e)
        {
            int id = (int)((Control)sender).Tag;
            bool isIncluded;
           ;
            PoliceCardIndex.FormListToPrint(id, out isIncluded);
            HighlightAdded(isIncluded, (sender as Button));
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PoliceCardIndex.FoundToWrite.Clear();
        }

        private void Write_Click(object sender, RoutedEventArgs e)
        {
            if (PoliceCardIndex.FoundToWrite.Count == 0)
            {
                MessageBox.Show("Ви не обрали жодної справи!");
                return;
            }
            PoliceCardIndex.WriteResults();
        }

        private void byDate_Click(object sender, RoutedEventArgs e)
        {
            dateClick += 1;
            if (dateClick % 2 == 1)
                DisplayFound("date");
            else { 
                DisplayFound("date DESC");
            }
        }

        private void byName_Click(object sender, RoutedEventArgs e)
        {
            nameClick += 1;
            if (nameClick % 2 == 1)
                DisplayFound("description");
            else
            {
                DisplayFound("description DESC");
            }
        }
    

        private void byNameDate_Click(object sender, RoutedEventArgs e)
        {
            dnClick += 1;
            if (dnClick % 2 == 1)
                DisplayFound("description, date");
            else
            {
                DisplayFound("description DESC, date DESC");
            }
        }
    }
}
