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
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for AddAffair.xaml
    /// </summary>
    public partial class AddAffair : Window
    {
        int id;

        // конструктор класу AddAffair
        public AddAffair()
        {
            InitializeComponent();
            checkBoxIsInBand.Checked += checkBox_Checked;
            checkBoxIsInBand.Unchecked += checkBox_Unchecked;
            comboBoxBandName.Visibility = Visibility.Hidden;
            AddCrime.Visibility = Visibility.Hidden;
            checkBoxAmResponsible.Visibility = Visibility.Hidden;

            //анімація кнопки додавання
            DoubleAnimation btnAnimation = new DoubleAnimation();
            btnAnimation.From = 0;
            btnAnimation.To = 1;
            btnAnimation.Duration = TimeSpan.FromSeconds(2);
            AddData.BeginAnimation(Button.OpacityProperty, btnAnimation);
        }

        //функції фінальної перевірки полів
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

        private bool IsReadyToBeAdded(TextBox textBox, out int value)
        {
            if (textBox.Background == Brushes.Transparent && textBox.Text != null && textBox.Text != "")
            {
                value = int.Parse(textBox.Text.Trim());
                return true;
            }

            value = 0;
            return false;
        }

        private bool IsReadyToBeAdded(ComboBox textBox, bool? isChecked, out string value, out bool isInBand)
        {
            if ((bool)isChecked)
            {
                isInBand = true;
                if (textBox.Background == Brushes.Transparent && textBox.Text != null && textBox.Text != "")
                {
                    value = textBox.Text.Trim();
                    return true;
                }
                else
                {
                    value = null;
                    return false;
                }
            }
            else
            {
                isInBand = false;
                value = null;
                return true;
            }

        }

        //метод очистки всіх полів на формі
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

        //обробники подій
        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            comboBoxBandName.Visibility = Visibility.Visible;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            comboBoxBandName.Visibility = Visibility.Hidden;
            comboBoxBandName.Text = "";
        }

        private void AddData_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxBandName.Background != Brushes.Salmon && comboBoxBandName.Text.Trim() != "")
            {
                ExtensionsToCheckInput.InsertIfUnique(comboBoxBandName, "Bands", "band_name");
            }

            string name;
            string surname;
            string nickname;
            DateTime regDate = DateTime.Now.Date;

            // конвертим дату
            string sqlFormattedRegDate = regDate.ToString("yyyy-MM-dd");

            int height;
            string eyeColor;
            string hairColor;
            string specialFeature;
            string citizenship;

            DateTime birth = (DateTime)birthPicker.SelectedDate;
            string dateOfBirth = birth.ToString("yyyy-MM-dd");

            string placeOfBirth;
            string lastAccomodation;
            string criminalJob;
            bool isInBand;
            string? bandName;
            int bandId;

            if (IsReadyToBeAdded(textBoxName, out name) &&
                IsReadyToBeAdded(textBoxSurname, out surname) &&
                IsReadyToBeAdded(textBoxNickname, out nickname) &&
                IsReadyToBeAdded(textBoxHeight, out height) &&
                IsReadyToBeAdded(ComboBoxEyeColor, out eyeColor) &&
                IsReadyToBeAdded(ComboBoxHairColor, out hairColor) &&
                IsReadyToBeAdded(textBoxSpecialFeatures, out specialFeature) &&
                IsReadyToBeAdded(textBoxCitizenship, out citizenship) &&
                IsReadyToBeAdded(textBoxBirthPlace, out placeOfBirth) &&
                IsReadyToBeAdded(textBoxLastAccomodation, out lastAccomodation) &&
                //IsReadyToBeAdded(textBoxLanguages, out languages) &&
                IsReadyToBeAdded(textBoxJob, out criminalJob) &&
                //IsReadyToBeAdded(ComboBoxTypeOfAffair, out affairType) &&
               // IsReadyToBeAdded(textBoxLastAffair, out lastAffair) &&
               // IsReadyToBeAdded(comboBoxdName, checkBoxIsInBand.IsChecked, out bandName, out isInBand))&&
                IsReadyToBeAdded(comboBoxBandName, out bandId, "Bands", "band_id", "band_name"))
            {
                if ((bool)checkBoxIsInBand.IsChecked)
                    isInBand = true;
                else { 
                    isInBand = false;
                }
                //тут додавання
                PoliceCardIndex.AddCriminal(name, surname, nickname, sqlFormattedRegDate, height, eyeColor, hairColor, specialFeature, citizenship, dateOfBirth, placeOfBirth, lastAccomodation, criminalJob, isInBand, out id, bandId);
                AddCrime.Visibility = Visibility.Visible;   

                /*              
                foreach (object el in Form.Children)
                {
                    if (el is TextBox)
                    {
                        Clean((TextBox)el);
                    }
                    else if (el is ComboBox)
                    {
                        Clean((ComboBox)el);
                    }
                }*/
            }

            else 
            {
                MessageBox.Show("Заповніть усі поля!");
            }
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxName, "Ім'я");
        }

        private void textBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxSurname, "Прізвище");
        }

        private void textBoxNickname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxNickname, "Прізвисько");
        }

        private void textBoxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CheckHeight(textBoxHeight);
        }
     

        private void ComboBoxHairColor_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxHairColor);
        }

        private void ComboBoxEyeColor_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxEyeColor);
        }
 
        private void textBoxSpecialFeatures_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenArrayTextChanged(textBoxSpecialFeatures);
        }

        private void textBoxCitizenship_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxCitizenship, true);
        }


        private void textBoxBirthPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxBirthPlace, true);
        }

        private void textBoxLastAccomodation_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxLastAccomodation, true);
        }

        private void textBoxJob_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxJob, true);
        }


        private void BackInAddForm_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void birthPicker_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void comboBoxBandName_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(comboBoxBandName, true);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.GetComboItems("band_name", "Bands", comboBoxBandName);
            ExtensionsToCheckInput.GetComboItems("affair_type", "Affair_Types", ComboBoxSpeciality);
        }

        private bool IsReadyToBeAdded(ComboBox comboBox, out int value, string tableN, string id, string cColumn)
        {
            if ((bool)checkBoxIsInBand.IsChecked == false && tableN == "Bands")
            {
                value = -1;
                return true;
            }
            if (comboBox.Background == Brushes.Transparent && comboBox.Text != null && comboBox.Text != "")
            {
                //(string tableN, string tableID, string tableC, string value)
                value = ExtensionsToCheckInput.GetIdForTextItems(tableN, id, cColumn, comboBox.Text.Trim());
                return true;
            }

            value = 0;
            return false;
        }


        private void textBoxCrimeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenArrayTextChanged(textBoxCrimeName);
        }

        private void ComboBoxSpeciality_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxSpeciality, true);
            if (PoliceCardIndex.GetDetSpeciality() == ComboBoxSpeciality.Text.Trim())
                checkBoxAmResponsible.Visibility = Visibility.Visible;
            else {
                checkBoxAmResponsible.Visibility = Visibility.Hidden;
            }
        }

        private void ComboBoxSpeciality_GotFocus(object sender, RoutedEventArgs e)
        {

        }


        private void dateCrPicker_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void textBoxTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CheckTime(textBoxTime);
        }

        private void checkBoxAmResponsible_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxRole_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxRole, true);
        }


        private void AddCrimeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxSpeciality.Background != Brushes.Salmon && ComboBoxSpeciality.Text.Trim() != "")
            {
                ExtensionsToCheckInput.InsertIfUnique(ComboBoxSpeciality, "Affair_Types", "affair_type");
            }
            int crime_id;
            string title;
            int type_id;
            string time;
            string crimeRole;

            if (IsReadyToBeAdded(textBoxCrimeName, out title) &&
                IsReadyToBeAdded(textBoxTime, out time) &&
                IsReadyToBeAdded(ComboBoxRole, out crimeRole) &&
                IsReadyToBeAdded(ComboBoxSpeciality, out type_id, "Affair_Types", "type_id", "affair_type"))
            {
                DateTime d = (DateTime)dateCrPicker.SelectedDate;
                time += ":00.00";
                TimeSpan t = TimeSpan.Parse(time);
                d += t;
                string date = d.ToString("yyyy-MM-dd HH:mm:ss.fff");

                if ((bool)checkBoxAmResponsible.IsChecked && checkBoxAmResponsible.IsVisible)
                {
                    PoliceCardIndex.AddCrime(out crime_id, type_id, title, date, PoliceCardIndex.DetectiveID);
                }
                else
                {
                    PoliceCardIndex.AddCrime(out crime_id, type_id, title, date);
                    PoliceCardIndex.AutoAssign(crime_id);
                }

                PoliceCardIndex.AddParticipant(id, crime_id, crimeRole);
                         
                foreach (object el in Form.Children)
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
                foreach (object el in AddCrime.Children)
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
            }

            else
            {
                MessageBox.Show("Заповніть усі поля!");
            }
        }
    }
}
