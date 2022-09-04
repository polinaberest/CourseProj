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
                InterpolCardIndex.ReadFromFile("criminals.txt");
                InterpolCardIndex.ReadFromFile("archived.txt");
            }
            InterpolCardIndex.SortByNames(InterpolCardIndex.Criminals);
            InterpolCardIndex.SortByNames(InterpolCardIndex.Archived);
        }

        private void EditAffair_Click(object sender, RoutedEventArgs e)
        {
            SearchAffair searchAffair = new SearchAffair();
            searchAffair.Show();
            this.Close();
        }

        private void AddAffair_Click(object sender, RoutedEventArgs e)
        {
            AddAffair addAffair = new AddAffair();
            addAffair.Show();
            this.Close();
        }


        private void SeeArchived_Click(object sender, RoutedEventArgs e)
        {
            InterpolCardIndex.SortByNames(InterpolCardIndex.Archived);
            Archived archive = new Archived(); 
            archive.Show();
            this.Close();
        }
    }
}
  