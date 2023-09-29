using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;
using LAB2ANO2SEMESTRE.Models;
using LAB2ANO2SEMESTRE.ModelViews;

namespace LAB2ANO2SEMESTRE.Views
{
    /// <summary>
    /// Interaction logic for ListaAtividades.xaml
    /// </summary>
    public partial class ListaAtividades : Window
    {
        // Declaração da ViewModel como propriedade
        public ListaAtividadesViewModel ViewModel { get; set; }

        public ListaAtividades()
        {
            InitializeComponent();
            ViewModel = new ListaAtividadesViewModel();
            ViewModel.LoadData("Dados/Atividades.xml");
            this.DataContext = ViewModel;
            AtividadesListView.ItemsSource = ViewModel.Atividades;  // Liga a ListView aos dados
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Obter a atividade que está associada ao botão que foi clicado
            var button = (Button)sender;
            var atividade = (Atividade)button.DataContext;

            this.Close();

            // Abre a janela "EditarAtividade" para a atividade selecionada
            var editarAtividadeWindow = new EditarAtividade(atividade);
            editarAtividadeWindow.ShowDialog();
            
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Obter a atividade que está associada ao botão que foi clicado
            var button = (Button)sender;
            var atividade = (Atividade)button.DataContext;

            // Perguntar ao usuário se ele realmente deseja apagar a atividade
            var messageBoxResult = MessageBox.Show(
                "Tem certeza que deseja apagar a atividade?",
                "Confirmação de Apagar",
                MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // Remover a atividade da lista e atualizar o XML
                ViewModel.Atividades.Remove(atividade);
                ViewModel.SaveData("Dados/Atividades.xml");
            }
        }
    }
}
