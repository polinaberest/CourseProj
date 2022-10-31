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
using System.Data.SqlClient;
using System.Data;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        
        public Registration()
        {
            InitializeComponent();
        }

        private void BackInAuthorizationForm_Click(object sender, RoutedEventArgs e)
        {
            Authorization auth = new Authorization();
            auth.Show();
            this.Close();
        }

        private void textBoxPersonalNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CheckPersonalNumber(textBoxPersonalNumber);
        }

        private void Registrate_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxDepartment.Background != Brushes.Salmon && ComboBoxDepartment.Text.Trim() != "")
            {
                ExtensionsToCheckInput.InsertIfUnique(ComboBoxDepartment, "Departments", "department_name");
            }
            if (ComboBoxSpeciality.Background != Brushes.Salmon && ComboBoxSpeciality.Text.Trim() != "")
            {
                ExtensionsToCheckInput.InsertIfUnique(ComboBoxSpeciality, "Affair_Types", "affair_type");
            }

            string name;
            string surname;
            int badgeNumber;
            int depNumber;
            int specialityIdx;
            string pass;
            DateTime regDate = DateTime.Now;    
            DateTime lastVisit = DateTime.Now;

            // конвертим дату
            //2005-12-31 00:00:00.000
            string sqlFormattedRegDate = regDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string sqlFormattedLastVisit = lastVisit.ToString("yyy-MM-dd HH:mm:ss.fff");


            if (IsReadyToBeAdded(textBoxName, out name) &&
                IsReadyToBeAdded(textBoxSurname, out surname) &&
                IsReadyToBeAdded(ComboBoxSpeciality, out specialityIdx, "Affair_Types", "type_id", "affair_type") &&
                IsReadyToBeAdded(ComboBoxDepartment, out depNumber, "Departments", "department_id", "department_name") &&
                IsReadyToBeAdded(textBoxPersonalNumber, out badgeNumber) &&
                IsReadyToBeAdded(passBoxInitial, out pass) &&
                passBoxSecond.Background == Brushes.Transparent && passBoxSecond.Password != null && passBoxSecond.Password != ""
                )
            {
                //тут було додавання
                // public static void AddDetective(string name, string surname, int badge_num, int department_id, DateTime reg_date, DateTime last_visit_date, string pass, int type_id)
                //2004-12-31 
                PoliceCardIndex.AddDetective(name, surname, badgeNumber, depNumber, sqlFormattedRegDate, sqlFormattedLastVisit, pass, specialityIdx, out PoliceCardIndex.DetectiveID);

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
                    else if (el is PasswordBox)
                    {
                        Clean((PasswordBox)el);
                    }
                }

                MessageBox.Show($"Вітаємо, детективе {surname}, Ви успішно зареєстровані!");

                PoliceCardIndex.IsDetective = true;
                MainWindow main = new MainWindow();
                this.Close();
                main.Show();
            }

            else
            {
                MessageBox.Show("Заповніть коректно усі поля!");
            }
        }

        private void passBoxInitial_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CheckPass(passBoxInitial);
        }

        private void passBoxSecond_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CheckPass(passBoxSecond, passBoxInitial);
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxName, "Ім'я");
        }

        private void textBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxSurname, "Прізвище");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void ComboBoxSpeciality_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxSpeciality, true); //?????????????????????????????????????
        }

        private void ComboBoxSpeciality_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxDepartment_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxDepartment, true);
        }

        private void ComboBoxDepartment_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.GetComboItems("department_name", "Departments",  ComboBoxDepartment);
            ExtensionsToCheckInput.GetComboItems("affair_type", "Affair_Types", ComboBoxSpeciality);
        }

        //функції фінальної перевірки
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

        private bool IsReadyToBeAdded(PasswordBox passBox, out string value)
        {
            if (passBox.Background == Brushes.Transparent && passBox.Password != null && passBox.Password != "")
            {
                value = passBox.Password.Trim();
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
        private void Clean(PasswordBox inputbox)
        {
            inputbox.Password = String.Empty;
            inputbox.Background = Brushes.Transparent;
            inputbox.ToolTip = null;
        }
    }
}
