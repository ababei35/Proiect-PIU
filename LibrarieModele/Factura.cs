using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    public class Factura
    {
        private const char SEPARATOR_FISIER = ';';

        public int IdFactura { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; } 
        public string Telefon { get; set; } 
        public string Adresa { get; set; }
        public string ProdusCumparat { get; set; }
        public double CantitateCumparata { get; set; }
        public DateTime DataFacturii { get; set; }
        public string MetodaPlata { get; set; }
        public string TipClient { get; set; }
        public string Livrare { get; set; }

        public Factura(int id, string nume, string prenume, string telefon, string adresa, string produs, double cantitate)
        {
            IdFactura = id;
            Nume = nume;
            Prenume = prenume;
            Telefon = telefon;
            Adresa = adresa;
            ProdusCumparat = produs;
            CantitateCumparata = cantitate;
        }

        public Factura(string linieFisier)
        {
            string[] dateFisier = linieFisier.Split(';');

            IdFactura = int.Parse(dateFisier[0]);
            Nume = dateFisier[1];
            Prenume = dateFisier[2];
            Telefon = dateFisier[3];
            Adresa = dateFisier[4];
            ProdusCumparat = dateFisier[5];
            CantitateCumparata = double.Parse(dateFisier[6]);

            if (dateFisier.Length >= 11)
            {
                if (DateTime.TryParse(dateFisier[7], out DateTime dataCitita))
                    DataFacturii = dataCitita;
                else
                    DataFacturii = DateTime.Today;

                MetodaPlata = dateFisier[8];
                TipClient = dateFisier[9];
                Livrare = dateFisier[10];
            }
            else
            {
                DataFacturii = DateTime.Today;
                MetodaPlata = "Nespecificat";
                TipClient = "Fizică";
                Livrare = "Nu";
            }
        }

        public string ConversieLaSirPentruFisier()
        {
            return $"{IdFactura};{Nume};{Prenume};{Telefon};{Adresa};{ProdusCumparat};{CantitateCumparata};{DataFacturii.ToString("dd/MM/yyyy")};{MetodaPlata};{TipClient};{Livrare}";
        }

        public string Info()
        {
            return $"Factura #{IdFactura} | Client: {Nume} {Prenume} (Tel: {Telefon}, Loc: {Adresa}) | A cumparat: {CantitateCumparata} kg de '{ProdusCumparat}'";
        }
    }
}
