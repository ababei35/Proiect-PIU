using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media;
using LibrarieModele;
using NivelStocareData;

namespace InterfataWPF
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private IStocareFacturi adminFacturi;

        public ObservableCollection<Factura> ListaFacturi { get; set; }
        public ObservableCollection<string> Produse { get; set; }
        public ObservableCollection<string> MetodePlata { get; set; }

        private string _textCautare;
        public string TextCautare { get => _textCautare; set { _textCautare = value; OnPropertyChanged(); } }

        private string _nume;
        public string Nume { get => _nume; set { _nume = value; OnPropertyChanged(); } }

        private string _prenume;
        public string Prenume { get => _prenume; set { _prenume = value; OnPropertyChanged(); } }

        private string _telefon;
        public string Telefon { get => _telefon; set { _telefon = value; OnPropertyChanged(); } }

        private string _adresa;
        public string Adresa { get => _adresa; set { _adresa = value; OnPropertyChanged(); } }

        private string _cantitate;
        public string Cantitate { get => _cantitate; set { _cantitate = value; OnPropertyChanged(); } }

        private string _produsSelectat;
        public string ProdusSelectat { get => _produsSelectat; set { _produsSelectat = value; OnPropertyChanged(); } }

        private string _metodaPlataSelectata;
        public string MetodaPlataSelectata { get => _metodaPlataSelectata; set { _metodaPlataSelectata = value; OnPropertyChanged(); } }

        private DateTime _dataFacturii = DateTime.Today;
        public DateTime DataFacturii { get => _dataFacturii; set { _dataFacturii = value; OnPropertyChanged(); } }

        private bool _isPersoanaFizica = true;
        public bool IsPersoanaFizica { get => _isPersoanaFizica; set { _isPersoanaFizica = value; OnPropertyChanged(); } }

        private bool _isLivrare;
        public bool IsLivrare { get => _isLivrare; set { _isLivrare = value; OnPropertyChanged(); } }
        public ObservableCollection<string> CriteriiCautare { get; set; }

        private string _criteriuSelectat;
        public string CriteriuSelectat { get => _criteriuSelectat; set { _criteriuSelectat = value; OnPropertyChanged(); } }

        private Factura _facturaSelectata;
        public Factura FacturaSelectata
        {
            get => _facturaSelectata;
            set
            {
                _facturaSelectata = value;
                OnPropertyChanged();
                IncarcaFacturaInFormular();
            }
        }

        private string _mesajText;
        public string MesajText { get => _mesajText; set { _mesajText = value; OnPropertyChanged(); } }

        private Brush _mesajCuloare;
        public Brush MesajCuloare { get => _mesajCuloare; set { _mesajCuloare = value; OnPropertyChanged(); } }

        public ICommand AdaugaCommand { get; }
        public ICommand ModificaCommand { get; }
        public ICommand StergeCommand { get; }
        public ICommand ResetCommand { get; }

        public ICommand CautaCommand { get; }

        public MainViewModel()
        {
            adminFacturi = new AdministrareFacturiFisierText("FacturiFinal.txt");

            Produse = new ObservableCollection<string> { "Pasari", "Porcine", "Bovine", "Pesti" };
            MetodePlata = new ObservableCollection<string> { "Cash", "Rate", "Card Bancar", "Transfer (OP)" };

            ListaFacturi = new ObservableCollection<Factura>(adminFacturi.GetFacturi());

            AdaugaCommand = new RelayCommand(AdaugaFactura);
            ModificaCommand = new RelayCommand(ModificaFactura);
            StergeCommand = new RelayCommand(StergeFactura);
            ResetCommand = new RelayCommand(ResetareFormular);
            CautaCommand = new RelayCommand(CautaFactura);
            CriteriiCautare = new ObservableCollection<string>
            {
                "ID", "Nume", "Prenume", "Telefon", "Adresa",
                "Tip Furaj", "Cantitate", "Data", "Metoda Plată",
                "Tip Client", "Livrare"
            };
            CriteriuSelectat = CriteriiCautare[1];
        }

        private void IncarcaFacturaInFormular()
        {
            if (FacturaSelectata != null)
            {
                Nume = FacturaSelectata.Nume;
                Prenume = FacturaSelectata.Prenume;
                Telefon = FacturaSelectata.Telefon;
                Adresa = FacturaSelectata.Adresa;
                Cantitate = FacturaSelectata.CantitateCumparata.ToString();
                ProdusSelectat = FacturaSelectata.ProdusCumparat;
                MetodaPlataSelectata = FacturaSelectata.MetodaPlata;
                DataFacturii = FacturaSelectata.DataFacturii;
                IsPersoanaFizica = FacturaSelectata.TipClient == "Fizică";
                IsLivrare = FacturaSelectata.Livrare == "Da";
            }
        }

        private void AdaugaFactura(object obj)
        {
            if (!ValideazaDate()) return;

            int idNou = ListaFacturi.Count > 0 ? ListaFacturi.Max(f => f.IdFactura) + 1 : 1;
            Factura f = new Factura(idNou, Nume, Prenume, Telefon, Adresa, ProdusSelectat, double.Parse(Cantitate))
            {
                DataFacturii = this.DataFacturii,
                MetodaPlata = this.MetodaPlataSelectata,
                TipClient = this.IsPersoanaFizica ? "Fizică" : "Juridică",
                Livrare = this.IsLivrare ? "Da" : "Nu"
            };

            ListaFacturi.Add(f);
            adminFacturi.AdaugaFactura(f);

            ResetareFormular(null);
            ArataMesaj("Succes! Factură adăugată.", Brushes.Green);
        }

        private void ModificaFactura(object obj)
        {
            if (FacturaSelectata == null) { ArataMesaj("Selectați o factură din tabel!", Brushes.Red); return; }
            if (!ValideazaDate()) return;

            FacturaSelectata.Nume = Nume;
            FacturaSelectata.Prenume = Prenume;
            FacturaSelectata.Telefon = Telefon;
            FacturaSelectata.Adresa = Adresa;
            FacturaSelectata.CantitateCumparata = double.Parse(Cantitate);
            FacturaSelectata.ProdusCumparat = ProdusSelectat;
            FacturaSelectata.MetodaPlata = MetodaPlataSelectata;
            FacturaSelectata.DataFacturii = DataFacturii;
            FacturaSelectata.TipClient = IsPersoanaFizica ? "Fizică" : "Juridică";
            FacturaSelectata.Livrare = IsLivrare ? "Da" : "Nu";

            adminFacturi.ModificaFactura(FacturaSelectata);

            // Truc pentru reîmprospătarea listei în UI
            var temp = ListaFacturi.ToList();
            ListaFacturi.Clear();
            foreach (var item in temp) ListaFacturi.Add(item);

            ArataMesaj("Succes! Factură modificată.", Brushes.Green);
        }

        private void StergeFactura(object obj)
        {
            if (FacturaSelectata != null)
            {
                int idDeSters = FacturaSelectata.IdFactura;
                ListaFacturi.Remove(FacturaSelectata);

                adminFacturi.StergeFactura(idDeSters);

                ResetareFormular(null);
                ArataMesaj("Succes! Factură ștearsă.", Brushes.Green);
            }
            else
            {
                System.Windows.MessageBox.Show("Selectați o factură din tabel pentru a o șterge.");
            }
        }

        private void ResetareFormular(object obj)
        {
            Nume = Prenume = Telefon = Adresa = Cantitate = string.Empty;
            ProdusSelectat = Produse.FirstOrDefault();
            MetodaPlataSelectata = MetodePlata.FirstOrDefault();
            DataFacturii = DateTime.Today;
            IsPersoanaFizica = true;
            IsLivrare = false;
            MesajText = string.Empty;
        }

        private bool ValideazaDate()
        {
            if (string.IsNullOrWhiteSpace(Nume) || Nume.Length > 15) { ArataMesaj("Eroare: Nume invalid!", Brushes.Red); return false; }
            if (string.IsNullOrWhiteSpace(Prenume)) { ArataMesaj("Eroare: Prenume invalid!", Brushes.Red); return false; }
            if (string.IsNullOrWhiteSpace(Telefon) || Telefon.Length < 10 || !Telefon.All(char.IsDigit))
            {
                ArataMesaj("Eroare: Telefonul trebuie să conțină doar cifre (min. 10)!", Brushes.Red);
                return false;
            }
            if (string.IsNullOrWhiteSpace(Adresa)) { ArataMesaj("Eroare: Adresa nu poate fi goală!", Brushes.Red); return false; }
            if (!double.TryParse(Cantitate, out double c) || c <= 0) { ArataMesaj("Eroare: Cantitate invalidă!", Brushes.Red); return false; }

            return true;
        }

        private void ArataMesaj(string mesaj, Brush culoare)
        {
            MesajText = mesaj;
            MesajCuloare = culoare;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void CautaFactura(object obj)
        {
            var facturiDinFisier = adminFacturi.GetFacturi();

            if (string.IsNullOrWhiteSpace(TextCautare))
            {
                ListaFacturi.Clear();
                foreach (var f in facturiDinFisier) ListaFacturi.Add(f);
                MesajText = "";
                return;
            }

            var text = TextCautare.ToLower().Trim();

            var rezultate = facturiDinFisier.Where(f =>
            {
                if (CriteriuSelectat == "ID")
                    return f.IdFactura.ToString().Contains(text);

                if (CriteriuSelectat == "Nume")
                    return (f.Nume ?? "").ToLower().Contains(text);

                if (CriteriuSelectat == "Prenume")
                    return (f.Prenume ?? "").ToLower().Contains(text);

                if (CriteriuSelectat == "Telefon")
                    return (f.Telefon ?? "").ToLower().Contains(text);

                if (CriteriuSelectat == "Adresa")
                    return (f.Adresa ?? "").ToLower().Contains(text);

                if (CriteriuSelectat == "Tip Furaj")
                    return (f.ProdusCumparat ?? "").ToLower().Contains(text);

                if (CriteriuSelectat == "Cantitate")
                    return f.CantitateCumparata.ToString().Contains(text);

                if (CriteriuSelectat == "Data")
                    return f.DataFacturii.ToString("dd/MM/yyyy").Contains(text) || f.DataFacturii.ToString().Contains(text);

                if (CriteriuSelectat == "Metoda Plată")
                    return (f.MetodaPlata ?? "").ToLower().Contains(text);

                if (CriteriuSelectat == "Tip Client")
                    return (f.TipClient ?? "").ToLower().Contains(text);

                if (CriteriuSelectat == "Livrare")
                    return (f.Livrare ?? "").ToLower().Contains(text);

                return false;
            }).ToList();

            ListaFacturi.Clear();
            foreach (var f in rezultate) ListaFacturi.Add(f);

            if (rezultate.Count == 0)
            {
                ArataMesaj("Niciun rezultat găsit pentru criteriul selectat.", Brushes.Orange);
            }
            else
            {
                MesajText = "";
            }
        }
    }
}