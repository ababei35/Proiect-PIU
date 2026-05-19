using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using LibrarieModele;
using NivelStocareData;

namespace InterfataWPF
{
    public partial class MainWindow : Window
    {
        private const int MAX_LUNGIME_NUME = 15;
        private ObservableCollection<Factura> listaFacturi;
        private IStocareFacturi adminFacturi;

        public MainWindow()
        {
            InitializeComponent();
            adminFacturi = new AdministrareFacturiFisierText("FacturiFinal.txt");
            IncarcaDate();
        }

        private void IncarcaDate()
        {
            cmbProdus.ItemsSource = new string[] { "Pasari", "Porcine", "Bovine", "Pesti" };
            cmbProdus.SelectedIndex = 0;

            lstPlata.ItemsSource = new string[] { "Cash", "Rate", "Card Bancar", "Transfer (OP)" };
            lstPlata.SelectedIndex = 0;

            dpDataFacturii.SelectedDate = DateTime.Today;

            var facturiDinFisier = adminFacturi.GetFacturi();
            listaFacturi = new ObservableCollection<Factura>(facturiDinFisier);

            dgFacturi.ItemsSource = listaFacturi;
        }

        private void DgFacturi_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgFacturi.SelectedItem is Factura f)
            {
                txtNume.Text = f.Nume;
                txtPrenume.Text = f.Prenume;
                txtTelefon.Text = f.Telefon;
                txtAdresa.Text = f.Adresa;
                txtCantitate.Text = f.CantitateCumparata.ToString();
                cmbProdus.SelectedItem = f.ProdusCumparat;
                dpDataFacturii.SelectedDate = f.DataFacturii;
                lstPlata.SelectedItem = f.MetodaPlata;
                if (f.TipClient == "Fizică") rbFizic.IsChecked = true; else rbJuridic.IsChecked = true;
                cbLivrare.IsChecked = (f.Livrare == "Da");
            }
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaDateFactura())
            {
                return;
            }

            double cantitate = double.Parse(txtCantitate.Text.Trim());
            string produsSelectat = cmbProdus.SelectedItem.ToString();

            DateTime data = dpDataFacturii.SelectedDate ?? DateTime.Today;
            string metodaPlata = lstPlata.SelectedItem != null ? lstPlata.SelectedItem.ToString() : "Nespecificat";
            string tipClient = rbFizic.IsChecked == true ? "Fizică" : "Juridică";
            string livrare = cbLivrare.IsChecked == true ? "Da" : "Nu";

            int idNou = listaFacturi.Count > 0 ? listaFacturi.Max(f => f.IdFactura) + 1 : 1;

            Factura facturaNoua = new Factura(idNou, txtNume.Text.Trim(), txtPrenume.Text.Trim(), txtTelefon.Text.Trim(), txtAdresa.Text.Trim(), produsSelectat, cantitate);

            facturaNoua.DataFacturii = data;
            facturaNoua.MetodaPlata = metodaPlata;
            facturaNoua.TipClient = tipClient;
            facturaNoua.Livrare = livrare;

            listaFacturi.Add(facturaNoua);
            adminFacturi.AdaugaFactura(facturaNoua);

            BtnReset_Click(null, null);

            tbMesaj.Foreground = Brushes.Green;
            tbMesaj.Text = "Succes! Factură adăugată și salvată în fișier.";
            tbMesaj.Visibility = Visibility.Visible;
        }

        private void BtnModifica_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgFacturi.SelectedItem is Factura f))
            {
                MessageBox.Show("Selectați o factură din tabel pentru a o modifica.");
                return;
            }

            if (!ValideazaDateFactura())
            {
                return;
            }

            f.Nume = txtNume.Text.Trim();
            f.Prenume = txtPrenume.Text.Trim();
            f.Telefon = txtTelefon.Text.Trim();
            f.Adresa = txtAdresa.Text.Trim();
            f.CantitateCumparata = double.Parse(txtCantitate.Text.Trim());
            f.ProdusCumparat = cmbProdus.SelectedItem.ToString();
            f.DataFacturii = dpDataFacturii.SelectedDate ?? DateTime.Today;
            f.MetodaPlata = lstPlata.SelectedItem != null ? lstPlata.SelectedItem.ToString() : "Nespecificat";
            f.TipClient = rbFizic.IsChecked == true ? "Fizică" : "Juridică";
            f.Livrare = cbLivrare.IsChecked == true ? "Da" : "Nu";

            adminFacturi.ModificaFactura(f);

            dgFacturi.ItemsSource = null;
            dgFacturi.ItemsSource = listaFacturi;

            tbMesaj.Foreground = Brushes.Green;
            tbMesaj.Text = "Succes! Factura a fost modificată în fișier.";
            tbMesaj.Visibility = Visibility.Visible;
        }

        private void BtnSterge_Click(object sender, RoutedEventArgs e)
        {
            if (dgFacturi.SelectedItem is Factura f)
            {
                listaFacturi.Remove(f);
                tbMesaj.Foreground = Brushes.Green;
                tbMesaj.Text = "Succes! Factura a fost eliminată.";
                tbMesaj.Visibility = Visibility.Visible;
                BtnReset_Click(null, null);
            }
            else
            {
                MessageBox.Show("Selectați o factură din tabel pentru a o șterge.");
            }
        }

        private bool ValideazaDateFactura()
        {
            bool isValid = true;
            string erori = "";

            txtNume.BorderBrush = Brushes.Gray;
            txtPrenume.BorderBrush = Brushes.Gray;
            txtTelefon.BorderBrush = Brushes.Gray;
            txtAdresa.BorderBrush = Brushes.Gray;
            txtCantitate.BorderBrush = Brushes.Gray;

            if (txtNume.Text.Trim().Length == 0 || txtNume.Text.Trim().Length > MAX_LUNGIME_NUME)
            {
                txtNume.BorderBrush = Brushes.Red;
                erori += "Numele este invalid.\n";
                isValid = false;
            }

            if (txtPrenume.Text.Trim().Length == 0 || txtPrenume.Text.Trim().Length > MAX_LUNGIME_NUME)
            {
                txtPrenume.BorderBrush = Brushes.Red;
                erori += "Prenumele este invalid.\n";
                isValid = false;
            }

            if (txtTelefon.Text.Trim().Length == 0)
            {
                txtTelefon.BorderBrush = Brushes.Red;
                erori += "Telefonul nu poate fi gol.\n";
                isValid = false;
            }

            if (txtAdresa.Text.Trim().Length == 0)
            {
                txtAdresa.BorderBrush = Brushes.Red;
                erori += "Adresa nu poate fi goală.\n";
                isValid = false;
            }

            if (!double.TryParse(txtCantitate.Text.Trim(), out double c) || c <= 0)
            {
                txtCantitate.BorderBrush = Brushes.Red;
                erori += "Cantitatea trebuie să fie pozitivă.\n";
                isValid = false;
            }

            if (!isValid)
            {
                tbMesaj.Foreground = Brushes.Red;
                tbMesaj.Text = erori;
                tbMesaj.Visibility = Visibility.Visible;
            }

            return isValid;
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            txtNume.Clear();
            txtPrenume.Clear();
            txtTelefon.Clear();
            txtAdresa.Clear();
            txtCantitate.Clear();

            cmbProdus.SelectedIndex = 0;
            lstPlata.SelectedIndex = 0;
            dpDataFacturii.SelectedDate = DateTime.Today;
            rbFizic.IsChecked = true;
            cbLivrare.IsChecked = false;

            txtNume.BorderBrush = Brushes.Gray;
            txtPrenume.BorderBrush = Brushes.Gray;
            txtTelefon.BorderBrush = Brushes.Gray;
            txtAdresa.BorderBrush = Brushes.Gray;
            txtCantitate.BorderBrush = Brushes.Gray;

            tbMesaj.Visibility = Visibility.Collapsed;
        }

        private void BtnCauta_Click(object sender, RoutedEventArgs e)
        {
            string textCautat = txtCautare.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(textCautat))
            {
                dgFacturi.ItemsSource = listaFacturi;
                return;
            }

            var rezultate = listaFacturi.Where(f =>
                f.Nume.ToLower().Contains(textCautat) ||
                f.Prenume.ToLower().Contains(textCautat) ||
                f.Telefon.Contains(textCautat)).ToList();

            dgFacturi.ItemsSource = rezultate;

            if (rezultate.Count == 0)
            {
                MessageBox.Show("Niciun rezultat găsit.");
            }
        }
    }
}