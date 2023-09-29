using LAB2ANO2SEMESTRE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.ModelViews
{
    internal class AtividadeViewModel
    {
        public List<TipoAtividade> TiposAtividade { get; private set; }
        public List<Calcado> Calcados { get; private set; }

        public AtividadeViewModel()
        {
            TiposAtividade = new List<TipoAtividade>();
            Calcados = new List<Calcado>();

            // Adicione o caminho para o seu arquivo XML de TiposAtividade
            XDocument docAtividades = XDocument.Load("Dados/TiposAtividade.xml");

            foreach (XElement no in docAtividades.Root.Elements("tipoAtividade"))
            {
                TipoAtividade atividade = new TipoAtividade();
                atividade.Id = Int32.Parse(no.Attribute("id").Value);
                atividade.Name = no.Attribute("nome").Value;

                TiposAtividade.Add(atividade);
            }

            // Adicione o caminho para o seu arquivo XML de calcados
            XDocument docCalcados = XDocument.Load("Dados/calcados.xml");

            foreach (XElement no in docCalcados.Root.Elements("Calcado"))
            {
                bool isActive = Boolean.Parse(no.Attribute("IsActive").Value);

                if (isActive)
                {
                    Calcado calcado = new Calcado();
                    calcado.Id = Int32.Parse(no.Attribute("Id").Value);
                    calcado.Brand = no.Attribute("Brand").Value;
                    calcado.Model = no.Attribute("Model").Value;
                    calcado.Color = no.Attribute("Color").Value;
                    double nrKilometros;
                    if (Double.TryParse(no.Attribute("NrKilometros").Value, out nrKilometros))
                    {
                        calcado.NrKilometros = nrKilometros;
                    }
                    else
                    {
                        // Trate o erro de conversão, por exemplo, atribuindo um valor padrão ou lidando com a lógica apropriada.
                    }

                    calcado.PurchaseDate = DateTime.Parse(no.Attribute("PurchaseDate").Value);
                    calcado.IsActive = isActive;

                    Calcados.Add(calcado);
                }
            }
        }
        public void UpdateActivityInXML(Atividade atividade, string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            var activityNode = doc.Descendants("Atividade").FirstOrDefault(x => x.Attribute("Id").Value == atividade.Id.ToString());

            if (activityNode != null)
            {
                // Atualiza as propriedades da atividade no XML.
                activityNode.Attribute("Description").Value = atividade.Description;
                activityNode.Attribute("StartTime").Value = atividade.StartTime.ToString();
                activityNode.Attribute("EndTime").Value = atividade.EndTime.ToString();
                activityNode.Attribute("Distance").Value = atividade.Distance.ToString();
                activityNode.Attribute("Rhythm").Value = atividade.Rhythm.ToString();
                activityNode.Attribute("ShoeId").Value = atividade.ShoeId.ToString();
                activityNode.Attribute("TipoAtividade").Value = atividade.TipoAtividade.Name;
                activityNode.Attribute("Calcado").Value = atividade.Calcado.Model;

                doc.Save(filePath);
            }
        }

    }
}
