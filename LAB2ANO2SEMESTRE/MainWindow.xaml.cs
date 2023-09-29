using LAB2ANO2SEMESTRE.Views;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var doc = XDocument.Load("Dados/Atividades.xml");

            // Obter todas as atividades
            var atividadesXml = doc.Descendants("Atividade");

            // Calcular a distância total
            double distanciaTotal = atividadesXml
    .Where(a => double.TryParse(a.Attribute("Distance").Value, out _))
    .Sum(a => double.Parse(a.Attribute("Distance").Value));

            // Calcular o tempo total
            TimeSpan tempoTotal = TimeSpan.Zero;
            foreach (var atividadeXml in atividadesXml)
            {
                DateTime inicio = DateTime.Parse(atividadeXml.Attribute("StartTime").Value);
                DateTime termino = DateTime.Parse(atividadeXml.Attribute("EndTime").Value);
                tempoTotal += termino - inicio;
            }

            // Exibir a distância e o tempo total nos TextBlocks
            this.DistanciaTotalTextBlock.Text = distanciaTotal.ToString();
            this.TempoTotalTextBlock.Text = tempoTotal.ToString();

            // Exibir as últimas atividades na ListView
            // Assumindo que as atividades mais recentes estão no final do arquivo XML
            foreach (var atividadeXml in atividadesXml.TakeLast(5))
            {
                var distanceAttribute = atividadeXml.Attribute("Distance");
                double distanceValue = 0;

                if (distanceAttribute != null)
                {
                    double.TryParse(distanceAttribute.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out distanceValue);
                }

                var activityType = atividadeXml.Attribute("TipoAtividade")?.Value ?? string.Empty;

                this.UltimasAtividadesListView.Items.Add(new LAB2ANO2SEMESTRE.Models.Atividade
                {
                    Distance = distanceValue,
                    TipoAtividade = new LAB2ANO2SEMESTRE.Models.TipoAtividade
                    {
                        Name = activityType
                    }
                });

            }

        }

        private void ObjetivosButton_Click(object sender, EventArgs e)
        {
            Objetivos objetivosWindow = new Objetivos();
            objetivosWindow.ShowDialog();
        }
        private void AdicionarCalcadoButton_Click(object sender, RoutedEventArgs e)
        {
            AdicionarCalcado adicionarCalcadoWindow = new AdicionarCalcado();
            adicionarCalcadoWindow.ShowDialog();
        }
        private void ListaCalcadoButton_Click(object sender, RoutedEventArgs e)
        {
            var listaCalcadoWindow = new ListaCalcado();
            listaCalcadoWindow.Show();
        }

        private void AdicionarAtividadeFisica_Click(object sender, RoutedEventArgs e)
        {
            AdicionarAtividade adicionarAtividadeWindow = new AdicionarAtividade();
            adicionarAtividadeWindow.ShowDialog(); // Isto abrirá a janela AdicionarAtividade como um diálogo modal.
        }

        private void AtividadesButton_Click(object sender, RoutedEventArgs e)
        {
            var listaAtividadesWindow = new ListaAtividades();
            listaAtividadesWindow.Show();
        }

        private void AlterarCaracteristicasFisicas_Click(object sender, RoutedEventArgs e)
        {
            AlterarCaracteristicaFisica alterarCaracteristicaFisicaWindow = new AlterarCaracteristicaFisica();
            alterarCaracteristicaFisicaWindow.ShowDialog(); // Isto abrirá a janela AlterarCaracteristicaFisica como um diálogo modal.
        }

        private void PerfilButton_Click(object sender, RoutedEventArgs e)
        {
            Perfil1 perfil1Window = new Perfil1();
            perfil1Window.ShowDialog();

        }

        private void PeriodoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Obter o período selecionado
            string periodo = ((ComboBoxItem)PeriodoComboBox.SelectedItem).Content.ToString();

            // Obter a data de início com base no período selecionado
            DateTime inicio = DateTime.Now;
            switch (periodo)
            {
                case "Semana":
                    inicio = DateTime.Now.AddDays(-7);
                    break;
                case "Mês":
                    inicio = DateTime.Now.AddMonths(-1);
                    break;
                case "Ano":
                    inicio = DateTime.Now.AddYears(-1);
                    break;
            }

            // Obter as atividades do período selecionado
            var doc = XDocument.Load("Dados/Atividades.xml");
            var atividadesXml = doc.Descendants("Atividade").Where(a => DateTime.Parse(a.Attribute("StartTime").Value) >= inicio);

            // Calcular a distância total
            double distanciaTotal = atividadesXml.Sum(a =>
            {
                string distanceStr = a.Attribute("Distance").Value;
                if (!string.IsNullOrEmpty(distanceStr) && double.TryParse(distanceStr, out double distance))
                {
                    return distance;
                }
                else
                {
                    // Trate o erro de conversão, por exemplo, retornando um valor padrão ou lidando com a lógica apropriada.
                    return 0.0; // Valor padrão caso a conversão falhe
                }
            });


            // Calcular o tempo total
            TimeSpan tempoTotal = TimeSpan.Zero;
            foreach (var atividadeXml in atividadesXml)
            {
                DateTime inicioAtividade = DateTime.Parse(atividadeXml.Attribute("StartTime").Value);
                DateTime terminoAtividade = DateTime.Parse(atividadeXml.Attribute("EndTime").Value);
                tempoTotal += terminoAtividade - inicioAtividade;
            }

            // Exibir a distância e o tempo total nos TextBlocks
            DistanciaTotalTextBlock.Text = distanciaTotal.ToString();
            TempoTotalTextBlock.Text = tempoTotal.ToString();

            // Limpar e exibir as últimas atividades na ListView
            UltimasAtividadesListView.Items.Clear();
            foreach (var atividadeXml in atividadesXml)
            {
                string distanceStr = atividadeXml.Attribute("Distance").Value;
                if (!string.IsNullOrEmpty(distanceStr) && double.TryParse(distanceStr, out double distance))
                {
                    UltimasAtividadesListView.Items.Add(new LAB2ANO2SEMESTRE.Models.Atividade
                    {
                        Distance = distance,
                        TipoAtividade = new LAB2ANO2SEMESTRE.Models.TipoAtividade
                        {
                            Name = atividadeXml.Attribute("TipoAtividade").Value
                        }
                    });
                }
                else
                {
                    // Trate o erro de conversão, por exemplo, pulando a iteração ou lidando com a lógica apropriada.
                }
            }

        }

    }
}