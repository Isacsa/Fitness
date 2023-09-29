using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LAB2ANO2SEMESTRE.Models;
using System.Globalization;

namespace LAB2ANO2SEMESTRE.ModelViews
{
    public class ListaCalcadoViewModel
    {
        public List<Calcado> Calcados { get; set; }

        public ListaCalcadoViewModel()
        {
            Calcados = new List<Calcado>();
        }

        public void LoadData(string filePath)
        {
            var doc = XDocument.Load(filePath);

            foreach (var node in doc.Descendants("Calcado"))
            {
                var calcado = new Calcado()
                {
                    Id = int.Parse(node.Attribute("Id").Value),
                    Brand = node.Attribute("Brand").Value,
                    Model = node.Attribute("Model").Value,
                    Color = node.Attribute("Color").Value
                };

                double nrKilometros;
                if (double.TryParse(node.Attribute("NrKilometros").Value, out nrKilometros))
                {
                    calcado.NrKilometros = nrKilometros;
                }
                else
                {
                    
                }

                DateTime purchaseDate;
                if (DateTime.TryParse(node.Attribute("PurchaseDate").Value, out purchaseDate))
                {
                    calcado.PurchaseDate = purchaseDate;
                }
                else
                {
                    
                }

                bool isActive;
                if (bool.TryParse(node.Attribute("IsActive").Value, out isActive))
                {
                    calcado.IsActive = isActive;
                }
                else
                {
                    
                }

                Calcados.Add(calcado);
            }
        }

        public void SaveData(string filePath)
        {
            var doc = new XDocument();
            var root = new XElement("Calcados");

            foreach (var calcado in Calcados)
            {
                var node = new XElement("Calcado",
                    new XAttribute("Id", calcado.Id),
                    new XAttribute("Brand", calcado.Brand),
                    new XAttribute("Model", calcado.Model),
                    new XAttribute("Color", calcado.Color),
                    new XAttribute("NrKilometros", calcado.NrKilometros),
                    new XAttribute("PurchaseDate", calcado.PurchaseDate),
                    new XAttribute("IsActive", calcado.IsActive));

                root.Add(node);
            }
            doc.Add(root);
            doc.Save(filePath);
        }
    }
}
