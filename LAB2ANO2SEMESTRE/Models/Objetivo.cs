using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.Models
{
    internal class Objetivo
    {
        public int Id { get; set; }
        public Tipo GoalType { get; set; }
        public double TargetValue { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsActive { get; set; }
        public DateTime? AchievementDate { get; set; }


        public XElement ToXML()
        {
            return new XElement("Objetivo",
                new XAttribute("id", Id),
                new XAttribute("type", GoalType.Id),
                new XAttribute("value", TargetValue),
                new XAttribute("deadline", Deadline.HasValue ? Deadline.Value.ToString("s") : string.Empty),
                new XAttribute("isActive", IsActive),
                new XAttribute("achievementDate", AchievementDate.HasValue ? AchievementDate.Value.ToString("s") : string.Empty)
            );
        }
    }
    
}
