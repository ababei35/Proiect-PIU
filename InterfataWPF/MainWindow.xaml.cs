using System.Windows;
using LibrarieModele; // Importăm modelele din proiectul tău

namespace InterfataWPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // 1. Creăm o entitate "mock" (de test) din proiectul tău. 
            // Folosim constructorul clasei Factura pe care o ai deja în LibrarieModele.
            Factura facturaTest = new Factura(1, "Ion", "Popescu", "0722123456", "Suceava", "Porumb Măcinat", 150);

            // 2. Afișăm informațiile entității în interfață, atribuind textul etichetelor
            lblClient.Content = $"Client: {facturaTest.Nume} {facturaTest.Prenume} (Tel: {facturaTest.Telefon})";
            lblProdus.Content = $"Produs achiziționat: {facturaTest.ProdusCumparat}";
            lblCantitate.Content = $"Cantitate: {facturaTest.CantitateCumparata} kg";
        }
    }
}