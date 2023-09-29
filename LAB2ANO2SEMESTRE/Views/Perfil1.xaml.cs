using LAB2ANO2SEMESTRE.Models;
using LAB2ANO2SEMESTRE.ModelViews;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace LAB2ANO2SEMESTRE.Views
{
    public partial class Perfil1 : Window
    {
        private PerfilViewModel perfilViewModel;

        public Perfil1()
        {
            InitializeComponent();

            // Inicializa o ViewModel
            perfilViewModel = new PerfilViewModel();
            this.DataContext = perfilViewModel;

            // Carrega o perfil do arquivo XML
            perfilViewModel.Load();

            // Atualiza os campos de entrada com os dados carregados
            NomeTextBox.Text = perfilViewModel.Perfil.Nome;
            DataNascimentoDatePicker.SelectedDate = perfilViewModel.Perfil.DataNascimento;
            SexoComboBox.SelectedItem = perfilViewModel.Perfil.Sexo;

            // Carrega a imagem do perfil
            if (!string.IsNullOrEmpty(perfilViewModel.Perfil.FotografiaPath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(perfilViewModel.Perfil.FotografiaPath, UriKind.Relative);
                bitmap.EndInit();

                // Definindo a fonte do controle de imagem para a imagem selecionada
                FotoPerfilImage.Source = bitmap;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Cria um novo perfil com os dados inseridos pelo usuário
            Perfil perfil = new Perfil
            {
                Nome = NomeTextBox.Text,
                DataNascimento = (DateTime)DataNascimentoDatePicker.SelectedDate,
                FotografiaPath = ((BitmapImage)FotoPerfilImage.Source).UriSource.ToString(),
                Sexo = SexoComboBox.SelectedItem.ToString()
            };

            // Salva o perfil no arquivo XML
            perfilViewModel.Save(perfil);

            // Mostra uma mensagem de sucesso
            MessageBox.Show("Perfil atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AtualizarFotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png";
            if (dialog.ShowDialog() == true)
            {
                string filename = dialog.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(filename);
                bitmap.EndInit();

                // Definindo a fonte do controle de imagem para a imagem selecionada
                FotoPerfilImage.Source = bitmap;
            }
        }
    }
}
