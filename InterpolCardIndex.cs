using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProj
{
    internal static class InterpolCardIndex
    {
        public static List<CrimeBand>? allBands;

        public static List<Criminal>? criminals;

        public static List<Criminal>? criminalsFoundByRequest;

        public static List<Criminal>? archived;

        public static List<Criminal>? foundToWrite;

        static InterpolCardIndex()
        {
            allBands = new List<CrimeBand>();
            criminals = new List<Criminal>();
            criminalsFoundByRequest = new List<Criminal>();
            archived = new List<Criminal>();
            foundToWrite = new List<Criminal>();
            //SortByNames(criminals);
        }

        public static void AddCriminal(Criminal criminal)
        {
            criminals.Add(criminal);
            SortByNames(criminals);
        }

        public static void AddBand(CrimeBand band)
        {
            allBands.Add(band);
        }

        public static void WriteToFile(string p)
        {
            string path = p;
            
            string str = "";
            string bandName = "-1";
            if (path == "criminals.txt")
            {
                foreach (var item in criminals)
                {
                    if (item.IsInBand)
                    {
                        bandName = item.BandName;
                    }
                    str += item.Name + ";" + item.Surname + ";" + item.Nickname + ";" + item.Height + ";"
                        + item.EyeColor + ";" + item.HairColor + ";" + item.SpecialFeatures + ";" + item.Citizenship + ";"
                        + item.DateOfBirth + ";" + item.PlaceOfBirth + ";" + item.LastAccomodation + ";"
                        + item.Languages + ";" + item.CriminalJob + ";" + item.LastAffair + ";" + item.IsInBand + ";" + bandName + "\n";
                }
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(str);
                    writer.Close();
                }
            }
            else if (path == "archived.txt")
            {
                foreach (var item in archived)
                {
                    if (item.IsInBand)
                    {
                        bandName = item.BandName;
                    }
                    str += item.Name + ";" + item.Surname + ";" + item.Nickname + ";" + item.Height + ";"
                        + item.EyeColor + ";" + item.HairColor + ";" + item.SpecialFeatures + ";" + item.Citizenship + ";"
                        + item.DateOfBirth + ";" + item.PlaceOfBirth + ";" + item.LastAccomodation + ";"
                        + item.Languages + ";" + item.CriminalJob + ";" + item.LastAffair + ";" + item.IsInBand + ";" + bandName + ";" + "\n";
                }
                using (StreamWriter writer = new StreamWriter(path, false))
                {
                    writer.Write(str);
                    writer.Close();
                }
            }
        }

        public static void ReadFromFile(string p)
        {
            string path = p;


            using (StreamReader reader = new StreamReader(path))
            {
                string? str;
                
                while ((str = reader.ReadLine()) != null) //null
                {
                    string[] propArr = str.Split(";", StringSplitOptions.RemoveEmptyEntries);
                    //вотуть прописать все поля имя = массив[0]
                    string name = propArr[0];
                    string surname = propArr[1];
                    string nickname = propArr[2];
                    int height = int.Parse(propArr[3]);
                    string eyeColor = propArr[4];
                    string hairColor = propArr[5];
                    string specialFeatures = propArr[6];
                    string citizenship = propArr[7];
                    string dateOfBirth = propArr[8];
                    string placeOfBirth = propArr[9];
                    string lastAcc = propArr[10];
                    string languages = propArr[11];
                    string job = propArr[12];
                    string lastAffair = propArr[13];
                    bool isInBand = Convert.ToBoolean(propArr[14]);
                    string bandName = propArr[15];

                    if (bandName == "-1")
                    {
                        bandName = "";
                    }

                    //добавить в лист криминалов
                    Criminal criminal = new Criminal(name, surname, nickname, height, eyeColor, hairColor, specialFeatures, citizenship, dateOfBirth, placeOfBirth, lastAcc, languages, job, lastAffair, isInBand, bandName);
                    if (p == "criminals.txt")
                        criminals.Add(criminal);
                    else if (p == "archived.txt")
                    {
                        archived.Add(criminal);
                        if (criminal.IsInBand)
                        {
                            foreach (var band in allBands)
                            {
                                if (band.BandName == criminal.BandName)
                                {
                                    band.members.Remove(criminal);
                                }
                            }
                        }
                    }
                    //ТУТ СДЕЛАТЬ ИСКЛЮЧЕНИЕ ИЗ ЧЛЕНОВ БАНДЫ, ЕСЛИ ЧУВАК В БАНДЕ
                }
                reader.Close();
            }
            
        }

        public static void SearchNotinBand(Criminal prototype, int[] hRange) 
        {
            if (criminals.Count==0)
            {
                // если все поля пустые или база преступников пуста, то нам нужно вывести на екран НИЧЕГО НЕ НАЙДЕНО
                return;
            }
            foreach (Criminal criminal in criminals)
            {
                    var processedProto = (Criminal)prototype.Clone();

                    MakeNullsEquivalent(processedProto, criminal);
                    if (CompareCriminals(processedProto, criminal, hRange))
                    {
                        criminalsFoundByRequest.Add(criminal);
                    }
            }
        }

        public static void SearchInBand(Criminal prototype, int[] hRange)
        {
            if (criminals.Count == 0 || allBands.Count == 0)
            {
                // база преступников / банд пуста, то нам нужно вывести на екран НИЧЕГО НЕ НАЙДЕНО
                return;
            }
            if (prototype.BandName == "")
            {
                allBands.RemoveAt(allBands.Count-1);
                foreach (Criminal criminal in criminals)
                {
                    if (criminal.IsInBand)
                    {
                        var processedProto = (Criminal)prototype.Clone();

                        MakeNullsEquivalent(processedProto, criminal);
                        if (CompareCriminals(processedProto, criminal, hRange))
                        {
                            criminalsFoundByRequest.Add(criminal);
                        }
                    }
                }
                return;
            }
            foreach (CrimeBand band in allBands)
            {
                if (prototype.BandName == band.BandName && band.members.Count!=0)
                {
                    band.members.Remove(prototype);
                    foreach (Criminal criminal in band.members)
                    {
                        var processedProto = (Criminal)prototype.Clone();
                        MakeNullsEquivalent(processedProto, criminal);

                        if (CompareCriminals(processedProto, criminal, hRange))
                        {
                            criminalsFoundByRequest.Add(criminal);
                        }
                    }
                }
            }
            /*foreach (CrimeBand band in allBands)
            {
                if (band.members.Count == 0)
                {
                    allBands.Remove(band);
                    return;
                }
            }*/
           
        }

        public static void MakeNullsEquivalent(Criminal prototype, Criminal criminal)
        { 
            var protoProps = prototype.GetType().GetProperties();
            var crimProps = criminal.GetType().GetProperties();
            int i = 0;
            foreach (var prop in crimProps)
            {
                 if (protoProps[i].GetValue(prototype)?.ToString() == "" || 
                    protoProps[i].GetValue(prototype)?.ToString() == "0" || 
                    protoProps[i].GetValue(prototype)?.ToString() == null)
                 { 
                    protoProps[i].SetValue(prototype, prop.GetValue(criminal));
                 }
                i++;
            }

        }

        public static bool CompareCriminals(Criminal prototype, Criminal criminal, int[] hRange)
        {
            if (prototype.Name == criminal.Name &&
                prototype.Surname == criminal.Surname &&
                prototype.Nickname == criminal.Nickname &&
                prototype.EyeColor == criminal.EyeColor &&
                prototype.HairColor == criminal.HairColor &&
                prototype.SpecialFeatures == criminal.SpecialFeatures &&
                prototype.Citizenship == criminal.Citizenship &&
                prototype.DateOfBirth == criminal.DateOfBirth &&
                prototype.PlaceOfBirth == criminal.PlaceOfBirth &&
                prototype.LastAccomodation == criminal.LastAccomodation &&
                prototype.CriminalJob == criminal.CriminalJob &&
                prototype.Languages == criminal.Languages &&
                prototype.LastAffair == criminal.LastAffair &&
                prototype.BandName == criminal.BandName)
            {
                if (hRange[0] == -1)
                    return true;
                else if (HeightEquals(criminal.Height, hRange))
                    return true;
            }
                    
            return false;
        }

        private static bool HeightEquals(int crHeight, int[] range)
        {
            if (crHeight >= range[0] && crHeight <= range[1])
                return true;
            return false;
        }

        public static void SortByNames(List<Criminal> list)
        {
            list.Sort(delegate (Criminal x, Criminal y)
            {
                if (x.Name + x.Surname == null && y.Name + y.Surname == null) return 0;
                else if (x.Name + x.Surname == null) return -1;
                else if (y.Name + y.Surname == null) return 1;
                else return (x.Name + x.Surname).CompareTo(y.Name + y.Surname);
            });
        }

        public static void DeleteAffair(Criminal processed)
        {
            criminalsFoundByRequest.Remove(processed);
            criminals.Remove(processed);

            if (processed.IsInBand)
            {
                foreach (var band in allBands)
                {
                    if (band.BandName == processed.BandName)
                    {
                        band.members.Remove(processed);
                    }
                }
            }
        }

        public static void ArchiveAffair(Criminal processed)
        {
            archived.Add(processed);
            criminalsFoundByRequest.Remove(processed);
            criminals.Remove(processed);
            if (processed.IsInBand)
            {
                foreach (var band in allBands)
                {
                    if (band.BandName == processed.BandName)
                    {
                        band.members.Remove(processed);
                    }
                }
            }
        }

        public static void Unarchive(Criminal criminal)
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

        public static void FormListToPrint(Criminal criminal, out bool isIncluded)
        {
            isIncluded = false;
            if (foundToWrite.Count == 0)
            {
                foundToWrite.Add(criminal);
                isIncluded = true;
                return;
            }
            if (foundToWrite.Contains(criminal))
            {
                foundToWrite.Remove(criminal);
                return;
            }
            else {
                foundToWrite.Add(criminal);
                isIncluded = true;
                return;
            }
        }

        public static void WriteResults()
        {
            string path = "";
            string header = "\t\t\tВитяг з картотеки Інтерполу\n\n";
            string str = "";
            string band = "";
            string footer = "\n\n\t\t\tДата та час звернення до картотеки Інтерполу: " + DateTime.Now;
            // Configure save file dialog box
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "results"; // Default file name
            dialog.DefaultExt = ".pdf"; // Default file extension
            dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            bool? result = dialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                path = dialog.FileName;
            }
            foreach (Criminal criminal in foundToWrite)
            {
                if (criminal.IsInBand)
                {
                    band = "\nЗлочинець є членом банди " + criminal.BandName;
                }
                str += (foundToWrite.IndexOf(criminal) + 1) + ". "
                    + criminal.ToString() + "\nДата народження: " + criminal.DateOfBirth
                    + "\nЗріст: " + criminal.Height
                    + "\nКолір очей: " + criminal.EyeColor
                    + "\tКолір волосся: " + criminal.HairColor
                    + "\nОсобливі прикмети: " + criminal.SpecialFeatures
                    + "\nГромадянство: " + criminal.Citizenship
                    + "\nМісце народження: " + criminal.PlaceOfBirth
                    + "\tМісце останнього проживання: " + criminal.LastAccomodation
                    + "\nВолодіє мовами: " + criminal.Languages
                    + "\nКримінальний фах: " + criminal.CriminalJob
                    + "\nОстання справа: " + criminal.LastAffair
                    + band + "\n\n";
            }
            using (StreamWriter writer = new StreamWriter(path, false))
            {
                writer.Write(header + str + footer);
                writer.Close();
            }

        }
    }
}
