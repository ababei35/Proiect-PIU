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
            string[] date = linieFisier.Split(SEPARATOR_FISIER);
            IdFactura = Convert.ToInt32(date[0]);
            Nume = date[1];
            Prenume = date[2];
            Telefon = date[3];
            Adresa = date[4];
            ProdusCumparat = date[5];
            CantitateCumparata = Convert.ToDouble(date[6]);
        }

        public string ConversieLaSirPentruFisier()
        {
            return $"{IdFactura}{SEPARATOR_FISIER}{Nume}{SEPARATOR_FISIER}{Prenume}{SEPARATOR_FISIER}{Telefon}{SEPARATOR_FISIER}{Adresa}{SEPARATOR_FISIER}{ProdusCumparat}{SEPARATOR_FISIER}{CantitateCumparata}";
        }

        public string Info()
        {
            return $"Factura #{IdFactura} | Client: {Nume} {Prenume} (Tel: {Telefon}, Loc: {Adresa}) | A cumparat: {CantitateCumparata} kg de '{ProdusCumparat}'";
        }
    }
}
