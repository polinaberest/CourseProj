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
    /// Interaction logic for EditAffair.xaml
    /// </summary>
    public partial class EditAffair : Window
    {
        internal Criminal processed;

        private bool isEdited;
        private bool bandNameIsEdited;
        public EditAffair(object criminal)
        {
            InitializeComponent();
            processed = (Criminal)criminal;
            FillAllFields(processed);
            //изменения происходят с объектами списка
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxName, "Ім'я");
            isEdited = true;
        }

        private void textBoxSurname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxSurname, "Прізвище");
            isEdited = true;
        }

        private void textBoxNickname_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxNickname, "Прізвисько");
            isEdited = true;
        }

        private void textBoxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CheckHeight(textBoxHeight);
            isEdited = true;
        }

        private void textBoxEyeColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxEyeColor);
            isEdited = true;
        }

        private void textBoxHairColor_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxHairColor);
            isEdited = true;
        }

        private void textBoxSpecialFeatures_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenArrayTextChanged(textBoxSpecialFeatures);
            isEdited = true;
        }

        private void textBoxCitizenship_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxCitizenship, true);
            isEdited = true;
        }

        private void textBoxBirthday_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.DateIsCorrect(textBoxBirthday);
            isEdited = true;
        }

        private void textBoxBirthPlace_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxBirthPlace, true);
            isEdited = true;
        }

        private void textBoxLastAccomodation_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxLastAccomodation, true);
            isEdited = true;
        }

        private void textBoxLanguages_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenArrayTextChanged(textBoxLanguages);
            isEdited = true;
        }

        private void textBoxJob_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxJob, true);
            isEdited = true;
        }

        private void textBoxLastAffair_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxLastAffair, true);
            isEdited = true;
        }

        private void textBoxBandName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtensionsToCheckInput.CommonWarningWhenTextChanged(textBoxBandName, true);
            isEdited = true;
            bandNameIsEdited = true;
        }

        private void DeleteData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditData_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxBandName.Text == "")
                textBoxBandName.Text = "НЕ є членом банди";

            if (!isEdited)
            {
                MessageBox.Show("Ви не внесли жодних змін!");
                return;
            }
            if (IsReadyToBeEdited())
            {
                if ((processed.IsInBand == false && !bandNameIsEdited)
                   || (processed.IsInBand && !bandNameIsEdited))
                {
                    processed.Name = textBoxName.Text.Trim();
                    processed.Surname = textBoxSurname.Text.Trim();
                    processed.Nickname = textBoxNickname.Text.Trim();
                    processed.Height = int.Parse(textBoxHeight.Text.Trim());
                    processed.HairColor = textBoxHairColor.Text.Trim();
                    processed.EyeColor = textBoxEyeColor.Text.Trim();
                    processed.SpecialFeatures = textBoxSpecialFeatures.Text.Trim();
                    processed.Citizenship = textBoxCitizenship.Text.Trim();
                    processed.DateOfBirth = textBoxBirthday.Text.Trim();
                    processed.LastAccomodation = textBoxLastAccomodation.Text.Trim();
                    processed.PlaceOfBirth = textBoxBirthPlace.Text.Trim();
                    processed.Languages = textBoxLanguages.Text.Trim();
                    processed.CriminalJob = textBoxJob.Text.Trim();
                    processed.LastAffair = textBoxLastAffair.Text.Trim();

                    InterpolCardIndex.SortByNames(InterpolCardIndex.criminals);
                    //вызвать метод сортировки листа преступников по ФИО
                    MessageBox.Show("Зміни збережено! - без змін банди");
                }
                else
                {
                    if (bandNameIsEdited)
                    {
                        InterpolCardIndex.criminalsFoundByRequest.Remove(processed);
                        InterpolCardIndex.criminals.Remove(processed);
                        if (processed.IsInBand)
                        {
                            foreach (var band in InterpolCardIndex.allBands)
                            {
                                if (band.BandName == processed.BandName)
                                {
                                    band.members.Remove(processed);
                                }
                            }
                            foreach (CrimeBand band in InterpolCardIndex.allBands)
                            {
                                if (band.members.Count == 0)
                                {
                                    InterpolCardIndex.allBands.Remove(band);
                                }
                            }
                        }
                        bool _isInBand = false;
                        string _bandName = textBoxBandName.Text.Trim();
                        if (_bandName != "НЕ є членом банди")
                        {
                            _isInBand = true;
                        }

                        InterpolCardIndex.AddCriminal(new Criminal(textBoxName.Text.Trim(), textBoxSurname.Text.Trim(), textBoxNickname.Text.Trim(),
                                int.Parse(textBoxHeight.Text.Trim()), textBoxEyeColor.Text.Trim(), textBoxHairColor.Text.Trim(), textBoxSpecialFeatures.Text.Trim(),
                            textBoxCitizenship.Text.Trim(), textBoxBirthday.Text.Trim(), textBoxBirthPlace.Text.Trim(), textBoxLastAccomodation.Text.Trim(), textBoxLanguages.Text.Trim(), textBoxJob.Text.Trim(), textBoxLastAffair.Text.Trim(), _isInBand, _bandName));
                        InterpolCardIndex.SortByNames(InterpolCardIndex.criminals);
                        MessageBox.Show("Зміни збережено! - зміна назви банди");
                    }
                }
                BackInResultsForm_Click(null, null);
            }
            else 
            {
                MessageBox.Show("Внесені зміни не є коректними!");
            }
        }

        private void ArchiveData_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FillAllFields(Criminal criminal)
        {
            textBoxName.Text = criminal.Name;
            textBoxSurname.Text = criminal.Surname;
            textBoxNickname.Text = criminal.Nickname;
            textBoxHeight.Text = criminal.Height.ToString();
            textBoxEyeColor.Text = criminal.EyeColor;
            textBoxHairColor.Text = criminal.HairColor;
            textBoxSpecialFeatures.Text = criminal.SpecialFeatures;
            textBoxCitizenship.Text = criminal.Citizenship;
            textBoxBirthday.Text = criminal.DateOfBirth;
            textBoxBirthPlace.Text = criminal.PlaceOfBirth;
            textBoxLastAccomodation.Text = criminal.LastAccomodation;
            textBoxLanguages.Text = criminal.Languages;
            textBoxJob.Text = criminal.CriminalJob;
            textBoxLastAffair.Text = criminal.LastAffair;
            
            if (criminal.IsInBand)
            {
                textBoxBandName.Text = criminal.BandName;
            }
            else if (!criminal.IsInBand)
            {
                textBoxBandName.Text = "НЕ є членом банди";
            }
            isEdited = false;
            bandNameIsEdited = false;
        }

        private bool IsReadyToBeEdited()
        {
            int count = 0;

            foreach (object el in Form.Children)
            {
                if (el is TextBox)
                {
                    TextBox tb = (TextBox)el;
                    if (tb.Background == Brushes.Transparent && tb.Text != null && tb.Text != "")
                    {
                        count++;
                    }
                }
            }

            if(count==15)
                return true;
            return false;
        }

        private void BackInResultsForm_Click(object sender, RoutedEventArgs e)
        {
            SearchAffair search = new SearchAffair();
            search.Show();
            this.Close();
        }
    }
}
