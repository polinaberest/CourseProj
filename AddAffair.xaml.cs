﻿using System;
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
using System.Text.RegularExpressions;
using System.Windows.Media.Animation;

namespace CourseProj
{
    /// <summary>
    /// Interaction logic for AddAffair.xaml
    /// </summary>
    public partial class AddAffair : Window
    {

        public AddAffair()
        {
            InitializeComponent();
            checkBoxIsInBand.Checked += checkBox_Checked;
            checkBoxIsInBand.Unchecked += checkBox_Unchecked;
            //button animation
            DoubleAnimation btnAnimation = new DoubleAnimation();
            btnAnimation.From = 0;
            btnAnimation.To = 1;
            btnAnimation.Duration = TimeSpan.FromSeconds(2);
            AddData.BeginAnimation(Button.OpacityProperty, btnAnimation);
        }

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
            textBoxBandName.Visibility = Visibility.Visible;
        }

        private void checkBox_Unchecked(object sender, RoutedEventArgs e)
        {
            textBoxBandName.Visibility = Visibility.Hidden;
            textBoxBandName.Text = "";
        }

        private void AddData_Click(object sender, RoutedEventArgs e)
        {
            string name;
            string surname;
            string nickname;
            int height;
            string eyeColor;
            string hairColor;
            string specialFeatures;
            string citizenship;
            string dateOfBirth;
            string placeOfBirth;
            string lastAccomodation;
            string languages;
            string criminalJob;
            string lastAffair;
            bool isInBand;
            string? bandName;

            if (IsReadyToBeAdded(textBoxName, out name) &&
                IsReadyToBeAdded(textBoxSurname, out surname) &&
                IsReadyToBeAdded(textBoxNickname, out nickname) &&
                IsReadyToBeAdded(textBoxHeight, out height) &&
                IsReadyToBeAdded(textBoxEyeColor, out eyeColor) &&
                IsReadyToBeAdded(textBoxHairColor, out hairColor) &&
                IsReadyToBeAdded(textBoxSpecialFeatures, out specialFeatures) &&
                IsReadyToBeAdded(textBoxCitizenship, out citizenship) &&
                IsReadyToBeAdded(textBoxBirthday, out dateOfBirth) &&
                IsReadyToBeAdded(textBoxBirthPlace, out placeOfBirth) &&
                IsReadyToBeAdded(textBoxLastAccomodation, out lastAccomodation) &&
                IsReadyToBeAdded(textBoxLanguages, out languages) &&
                IsReadyToBeAdded(textBoxJob, out criminalJob) &&
                IsReadyToBeAdded(textBoxLastAffair, out lastAffair) &&
                IsReadyToBeAdded(textBoxBandName, checkBoxIsInBand.IsChecked, out bandName, out isInBand))
            {
                InterpolCardIndex.AddCriminal(new Criminal(name, surname, nickname, height, eyeColor, hairColor, specialFeatures,
                  citizenship, dateOfBirth, placeOfBirth, lastAccomodation, languages, criminalJob, lastAffair, isInBand, bandName));

                foreach (object el in Form.Children)
                {
                    if (el is TextBox)
                    {
                        CleansenForm((TextBox)el);
                    }
                }

                //MessageBox.Show("Це - Тарас, йому нормас!");
            }

            else 
            {
                MessageBox.Show("Заповніть усі поля!");
            }
        }

        //обробники подій
        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxName, "Ім'я");
        }

        private void textBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxSurname, "Прізвище");
        }

        private void textBoxNickname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxNickname, "Прізвисько");
        }

        private void textBoxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CheckHeight(textBoxHeight);
        }

        private void textBoxEyeColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxEyeColor);
        }

        private void textBoxHairColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxHairColor);
        }

        private void textBoxSpecialFeatures_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenArrayTextChanged(textBoxSpecialFeatures);
        }

        private void textBoxCitizenship_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxCitizenship, true);
        }

        private void textBoxBirthday_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.DateIsCorrect(textBoxBirthday);
        }

        private void textBoxBirthPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxBirthPlace, true);
        }

        private void textBoxLastAccomodation_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxLastAccomodation, true);
        }

        private void textBoxLanguages_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenArrayTextChanged(textBoxLanguages);
        }

        private void textBoxJob_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxJob, true);
        }

        private void textBoxLastAffair_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxLastAffair, true);
        }

        private void textBoxBandName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxBandName, true);
        }

        //функції для обробників подій

        



        //функція фінальної перевірки - на кнопці

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

        private bool IsReadyToBeAdded(TextBox textBox, bool? isChecked, out string value, out bool isInBand)
        {
            if ((bool)isChecked)
            {
                isInBand = true;
                if (textBox.Background == Brushes.Transparent && textBox.Text != null && textBox.Text != "")
                {
                    value = textBox.Text.Trim();
                    return true;
                }
                else 
                {
                    value = null;
                    return false;
                }
            }
            else
            {
                isInBand = false;
                value = null; 
                return true;
            }
            
        }

        //метод очистки всіх полів на формі

        private void CleansenForm(TextBox textBox)
        {
            textBox.Text = String.Empty;
            textBox.Background = Brushes.Transparent;
            textBox.ToolTip = null;
        }

        private void BackInAddForm_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            InterpolCardIndex.WriteToFile("criminals.txt");
            //InterpolCardIndex.WriteToFile("archived.txt");
        }
    }
}
