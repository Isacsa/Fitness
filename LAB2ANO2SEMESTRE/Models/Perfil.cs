using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.Models
{
    public class Perfil
    {

        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public string FotografiaPath { get; set; }

        public XElement ToXML()
        {
            return new XElement("Perfil",
                new XElement("Nome", Nome),
                new XElement("DataNascimento", DataNascimento.ToString("yyyy-MM-dd")),
                new XElement("Sexo", Sexo));
        

           
        }
    }
}
