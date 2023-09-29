using LAB2ANO2SEMESTRE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.ModelViews
{
    public class CalcadoViewModel
    {
        public List<Calcado> Calcados { get; set; } = new List<Calcado>();

        public event Action CalcadoAdicionado;
        public event Action DadosCarregados;

        public void AdicionaCalcado(Calcado calcado)
        {
            // Encontra o maior Id atualmente em uso.
            int maxId = 0;
            if (Calcados.Count > 0)
            {
                maxId = Calcados.Max(c => c.Id);
            }

            // Atribui o próximo Id ao novo Calcado.
            calcado.Id = maxId + 1;

            Calcados.Add(calcado);

            CalcadoAdicionado?.Invoke();
        }

        public void SaveToXML(string filePath)
        {
            XDocument doc = new XDocument(new XElement("Calcados"));

            foreach (Calcado c in Calcados)
            {
                doc.Root.Add(c.ToXML());
            }

            doc.Save(filePath);
        }

        public void LoadFromXML(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            XDocument doc = XDocument.Load(filePath);

            foreach (XElement no in doc.Root.Elements("Calcado"))
            {
                Calcado calcado = new Calcado();
                calcado.Id = int.Parse(no.Attribute("Id").Value);
                calcado.Brand = no.Attribute("Brand").Value;
                calcado.Model = no.Attribute("Model").Value;
                calcado.Color = no.Attribute("Color").Value;

                double nrKilometros;
                if (double.TryParse(no.Attribute("NrKilometros").Value, out nrKilometros))
                {
                    calcado.NrKilometros = nrKilometros;
                }
                else
                {
                    // Tratar o erro de conversão de NrKilometros, se necessário.
                }

                DateTime purchaseDate;
                if (DateTime.TryParse(no.Attribute("PurchaseDate").Value, out purchaseDate))
                {
                    calcado.PurchaseDate = purchaseDate;
                }
                else
                {
                    // Tratar o erro de conversão de PurchaseDate, se necessário.
                }

                bool isActive;
                if (bool.TryParse(no.Attribute("IsActive").Value, out isActive))
                {
                    calcado.IsActive = isActive;
                }
                else
                {
                    // Tratar o erro de conversão de IsActive, se necessário.
                }

                Calcados.Add(calcado);
            }

            DadosCarregados?.Invoke();
        }

        public void UpdateShoeInXML(Calcado calcado, string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            var shoeNode = doc.Descendants("Calcado").FirstOrDefault(x => x.Attribute("Id").Value == calcado.Id.ToString());

            if (shoeNode != null)
            {
                // Atualiza as propriedades do calcado no XML.
                shoeNode.Attribute("Brand").Value = calcado.Brand;
                shoeNode.Attribute("Model").Value = calcado.Model;
                shoeNode.Attribute("Color").Value = calcado.Color;
                shoeNode.Attribute("NrKilometros").Value = calcado.NrKilometros.ToString();
                shoeNode.Attribute("PurchaseDate").Value = calcado.PurchaseDate.ToString();
                shoeNode.Attribute("IsActive").Value = calcado.IsActive.ToString();

                doc.Save(filePath);
            }
        }
    }
}
