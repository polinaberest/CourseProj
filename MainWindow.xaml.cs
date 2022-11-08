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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // лічильник кількості разів ініціалізації вікна за час роботи програми
        public static int initializer = 0;

        // конструктор класу
        public MainWindow()
        {
            initializer += 1;

            InitializeComponent();
            if (initializer == 1)
            {
                //PoliceCardIndex.ReadFromFile("criminals.txt");
                //PoliceCardIndex.ReadFromFile("archived.txt");
            }
            if (!PoliceCardIndex.IsDetective)
            {
                WarningTextBox.Visibility = Visibility.Visible;
                ChooseButtonText.Visibility = Visibility.Hidden;
                AddAffair.Background = Brushes.Gray;
                PersonalInfo.Visibility = Visibility.Hidden;
            }
            PoliceCardIndex.SortByNames(PoliceCardIndex.Criminals);
            PoliceCardIndex.SortByNames(PoliceCardIndex.Archived);
        }

        private void EditAffair_Click(object sender, RoutedEventArgs e)
        {
            SearchAffair searchAffair = new SearchAffair();
            searchAffair.Show();
            this.Close();
        }

        private void AddAffair_Click(object sender, RoutedEventArgs e)
        {
            if (PoliceCardIndex.IsDetective)
            {
                AddAffair addAffair = new AddAffair();
                addAffair.Show();
                this.Close();
            }
            else 
            {
                MessageBox.Show("Авторизуйтеся або зареєструйтеся в додатку, щоб додавати нові справи до картотеки!");
            }
        }


        private void SeeArchived_Click(object sender, RoutedEventArgs e)
        {
            PoliceCardIndex.SortByNames(PoliceCardIndex.Archived);
            Archived archive = new Archived(); 
            archive.Show();
            this.Close();
        }

        private void BackInAuthorization_Click(object sender, RoutedEventArgs e)
        {
            Authorization auth = new Authorization();
            auth.Show();
            this.Close();
        }

        private void PersonalInfo_Click(object sender, RoutedEventArgs e)
        {
            DetAffair det = new DetAffair();
            det.Show();
            this.Close();
        }
    }
}
  