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
/*            MessageBoxResult result = MessageBox.Show("Ви впевнені, що хочете видалити анкету ?\nНатискаючи ОК, Ви підтверджуєте, що злочинець мертвий",
                "Підтвердження видалення",
                MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                MessageBox.Show("Справу видалено з картотеки");
                PoliceCardIndex.DeleteAffair(processed);
                BackInResultsForm_Click(null, null);
            }*/
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
    }
}
