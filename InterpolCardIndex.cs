using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProj
{
    internal class InterpolCardIndex
    {
        public static List<CrimeBand>? allBands;

        public static List<Criminal>? criminals;

        public InterpolCardIndex()
        {
            allBands = new List<CrimeBand>();
            criminals = new List<Criminal>();
        }

        public static void AddCriminal(Criminal criminal)
        {
            criminals.Add(criminal);  
        }

        public static void AddBand(CrimeBand band)
        {
            allBands.Add(band);
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
                if (!criminal.IsInBand) 
                {
                    var processedProto = prototype.Clone();
                    
                        
                    //MakeNullsEquivalent(processedProto, criminal);
                    //CompareCriminals(processedProto, criminal);
                }
            }
        }

       

    }
}
