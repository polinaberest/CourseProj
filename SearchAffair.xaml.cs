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

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for SearchAffair.xaml
    /// </summary>
    public partial class SearchAffair : Window
    {
        public SearchAffair()
        {
            InitializeComponent();
            checkBoxIsInBand.Checked += checkBoxIsInBand_Checked;
            checkBoxIsInBand.Unchecked += checkBoxIsInBand_Unchecked;
        
            //button animation
            DoubleAnimation btnAnimation = new DoubleAnimation();
            btnAnimation.From = 0;
            btnAnimation.To = 1;
            btnAnimation.Duration = TimeSpan.FromSeconds(2);
            SearchData.BeginAnimation(Button.OpacityProperty, btnAnimation);
        }

        private void textBoxBirthday_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.DateIsCorrect(textBoxBirthday);
            if (textBoxBirthday.Text.Trim() == "")
            {
                textBoxBirthday.ToolTip = null;
                textBoxBirthday.Background = Brushes.Transparent;
            }
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

        private void Search()
        {
            if (!CheckAbsoluteEmpty(textBoxBirthday, textBoxHeight))
            {
                InterpolCardIndex.criminalsFoundByRequest.Clear(); // очищуємо список знайдених з минулого разу
                if (checkBoxIsInBand.IsChecked == true)
                {
                    // логика поиска в листе всех банд по названию банды. Потом в найденной по названию банде ищем в членах банды criminal' а с такими параметрами;
                    Criminal prototype = CreateCriminalProto(textBoxName, textBoxSurname, textBoxNickname, textBoxHeight, textBoxEyeColor, textBoxHairColor, textBoxSpecialFeatures, textBoxCitizenship, textBoxBirthday, textBoxBirthPlace, textBoxLastAccomodation, textBoxLanguages, textBoxJob, textBoxLastAffair, true, textBoxBandName.Text.Trim());

                    InterpolCardIndex.SearchInBand(prototype);
                    //MessageBox.Show("Це - Сава, він у банді!");

                }
                if (checkBoxIsInBand.IsChecked == false)
                {
                    Criminal prototype = CreateCriminalProto(textBoxName, textBoxSurname, textBoxNickname, textBoxHeight, textBoxEyeColor, textBoxHairColor, textBoxSpecialFeatures, textBoxCitizenship, textBoxBirthday, textBoxBirthPlace, textBoxLastAccomodation, textBoxLanguages, textBoxJob, textBoxLastAffair, false, null);
                    InterpolCardIndex.SearchNotinBand(prototype);
                    //MessageBox.Show("Це - Сава, він красава без банди!");
                }
                SearchResults results = new SearchResults();
                results.Show();
                this.Close();
            }
            else
                MessageBox.Show("Нема інформації для здійснення пошуку!");
        }

        private bool CheckAbsoluteEmpty(TextBox textBoxBirthday, TextBox textBoxHeight)
        {
            int count = 0;
            int h;
            if (textBoxBirthday.Background == Brushes.Salmon) 
            {
                textBoxBirthday.Text = "";
            }
            if (!int.TryParse(textBoxHeight.Text.Trim(), out h))
            {
                textBoxHeight.Text = "";
            }
            foreach (object el in Form.Children)
            {
                if (el is TextBox)
                {
                    TextBox box = (TextBox)el;
                    if(box.Text == "" || box.Text == null)
                        count++;
                }
            }
            if ((bool)checkBoxIsInBand.IsChecked && count == 15)
            {
                return true;
            }
            else if ((!(bool)checkBoxIsInBand.IsChecked) && count == 15)
            { 
                return true;
            }
            return false;

        }

        private Criminal CreateCriminalProto(
        TextBox nameTB,
        TextBox surnameTB,
        TextBox nicknameTB,
        TextBox heightTB,
        TextBox eyeColorTB,
        TextBox hairColorTB,
        TextBox specialFeaturesTB,
        TextBox citizenshipTB,
        TextBox dateOfBirthTB,
        TextBox placeOfBirthTB,
        TextBox lastAccomodationTB,
        TextBox languagesTB,
        TextBox criminalJobTB,
        TextBox lastAffairTB,
        bool isInBand,
        string? bandName)
        {
            string name = nameTB.Text.Trim();
            string surname = surnameTB.Text.Trim();
            string nickname = nicknameTB.Text.Trim();
            int height;
            int.TryParse(heightTB.Text.Trim(), out height);
            string eyeColor = eyeColorTB.Text.Trim();
            string hairColor = hairColorTB.Text.Trim();
            string specialFeatures = specialFeaturesTB.Text.Trim();
            string citizenship = citizenshipTB.Text.Trim();
            string dateOfBirth = dateOfBirthTB.Text.Trim();
            if(dateOfBirthTB.Background != Brushes.Transparent)
            {
                dateOfBirth = "";
            }
            string placeOfBirth = placeOfBirthTB.Text.Trim();
            string lastAccomodation = lastAccomodationTB.Text.Trim();   
            string languages = languagesTB.Text.Trim();
            string criminalJob = criminalJobTB.Text.Trim();
            string lastAffair = lastAffairTB.Text.Trim();

            Criminal prototype = new Criminal(name, surname, nickname, height, eyeColor, hairColor, specialFeatures, citizenship, dateOfBirth, placeOfBirth, lastAccomodation, languages, criminalJob, lastAffair, isInBand, bandName);
            return prototype;
        }

        private void BackInSearchForm_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }
    }
}
