using LAB2ANO2SEMESTRE.Models;
using LAB2ANO2SEMESTRE.ModelViews;
using System;
using System.Windows;

namespace LAB2ANO2SEMESTRE.Views
{
    public partial class Objetivos : Window
    {
        private ObjetivoViewModel _viewModel;

        public Objetivos()
        {
            InitializeComponent();

            _viewModel = new ObjetivoViewModel();
            this.DataContext = _viewModel;

            GoalTypeComboBox.ItemsSource = _viewModel.TiposObjetivo;
            GoalTypeComboBox.SelectedIndex = -1;
        }

        private void AddGoal_Click(object sender, RoutedEventArgs e)
        {
            if (GoalTypeComboBox.SelectedItem is not Tipo selectedType) return;

            double targetValue;
            if (!double.TryParse(TargetValueTextBox.Text, out targetValue)) return;

            Objetivo newGoal = new Objetivo()
            {
                Id = _viewModel.GetNewId(),
                GoalType = selectedType,
                TargetValue = targetValue,
                Deadline = DeadlineDatePicker.SelectedDate,
                IsActive = true,
                AchievementDate = null // Data de alcance será nula inicialmente
            };

            _viewModel.AdicionaObjetivo(newGoal);

            MessageBox.Show("Um novo objetivo foi adicionado com sucesso!");
        }
        private void UpdateGoal_Click(object sender, RoutedEventArgs e)
        {
            if (GoalsListView.SelectedItem is Objetivo selectedGoal && AchievementDatePicker.SelectedDate.HasValue)
            {
                selectedGoal.AchievementDate = AchievementDatePicker.SelectedDate;
                _viewModel.AtualizaObjetivo(selectedGoal);

                MessageBox.Show("Objetivo atualizado com sucesso!");
            }
            else
            {
                MessageBox.Show("Por favor, selecione um objetivo e defina a data de conclusão antes de tentar atualizar.");
            }
        }

    }
}
