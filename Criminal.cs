﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CourseProj
{
    internal class Criminal:ICloneable
    {
        private string name;
        private string surname;
        private string nickname;
        private int height;
        private string eyeColor;
        private string hairColor;
        private string specialFeatures;
        private string citizenship;
        private string dateOfBirth;
        private string placeOfBirth;
        private string lastAccomodation;
        private string languages;
        private string criminalJob;
        private string lastAffair;
        public bool IsInBand;
        private string? bandName;
        private CrimeBand band;

        public Criminal()
        {

        }

        public Criminal(
            string name,
            string surname,
            string nickname,
            int height,
            string eyeColor,
            string hairColor,
            string specialFeatures,
            string citizenship, 
            string dateOfBirth,
            string placeOfBirth,
            string lastAccomodation,
            string languages,
            string criminalJob,
            string lastAffair,
            bool isInBand,
            string? bandName)
        {
            Name = name;
            Surname = surname;
            Nickname = nickname;
            Height = height;
            EyeColor = eyeColor;
            HairColor = hairColor;
            SpecialFeatures = specialFeatures;
            Citizenship = citizenship;
            DateOfBirth = dateOfBirth;
            PlaceOfBirth = placeOfBirth;
            LastAccomodation = lastAccomodation;
            Languages = languages;
            CriminalJob = criminalJob;
            LastAffair = lastAffair;
            if (isInBand)
            {
                IsInBand = true;
                //тут функція перевірки на адекватність, обрізки(валідація) bandname
                BandName = AdoptBandName(bandName);
                band = SearchBand(BandName);
            }
            
        }

        //логіка пошуку банди, в якій є цей злочинець
        private CrimeBand SearchBand(string bandName)
        {
            if(InterpolCardIndex.allBands != null)
            { 
                foreach (CrimeBand band in InterpolCardIndex.allBands)
                {
                    if (band.BandName == bandName)
                    {
                        band.AddMember(this);
                        return band;
                    }
                }
            }

            CrimeBand newBand = new CrimeBand(bandName, new List<Criminal> { this });
            return newBand;
        }

        private string AdoptBandName(string bandName)
        {
            bandName = bandName.Replace('-', ' ');
            string[] wordArr = bandName.Split(" ");
            if (bandName != "" && bandName != null)
            {
                for (int i = 0; i < wordArr.Length; i++)
                {
                    wordArr[i] = wordArr[i].Trim();
                    wordArr[i] = (wordArr[i].Substring(0, 1).ToUpper() + wordArr[i].Substring(1).ToLower()).Trim();
                }
            }
            bandName = String.Join(" ", wordArr);
            return bandName;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return this.Name + " " + this.Surname + " (" + this.Nickname + ") ";
        }




        public string Name
        {
            get { return name; }
            set 
            {
                if (value.Length >= 1)
                    name = (value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower()).Trim();
                else
                    name = "";
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                if (value.Length >= 1)
                    surname = (value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower()).Trim();
                else
                    surname = "";
            }
        }

        public string Nickname
        {
            get { return nickname; }
            set
            {
                if (value.Length >= 1)
                    nickname = (value.Substring(0, 1).ToUpper() + value.Substring(1).ToLower()).Trim();
                else
                    nickname = "";
            }
        }

        public int Height
        { 
            get { return height; }
            set { height = value; }
        }

        public string EyeColor
        {
            get { return eyeColor; }
            set { eyeColor = value; }
        }

        public string HairColor
        {
            get { return hairColor; }
            set { hairColor = value; }
        }

        public string SpecialFeatures
        {
            get { return specialFeatures; }
            set { specialFeatures = value; }
        }

        public string Citizenship
        {
            get { return citizenship; }
            set { citizenship = value; }
        }

        public string DateOfBirth
        {
            get { return dateOfBirth; }
            set { dateOfBirth = value; }
        }

        public string PlaceOfBirth
        {
            get { return placeOfBirth; }
            set { placeOfBirth = value; }
        }

        public string LastAccomodation
        {
            get { return lastAccomodation; }
            set { lastAccomodation = value; }
        }

        public string Languages
        {
            get { return languages; }
            set { languages = value; }
        }

        public string CriminalJob
        {
            get { return criminalJob; }
            set { criminalJob = value; }
        }

        public string LastAffair
        {
            get { return lastAffair; }
            set { lastAffair = value; }
        }

       
        public CrimeBand Band
        {
            get { return band; }
            set { band = value; }
        }

       public string BandName
       {
            get { return bandName; }
            set { bandName = value; }
       }
    
    }
}
