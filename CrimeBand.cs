﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProj
{
    internal class CrimeBand
    {
        public List<Criminal> members;

        public string BandName { get; set; }
        
        public CrimeBand(string bandName, List<Criminal> members)
        {
            BandName = bandName;
            this.members = members;
            InterpolCardIndex.AddBand(this);
        }

        public string ShowMembers()
        {
            string str = "";
            foreach (var member in members)
            {
                str += member.Name + " " + member.Surname + "\n";
            }
            return str; 
        }

        public void AddMember(Criminal criminal) 
        {
            members.Add(criminal);
        }

        public void RemoveCriminal(Criminal criminal)
        {
            members.Remove(criminal);
        }
    }
}
