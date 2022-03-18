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


namespace CourseProj
{
    /// <summary>
    /// Interaction logic for AddAffair.xaml
    /// </summary>
    public partial class AddAffair : Window
    {

        public AddAffair()
        {
            InitializeComponent();
            checkBoxIsInBand.Checked += checkBox_Checked;
            checkBoxIsInBand.Unchecked += checkBox_Unchecked;
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            textBoxBandName.Visibility = Visibility.Visible;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            textBoxBandName.Visibility = Visibility.Hidden;
        }

        private void AddData_Click(object sender, RoutedEventArgs e)
        {

            // пока задний фон красный + все поля не пустые (кроме краймбэнда)!!!!
            //тут вызвать AddCriminal и передать ему все данные? тут инициализировать переменные имя, фамилия ...?
            
        }

        //обробники подій
        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxName, "Ім'я");
        }

        private void textBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxSurname, "Прізвище");
        }

        private void textBoxNickname_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxNickname, "Прізвисько");
        }

        private void textBoxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value  = textBoxHeight.Text.Trim();
            int i = 0;

            if (!int.TryParse(value, out i))
            {
                textBoxHeight.ToolTip = "Уведіть у поле число!";
                textBoxHeight.Background = Brushes.Salmon;
            }
            else if (int.TryParse(value, out i))
            { 
                int height = int.Parse(value);

                if (height < 60 || height > 260)
                {
                    textBoxHeight.ToolTip = "Уведіть у поле число в діапазоні від 60 до 260!";
                    textBoxHeight.Background = Brushes.Salmon;
                }
                else
                {
                    textBoxHeight.ToolTip = null;
                    textBoxHeight.Background = Brushes.Transparent;
                }
            }
        }

        private void textBoxEyeColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxEyeColor);
        }

        private void textBoxHairColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxHairColor);
        }

        private void textBoxCitizenship_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxCitizenship, true);
        }

        private void textBoxBirthday_TextChanged(object sender, TextChangedEventArgs e)
        {
            DateIsCorrect(textBoxBirthday);

        }

        private void textBoxBirthPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxBirthPlace, true);
        }

        private void textBoxLastAccomodation_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxLastAccomodation, true);
        }

        private void textBoxLanguages_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenArrayTextChanged(textBoxLanguages);
        }

        private void textBoxJob_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxJob, true);
        }

        private void textBoxLastAffair_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxLastAffair, true);
        }

        private void textBoxBandName_TextChanged(object sender, TextChangedEventArgs e)
        {
            CommonWarningWhenTextChanged(textBoxBandName, true);
        }

        //функції для обробників подій

        private void CommonWarningWhenTextChanged(TextBox textBox, string naming)
        {
            string value = textBox.Text.Trim();
            
            if (value.Length > 50)
            {
                textBox.ToolTip = naming + " має бути коротшим за 50 символів!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.Length < 2)
            {
                textBox.ToolTip = naming + " повинно мати принаймні 2 символи!";
                textBox.Background = Brushes.Salmon;
            }
            else if (ContainsNumbers(value))
            {
                textBox.ToolTip = naming + " не має містити цифри!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().StartsWith("ь") || value.ToLower().StartsWith("ъ") || value.ToLower().StartsWith("ы"))
            {
                textBox.ToolTip = naming + " не має починатися з цього знаку!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") || value.ToLower().Contains("$")|| value.ToLower().Contains("@")|| value.ToLower().Contains("%"))
            {
                textBox.ToolTip = naming + " не має містити цих знаків: ! ? # @ $ % , * | / + =";
                textBox.Background = Brushes.Salmon;
            }
            else
            {
                textBox.ToolTip = null;
                textBox.Background = Brushes.Transparent;
            }
        }

        private void CommonWarningWhenTextChanged(TextBox textBox)
        {
            string value = textBox.Text.Trim();
            
            if (value.Length > 50)
            {
                textBox.ToolTip = "Поле має бути коротшим за 50 символів!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.Length < 3)
            {
                textBox.ToolTip = "Поле повинно мати принаймні 3 символи!";
                textBox.Background = Brushes.Salmon;
            }
            else if (ContainsNumbers(value))
            {
                textBox.ToolTip = "Поле не може містити цифри!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().StartsWith("ь") || value.ToLower().StartsWith("ъ") || value.ToLower().StartsWith("ы"))
            {
                textBox.ToolTip = "Поле не має починатися з цього знаку!";
                textBox.Background = Brushes.Salmon;
            }
            else if (HasSeveralWords(value))
            {
                textBox.ToolTip = "Уведіть у поле одне слово!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%"))
            {
                textBox.ToolTip = "Поле не має містити цих знаків: ! ? # @ $ % , * | / + =";
                textBox.Background = Brushes.Salmon;
            }
            else
            {
                textBox.ToolTip = null;
                textBox.Background = Brushes.Transparent;
            }
        }

        private void CommonWarningWhenTextChanged(TextBox textBox, bool hasSeveralWords)
        {
            string value = textBox.Text.Trim();
            int i = 0;
            if (value.Length > 60)
            {
                textBox.ToolTip = "Поле має бути коротшим за 60 символів!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.Length < 5)
            {
                textBox.ToolTip = "Поле повинно мати принаймні 5 символів!";
                textBox.Background = Brushes.Salmon;
            }
            else if (ContainsNumbers(value))
            {
                textBox.ToolTip = "Поле не може містити цифри!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().StartsWith("ь") || value.ToLower().StartsWith("ъ") || value.ToLower().StartsWith("ы"))
            {
                textBox.ToolTip = "Поле не має починатися з цього знаку!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%"))
            {
                textBox.ToolTip = "Поле не має містити цих знаків: ! ? # @ $ % , * | / + =";
                textBox.Background = Brushes.Salmon;
            }
            else
            {
                textBox.ToolTip = null;
                textBox.Background = Brushes.Transparent;
            }
        }

        private void CommonWarningWhenArrayTextChanged(TextBox textBox)
        {
            string value = textBox.Text.Trim();

            if (value.Length > 100)
            {
                textBox.ToolTip = "Поле може містити до 100 символів!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.Length < 3)
            {
                textBox.ToolTip = "Поле повинно містити принаймні 3 символи!";
                textBox.Background = Brushes.Salmon;
            }
            else if (ContainsNumbers(value))
            {
                textBox.ToolTip = "Поле не має містити цифри!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().StartsWith("ь") || value.ToLower().StartsWith("ъ") || value.ToLower().StartsWith("ы"))
            {
                textBox.ToolTip = "Поле не має починатися з цього знаку!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") || value.ToLower().Contains("/") || value.ToLower().Contains("#") || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%"))
            {
                textBox.ToolTip = "Поле не має містити цих знаків: ! ? # @ $ % * | / + =";
                textBox.Background = Brushes.Salmon;
            }
            else
            {
                textBox.ToolTip = null;
                textBox.Background = Brushes.Transparent;
            }
        }

        private bool ContainsNumbers(string value)
        {
            Regex regex = new Regex(@"\d");
            Match match = regex.Match(value);

            if (match.Success)
                return true;
            
            return false;
        }

        private bool HasSeveralWords(string value)
        {
            string[] wordArray = value.Split(" ");

            if (wordArray.Length>1)
                return true;
            return false;
        }

        private void DateIsCorrect(TextBox textBox)
        {
            string str = textBox.Text.Trim();
            DateTime date;

            if (!DateTime.TryParse(str, out date)||str.Length<10||str.Contains("/"))
            {
                textBox.ToolTip = "Уведіть дату у форматі ДД.ММ.РРРР !";
                textBox.Background = Brushes.Salmon;
            }
            else
            {
                string[] arr = str.Split(".");
                int i = 0;
                int.TryParse(arr[2], out i);
                if (i < 1900 || i > 2022)
                {
                    textBox.ToolTip = "Уведіть дату в діапазоні від 1900 до 2022 року включно!";
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
}
