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
    }
}
