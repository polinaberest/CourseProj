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


namespace CourseProj
{
    /// <summary>
    /// Interaction logic for ChangePass.xaml
    /// </summary>
    public partial class ChangePass : Window
    {
        int number;
        string pass;

        public ChangePass(int number)
        {
            InitializeComponent();
            Change.IsEnabled = false;
            this.number = number;
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            if (Change.IsEnabled == true &&
               IsReadyToBeAdded(passBoxInitial, out pass) &&
               passBoxSecond.Background == Brushes.Transparent && passBoxSecond.Password != null && passBoxSecond.Password != "" &&
               passBoxOld.Background == Brushes.Transparent && passBoxOld.Password != null && passBoxOld.Password != "")
            {
                //змінити пароль
                SqlCommand command = new SqlCommand($"UPDATE Detectives SET pass = '{pass}' WHERE detective_id = {PoliceCardIndex.DetectiveID};", PoliceCardIndex.GetSqlConnection());
                PoliceCardIndex.OpenConnection();
                command.ExecuteNonQuery();
                PoliceCardIndex.CloseConnection();

                MessageBox.Show($"Пароль змінено!");
                this.Close();
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

        private void passBoxOld_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void passBoxOld_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passBoxOld.Password.Trim() == "")
            {
                passBoxOld.ToolTip = "Введіть старий пароль!";
                passBoxOld.Background = Brushes.Salmon;
                Change.IsEnabled = false;
            }
            if (ExtensionsToCheckInput.PasswordMatches(passBoxOld.Password.Trim(), number))
            {
                passBoxOld.Background = Brushes.Transparent;
                Change.IsEnabled = true;
            }
            else
            {
                passBoxOld.ToolTip = "Неправильний старий пароль. Вхід не дозволено";
                passBoxOld.Background = Brushes.Salmon;
                Change.IsEnabled = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void BackInDetAffair_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
    }
}
