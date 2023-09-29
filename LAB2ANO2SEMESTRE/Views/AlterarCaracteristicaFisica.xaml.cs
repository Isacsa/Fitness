using LAB2ANO2SEMESTRE.Models;
using LAB2ANO2SEMESTRE.ModelViews;
using System;
using System.Windows;

namespace LAB2ANO2SEMESTRE.Views
{
    public partial class AlterarCaracteristicaFisica : Window
    {
        private CaracteristicasFisicasViewModel _viewModel;

        public AlterarCaracteristicaFisica()
        {
            InitializeComponent();

            _viewModel = new CaracteristicasFisicasViewModel();
            this.DataContext = _viewModel;

            ChangeTypeComboBox.ItemsSource = _viewModel.TiposRegistoFisico;
            ChangeTypeComboBox.SelectedIndex = -1;

            ChangeTypeComboBox.SelectionChanged += (s, e) =>
            {
                if (ChangeTypeComboBox.SelectedItem is TipoRegistoFisico selectedType)
                {
                    UnitTextBlock.Text = selectedType.Unidade;
                }
            };
        }

        private void AddPhysicalChange_Click(object sender, RoutedEventArgs e)
        {
            if (ChangeTypeComboBox.SelectedItem is not TipoRegistoFisico selectedType) return;

            double value;
            if (!double.TryParse(NewValueTextBox.Text, out value)) return;

            AlteracoesFisicas alteracao = new AlteracoesFisicas()
            {
                Id = _viewModel.GetNewId(),
                Characteristic = (TipoRegistoFisico)ChangeTypeComboBox.SelectedItem,
                Date = MeasurementDatePicker.SelectedDate.Value,
                Value = double.Parse(NewValueTextBox.Text)
            };

            _viewModel.AdicionaAlteracao(alteracao);

            MessageBox.Show("Uma nova alteração física foi adicionada com sucesso!");
        }
    }
}
