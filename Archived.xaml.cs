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
        public Archived()
        {
            InitializeComponent();
            DisplayArchive();
        }

        private void BackInMain_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void DisplayArchive()
        {
            if (InterpolCardIndex.archived.Count == 0)
            {
                NothingFound.Visibility = Visibility.Visible;
                return;
            }
            int i = 1;

            foreach (Criminal criminal in InterpolCardIndex.archived)
            {
                Button button = new Button();
                button.Content = i + ". " + criminal.ToString() + "\t" + criminal.DateOfBirth + " Остання справа: " + criminal.LastAffair;
                button.FontSize = 20;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int count = (int)((Control)sender).Tag;
            MessageBoxResult result = MessageBox.Show("Ви впевнені, що хочете перемістити анкету з архіву до основної картотеки ?\nНатискаючи ОК, Ви підтверджуєте, що злочинець більше не належить до тих, хто виправився", "Header", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                Unarchive(InterpolCardIndex.archived[count]);
                MessageBox.Show("Справу деархівовано");
                InterpolCardIndex.SortByNames(InterpolCardIndex.archived);

                Archived arch = new Archived();
                arch.Show();
                this.Close();
            }            
        }

        private void Unarchive(Criminal criminal)
        { 
            InterpolCardIndex.AddCriminal(criminal);
            if (criminal.IsInBand)
            {
                if (InterpolCardIndex.allBands != null)
                {
                    foreach (CrimeBand band in InterpolCardIndex.allBands)
                    {
                        if (band.BandName == criminal.BandName)
                        {
                            band.AddMember(criminal);
                        }
                    }
                }

                else
                {
                    CrimeBand newBand = new CrimeBand(criminal.BandName, new List<Criminal> { criminal });
                }
            }
            InterpolCardIndex.archived.Remove(criminal);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            InterpolCardIndex.WriteToFile("criminals.txt");
            InterpolCardIndex.WriteToFile("archived.txt");
        }
    }
}
