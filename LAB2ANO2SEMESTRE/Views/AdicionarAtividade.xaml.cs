using LAB2ANO2SEMESTRE.ModelViews;
using System;
using System.Windows;
using System.Windows.Controls;
using LAB2ANO2SEMESTRE.Models;
using System.Xml.Linq;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Xml;

using Microsoft.Win32;
using Geo = GeoCoordinatePortable.GeoCoordinate;

namespace LAB2ANO2SEMESTRE.Views
{
    /// <summary>
    /// Interaction logic for AdicionarAtividade.xaml
    /// </summary>
    public partial class AdicionarAtividade : Window
    {
        AtividadeViewModel viewModel;

        public AdicionarAtividade()
        {
            InitializeComponent();

            viewModel = new AtividadeViewModel();
            ShoeComboBox.ItemsSource = viewModel.Calcados;
            cbTiposAtividade.ItemsSource = viewModel.TiposAtividade;

            this.DataContext = viewModel;
        }

        private void AddActivity_Click(object sender, RoutedEventArgs e)
        {
            // Carrega o arquivo XML
            XDocument doc;
            string filePath = "Dados/Atividades.xml";
            if (File.Exists(filePath))
            {
                doc = XDocument.Load(filePath);
            }
            else
            {
                doc = new XDocument();
                doc.Add(new XElement("Atividades"));
            }

            // Gera um ID exclusivo
            int maxId = 0;
            foreach (XElement no in doc.Root.Elements("Atividade"))
            {
                int id = Int32.Parse(no.Attribute("Id").Value);
                if (id > maxId)
                {
                    maxId = id;
                }
            }
            int uniqueId = maxId + 1;

            // Cria uma nova atividade com os dados dos campos
            Atividade atividade = new Atividade()
            {
                Id = uniqueId,
                TipoAtividade = (TipoAtividade)cbTiposAtividade.SelectedItem,
                Description = Descricao.Text,
                StartTime = DataAtividade.SelectedDate.Value.AddHours(Double.Parse(HoraInicio.Text)).AddMinutes(Double.Parse(MinutoInicio.Text)),
                EndTime = DataAtividade.SelectedDate.Value.AddHours(Double.Parse(HoraFim.Text)).AddMinutes(Double.Parse(MinutoFim.Text)),
                Distance = double.Parse(Distancia.Text),
                ShoeId = ((Calcado)ShoeComboBox.SelectedItem).Id,
                Calcado = (Calcado)ShoeComboBox.SelectedItem
            };

            // Calcula a duração em minutos e depois o ritmo
            double duration = (atividade.EndTime - atividade.StartTime).TotalMinutes;
            atividade.Rhythm = duration / atividade.Distance;

            // Atualize a distância do calçado
            Calcado selectedShoe = (Calcado)ShoeComboBox.SelectedItem;
            selectedShoe.NrKilometros += atividade.Distance;

            // Atualize o arquivo XML do calçado
            UpdateShoeInXML(selectedShoe, "Dados/Calcados.xml");

            // Adiciona a nova atividade
            doc.Root.Add(atividade.ToXML());

            // Salva o arquivo
            doc.Save(filePath);

            // Exibe uma mensagem informando que uma nova atividade foi adicionada
            MessageBox.Show("Uma nova atividade foi adicionada com sucesso!");
        }

        public void UpdateShoeInXML(Calcado shoe, string filePath)
        {
            XDocument doc = XDocument.Load(filePath);
            var shoeNode = doc.Descendants("Calcado").FirstOrDefault(x => x.Attribute("Id").Value == shoe.Id.ToString());

            if (shoeNode != null)
            {
                shoeNode.Attribute("NrKilometros").Value = shoe.NrKilometros.ToString();
                doc.Save(filePath);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "GPX files (*.gpx)|*.gpx";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true) // Se o usuário selecionar um arquivo
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(openFileDialog.FileName); // Use o caminho do arquivo selecionado

                XmlNodeList points = doc.GetElementsByTagName("trkpt");
                GeoCoordinatePortable.GeoCoordinate previousPoint = null;
                double totalDistance = 0;

                DateTime startTime = DateTime.MinValue;
                DateTime endTime = DateTime.MinValue;
                DateTime currentTime = DateTime.MinValue;

                foreach (XmlNode point in points)
                {
                    double lat = double.Parse(point.Attributes["lat"].Value, CultureInfo.InvariantCulture);
                    double lon = double.Parse(point.Attributes["lon"].Value, CultureInfo.InvariantCulture);
                    GeoCoordinatePortable.GeoCoordinate currentPoint = new GeoCoordinatePortable.GeoCoordinate(lat, lon);

                    if (previousPoint != null)
                    {
                        totalDistance += previousPoint.GetDistanceTo(currentPoint); // A distância é em metros
                    }

                    previousPoint = currentPoint;

                    // Para o horário de início e término
                    XmlNode timeNode = point.SelectSingleNode("time");
                    if (timeNode != null)
                    {
                        string timeString = timeNode.InnerText;

                        if (DateTime.TryParseExact(timeString, "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out currentTime))
                        {
                            if (startTime == DateTime.MinValue)
                            {
                                startTime = currentTime.ToLocalTime();
                            }

                            endTime = currentTime.ToLocalTime();
                        }
                        else
                        {
                            // Lida com o erro de extração do horário, se necessário
                        }
                    }
                }

                totalDistance = totalDistance / 1000; // Convertendo metros para quilômetros
                Distancia.Text = totalDistance.ToString("0.##"); // Exibir distância na TextBox Distancia

                // Exibir horário de início e término nas TextBoxes respectivas
                HoraInicio.Text = startTime.Hour.ToString();
                MinutoInicio.Text = startTime.Minute.ToString();
                HoraFim.Text = endTime.Hour.ToString();
                MinutoFim.Text = endTime.Minute.ToString();

                Console.WriteLine($"A distância total da viagem é {totalDistance} km"); // Distância em quilômetros
            }
        }




    }



}


