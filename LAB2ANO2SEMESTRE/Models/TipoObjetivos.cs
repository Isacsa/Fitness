using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace LAB2ANO2SEMESTRE.Models
{
    public class TipoObjetivos : Tipo
    {
        public string Unidade { get; set; }

        public new XElement ToXML()
        {
            XElement no = new XElement("tipoObjetivos");
            no.Add(new XAttribute("id", Id));
            no.Add(new XAttribute("nome", Name));
            no.Add(new XAttribute("unidade", Unidade));

            return no;
        }


    }
}
