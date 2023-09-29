using System;
using System.Globalization;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.Models
{
    public class Atividade
    {
        public int Id { get; set; }

        public TipoAtividade TipoAtividade { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Distance { get; set; }
        public double Rhythm { get; set; }
        public int? ShoeId { get; set; }
        public Calcado Calcado { get; set; }

        public XElement ToXML()
        {
            CultureInfo ci = new CultureInfo(CultureInfo.CurrentCulture.Name);
            ci.NumberFormat.NumberDecimalSeparator = ",";

            XElement no = new XElement("Atividade");
            no.Add(new XAttribute("Id", Id));
            no.Add(new XAttribute("TipoAtividade", TipoAtividade.Name));
            no.Add(new XAttribute("Description", Description));
            no.Add(new XAttribute("StartTime", StartTime.ToString()));
            no.Add(new XAttribute("EndTime", EndTime.ToString()));
            no.Add(new XAttribute("Distance", Distance));
            no.Add(new XAttribute("Rhythm", Rhythm));
            no.Add(new XAttribute("ShoeId", ShoeId));
            no.Add(new XAttribute("Calcado", Calcado.Model));

            return no;
        }
    }
}
