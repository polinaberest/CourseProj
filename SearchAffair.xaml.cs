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
using System.Windows.Media.Animation;
using System.Data;
using System.Data.SqlClient;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for SearchAffair.xaml
    /// </summary>
    public partial class SearchAffair : Window
    {
        string query = $"SELECT c.criminal_id, first_name, surname, nickname, add_date, height, eye_color, hair_color, special_feature, citizenship, birth_date, birth_place, last_accomodation, criminal_job, is_in_band, band_name, c.band_id, crime_role, cr.crime_id, cr.type_id, t.affair_type, title, commit_date, detective_id INTO Request FROM (((Criminals c LEFT JOIN Bands b ON (c.band_id = b.band_id)) LEFT JOIN Participants p ON (c.criminal_id = p.criminal_id)) LEFT JOIN Crimes cr ON (cr.crime_id = p.crime_id)) LEFT JOIN Affair_Types t ON (cr.type_id = t.type_id) WHERE is_archived = 0;";

        string delQuery = "DROP TABLE Request";

        int[] intRange;

        DateTime today = DateTime.Now;
        string[] birthDateLimits;

        string[] addDateLimits;

        string[] crimeDateLimits;

        bool defHeight = false;
        bool defAddDate = false;
        bool defAge = false;

        DateTime? selectedCrDate;

        // конструктор класу
        public SearchAffair()
        {
            InitializeComponent();
            checkBoxIsInBand.Checked += checkBoxIsInBand_Checked;
            checkBoxIsInBand.Unchecked += checkBoxIsInBand_Unchecked;
        
            // анімація нопки пошуку
            DoubleAnimation btnAnimation = new DoubleAnimation();
            btnAnimation.From = 0;
            btnAnimation.To = 1;
            btnAnimation.Duration = TimeSpan.FromSeconds(2);
            SearchData.BeginAnimation(Button.OpacityProperty, btnAnimation);
        }

        private void Search()
        {

            if (!CheckAbsoluteEmpty())
            {
                SqlCommand command = new SqlCommand(query, PoliceCardIndex.GetSqlConnection());
                SqlCommand delCommand = new SqlCommand(delQuery, PoliceCardIndex.GetSqlConnection());

                PoliceCardIndex.OpenConnection();
                delCommand.ExecuteNonQuery();
                command.ExecuteNonQuery();
                PoliceCardIndex.CloseConnection();

                PoliceCardIndex.IdxsFoundByRequest.Clear();

                if (textBoxName.Text.Trim() != "" && textBoxName.Text.Trim() != null)
                {
                    DeleteUnnecessary("first_name", textBoxName.Text.Trim());
                }
                if (textBoxSurname.Text.Trim() != "" && textBoxSurname.Text.Trim() != null)
                {
                    DeleteUnnecessary("surname", textBoxSurname.Text.Trim());
                }
                if (textBoxNickname.Text.Trim() != "" && textBoxNickname.Text.Trim() != null)
                {
                    DeleteUnnecessary("nickname", textBoxNickname.Text.Trim());
                }
                if (ComboBoxHairColor.Text.Trim() != "" && ComboBoxHairColor.Text.Trim() != null)
                {
                    DeleteUnnecessary("hair_color", ComboBoxHairColor.Text.Trim());
                }
                if (ComboBoxEyeColor.Text.Trim() != "" && ComboBoxEyeColor.Text.Trim() != null)
                {
                    DeleteUnnecessary("eye_color", ComboBoxEyeColor.Text.Trim());
                }
                if (textBoxSpecialFeatures.Text.Trim() != "" && textBoxSpecialFeatures.Text.Trim() != null)
                {
                    DeleteUnnecessary("special_feature", textBoxSpecialFeatures.Text.Trim());
                }
                if (textBoxCitizenship.Text.Trim() != "" && textBoxCitizenship.Text.Trim() != null)
                {
                    DeleteUnnecessary("citizenship", textBoxCitizenship.Text.Trim());
                }
                if (textBoxBirthPlace.Text.Trim() != "" && textBoxBirthPlace.Text.Trim() != null)
                {
                    DeleteUnnecessary("birth_place", textBoxBirthPlace.Text.Trim());
                }
                if (textBoxLastAccomodation.Text.Trim() != "" && textBoxLastAccomodation.Text.Trim() != null)
                {
                    DeleteUnnecessary("last_accomodation", textBoxLastAccomodation.Text.Trim());
                }
                if (textBoxJob.Text.Trim() != "" && textBoxJob.Text.Trim() != null)
                {
                    DeleteUnnecessary("criminal_job", textBoxJob.Text.Trim());
                }
                if ((bool)checkBoxIsInBand.IsChecked && textBoxBandName.Text.Trim() != "" && textBoxBandName.Text.Trim() != null)
                {
                    DeleteUnnecessary("band_name", textBoxBandName.Text.Trim());
                }
                if (ComboBoxRole.Text.Trim() != "" && ComboBoxRole.Text.Trim() != null)
                {
                    DeleteUnnecessary("crime_role", ComboBoxRole.Text.Trim());
                }
                if (ComboBoxSpeciality.Text.Trim() != "" && ComboBoxSpeciality.Text.Trim() != null)
                {
                    DeleteUnnecessary("affair_type", ComboBoxSpeciality.Text.Trim());
                }
                if (textBoxTitle.Text.Trim() != "" && textBoxTitle.Text.Trim() != null)
                {
                    DeleteUnnecessary("title", textBoxTitle.Text.Trim());
                }
                if (defAddDate)
                {
                    DeleteUnnecessary("add_date", addDateLimits);
                }
                if (defAge)
                {
                    DeleteUnnecessary("birth_date", birthDateLimits);
                }
                if (defHeight)
                {
                    DeleteUnnecessary("height", intRange);
                }
                if (dateCrPicker.SelectedDate != null)
                {
                    string crDate = dateCrPicker.SelectedDate?.ToString("yyyy-MM-dd"); // like date%
                    DeleteUnnecessaryLike(crDate);
                }

                FormIdxList();

                SearchResults results = new SearchResults();
                results.Show();
                this.Close();
            }
            else
                MessageBox.Show("Нема інформації для здійснення пошуку!");
        }

        private void DeleteUnnecessary(string column, string value)
        {
            string delRowsQueryT = $"DELETE FROM Request WHERE {column} <> '{value}';";

            SqlCommand delCommand = new SqlCommand(delRowsQueryT, PoliceCardIndex.GetSqlConnection());

            PoliceCardIndex.OpenConnection();
            delCommand.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();
        }

        private void DeleteUnnecessary(string column, string[] range)
        {
            string delRowsQueryT = $"DELETE FROM Request WHERE {column} NOT BETWEEN '{range[0]}' AND '{range[1]}';";

            SqlCommand delCommand = new SqlCommand(delRowsQueryT, PoliceCardIndex.GetSqlConnection());

            PoliceCardIndex.OpenConnection();
            delCommand.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();
        }

        private void DeleteUnnecessary(string column, int[] range)
        {
            string delRowsQueryT = $"DELETE FROM Request WHERE {column} NOT BETWEEN {range[0]} AND {range[1]};";

            SqlCommand delCommand = new SqlCommand(delRowsQueryT, PoliceCardIndex.GetSqlConnection());

            PoliceCardIndex.OpenConnection();
            delCommand.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();
        }

        private void DeleteUnnecessaryLike(string mask)
        {
            string delRowsQueryT = $"DELETE FROM Request WHERE CONVERT(VARCHAR(25), reg_date, 126) like '{mask}';";

            SqlCommand delCommand = new SqlCommand(delRowsQueryT, PoliceCardIndex.GetSqlConnection());

            PoliceCardIndex.OpenConnection();
            delCommand.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();
        }

        private void FormIdxList()
        {
            PoliceCardIndex.IdxsFoundByRequest.Clear();

            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT criminal_id FROM Request;", PoliceCardIndex.GetSqlConnection());
            DataTable tableID = new DataTable();
            adapter.Fill(tableID);

            for (int i = 0; i < tableID.Rows.Count; i++)
            {
                PoliceCardIndex.IdxsFoundByRequest.Add((int)tableID.Rows[i]["criminal_id"]);

            }

            PoliceCardIndex.IdxsFoundByRequest = PoliceCardIndex.IdxsFoundByRequest.Distinct().ToList();
        }

        private bool CheckAbsoluteEmpty()
        {
            int count = 0;
            
            if (!defAge)
            {
                count++;
            }
            if (!defHeight)
            {
                count++;
            }
            if (!defAddDate)
            {
                count++;
            }
            if (selectedCrDate == null)
            {
                count++;
            }
            foreach (object el in Form.Children)
            {
                if (el is TextBox)
                {
                    TextBox box = (TextBox)el;
                    if (box.Text == "" || box.Text == null)
                        count++;
                }
                else if (el is ComboBox)
                {
                    ComboBox box = (ComboBox)el;
                    if (box.Text == "" || box.Text == null)
                        count++;
                }
            }
            if ((bool)checkBoxIsInBand.IsChecked && count == 18)
            {
                return false;
            }
            else if ((!(bool)checkBoxIsInBand.IsChecked) && count == 18)
            {
                return true;
            }
            return false;
        }

        

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            defHeight = false;
            intRange = new int[] {  -1, -1 };
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            defHeight = true;
            intRange = new int[] { 0, 140 };
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            defHeight = true;
            intRange = new int[] { 141, 160 };
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            defHeight = true;
            intRange = new int[] { 161, 180 };
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            defHeight = true;
            intRange = new int[] { 181, 270 };
        }

        private void checkBoxIsInBand_Checked(object sender, RoutedEventArgs e)
        {
            textBoxBandName.Visibility = Visibility.Visible;
        }

        private void checkBoxIsInBand_Unchecked(object sender, RoutedEventArgs e)
        {
            textBoxBandName.Visibility = Visibility.Hidden;
        }

        private void SearchData_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void SearchMainRoot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Search();
            }
        }

        private void BackInSearchForm_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        //границі дати додавання справ
        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            defAddDate = false;

        }

        private void RadioButton_Checked_6(object sender, RoutedEventArgs e)
        {
            defAddDate = true;
            addDateLimits = new string[] { today.ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd") };
        }

        private void RadioButton_Checked_7(object sender, RoutedEventArgs e)
        {
            defAddDate = true;
            addDateLimits = new string[] { today.AddDays(-31).ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd") };
        }

        private void RadioButton_Checked_8(object sender, RoutedEventArgs e)
        {
            defAddDate = true;
            addDateLimits = new string[] { today.AddYears(-100).ToString("yyyy-MM-dd"), today.AddDays(-32).ToString("yyyy-MM-dd") };
        }

        //гранниці віку злочинців
        private void RadioButton_Checked_9(object sender, RoutedEventArgs e)
        {
            defAge = false;
            
        }

        private void RadioButton_Checked_10(object sender, RoutedEventArgs e)
        {
            defAge = true;
            birthDateLimits = new string[] { today.AddYears(-18).ToString("yyyy-MM-dd"), today.ToString("yyyy-MM-dd") };
        }

        private void RadioButton_Checked_11(object sender, RoutedEventArgs e)
        {
            defAge = true;
            birthDateLimits = new string[] { today.AddYears(-30).ToString("yyyy-MM-dd"), today.AddYears(-18).ToString("yyyy-MM-dd") };
        }

        private void RadioButton_Checked_12(object sender, RoutedEventArgs e)
        {
            defAge = true;
            birthDateLimits = new string[] { today.AddYears(-50).ToString("yyyy-MM-dd"), today.AddYears(-30).ToString("yyyy-MM-dd") };
        }

        private void RadioButton_Checked_13(object sender, RoutedEventArgs e)
        {
            defAge = true;
            birthDateLimits = new string[] { today.AddYears(-120).ToString("yyyy-MM-dd"), today.AddYears(-50).ToString("yyyy-MM-dd") };
        }

        private void ComboBoxRole_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxSpeciality_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dateCrPicker_LostFocus(object sender, RoutedEventArgs e)
        {
            selectedCrDate = dateCrPicker.SelectedDate;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.GetComboItems("affair_type", "Affair_Types", ComboBoxSpeciality);
        }
    }
}
