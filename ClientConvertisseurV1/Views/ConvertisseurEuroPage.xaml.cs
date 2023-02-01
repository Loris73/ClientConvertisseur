// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using ClientConvertisseurV1.Models;
using ClientConvertisseurV1.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ClientConvertisseurV1.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ConvertisseurEuroPage : Page, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private double montantEuros;
        public double MontantEuros
        {
            get { return montantEuros; }
            set { montantEuros = value; }
        }

        private ObservableCollection<Devise> devises;
        public ObservableCollection<Devise> Devises
        {
            get { return devises; }
            set { 
                devises = value;
                OnPropertyChanged("Devises");
            }
        }

        private Devise selectedDevise;
        public Devise SelectedDevise
        {
            get { return selectedDevise; }
            set { selectedDevise = value; }
        }

        private double montantEnDevise;
        public double MontantEnDevise
        {
            get { return montantEnDevise; }
            set
            {
                montantEnDevise = value;
                OnPropertyChanged("MontantEnDevise");
            }
        }







        public ConvertisseurEuroPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            GetDataOnLoadAsync();
        }

        private async void GetDataOnLoadAsync()
        {
            WSService service = new WSService("https://localhost:7275/api/");
            List<Devise> result = await service.GetDevisesAsync("devises");
            if (result != null)
                Devises = new ObservableCollection<Devise>(result);
            else
                MessageAsync("API non disponible !", "Erreur");
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void convertir_btn_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedDevise == null)
            {
                MessageAsync("Vous devez selectionner une devises !", "Erreur");
                return;
            }
            this.MontantEnDevise = CalculMontantDevise(this.MontantEuros, this.SelectedDevise.Taux);
        }

        private async void MessageAsync(string message, string title)
        {
            ContentDialog cd = new ContentDialog
            {
                Content = message,
                Title = title,
                CloseButtonText = "OK",
            };
            cd.XamlRoot = this.Content.XamlRoot;
            await cd.ShowAsync();
        }

        private double CalculMontantDevise(double montant, double taux)
        {
            double res = montant * taux;
            return res;
        }
    }
}
