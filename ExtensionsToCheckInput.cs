using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Controls;

namespace CourseProj
{
    internal static class ExtensionsToCheckInput
    {
        public static bool ContainsNumbers(string value)
        {
            Regex regex = new Regex(@"\d");
            Match match = regex.Match(value);

            if (match.Success)
                return true;

            return false;
        }

        public static bool HasSeveralWords(string value)
        {
            string[] wordArray = value.Split(" ");

            if (wordArray.Length > 1)
                return true;
            return false;
        }

        public static void DateIsCorrect(TextBox textBox)
        {
            string str = textBox.Text.Trim();
            DateTime date;

            if (!DateTime.TryParse(str, out date) || str.Length < 10 || str.Contains("/"))
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

        public static void CheckHeight(TextBox textBox)
        {
            string value = textBox.Text.Trim();
            int i = 0;

            if (!int.TryParse(value, out i))
            {
                textBox.ToolTip = "Уведіть у поле число!";
                textBox.Background = Brushes.Salmon;
            }
            else if (int.TryParse(value, out i))
            {
                int height = int.Parse(value);

                if (height < 60 || height > 260)
                {
                    textBox.ToolTip = "Уведіть у поле число в діапазоні від 60 до 260!";
                    textBox.Background = Brushes.Salmon;
                }
                else
                {
                    textBox.ToolTip = null;
                    textBox.Background = Brushes.Transparent;
                }
            }
        }

        public static void CommonWarningWhenTextChanged(TextBox textBox, string naming)
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
            else if (ExtensionsToCheckInput.ContainsNumbers(value))
            {
                textBox.ToolTip = naming + " не має містити цифри!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().StartsWith("ь") || value.ToLower().StartsWith("ъ") || value.ToLower().StartsWith("ы"))
            {
                textBox.ToolTip = naming + " не має починатися з цього знаку!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%") || value.ToLower().Contains(";"))
            {
                textBox.ToolTip = naming + " не має містити цих знаків: ! ? # @ $ % , ; * | / + =";
                textBox.Background = Brushes.Salmon;
            }
            else
            {
                textBox.ToolTip = null;
                textBox.Background = Brushes.Transparent;
            }
        }

        public static void CommonWarningWhenTextChanged(TextBox textBox)
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
            else if (ExtensionsToCheckInput.ContainsNumbers(value))
            {
                textBox.ToolTip = "Поле не може містити цифри!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().StartsWith("ь") || value.ToLower().StartsWith("ъ") || value.ToLower().StartsWith("ы"))
            {
                textBox.ToolTip = "Поле не має починатися з цього знаку!";
                textBox.Background = Brushes.Salmon;
            }
            else if (ExtensionsToCheckInput.HasSeveralWords(value))
            {
                textBox.ToolTip = "Уведіть у поле одне слово!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%") || value.ToLower().Contains(";"))
            {
                textBox.ToolTip = "Поле не має містити цих знаків: ! ? # @ $ % , ; * | / + =";
                textBox.Background = Brushes.Salmon;
            }
            else
            {
                textBox.ToolTip = null;
                textBox.Background = Brushes.Transparent;
            }
        }

        public static void CommonWarningWhenTextChanged(TextBox textBox, bool hasSeveralWords)
        {
            string value = textBox.Text.Trim();

            if (value.Length > 60)
            {
                textBox.ToolTip = "Поле має бути коротшим за 60 символів!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.Length < 3)
            {
                textBox.ToolTip = "Поле повинно мати принаймні 3 символи!";
                textBox.Background = Brushes.Salmon;
            }
            else if (ExtensionsToCheckInput.ContainsNumbers(value))
            {
                textBox.ToolTip = "Поле не може містити цифри!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().StartsWith("ь") || value.ToLower().StartsWith("ъ") || value.ToLower().StartsWith("ы"))
            {
                textBox.ToolTip = "Поле не має починатися з цього знаку!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%") || value.ToLower().Contains(";"))
            {
                textBox.ToolTip = "Поле не має містити цих знаків: ! ? # @ $ % , ; * | / + =";
                textBox.Background = Brushes.Salmon;
            }
            else
            {
                textBox.ToolTip = null;
                textBox.Background = Brushes.Transparent;
            }
        }

        public static void CommonWarningWhenTextChanged(ComboBox comboBox)
        {
            string value = comboBox.Text.Trim();

            if (value.Length > 50)
            {
                comboBox.ToolTip = "Поле має бути коротшим за 50 символів!";
                comboBox.Background = Brushes.Salmon;
            }
            else if (value.Length < 3)
            {
                comboBox.ToolTip = "Поле повинно мати принаймні 3 символи!";
                comboBox.Background = Brushes.Salmon;
            }
            else if (ContainsNumbers(value))
            {
                comboBox.ToolTip = "Поле не може містити цифри!";
                comboBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().StartsWith("ь") || value.ToLower().StartsWith("ъ") || value.ToLower().StartsWith("ы"))
            {
                comboBox.ToolTip = "Поле не має починатися з цього знаку!";
                comboBox.Background = Brushes.Salmon;
            }
            else if (HasSeveralWords(value))
            {
                comboBox.ToolTip = "Уведіть у поле одне слово!";
                comboBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%") || value.ToLower().Contains(";"))
            {
                comboBox.ToolTip = "Поле не має містити цих знаків: ! ? # @ $ % , ; * | / + =";
                comboBox.Background = Brushes.Salmon;
            }
            else
            {
                comboBox.ToolTip = null;
                comboBox.Background = Brushes.Transparent;
            }
        }

        public static void CommonWarningWhenArrayTextChanged(TextBox textBox)
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
            else if (ExtensionsToCheckInput.ContainsNumbers(value))
            {
                textBox.ToolTip = "Поле не має містити цифри!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().StartsWith("ь") || value.ToLower().StartsWith("ъ") || value.ToLower().StartsWith("ы"))
            {
                textBox.ToolTip = "Поле не має починатися з цього знаку!";
                textBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") || value.ToLower().Contains("/") || value.ToLower().Contains("#") || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%") || value.ToLower().Contains(";"))
            {
                textBox.ToolTip = "Поле не має містити цих знаків: ! ? # @ $ % ; * | / + =";
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
