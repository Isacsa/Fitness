using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.Models
{
    public class Calcado
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public double NrKilometros { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsActive { get; set; }

        public XElement ToXML()
        {
            XElement no = new XElement("Calcado");
            no.Add(new XAttribute("Id", Id));
            no.Add(new XAttribute("Brand", Brand));
            no.Add(new XAttribute("Model", Model));
            no.Add(new XAttribute("Color", Color));
            no.Add(new XAttribute("NrKilometros", NrKilometros));
            no.Add(new XAttribute("PurchaseDate", PurchaseDate.ToShortDateString()));
            no.Add(new XAttribute("IsActive", IsActive));

            return no;
        }
    }
}
