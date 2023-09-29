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
    internal class ObjetivoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public List<Tipo> TiposObjetivo { get; private set; }

        private ObservableCollection<Objetivo> _objetivos;
        public ObservableCollection<Objetivo> Objetivos
        {
            get { return _objetivos; }
            set
            {
                _objetivos = value;
                OnPropertyChanged("Objetivos");
            }
        }

        private string dataPath = "Dados";

        public ObjetivoViewModel()
        {
            TiposObjetivo = new List<Tipo>();
            Objetivos = new ObservableCollection<Objetivo>();

            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            XElement xelement = XElement.Load($"{dataPath}/TiposObjetivo.xml");
            IEnumerable<XElement> tiposObjetivo = xelement.Elements();

            foreach (var tipo in tiposObjetivo)
            {
                Tipo tipoObjetivo = new Tipo()
                {
                    Id = int.Parse(tipo.Attribute("id").Value),
                    Name = tipo.Attribute("nome").Value
                };

                TiposObjetivo.Add(tipoObjetivo);
            }

            if (File.Exists($"{dataPath}/Objetivos.xml"))
            {
                XDocument doc = XDocument.Load($"{dataPath}/Objetivos.xml");
                IEnumerable<XElement> objetivos = doc.Root.Elements();

                foreach (var objetivo in objetivos)
                {
                    Objetivo obj = new Objetivo
                    {
                        Id = (int)objetivo.Attribute("id"),
                        GoalType = TiposObjetivo.Find(x => x.Id == (int)objetivo.Attribute("type")),
                        TargetValue = (double)objetivo.Attribute("value"),
                        IsActive = (bool)objetivo.Attribute("isActive"),
                    };

                    string deadlineStr = objetivo.Attribute("deadline")?.Value;
                    if (!string.IsNullOrEmpty(deadlineStr))
                    {
                        obj.Deadline = DateTime.Parse(deadlineStr);
                    }

                    string achievementDateStr = objetivo.Attribute("achievementDate")?.Value;
                    if (!string.IsNullOrEmpty(achievementDateStr))
                    {
                        obj.AchievementDate = DateTime.Parse(achievementDateStr);
                    }

                    Objetivos.Add(obj);
                }
            }
        }

        public int GetNewId()
        {
            string filePath = $"{dataPath}/Objetivos.xml";
            if (!File.Exists(filePath))
            {
                return 1;
            }

            XDocument doc = XDocument.Load(filePath);
            var count = doc.Root.Elements().Count();
            return count + 1;
        }

        public void AdicionaObjetivo(Objetivo objetivo)
        {
            Objetivos.Add(objetivo);

            string filePath = $"{dataPath}/Objetivos.xml";
            XDocument doc;
            if (File.Exists(filePath))
            {
                doc = XDocument.Load(filePath);
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement("Objetivos"));
            }

            doc.Root.Add(objetivo.ToXML());
            doc.Save(filePath);
        }

        public void AtualizaObjetivo(Objetivo objetivo)
        {
            // Atualizando a lista em memória
            int index = Objetivos.IndexOf(objetivo);
            if (index != -1)
            {
                Objetivos[index] = objetivo;  // Isso fará com que a ObservableCollection dispare um evento de alteração de propriedade, atualizando a interface do usuário.
            }

            // Atualizando o arquivo XML
            string filePath = $"{dataPath}/Objetivos.xml";
            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);

                var xmlObjetivo = doc.Root.Elements().FirstOrDefault(x => (int)x.Attribute("id") == objetivo.Id);
                if (xmlObjetivo != null)
                {
                    xmlObjetivo.SetAttributeValue("type", objetivo.GoalType.Id);
                    xmlObjetivo.SetAttributeValue("value", objetivo.TargetValue);
                    xmlObjetivo.SetAttributeValue("deadline", objetivo.Deadline);
                    xmlObjetivo.SetAttributeValue("isActive", objetivo.IsActive);
                    xmlObjetivo.SetAttributeValue("achievementDate", objetivo.AchievementDate);

                    doc.Save(filePath);
                }
            }
        }



        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
