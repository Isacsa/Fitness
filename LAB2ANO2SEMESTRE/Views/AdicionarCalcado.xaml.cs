using LAB2ANO2SEMESTRE.Models;
using LAB2ANO2SEMESTRE.ModelViews;
using System;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.Views
{
    public partial class AdicionarCalcado : Window
    {
        private CalcadoViewModel _viewModel;
        private string dataPath = "Dados";

        public AdicionarCalcado()
        {
            InitializeComponent();

            _viewModel = new CalcadoViewModel();
            _viewModel.CalcadoAdicionado += ViewModel_CalcadoAdicionado;
            _viewModel.DadosCarregados += ViewModel_DadosCarregados;

            // Cria a pasta "Dados" se ela não existir
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            // Carrega os dados do arquivo XML quando a janela é aberta
            _viewModel.LoadFromXML($"{dataPath}/calcados.xml");
        }

        private void ViewModel_DadosCarregados()
        {
            // Limpa os campos de entrada na tela
            BrandTextBox.Text = "";
            ModelTextBox.Text = "";
            ColorTextBox.Text = "";
            MaxKilometersTextBox.Text = "";
            PurchaseDatePicker.SelectedDate = null;
            IsActiveCheckBox.IsChecked = false;
        }

        private void ViewModel_CalcadoAdicionado()
        {
            ViewModel_DadosCarregados(); // Limpa os campos

            // Mostra uma mensagem quando um calcado é adicionado
            MessageBox.Show("Um novo calcado foi adicionado com sucesso!");

            // Salva os dados no arquivo XML sempre que um calcado é adicionado
            _viewModel.SaveToXML($"{dataPath}/calcados.xml");
        }

        private void AdicionarCalcado_Click(object sender, RoutedEventArgs e)
        {
            // Coletar os dados inseridos pelo usuário e criar um novo objeto Calcado.
            Calcado newCalcado = new Calcado();

            // Preencher os atributos de newCalcado com os dados dos campos de entrada
            newCalcado.Brand = BrandTextBox.Text;
            newCalcado.Model = ModelTextBox.Text;
            newCalcado.Color = ColorTextBox.Text;
            double nrKilometros;
            if (!string.IsNullOrEmpty(MaxKilometersTextBox.Text) && double.TryParse(MaxKilometersTextBox.Text, out nrKilometros))
            {
                newCalcado.NrKilometros = nrKilometros;
            }
            else
            {
                // Trate o erro de conversão, por exemplo, exibindo uma mensagem de erro ao usuário.
            }

            newCalcado.PurchaseDate = (DateTime)PurchaseDatePicker.SelectedDate;
            newCalcado.IsActive = (bool)IsActiveCheckBox.IsChecked;

            _viewModel.AdicionaCalcado(newCalcado);
        }


    }
}
