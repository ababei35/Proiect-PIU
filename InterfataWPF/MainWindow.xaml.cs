using System.Windows;
using System.Windows.Media;
using LibrarieModele;

namespace InterfataWPF
{
    public partial class MainWindow : Window
    {
        // 1. Constante pentru limite (Cerinta Lab 7)
        private const int MAX_LUNGIME_NUME = 15;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnAdauga_Click(object sender, RoutedEventArgs e)
        {
            // Apelăm metoda de validare cerută de laborator
            if (!ValideazaDateFactura())
            {
                return; // Oprim execuția dacă sunt erori
            }

            // Dacă ajungem aici, datele sunt corecte. Creăm obiectul!
            double cantitate = double.Parse(txtCantitate.Text.Trim());
            Factura facturaNoua = new Factura(1, txtNume.Text.Trim(), txtPrenume.Text.Trim(), "-", "-", "Furaj", cantitate);

            // Afișăm succesul
            tbMesaj.Foreground = Brushes.Green;
            tbMesaj.Text = $"Succes! Factură creată pentru {facturaNoua.Nume}.";
            tbMesaj.Visibility = Visibility.Visible;
        }

        // Metoda separată de validare (Cerinta Lab 7)
        private bool ValideazaDateFactura()
        {
            bool isValid = true;
            string erori = "";

            // Resetăm culorile marginilor la gri înainte de a verifica
            txtNume.BorderBrush = Brushes.Gray;
            txtPrenume.BorderBrush = Brushes.Gray;
            txtCantitate.BorderBrush = Brushes.Gray;

            if (txtNume.Text.Trim().Length == 0 || txtNume.Text.Trim().Length > MAX_LUNGIME_NUME)
            {
                txtNume.BorderBrush = Brushes.Red; // Evidențiem vizual controlul
                erori += $"Numele trebuie să aibă între 1 și {MAX_LUNGIME_NUME} caractere.\n";
                isValid = false;
            }

            if (txtPrenume.Text.Trim().Length == 0 || txtPrenume.Text.Trim().Length > MAX_LUNGIME_NUME)
            {
                txtPrenume.BorderBrush = Brushes.Red;
                erori += $"Prenumele trebuie să aibă între 1 și {MAX_LUNGIME_NUME} caractere.\n";
                isValid = false;
            }

            // Folosim double.TryParse ca în exemplu
            if (!double.TryParse(txtCantitate.Text.Trim(), out double c) || c <= 0)
            {
                txtCantitate.BorderBrush = Brushes.Red;
                erori += "Cantitatea trebuie să fie un număr valid (pozitiv).\n";
                isValid = false;
            }

            if (!isValid)
            {
                tbMesaj.Foreground = Brushes.Red;
                tbMesaj.Text = erori;
                tbMesaj.Visibility = Visibility.Visible; // Afișăm erorile
            }

            return isValid;
        }

        // 2. Metoda pentru butonul Reset (Cerinta Lab 7)
        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            // Curățăm textul
            txtNume.Clear();
            txtPrenume.Clear();
            txtCantitate.Clear();

            // Resetăm marginile
            txtNume.BorderBrush = Brushes.Gray;
            txtPrenume.BorderBrush = Brushes.Gray;
            txtCantitate.BorderBrush = Brushes.Gray;

            // Ascundem mesajul
            tbMesaj.Visibility = Visibility.Collapsed;
        }
    }
}