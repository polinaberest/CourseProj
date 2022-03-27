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
            checkBoxIsInBand.Unchecked += checkBoxIsInBand_Checked;
        
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
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxName);
        }

        private void textBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxSurname);
        }

        private void textBoxNickname_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxNickname);
        }

        private void textBoxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CheckHeight(textBoxHeight);
            if (textBoxHeight.Text.Trim() == null || textBoxHeight.Text.Trim() == "")
            {
                textBoxHeight.ToolTip = null;
                textBoxHeight.Background = Brushes.Transparent;
            }
        }

        private void textBoxEyeColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxEyeColor);
        }

        private void textBoxHairColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxHairColor);
        }

        private void textBoxSpecialFeatures_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxSpecialFeatures);
        }

        private void textBoxCitizenship_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxCitizenship);
        }

        private void textBoxBirthPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxBirthPlace);
        }

        private void textBoxLastAccomodation_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxLastAccomodation);
        }

        private void textBoxLanguages_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxLanguages);
        }

        private void textBoxJob_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxJob);
        }

        private void textBoxLastAffair_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxLastAffair);
        }

        private void textBoxBandName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckLength(textBoxBandName);
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

        }

        private void CheckLength(TextBox textBox)
        {
            string value = textBox.Text.Trim();

            if (value.Length > 100)
            {
                textBox.ToolTip = "Уведіть менше 100 символів для пошуку";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.Length < 2 && value.Length>0)
            {
                textBox.ToolTip = "Уведіть принаймні 2 символи для пошуку";
                textBox.Background = Brushes.Salmon;
            }
            else
            {
                textBox.ToolTip = null;
                textBox.Background = Brushes.Transparent;
            }
        }
    }
}
