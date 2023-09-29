using System;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.Models
{
    public class AlteracoesFisicas
    {
        public int Id { get; set; }
        public TipoRegistoFisico Characteristic { get; set; }
        public DateTime Date { get; set; }
        public double Value { get; set; }

        public XElement ToXML()
        {
            return new XElement("Alteracao",
                new XAttribute("id", Id),
                new XAttribute("type", Characteristic.Id),
                new XAttribute("date", Date.ToString("yyyy-MM-dd")),
                new XAttribute("value", Value));
        }
    }
}
