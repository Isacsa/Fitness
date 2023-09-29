using LAB2ANO2SEMESTRE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.ModelViews
{
    internal class CaracteristicasFisicasViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<TipoRegistoFisico> TiposRegistoFisico { get; private set; }

        private ObservableCollection<AlteracoesFisicas> _alteracoesFisicas;
        public ObservableCollection<AlteracoesFisicas> AlteracoesFisicas
        {
            get { return _alteracoesFisicas; }
            set
            {
                _alteracoesFisicas = value;
                OnPropertyChanged("AlteracoesFisicas");
            }
        }

        private string dataPath = "Dados";

        public CaracteristicasFisicasViewModel()
        {
            TiposRegistoFisico = new List<TipoRegistoFisico>();
            AlteracoesFisicas = new ObservableCollection<AlteracoesFisicas>();

            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            XElement xelement = XElement.Load($"{dataPath}/TiposRegistoFisico.xml");
            IEnumerable<XElement> tiposRegistoFisico = xelement.Elements();

            foreach (var tipo in tiposRegistoFisico)
            {
                TipoRegistoFisico tipoRegistoFisico = new TipoRegistoFisico()
                {
                    Id = int.Parse(tipo.Attribute("id").Value),
                    Name = tipo.Attribute("nome").Value,
                    Unidade = tipo.Attribute("unidade").Value
                };

                TiposRegistoFisico.Add(tipoRegistoFisico);
            }

            if (File.Exists($"{dataPath}/AlteracoesFisicas.xml"))
            {
                XDocument doc = XDocument.Load($"{dataPath}/AlteracoesFisicas.xml");
                IEnumerable<XElement> alteracoesFisicas = doc.Root.Elements();

                foreach (var alteracao in alteracoesFisicas)
                {
                    AlteracoesFisicas alteracoes = new AlteracoesFisicas
                    {
                        Id = (int)alteracao.Attribute("id"),
                        Characteristic = TiposRegistoFisico.Find(x => x.Id == (int)alteracao.Attribute("type")),
                        Date = DateTime.Parse((string)alteracao.Attribute("date")),
                        Value = (double)alteracao.Attribute("value")
                    };
                    AlteracoesFisicas.Add(alteracoes);
                }
            }
        }

        public int GetNewId()
        {
            string filePath = $"{dataPath}/AlteracoesFisicas.xml";
            if (!File.Exists(filePath))
            {
                return 1;
            }

            XDocument doc = XDocument.Load(filePath);
            var count = doc.Root.Elements().Count();
            return count + 1;
        }

        public void AdicionaAlteracao(AlteracoesFisicas alteracao)
        {
            AlteracoesFisicas.Add(alteracao);

            string filePath = $"{dataPath}/AlteracoesFisicas.xml";
            XDocument doc;
            if (File.Exists(filePath))
            {
                doc = XDocument.Load(filePath);
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement("Alteracoes"));
            }

            doc.Root.Add(alteracao.ToXML());
            doc.Save(filePath);
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
