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
    /// Interaction logic for ListaCalcado.xaml
    /// </summary>
    public partial class ListaCalcado : Window
    {
        // Declare ViewModel as a property
        public ListaCalcadoViewModel ViewModel { get; set; }

        public ListaCalcado()
        {
            InitializeComponent();
            ViewModel = new ListaCalcadoViewModel();
            ViewModel.LoadData("Dados/Calcados.xml");
            this.DataContext = ViewModel;
            CalcadosListView.ItemsSource = ViewModel.Calcados;  // Bind the ListView to the data
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the item that is associated with the button that was clicked
            var button = (Button)sender;
            var calcado = (Calcado)button.DataContext;

            this.Close();

            // Open the "EditarCalcado" window for the selected item
            var editarCalcadoeWindow = new EditarCalcado(calcado);
            editarCalcadoeWindow.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the item that is associated with the button that was clicked
            var button = (Button)sender;
            var calcado = (Calcado)button.DataContext;

            // Ask the user if they really want to delete the item
            var messageBoxResult = MessageBox.Show(
                "Tem certeza que deseja apagar o calcado?",
                "Confirmação de Apagar",
                MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                // Remove the item from the list and update the XML
                ViewModel.Calcados.Remove(calcado);
                ViewModel.SaveData("Dados/Calcados.xml");
            }
        }
    }
}
