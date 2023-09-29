using LAB2ANO2SEMESTRE.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.ModelViews
{
    public class ListaAtividadesViewModel
    {
        public List<Atividade> Atividades { get; set; }

        public ListaAtividadesViewModel()
        {
            Atividades = new List<Atividade>();
        }

        public void LoadData(string filePath)
        {
            var doc = XDocument.Load(filePath);

            CultureInfo ci = new CultureInfo("en-US");

            foreach (var node in doc.Descendants("Atividade"))
            {
                var atividade = new Atividade()
                {
                    Id = Convert.ToInt32(node.Attribute("Id").Value),
                    Description = node.Attribute("Description").Value,
                    StartTime = Convert.ToDateTime(node.Attribute("StartTime").Value),
                    EndTime = Convert.ToDateTime(node.Attribute("EndTime").Value),
                    Distance = Convert.ToDouble(node.Attribute("Distance").Value, ci),
                    Rhythm = Convert.ToDouble(node.Attribute("Rhythm").Value, ci),
                    ShoeId = node.Attribute("ShoeId") != null ? Convert.ToInt32(node.Attribute("ShoeId").Value) : (int?)null,
                    TipoAtividade = new TipoAtividade() { Name = node.Attribute("TipoAtividade").Value },
                    Calcado = new Calcado() { Model = node.Attribute("Calcado").Value }
                };
                Atividades.Add(atividade);
            }
        }

        public void SaveData(string filePath)
        {
            var doc = new XDocument();
            var root = new XElement("Atividades");
            CultureInfo ci = new CultureInfo("en-US");

            foreach (var atividade in Atividades)
            {
                var node = new XElement("Atividade",
                    new XAttribute("Id", atividade.Id),
                    new XAttribute("Description", atividade.Description),
                    new XAttribute("StartTime", atividade.StartTime),
                    new XAttribute("EndTime", atividade.EndTime),
                    new XAttribute("Distance", atividade.Distance),
                    new XAttribute("Rhythm", atividade.Rhythm),
                    new XAttribute("ShoeId", atividade.ShoeId?.ToString() ?? ""),
                    new XAttribute("TipoAtividade", atividade.TipoAtividade.Name),
                    new XAttribute("Calcado", atividade.Calcado.Model));

                root.Add(node);
            }
            doc.Add(root);
            doc.Save(filePath);
        }
       
    }
}
