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

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for Archived.xaml
    /// </summary>
    public partial class Archived : Window
    {
        // конструктор класу Archived
        public Archived()
        {
            InitializeComponent();
            DisplayArchive();
        }
        private void DisplayArchive()
        {
            if (PoliceCardIndex.Archived.Count == 0)
            {
                NothingFound.Visibility = Visibility.Visible;
                return;
            }
            int i = 1;

            foreach (Criminal criminal in PoliceCardIndex.Archived)
            {
                Button button = new Button();
                button.Content = i + ". " + criminal.ToString() + criminal.DateOfBirth +
                    "\t" + " Остання справа: " + criminal.LastAffair;
                button.FontSize = 20;
                button.HorizontalContentAlignment = HorizontalAlignment.Left;
                button.Margin = new Thickness(3);
                Color color = Color.FromRgb(204, 119, 34);
                button.Background = new SolidColorBrush(color);
                button.MaxWidth = 1200;
                button.Tag = i - 1;
                button.ToolTip = "Перемістити анкету з архіву до основної картотеки " + criminal.ToString();
                button.Click += Button_Click;
                ArchivedList.Children.Add(button);
                i++;
            }
        }

        // обробники подій
        private void BackInMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int count = (int)((Control)sender).Tag;
            MessageBoxResult result = MessageBox.Show(
                "Ви впевнені, що хочете перемістити анкету з архіву до основної картотеки ?\n" +
                "Натискаючи ОК, Ви підтверджуєте, що злочинець більше не належить до тих, хто виправився", 
                "Підтверждення деархівації", MessageBoxButton.OKCancel);
            
            if (result == MessageBoxResult.OK)
            {
                PoliceCardIndex.Unarchive(PoliceCardIndex.Archived[count]);
                MessageBox.Show("Справу деархівовано");
                PoliceCardIndex.SortByNames(PoliceCardIndex.Archived);
                Archived arch = new Archived();
                arch.Show();
                this.Close();
            }            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PoliceCardIndex.WriteToFile("criminals.txt");
            PoliceCardIndex.WriteToFile("archived.txt");
        }
    }
}
