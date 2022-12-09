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
    /// Interaction logic for Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void SkipAuthorization_Click(object sender, RoutedEventArgs e)
        {
            PoliceCardIndex.IsDetective = false;
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Authorize_Click(object sender, RoutedEventArgs e)
        {
            if (CheckAuthorization())
            {
                PoliceCardIndex.IsDetective = true;
                PoliceCardIndex.DetectiveID = PoliceCardIndex.FindIDbyBadge(Convert.ToInt32(textBoxPersonalNumber.Text.Trim()));

                DateTime lastVisit = DateTime.Now;
                string sqlFormattedLastVisit = lastVisit.ToString("yyy-MM-dd HH:mm:ss.fff");

                //змінити дату ласт візіт
                SqlCommand command = new SqlCommand($"UPDATE Detectives SET last_visit_date = '{sqlFormattedLastVisit}' WHERE detective_id = {PoliceCardIndex.DetectiveID};", PoliceCardIndex.GetSqlConnection());
                PoliceCardIndex.OpenConnection();
                command.ExecuteNonQuery();
                PoliceCardIndex.CloseConnection();

               // MessageBox.Show(PoliceCardIndex.DetectiveID.ToString());
                MainWindow main = new MainWindow();
                main.Show();
                this.Close();
            }
        }

        private void Registrate_Click(object sender, RoutedEventArgs e)
        {
            Registration reg = new Registration();
            reg.Show();
            this.Close();
        }

        private void textBoxPersonalNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void passBoxPass_PasswordChanged(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private bool CheckAuthorization()
        {
            if (textBoxPersonalNumber.Text.Trim() == "" || passBoxPass.Password.Trim() == "")
            {
                MessageBox.Show("Ви не заповнили обов'язкові поля для входу!");
                return false;
            }
            if (int.TryParse(textBoxPersonalNumber.Text, out int number))
            {
                if (PoliceCardIndex.IsInDetectives(number))
                {
                    if (ExtensionsToCheckInput.PasswordMatches(passBoxPass.Password.Trim(), number))
                        return true;
                    else
                        MessageBox.Show("Неправильний пароль. Вхід не дозволено");

                }
            }
            else if (!int.TryParse(textBoxPersonalNumber.Text, out int n))
            {
                MessageBox.Show("Введіть число у поле номеру значка");
            }
            return false;
        }
    }
}
