using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.Models
{
    public class TipoAtividade : Tipo
    {
        public new XElement ToXML()
        {
            XElement no = new XElement("tipoAtividade");
            no.Add(new XAttribute("id", Id));
            no.Add(new XAttribute("nome", Name));

            return no;
        }
    }
}
