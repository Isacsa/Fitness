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
    /// Interaction logic for EditarCalcado.xaml
    /// </summary>
    public partial class EditarCalcado : Window
    {
        public Calcado Calcado { get; set; }
        private CalcadoViewModel viewModel;

        public EditarCalcado(Calcado calcado)
        {
            InitializeComponent();
            Calcado = calcado;

            viewModel = new CalcadoViewModel();

            this.DataContext = Calcado;
            PopulateFields(calcado);
        }

        private void PopulateFields(Calcado calcado)
        {
            // Para preencher os campos corretamente, você precisará ajustar este código 
            BrandTextBox.Text = calcado.Brand;
            ModelTextBox.Text = calcado.Model;
            ColorTextBox.Text = calcado.Color;
            MaxKilometersTextBox.Text = calcado.NrKilometros.ToString();
            PurchaseDatePicker.SelectedDate = calcado.PurchaseDate.Date;
            IsActiveCheckBox.IsChecked = calcado.IsActive;
            // Adicione mais campos conforme necessário
        }

        private void EditarCalcado_Click(object sender, RoutedEventArgs e)
        {
            Calcado.Brand = BrandTextBox.Text;
            Calcado.Model = ModelTextBox.Text;
            Calcado.Color = ColorTextBox.Text;
            Calcado.NrKilometros = Double.Parse(MaxKilometersTextBox.Text);
            Calcado.PurchaseDate = PurchaseDatePicker.SelectedDate.Value;
            Calcado.IsActive = IsActiveCheckBox.IsChecked.Value;


            viewModel.UpdateShoeInXML(Calcado, "Dados/Calcados.xml");

            MessageBox.Show("O calçado foi atualizado com sucesso!");

            // Fechar essa janela
            this.Close();

            // Abrir a vista ListaCalcados
            ListaCalcado listaCalcados = new ListaCalcado();
            listaCalcados.Show();
        }
    }
}

