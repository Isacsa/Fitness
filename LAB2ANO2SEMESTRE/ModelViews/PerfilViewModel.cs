using LAB2ANO2SEMESTRE.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace LAB2ANO2SEMESTRE.ModelViews
{
    public class PerfilViewModel
    {
        public List<string> Sexos { get; } = new List<string> { "Masculino", "Feminino" };
        public Perfil Perfil { get; set; } = new Perfil();

        public void Save(Perfil perfil)
        {
            Perfil = perfil;

            string filePath = "Dados/perfil.xml";
            XDocument doc = new XDocument(new XElement("Perfil"));

            doc.Root.Add(perfil.ToXML());

            doc.Save(filePath);



          
            // Verifique e limpe o caminho da imagem, se necessário
            Uri uri = new Uri(perfil.FotografiaPath);
            string cleanedFotografiaPath = uri.LocalPath;

            // Copia a imagem selecionada para a pasta de dados, caso ainda não esteja lá.
            string pathFotografia = Path.Combine("Dados", Path.GetFileName(cleanedFotografiaPath));
            if (!File.Exists(pathFotografia))
            {
                File.Copy(cleanedFotografiaPath, pathFotografia);
            }



        }

        public void Load()
        {
            string filePath = "Dados/perfil.xml";
            if (!File.Exists(filePath))
            {
                return;
            }

            XDocument doc = XDocument.Load(filePath);

            XElement nomeElement = doc.Root.Element("Nome");
            XElement dataNascimentoElement = doc.Root.Element("DataNascimento");
            XElement sexoElement = doc.Root.Element("Sexo");

            Perfil = new Perfil
            {
                Nome = nomeElement != null ? nomeElement.Value : "",
                DataNascimento = dataNascimentoElement != null ? DateTime.Parse(dataNascimentoElement.Value) : default,
                Sexo = sexoElement != null ? sexoElement.Value : ""
            };

            // Carrega a imagem da pasta Dados com base no nome do arquivo
            string imageName = Perfil.Nome + ".jpg";  // Ajuste a extensão para a que você usa
            string imagePath = Path.Combine("Dados", imageName);

            if (File.Exists(imagePath))
            {
                Perfil.FotografiaPath = imagePath;
            }
        }
    }
}
