using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using LibrarieModele;

namespace InterfataWPF
{
    public partial class MainWindow : Window
    {
        private const int MAX_LUNGIME_NUME = 15;
        private List<Factura> listaFacturi;

        public MainWindow()
        {
            InitializeComponent();
            IncarcaDate();
        }

        private void IncarcaDate()
        {
            cmbProdus.ItemsSource = new List<string> { "Porumb", "Grâu", "Orz", "Soia", "Premix" };
            cmbProdus.SelectedIndex = 0;

            lstPlata.ItemsSource = new List<string> { "Cash", "Card Bancar", "Transfer (OP)" };
            lstPlata.SelectedIndex = 0;

            dpDataFacturii.SelectedDate = DateTime.Today;

            listaFacturi = new List<Factura>();

            // Creăm o factură de test folosind noul model complet
            Factura test1 = new Factura(1, "Popescu", "Ion", "0711111111", "Suceava", "Porumb", 100);
            test1.DataFacturii = DateTime.Today;
            test1.MetodaPlata = "Cash";
            test1.TipClient = "Fizică";
            test1.Livrare = "Da";

            listaFacturi.Add(test1);

            dgFacturi.ItemsSource = listaFacturi;
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaDateFactura())
            {
                return;
            }

            double cantitate = double.Parse(txtCantitate.Text.Trim());
            string produsSelectat = cmbProdus.SelectedItem.ToString();

            // Prelucrăm valorile din noile controale
            DateTime data = dpDataFacturii.SelectedDate ?? DateTime.Today;
            string metodaPlata = lstPlata.SelectedItem != null ? lstPlata.SelectedItem.ToString() : "Nespecificat";
            string tipClient = rbFizic.IsChecked == true ? "Fizică" : "Juridică";
            string livrare = cbLivrare.IsChecked == true ? "Da" : "Nu";

            // Creăm factura utilizând constructorul cu telefon și adresă
            Factura facturaNoua = new Factura(listaFacturi.Count + 1, txtNume.Text.Trim(), txtPrenume.Text.Trim(), txtTelefon.Text.Trim(), txtAdresa.Text.Trim(), produsSelectat, cantitate);

            // Adăugăm datele suplimentare în proprietăți
            facturaNoua.DataFacturii = data;
            facturaNoua.MetodaPlata = metodaPlata;
            facturaNoua.TipClient = tipClient;
            facturaNoua.Livrare = livrare;

            listaFacturi.Add(facturaNoua);

            dgFacturi.ItemsSource = null;
            dgFacturi.ItemsSource = listaFacturi;

            BtnReset_Click(null, null); // Refolosim logica de curățare

            tbMesaj.Foreground = Brushes.Green;
            tbMesaj.Text = "Succes! Factură adăugată în listă.";
            tbMesaj.Visibility = Visibility.Visible;
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
                f.Telefon.Contains(textCautat)).ToList(); // Am adăugat și căutare după telefon!

            dgFacturi.ItemsSource = rezultate;

            if (rezultate.Count == 0)
            {
                MessageBox.Show("Niciun rezultat găsit.");
            }
        }
    }
}