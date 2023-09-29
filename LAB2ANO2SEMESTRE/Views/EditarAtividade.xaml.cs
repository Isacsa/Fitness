using LAB2ANO2SEMESTRE.Models;
using LAB2ANO2SEMESTRE.ModelViews;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace LAB2ANO2SEMESTRE.Views
{
    /// <summary>
    /// Interaction logic for EditarAtividade.xaml
    /// </summary>
    public partial class EditarAtividade : Window
    {
        public Atividade Atividade { get; set; }
        private AtividadeViewModel viewModel;
        public EditarAtividade(Atividade atividade)
        {
            InitializeComponent();
            Atividade = atividade;

            viewModel = new AtividadeViewModel();
            ShoeComboBox.ItemsSource = viewModel.Calcados;
            cbTiposAtividade.ItemsSource = viewModel.TiposAtividade;

            this.DataContext = Atividade;
            PopulateFields(atividade);
        }

        private void PopulateFields(Atividade atividade)
        {
            // Para preencher os campos corretamente, você precisará ajustar este código 
            DataAtividade.SelectedDate = atividade.StartTime.Date;
            cbTiposAtividade.SelectedItem = atividade.TipoAtividade;
            Descricao.Text = atividade.Description;

            HoraInicio.Text = atividade.StartTime.Hour.ToString();
            MinutoInicio.Text = atividade.StartTime.Minute.ToString();

            HoraFim.Text = atividade.EndTime.Hour.ToString();
            MinutoFim.Text = atividade.EndTime.Minute.ToString();

            Distancia.Text = atividade.Distance.ToString();
            ShoeComboBox.SelectedItem = atividade.Calcado;
            // Adicione mais campos conforme necessário
        }
        private void EditarAtividade_Click(object sender, RoutedEventArgs e)
        {
            Atividade.Id = Atividade.Id;
            Atividade.TipoAtividade = (TipoAtividade)cbTiposAtividade.SelectedItem;
            Atividade.Description = Descricao.Text;
            Atividade.StartTime = DataAtividade.SelectedDate.Value.AddHours(Double.Parse(HoraInicio.Text)).AddMinutes(Double.Parse(MinutoInicio.Text));
            Atividade.EndTime = DataAtividade.SelectedDate.Value.AddHours(Double.Parse(HoraFim.Text)).AddMinutes(Double.Parse(MinutoFim.Text));
            Atividade.Distance = double.Parse(Distancia.Text);
            Atividade.ShoeId = ((Calcado)ShoeComboBox.SelectedItem).Id;
            Atividade.Calcado = (Calcado)ShoeComboBox.SelectedItem;

            double duration = (Atividade.EndTime - Atividade.StartTime).TotalMinutes;
            Atividade.Rhythm = duration / Atividade.Distance;

            viewModel.UpdateActivityInXML(Atividade, "Dados/Atividades.xml");

            MessageBox.Show("A atividade foi atualizada com sucesso!");

            // Close this window
            this.Close();

            // Open ListaAtividades view
            ListaAtividades listaAtividades = new ListaAtividades();
            listaAtividades.Show();
        }
    }
}
