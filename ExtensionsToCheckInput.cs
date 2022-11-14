using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Media;
using System.Windows.Controls;
using System.Data;
using System.Data.SqlClient;

namespace CourseProj
{
    // клас методів розширення для опрацювання та перевірки вхідних даних
    internal static class ExtensionsToCheckInput
    {
        // метод визначення, чи є в рядку числа
        public static bool ContainsNumbers(string value)
        {
            Regex regex = new Regex(@"\d");
            Match match = regex.Match(value);

            if (match.Success)
                return true;

            return false;
        }

        // метод визначення, чи складається рядок з кількох слів
        public static bool HasSeveralWords(string value)
        {
            string[] wordArray = value.Split(" ");

            if (wordArray.Length > 1)
                return true;
            return false;
        }

        // метод визначення коректності введення дати
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

        public static void CheckTime(TextBox textBox)
        {
            string str = textBox.Text.Trim();
            TimeOnly time;

            if (!TimeOnly.TryParse(str, out time) || str.Length < 5)
            {
                textBox.ToolTip = "Уведіть час у форматі ГГ:ХХ !";
                textBox.Background = Brushes.Salmon;
            }
            else
            {
                textBox.ToolTip = null;
                textBox.Background = Brushes.Transparent;
            }
        }

        // метод визначення правильності введення зросту
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

        //метод перевірки правильності введення номеру значка детектива
        public static void CheckPersonalNumber(TextBox textBox)
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
                int number = int.Parse(value);

                if (number < 100000 || number > 999999)
                {
                    textBox.ToolTip = "Уведіть у поле шестизначне число, що відповідає номеру вашого значка";
                    textBox.Background = Brushes.Salmon;
                }
                else
                {
                    if (PoliceCardIndex.IsUniqueDetectiveNumber(number))
                    {
                        textBox.ToolTip = null;
                        textBox.Background = Brushes.Transparent;
                    }

                    else
                    {
                        textBox.ToolTip = "Детектив з таким номером вже зареєстрований у системі! Уведіть ваш унікальний номер значка для реєстрації!";
                        textBox.Background = Brushes.Salmon;
                    }
                }
            }
        }

        //метод визначення збігу паролей при авторизації
        public static bool PasswordMatches(string pass, int badge_num)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT pass FROM Detectives WHERE badge_num = {badge_num};", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            string realPass = (string)table.Rows[0][0];
            if (realPass.Trim() == pass)
                return true;
            return false;
        }

        // метод визначення правильності введення паролю
        public static void CheckPass(PasswordBox passBox)
        {
            string value = passBox.Password.Trim();

            if (value.Length > 30)
            {
                passBox.ToolTip = "Пароль не має бути довшим за 50 символів!";
                passBox.Background = Brushes.Salmon;
            }
            else if (value.Length < 8)
            {
                passBox.ToolTip = "Пароль повинен мати принаймні 8 символів!";
                passBox.Background = Brushes.Salmon;
            }
            else if (!ExtensionsToCheckInput.ContainsNumbers(value))
            {
                passBox.ToolTip = "Пароль має містити принаймні одну цифру!";
                passBox.Background = Brushes.Salmon;
            }
            else if (Int32.TryParse(value, out int i))
            {
                passBox.ToolTip = "Пароль має складатися не лише з цифр!";
                passBox.Background = Brushes.Salmon;
            }
            else
            {
                passBox.ToolTip = null;
                passBox.Background = Brushes.Transparent;
            }
        }

        //метод перевірки повторного введення паролю
        public static void CheckPass(PasswordBox pass, PasswordBox initPass)
        {
            string value = pass.Password.Trim();
            string init = initPass.Password.Trim();

            if (value != init)
            {   
                pass.ToolTip = "Паролі не збігаються!";
                pass.Background = Brushes.Salmon;
            }
            else
            {
                pass.ToolTip = null;
                pass.Background = Brushes.Transparent;
            }
        }

        // метод визначення правильності введення текстових даних (з назвою поля)
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
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") 
                   || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") 
                   || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") 
                   || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%") 
                   || value.ToLower().Contains(";"))
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

        // метод визначення правильності введення текстових даних 
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
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") 
                || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") 
                || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") 
                || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%") 
                || value.ToLower().Contains(";"))
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

        // метод визначення правильності введення текстових даних у поле, де передбачено декілька слів 
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
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") 
                || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") 
                || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") 
                || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%") 
                || value.ToLower().Contains(";"))
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

        // метод визначення правильності вибору / введення у випадному списку
        public static void CommonWarningWhenTextChanged(ComboBox comboBox, bool hasSeveralWords = false)
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
            else if (HasSeveralWords(value) && !hasSeveralWords)
            {
                comboBox.ToolTip = "Уведіть у поле одне слово!";
                comboBox.Background = Brushes.Salmon;
            }
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") 
                || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") 
                || value.ToLower().Contains("/") || value.ToLower().Contains(",") || value.ToLower().Contains("#") 
                || value.ToLower().Contains("$") || value.ToLower().Contains("@") || value.ToLower().Contains("%") 
                || value.ToLower().Contains(";"))
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

        // метод визначення правильності введення текстових даних при введенні переліку
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
            else if (value.ToLower().Contains("!") || value.ToLower().Contains("+") || value.ToLower().Contains("=") 
                || value.ToLower().Contains("?") || value.ToLower().Contains("*") || value.ToLower().Contains("|") 
                || value.ToLower().Contains("/") || value.ToLower().Contains("#") || value.ToLower().Contains("$") 
                || value.ToLower().Contains("@") || value.ToLower().Contains("%") || value.ToLower().Contains(";"))
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

        public static void GetComboItems(string columnName, string tableName, ComboBox comboBox)
        {
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM " + tableName + " ORDER BY " + columnName, PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                comboBox.Items.Add(table.Rows[i][columnName].ToString());
            }
        }

        public static void InsertIfUnique(ComboBox comboBox, string tableN, string tableC)
        {
            bool isUnique = true;
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT {tableC} FROM {tableN};", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (table.Rows[i][tableC].ToString() == comboBox.Text.Trim())
                {
                    isUnique = false;
                    break;
                }
            }

            if (isUnique)
            {
                SqlCommand command = new SqlCommand($"INSERT INTO {tableN}({tableC}) VALUES('" + comboBox.Text.ToString() + "');", PoliceCardIndex.GetSqlConnection());
                PoliceCardIndex.OpenConnection();
                command.ExecuteNonQuery();
                PoliceCardIndex.CloseConnection();
            }
        }

        public static int GetIdForTextItems(string tableN, string tableID, string tableC, string value)
        {
            SqlDataAdapter adapter = new SqlDataAdapter($"SELECT {tableID} FROM {tableN} WHERE {tableC} = '{value}';", PoliceCardIndex.GetSqlConnection());
            //SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableN};", PoliceCardIndex.GetSqlConnection());
            DataTable table = new DataTable();
            adapter.Fill(table);
            int id = (int)table.Rows[0][0];
            return id;
        }

    }
}
