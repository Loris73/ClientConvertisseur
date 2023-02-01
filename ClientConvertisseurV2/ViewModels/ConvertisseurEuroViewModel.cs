using ClientConvertisseurV2.Models;
using ClientConvertisseurV2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientConvertisseurV2.ViewModels
{
    public class ConvertisseurEuroViewModel : ObservableObject, INotifyPropertyChanged
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
            set
            {
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






        public IRelayCommand BtnSetConversion { get; }

        public ConvertisseurEuroViewModel()
        {
            GetDataOnLoadAsync();
            BtnSetConversion = new RelayCommand(ActionSetConversion);
        }

        public void ActionSetConversion()
        {
            if (this.SelectedDevise == null)
            {
                MessageAsync("Vous devez selectionner une devises !", "Erreur");
                return;
            }
            this.MontantEnDevise = this.MontantEuros * this.SelectedDevise.Taux;
        }









        public double MontantEnDevise
        {
            get { return montantEnDevise; }
            set
            {
                montantEnDevise = value;
                OnPropertyChanged("MontantEnDevise");
            }
        }

        public async void GetDataOnLoadAsync()
        {
            WSService service = new WSService("https://localhost:7088/api/");
            List<Devise> result = await service.GetDevisesAsync("devises");
            if (result != null)
                Devises = new ObservableCollection<Devise>(result);
            else
                MessageAsync("API non disponible !", "Erreur");
        }

        private async void MessageAsync(string message, string title)
        {
            ContentDialog cd = new ContentDialog
            {
                Content = message,
                Title = title,
                CloseButtonText = "OK",
            };
            cd.XamlRoot = App.MainRoot.XamlRoot;
            await cd.ShowAsync();
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
