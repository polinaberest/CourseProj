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
    /// Interaction logic for EditAffair.xaml
    /// </summary>
    public partial class EditAffair : Window
    {
        //internal Criminal processed;
        int id = 0;
        private bool isEdited;
        private bool bandNameIsEdited;

        DataTable tableCr = new DataTable();

        DataTable crimesTable = new DataTable();

        string Name;
        string Surname;
        string Nickname;
        int Height;
        string AddDateS; // сдельть лейбл и дать ему текст
        string EyeColor;
        string HairColor;
        string SpecialFeature;
        string Citizenship;
        string BirthDate;
        string BirthPlace;
        string LastAccomodation;
        string CriminalJob;
        bool IsInBand;
        int BandID;
        string BandName;

        // конструктор класу з параметром
        public EditAffair(int id)
        {
            InitializeComponent();
            this.id = id;

            checkBoxAmResponsibleToNew.Visibility = Visibility.Hidden;

            FillAllFields(id);
        }

       private void Edit()
        {
            if (textBoxBandName.Text == "")
                textBoxBandName.Text = "НЕ є членом банди";

            DateTime birth = (DateTime)birthPicker.SelectedDate;
            string dateOfBirth = birth.ToString("yyyy-MM-dd");

            if (!isEdited)
            {
                MessageBox.Show("Ви не внесли жодних змін!");
                return;
            }
            if (IsReadyToBeEdited())
            {
                if ((IsInBand == false && !bandNameIsEdited)
                   || (IsInBand && !bandNameIsEdited)) //якщо в назві банди не відбулося змін
                {
                    
                    //update всього крім банди 
                    SqlCommand command = new SqlCommand($"UPDATE Criminals SET first_name = '{textBoxName.Text.Trim()}', surname = '{textBoxSurname.Text.Trim()}', nickname = '{textBoxNickname.Text.Trim()}', height = {int.Parse(textBoxHeight.Text.Trim())}, eye_color = '{ComboBoxEyeColor.Text.Trim()}', hair_color = '{ComboBoxHairColor.Text.Trim()}', special_feature = '{textBoxSpecialFeatures.Text.Trim()}', citizenship = '{textBoxCitizenship.Text.Trim()}', birth_date = '{dateOfBirth}', birth_place = '{textBoxBirthPlace.Text.Trim()}', last_accomodation = '{textBoxLastAccomodation.Text.Trim()}', criminal_job = '{textBoxJob.Text.Trim()}' WHERE criminal_id = {id};", PoliceCardIndex.GetSqlConnection());
                    PoliceCardIndex.OpenConnection();
                    command.ExecuteNonQuery();
                    PoliceCardIndex.CloseConnection();

                    MessageBox.Show("Зміни збережено! - без змін банди");
                }
                else
                {
                    if (bandNameIsEdited && textBoxBandName.Text.Trim() != "НЕ є членом банди")
                    {
                        if (textBoxBandName.Text.Trim() != "НЕ є членом банди" && textBoxBandName.Background != Brushes.Salmon && textBoxBandName.Text.Trim() != "")
                        {
                            ExtensionsToCheckInput.InsertIfUnique(textBoxBandName, "Bands", "band_name");
                        }

                        int b_id = ExtensionsToCheckInput.GetIdForTextItems("Bands", "band_id", "band_name", textBoxBandName.Text.Trim());

                        SqlCommand command = new SqlCommand($"UPDATE Criminals SET first_name = '{textBoxName.Text.Trim()}', surname = '{textBoxSurname.Text.Trim()}', nickname = '{textBoxNickname.Text.Trim()}', height = {int.Parse(textBoxHeight.Text.Trim())}, eye_color = '{ComboBoxEyeColor.Text.Trim()}', hair_color = '{ComboBoxHairColor.Text.Trim()}', special_feature = '{textBoxSpecialFeatures.Text.Trim()}', citizenship = '{textBoxCitizenship.Text.Trim()}', birth_date = '{dateOfBirth}', birth_place = '{textBoxBirthPlace.Text.Trim()}', last_accomodation = '{textBoxLastAccomodation.Text.Trim()}', criminal_job = '{textBoxJob.Text.Trim()}', is_in_band = '{true}', band_id = {b_id} WHERE criminal_id = {id};", PoliceCardIndex.GetSqlConnection());
                        PoliceCardIndex.OpenConnection();
                        command.ExecuteNonQuery();
                        PoliceCardIndex.CloseConnection();

                        MessageBox.Show("Зміни збережено! - зміна назви банди");
                    }
                    else if (bandNameIsEdited && textBoxBandName.Text.Trim() == "НЕ є членом банди")
                    {
                        SqlCommand command = new SqlCommand($"UPDATE Criminals SET first_name = '{textBoxName.Text.Trim()}', surname = '{textBoxSurname.Text.Trim()}', nickname = '{textBoxNickname.Text.Trim()}', height = {int.Parse(textBoxHeight.Text.Trim())}, eye_color = '{ComboBoxEyeColor.Text.Trim()}', hair_color = '{ComboBoxHairColor.Text.Trim()}', special_feature = '{textBoxSpecialFeatures.Text.Trim()}', citizenship = '{textBoxCitizenship.Text.Trim()}', birth_date = '{dateOfBirth}', birth_place = '{textBoxBirthPlace.Text.Trim()}', last_accomodation = '{textBoxLastAccomodation.Text.Trim()}', criminal_job = '{textBoxJob.Text.Trim()}', is_in_band = '{false}', band_id = NULL WHERE criminal_id = {id};", PoliceCardIndex.GetSqlConnection());
                        PoliceCardIndex.OpenConnection();
                        command.ExecuteNonQuery();
                        PoliceCardIndex.CloseConnection();
                    }
                }
                //BackInResultsForm_Click(null, null);
                PoliceCardIndex.SelectCriminalProps(id, out Name, out Surname, out Nickname, out AddDateS, out Height, out EyeColor, out HairColor, out SpecialFeature, out Citizenship, out BirthDate, out BirthPlace, out LastAccomodation, out CriminalJob, out IsInBand, out BandID, out BandName);
            }
            else
            {
                MessageBox.Show("Внесені зміни не є коректними!");
            }
        }

       private void Archive()
        {
            if (isEdited)
            {
                MessageBox.Show("Внесені зміни не буде збережено при архівуванні! Спершу збережіть зміни!");
                return;
            }
            PoliceCardIndex.ArchiveAffair(id);
            MessageBox.Show("Справу архівовано");
        }

        private void FillAllFields(int id)
        {
            
            PoliceCardIndex.SelectCriminalProps(id, out Name, out Surname, out Nickname, out AddDateS, out Height, out EyeColor, out HairColor,  out SpecialFeature, out Citizenship, out BirthDate, out BirthPlace, out LastAccomodation, out CriminalJob, out IsInBand, out BandID, out BandName);

            textBoxName.Text = Name;
            textBoxSurname.Text = Surname;
            textBoxNickname.Text = Nickname;
            AddDate.Text += AddDateS.Substring(0,10);
            textBoxHeight.Text = Height.ToString();
            ComboBoxEyeColor.Text = EyeColor;
            ComboBoxHairColor.Text = HairColor;
            textBoxSpecialFeatures.Text = SpecialFeature;
            textBoxCitizenship.Text = Citizenship;
            birthPicker.SelectedDate = Convert.ToDateTime(BirthDate);
            textBoxBirthPlace.Text = BirthPlace;
            textBoxLastAccomodation.Text = LastAccomodation;
            textBoxJob.Text = CriminalJob;

            if (IsInBand)
            {
                textBoxBandName.Text = BandName;
                this.BandID = BandID;
            }
            else if (!IsInBand)
            {
                textBoxBandName.Text = "НЕ є членом банди";
            }
            isEdited = false;
            bandNameIsEdited = false;
        }

        private bool IsReadyToBeEdited()
        {
            int count = 0;

            foreach (object el in Form.Children)
            {
                if (el is TextBox)
                {
                    TextBox tb = (TextBox)el;
                    if (tb.Background == Brushes.Transparent && tb.Text != null && tb.Text != "")
                    {
                        count++;
                    }
                }
                else if (el is ComboBox)
                {
                    ComboBox cb = (ComboBox)el;
                    if (cb.Background == Brushes.Transparent && cb.Text != null && cb.Text != "")
                    {
                        count++;
                    }
                }
            }

            if (count == 12)
                return true;
            return false;
        }

        // обробники подій
        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxName, "Ім'я");
            isEdited = true;
        }

        private void textBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxSurname, "Прізвище");
            isEdited = true;
        }

        private void textBoxNickname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxNickname, "Прізвисько");
            isEdited = true;
        }

        private void textBoxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CheckHeight(textBoxHeight);
            isEdited = true;
        }

        private void ComboBoxEyeColor_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxEyeColor);
        }

        private void ComboBoxHairColor_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxEyeColor);
        }

        private void ComboBoxEyeColor_GotFocus(object sender, RoutedEventArgs e)
        {
            isEdited = true;
        }

        private void ComboBoxHairColor_GotFocus(object sender, RoutedEventArgs e)
        {
            isEdited = true;
        }

        private void textBoxSpecialFeatures_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenArrayTextChanged(textBoxSpecialFeatures);
            isEdited = true;
        }

        private void textBoxCitizenship_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxCitizenship, true);
            isEdited = true;
        }

        private void textBoxBirthPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxBirthPlace, true);
            isEdited = true;
        }

        private void textBoxLastAccomodation_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxLastAccomodation, true);
            isEdited = true;
        }

        private void textBoxJob_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxJob, true);
            isEdited = true;
        }


        private void textBoxBandName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxBandName, true);
            isEdited = true;
            bandNameIsEdited = true;
        }

        private void DeleteData_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Ви впевнені, що хочете видалити анкету ?\nНатискаючи ОК, Ви підтверджуєте, що злочинець мертвий",
                "Підтвердження видалення",
                MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                MessageBox.Show("Справу видалено з картотеки");
                PoliceCardIndex.DeleteAffair(id);
                BackInResultsForm_Click(null, null);
            }
        }

        private void EditData_Click(object sender, RoutedEventArgs e)
        {
           Edit();
        }

        private void ArchiveData_Click(object sender, RoutedEventArgs e)
        {
            Archive();
            BackInResultsForm_Click(null, null);
        }

        private void BackInResultsForm_Click(object sender, RoutedEventArgs e)
        {
            SearchAffair search = new SearchAffair();
            search.Show();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           /* PoliceCardIndex.WriteToFile("criminals.txt");
            PoliceCardIndex.WriteToFile("archived.txt");*/
        }

        private void birthPicker_LostFocus(object sender, RoutedEventArgs e)
        {
            isEdited = true;
        }

        private void CrimesPerformed_LostFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void DeleteCrime_Click(object sender, RoutedEventArgs e)
        {
            var idx = CrimesPerformed.SelectedIndex;

            int crime_id = (int)crimesTable.Rows[idx]["ID злочину"];

            SqlCommand command = new SqlCommand($"DELETE FROM Participants WHERE crime_id = {crime_id} AND criminal_id = {id};", PoliceCardIndex.GetSqlConnection());
            PoliceCardIndex.OpenConnection();
            command.ExecuteNonQuery();
            PoliceCardIndex.CloseConnection();

            crimesTable.Rows[idx].Delete();
            CrimesPerformed.ItemsSource = crimesTable.AsDataView();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT c.crime_id AS 'ID злочину', c.title AS 'Назва злочину', a.affair_type AS 'Тип злочину', p.crime_role AS 'Роль злочинця', c.commit_date AS 'Дата скоєння злочину', d.surname AS 'Прізвище відповідального детектива', d.badge_num AS 'Номер значка детектива' FROM Crimes c, Participants p, Detectives d, Affair_Types a WHERE p.crime_id = c.crime_id AND p.criminal_id = {id} AND c.detective_id = d.detective_id AND c.type_id = a.type_id UNION SELECT c.crime_id AS 'ID злочину', c.title AS 'Назва злочину', a.affair_type AS 'Тип злочину', p.crime_role AS 'Роль злочинця', c.commit_date AS 'Дата скоєння злочину', '' AS 'Прізвище відповідального детектива', 0 AS 'Номер значка детектива' FROM Crimes c, Participants p, Affair_Types a WHERE p.crime_id = c.crime_id AND p.criminal_id = {id} AND c.detective_id IS NULL AND c.type_id = a.type_id;", PoliceCardIndex.GetSqlConnection());

            adapter.Fill(crimesTable);

            CrimesPerformed.ItemsSource = crimesTable.AsDataView();
            CrimesPerformed.AutoGenerateColumns = true;

            CrimesPerformed.Columns[0].IsReadOnly = true;
            CrimesPerformed.Columns[5].IsReadOnly = true;
            CrimesPerformed.Columns[6].IsReadOnly = true;

            CrimesPerformed.CanUserDeleteRows = true;

            if (crimesTable.Rows.Count == 0)
            {
                CrimesPerformed.Visibility = Visibility.Collapsed;
                DeleteCrime.Visibility = Visibility.Collapsed;
                EditCrime.Visibility = Visibility.Collapsed;
            }


            //для додавання до існуючого злочину
            ExtensionsToCheckInput.GetComboItems("title", "Crimes", ComboBoxExistingTitle);

            //для додавання нового злочину
            ExtensionsToCheckInput.GetComboItems("affair_type", "Affair_Types", ComboBoxSpecialityNew);
        }

        private void EditCrime_Click(object sender, RoutedEventArgs e)
        {
            var idx = CrimesPerformed.SelectedIndex;

            DateTime date;
            int type_id;

            if (idx == -1 || idx > (int)crimesTable.Rows.Count - 1)
                return;

            DataRowView item = CrimesPerformed.Items[idx] as DataRowView;

            if (item == null)
                return;
            try
            {
                int idC = (int)crimesTable.Rows[idx]["ID злочину"];

                if (Convert.ToString(item.Row.ItemArray[1]) != "" && Convert.ToString(item.Row.ItemArray[0]) != ""
                    && Convert.ToString(item.Row.ItemArray[2]) != ""
                    && Convert.ToString(item.Row.ItemArray[4]) != ""
                    && DateTime.TryParse((Convert.ToString(item.Row.ItemArray[4])), out date))
                {
                    ExtensionsToCheckInput.InsertIfUnique(Convert.ToString(item.Row.ItemArray[2]).Trim(), "Affair_Types", "affair_type");
                    type_id = ExtensionsToCheckInput.GetIdForTextItems("Affair_Types", "type_id", "affair_type", Convert.ToString(item.Row.ItemArray[2]).Trim());

                   string sqlFormattedDate = date.ToString("yyyy-MM-dd hh:mm:ss.fff");

                    SqlCommand commandP = new SqlCommand($"UPDATE Participants SET crime_role = '{(string)item.Row.ItemArray[3]}' WHERE crime_id = {idC} AND criminal_id = {id};", PoliceCardIndex.GetSqlConnection());

                    SqlCommand commandC = new SqlCommand($"UPDATE Crimes SET title = '{(string)item.Row.ItemArray[1]}', type_id = {type_id}, commit_date =  '{sqlFormattedDate}' WHERE crime_id = {idC};", PoliceCardIndex.GetSqlConnection());
                    PoliceCardIndex.OpenConnection();
                    commandP.ExecuteNonQuery();
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

        private void AddToExistingCrimeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxExistingTitle.Text != "" && ComboBoxRoleToExist.Text.Length > 3)
            {
                int crime_id = ExtensionsToCheckInput.GetIdForTextItems("Crimes", "crime_id", "title", ComboBoxExistingTitle.Text.Trim());
                if (crime_id > 0)
                {
                    PoliceCardIndex.AddParticipant(id, crime_id, ComboBoxRoleToExist.Text.Trim());

                    //оновлюємо таблицю злочинів
                    SqlDataAdapter adapter = new SqlDataAdapter($"SELECT c.crime_id AS 'ID злочину', c.title AS 'Назва злочину', a.affair_type AS 'Тип злочину', p.crime_role AS 'Роль злочинця', c.commit_date AS 'Дата скоєння злочину', d.surname AS 'Прізвище відповідального детектива', d.badge_num AS 'Номер значка детектива' FROM Crimes c, Participants p, Detectives d, Affair_Types a WHERE p.crime_id = c.crime_id AND p.criminal_id = {id} AND c.detective_id = d.detective_id AND c.type_id = a.type_id UNION SELECT c.crime_id AS 'ID злочину', c.title AS 'Назва злочину', a.affair_type AS 'Тип злочину', p.crime_role AS 'Роль злочинця', c.commit_date AS 'Дата скоєння злочину', '' AS 'Прізвище відповідального детектива', 0 AS 'Номер значка детектива' FROM Crimes c, Participants p, Affair_Types a WHERE p.crime_id = c.crime_id AND p.criminal_id = {id} AND c.detective_id IS NULL AND c.type_id = a.type_id;", PoliceCardIndex.GetSqlConnection());

                    crimesTable.Clear();
                    adapter.Fill(crimesTable);

                    CrimesPerformed.ItemsSource = crimesTable.AsDataView();
                    CrimesPerformed.Visibility = Visibility.Visible;
                    DeleteCrime.Visibility = Visibility.Visible;
                    EditCrime.Visibility = Visibility.Visible;

                    ComboBoxRoleToExist.Text = "";
                    ComboBoxExistingTitle.Text = "";
                }
            }
            else {
                MessageBox.Show("Введена інформація не є коректною!");
            }
        }

        private void ComboBoxExistingTitle_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxExistingTitle_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxRoleToExist_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void textBoxCrimeNameNew_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenArrayTextChanged(textBoxCrimeNameNew);
        }

        private void ComboBoxSpecialityNew_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxSpecialityNew, true);
            if (PoliceCardIndex.GetDetSpeciality() == ComboBoxSpecialityNew.Text.Trim())
                checkBoxAmResponsibleToNew.Visibility = Visibility.Visible;
            else
            {
                checkBoxAmResponsibleToNew.Visibility = Visibility.Hidden;
            }
        }

        private void ComboBoxSpecialityNew_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dateCrPickerNew_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void textBoxTimeNew_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CheckTime(textBoxTimeNew);
        }

        private void ComboBoxRoleNew_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxRoleNew, true);
        }

        private void checkBoxAmResponsibleToNew_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void AddCrimeNewBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxSpecialityNew.Background != Brushes.Salmon && ComboBoxSpecialityNew.Text.Trim() != "")
            {
                ExtensionsToCheckInput.InsertIfUnique(ComboBoxSpecialityNew, "Affair_Types", "affair_type");
            }
            int c_id;
            string title;
            int type_id;
            string time;
            string cRole;

            if (IsReadyToBeAdded(textBoxCrimeNameNew, out title) &&
                IsReadyToBeAdded(textBoxTimeNew, out time) &&
                IsReadyToBeAdded(ComboBoxRoleNew, out cRole) &&
                IsReadyToBeAdded(ComboBoxSpecialityNew, out type_id, "Affair_Types", "type_id", "affair_type"))
            {
                DateTime d = (DateTime)dateCrPickerNew.SelectedDate;
                time += ":00.00";
                TimeSpan t = TimeSpan.Parse(time);
                d += t;
                string date = d.ToString("yyyy-MM-dd HH:mm:ss.fff");

                if ((bool)checkBoxAmResponsibleToNew.IsChecked && checkBoxAmResponsibleToNew.IsVisible)
                {
                    PoliceCardIndex.AddCrime(out c_id, type_id, title, date, PoliceCardIndex.DetectiveID);
                }
                else
                {
                    PoliceCardIndex.AddCrime(out c_id, type_id, title, date);
                }

                PoliceCardIndex.AddParticipant(id, c_id, cRole);

                foreach (object el in AddNewCrime.Children)
                {
                    if (el is TextBox)
                    {
                        Clean((TextBox)el);
                    }
                    else if (el is ComboBox)
                    {
                        Clean((ComboBox)el);
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT c.crime_id AS 'ID злочину', c.title AS 'Назва злочину', a.affair_type AS 'Тип злочину', p.crime_role AS 'Роль злочинця', c.commit_date AS 'Дата скоєння злочину', d.surname AS 'Прізвище відповідального детектива', d.badge_num AS 'Номер значка детектива' FROM Crimes c, Participants p, Detectives d, Affair_Types a WHERE p.crime_id = c.crime_id AND p.criminal_id = {id} AND c.detective_id = d.detective_id AND c.type_id = a.type_id UNION SELECT c.crime_id AS 'ID злочину', c.title AS 'Назва злочину', a.affair_type AS 'Тип злочину', p.crime_role AS 'Роль злочинця', c.commit_date AS 'Дата скоєння злочину', '' AS 'Прізвище відповідального детектива', 0 AS 'Номер значка детектива' FROM Crimes c, Participants p, Affair_Types a WHERE p.crime_id = c.crime_id AND p.criminal_id = {id} AND c.detective_id IS NULL AND c.type_id = a.type_id;", PoliceCardIndex.GetSqlConnection());

                crimesTable.Clear();
                adapter.Fill(crimesTable);

                CrimesPerformed.ItemsSource = crimesTable.AsDataView();
                CrimesPerformed.Visibility = Visibility.Visible;
                DeleteCrime.Visibility = Visibility.Visible;
                EditCrime.Visibility = Visibility.Visible;
            }

            else
            {
                MessageBox.Show("Заповніть усі поля!");
            }
        }

        //метод очистки всіх полів
        private void Clean(ComboBox inputbox)
        {
            inputbox.Text = String.Empty;
            inputbox.Background = Brushes.Transparent;
            inputbox.ToolTip = null;
        }

        private void Clean(TextBox inputbox)
        {
            inputbox.Text = String.Empty;
            inputbox.Background = Brushes.Transparent;
            inputbox.ToolTip = null;
        }

        private bool IsReadyToBeAdded(TextBox textBox, out string value)
        {
            if (textBox.Background == Brushes.Transparent && textBox.Text != null && textBox.Text != "")
            {
                value = textBox.Text.Trim();
                return true;
            }

            value = null;
            return false;
        }

        private bool IsReadyToBeAdded(ComboBox comboBox, out string value)
        {
            if (comboBox.Background == Brushes.Transparent && comboBox.Text != null && comboBox.Text != "")
            {
                value = comboBox.Text.Trim();
                return true;
            }

            value = null;
            return false;
        }

        private bool IsReadyToBeAdded(ComboBox comboBox, out int value, string tableN, string id, string cColumn)
        {
            if (comboBox.Background == Brushes.Transparent && comboBox.Text != null && comboBox.Text != "")
            {
                //(string tableN, string tableID, string tableC, string value)
                value = ExtensionsToCheckInput.GetIdForTextItems(tableN, id, cColumn, comboBox.Text.Trim());
                return true;
            }

            value = 0;
            return false;
        }

        private void AssignBtn_Click(object sender, RoutedEventArgs e)
        {
            int crime_num;
            string affair_type_name;

            if (textBoxAffairNumber.Text.Trim() != "" && int.TryParse(textBoxAffairNumber.Text.Trim(), out crime_num))
            {
                SqlDataAdapter adapter = new SqlDataAdapter($"SELECT c.crime_id, c.type_id, title, a.affair_type FROM Crimes c, Participants p, Affair_Types a WHERE detective_id IS NULL AND c.crime_id = p.crime_id AND p.criminal_id = {id} AND c.crime_id = {crime_num} AND c.type_id = a.type_id", PoliceCardIndex.GetSqlConnection());
                DataTable crimeTable = new DataTable();
                adapter.Fill(crimeTable);

                if (crimeTable.Rows.Count > 0)
                {
                    affair_type_name = PoliceCardIndex.GetDetSpeciality();

                    if (crimeTable.Rows[0]["affair_type"].ToString() == affair_type_name)
                    {
                        //оновлення детектив_айді в злочині
                        SqlCommand commandAddDet = new SqlCommand($"UPDATE Crimes SET detective_id = {PoliceCardIndex.DetectiveID} WHERE crime_id = {crime_num};", PoliceCardIndex.GetSqlConnection());

                        PoliceCardIndex.OpenConnection();
                        commandAddDet.ExecuteNonQuery();
                        PoliceCardIndex.CloseConnection();

                        //оновлення виводу таблиці злочинів злочинця тут
                        SqlDataAdapter adapterDet = new SqlDataAdapter($"SELECT c.crime_id AS 'ID злочину', c.title AS 'Назва злочину', a.affair_type AS 'Тип злочину', p.crime_role AS 'Роль злочинця', c.commit_date AS 'Дата скоєння злочину', d.surname AS 'Прізвище відповідального детектива', d.badge_num AS 'Номер значка детектива' FROM Crimes c, Participants p, Detectives d, Affair_Types a WHERE p.crime_id = c.crime_id AND p.criminal_id = {id} AND c.detective_id = d.detective_id AND c.type_id = a.type_id UNION SELECT c.crime_id AS 'ID злочину', c.title AS 'Назва злочину', a.affair_type AS 'Тип злочину', p.crime_role AS 'Роль злочинця', c.commit_date AS 'Дата скоєння злочину', '' AS 'Прізвище відповідального детектива', 0 AS 'Номер значка детектива' FROM Crimes c, Participants p, Affair_Types a WHERE p.crime_id = c.crime_id AND p.criminal_id = {id} AND c.detective_id IS NULL AND c.type_id = a.type_id;", PoliceCardIndex.GetSqlConnection());

                        crimesTable.Clear();
                        adapterDet.Fill(crimesTable);


                        MessageBox.Show("Ви упішно взяли розслідування злочину `" + crimeTable.Rows[0]["title"].ToString() + "` під свою відповідальність!");
                    }
                    else
                    {
                        MessageBox.Show("Неможливо доручити вам розслідування, оскільки " + crimeTable.Rows[0]["affair_type"].ToString() + " не є вашою спеціалізацією");
                    }
                }
                else {
                    MessageBox.Show("Неможливо взяти відповідальність за цю справу\n - справу вже курує детектив\n або справи з цим номером не існує");
                    
                }

                textBoxAffairNumber.Text = "";
            }
            else {
                MessageBox.Show("Введіть номер справи!");
            }
        }

        private void textBoxAffairNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
