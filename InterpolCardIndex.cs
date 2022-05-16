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

        static InterpolCardIndex()
        {
            allBands = new List<CrimeBand>();
            criminals = new List<Criminal>();
            criminalsFoundByRequest = new List<Criminal>();
            archived = new List<Criminal>();
            //ReadFromFile();
            //Criminal cr = new Criminal("vasya", "pupkin", "vip", 185, "grey", "broun", "big nose", "russian", "18.04.2000", "Pupkinsk", "Hzz", "russian", "rushist", "operationZ", true, "banda");
            /*Criminal cr2 = new Criminal("vova", "putin", "hlo", 188, "red", "broun", "small", "russian", "19.04.2000", "Muhosransk", "Hzzz ", "russian", "rushist", "operationZ", true, "banda");
            Criminal cr3 = new Criminal("vova", "huuuutin", "hlo", 155, "red", "broun", "small", "russian", "19.04.2000", "Muhosransk", "Hzzz ", "russian", "rushist", "operationZ", false, null);
            Criminal cr4 = new Criminal("vova", "huuuuti", "hlo", 155, "red", "broun", "small", "russian", "19.04.2000", "Muhosransk", "Hzzz ", "russian", "rushist", "operationZ", false, null);
            Criminal cr5 = new Criminal("vova", "huuuti", "hlo", 155, "red", "broun", "small", "russian", "19.04.2000", "Muhosransk", "Hzzz ", "russian", "rushist", "operationZ", false, null);*/

           // criminals.Add(cr);
           /* criminals.Add(cr2);
           criminals.Add(cr3);
            criminals.Add(cr4);
            criminals.Add(cr5);*/


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

        public static void SearchNotinBand(Criminal prototype) 
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
                    if (CompareCriminals(processedProto, criminal))
                    {
                        criminalsFoundByRequest.Add(criminal);
                    }
            }
        }

        public static void SearchInBand(Criminal prototype)
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
                        if (CompareCriminals(processedProto, criminal))
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

                        if (CompareCriminals(processedProto, criminal))
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

        public static bool CompareCriminals(Criminal prototype, Criminal criminal)
        {
            if (prototype.Name == criminal.Name &&
                prototype.Surname == criminal.Surname &&
                prototype.Nickname == criminal.Nickname &&
                prototype.Height == criminal.Height &&
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
                //prototype.IsInBand == criminal.IsInBand &&
                prototype.BandName == criminal.BandName)
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
    }
}
