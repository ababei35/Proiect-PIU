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
            listaFacturi = new List<Factura>
            {
                new Factura(1, "Popescu", "Ion", "0711111111", "Suceava", "Porumb", 100),
                new Factura(2, "Ionescu", "Maria", "0722222222", "Iasi", "Grau", 50)
            };
            dgFacturi.ItemsSource = listaFacturi;
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            if (!ValideazaDateFactura())
            {
                return;
            }

            double cantitate = double.Parse(txtCantitate.Text.Trim());
            Factura facturaNoua = new Factura(listaFacturi.Count + 1, txtNume.Text.Trim(), txtPrenume.Text.Trim(), "-", "-", "Furaj", cantitate);

            listaFacturi.Add(facturaNoua);

            dgFacturi.ItemsSource = null;
            dgFacturi.ItemsSource = listaFacturi;

            tbMesaj.Foreground = Brushes.Green;
            tbMesaj.Text = "Succes! Factură adăugată în listă.";
            tbMesaj.Visibility = Visibility.Visible;

            txtNume.Clear();
            txtPrenume.Clear();
            txtCantitate.Clear();
            rbFizic.IsChecked = true;
            cbLivrare.IsChecked = false;
        }

        private bool ValideazaDateFactura()
        {
            bool isValid = true;
            string erori = "";

            txtNume.BorderBrush = Brushes.Gray;
            txtPrenume.BorderBrush = Brushes.Gray;
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
            txtCantitate.Clear();
            rbFizic.IsChecked = true;
            cbLivrare.IsChecked = false;

            txtNume.BorderBrush = Brushes.Gray;
            txtPrenume.BorderBrush = Brushes.Gray;
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
                f.Prenume.ToLower().Contains(textCautat)).ToList();

            dgFacturi.ItemsSource = rezultate;

            if (rezultate.Count == 0)
            {
                MessageBox.Show("Niciun rezultat găsit.");
            }
        }
    }
}