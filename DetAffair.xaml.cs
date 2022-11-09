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
    /// Interaction logic for DetAffair.xaml
    /// </summary>
    public partial class DetAffair : Window
    {
        string name;
        string surname;
        int badge_num;
        int department_id; 
        string reg_date;
        string last_visit_date;
        string pass;
        int type_id;
        string department_name;
        string affair_type;

        public DetAffair()
        {
            InitializeComponent();
            PoliceCardIndex.SelectDetectiveProps(PoliceCardIndex.DetectiveID, out name, out surname, out badge_num, out department_id, out reg_date, out last_visit_date, out pass, out type_id, out department_name, out affair_type);
            textBoxName.Text = name;
            textBoxSurname.Text = surname;
            BadgeNum.Text += badge_num.ToString();
            ComboBoxDepartment.Text = department_name;
            ComboBoxSpeciality.Text = affair_type;
            RegDate.Text += reg_date.ToString();
            LastDate.Text += last_visit_date.ToString();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.GetComboItems("department_name", "Departments", ComboBoxDepartment);
            ExtensionsToCheckInput.GetComboItems("affair_type", "Affair_Types", ComboBoxSpeciality);
        }

        private void BackInMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void ChangeDepartments_Click(object sender, RoutedEventArgs e)
        {
            EditTables editTables = new EditTables("dep");
            editTables.Show();
            this.Close();
        }

        private void ChangeSpecialities_Click(object sender, RoutedEventArgs e)
        {
            EditTables editTables = new EditTables("spec");
            editTables.Show();
            this.Close();
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
       {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxName, "Ім'я");
        }

        private void textBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxSurname, "Прізвище");
        }

        private void ComboBoxDepartment_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxDepartment, true);
        }

        private void ComboBoxDepartment_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBoxSpeciality_LostFocus(object sender, RoutedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(ComboBoxSpeciality, true);
        }

        private void ComboBoxSpeciality_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void ChangePass_Click(object sender, RoutedEventArgs e)
        {
            ChangePass ch = new ChangePass(badge_num);
            ch.Show();
        }

        private void SaveData_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBoxDepartment.Background != Brushes.Salmon && ComboBoxDepartment.Text.Trim() != "")
            {
                ExtensionsToCheckInput.InsertIfUnique(ComboBoxDepartment, "Departments", "department_name");
            }
            if (ComboBoxSpeciality.Background != Brushes.Salmon && ComboBoxSpeciality.Text.Trim() != "")
            {
                ExtensionsToCheckInput.InsertIfUnique(ComboBoxSpeciality, "Affair_Types", "affair_type");
            }

            string s;
            string n;
            int type;
            int dep;

            if (IsReadyToBeAdded(textBoxName, out n) &&
                IsReadyToBeAdded(textBoxSurname, out s) &&
                IsReadyToBeAdded(ComboBoxSpeciality, out type, "Affair_Types", "type_id", "affair_type") &&
                IsReadyToBeAdded(ComboBoxDepartment, out dep, "Departments", "department_id", "department_name")
                )
            {
                SqlCommand command = new SqlCommand($"UPDATE Detectives SET first_name = '{n}', surname = '{s}', type_id = {type}, department_id = {dep} WHERE detective_id = {PoliceCardIndex.DetectiveID};", PoliceCardIndex.GetSqlConnection());
                PoliceCardIndex.OpenConnection();
                command.ExecuteNonQuery();
                PoliceCardIndex.CloseConnection();

                MessageBox.Show("Особисті дані змінено!");
            }

            else {
                MessageBox.Show("Зміни не є коректними. Перевірте введені дані");
            }
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
    }
}
