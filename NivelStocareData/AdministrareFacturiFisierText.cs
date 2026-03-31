using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibrarieModele;

namespace NivelStocareData
{
    public class AdministrareFacturiFisierText : IStocareFacturi
    {
        private string numeFisier;

        public AdministrareFacturiFisierText(string numeFisier)
        {
            this.numeFisier = numeFisier;
            Stream streamFisierText = File.Open(numeFisier, FileMode.OpenOrCreate);
            streamFisierText.Close();
        }

        public void AdaugaFactura(Factura factura)
        {
            using (StreamWriter streamWriter = new StreamWriter(numeFisier, true))
            {
                streamWriter.WriteLine(factura.ConversieLaSirPentruFisier());
            }
        }

        public List<Factura> GetFacturi()
        {
            List<Factura> facturi = new List<Factura>();
            using (StreamReader streamReader = new StreamReader(numeFisier))
            {
                string linieFisier;
                while ((linieFisier = streamReader.ReadLine()) != null)
                {
                    facturi.Add(new Factura(linieFisier));
                }
            }
            return facturi;
        }

        public Factura GetFacturaDupaNumeComplet(string nume, string prenume)
        {
            return GetFacturi().FirstOrDefault(f => f.Nume.ToLower() == nume.ToLower() && f.Prenume.ToLower() == prenume.ToLower());
        }

        public List<Factura> GetFacturiDupaProdus(string numeProdus)
        {
            return GetFacturi().Where(f => f.ProdusCumparat.ToLower() == numeProdus.ToLower()).ToList();
        }

        public bool ModificaFactura(Factura facturaActualizata)
        {
            List<Factura> facturi = GetFacturi();
            bool gasit = false;

            for (int i = 0; i < facturi.Count; i++)
            {
                if (facturi[i].IdFactura == facturaActualizata.IdFactura)
                {
                    facturi[i] = facturaActualizata;
                    gasit = true;
                    break;
                }
            }

            if (gasit)
            {
                using (StreamWriter sw = new StreamWriter(numeFisier, false))
                {
                    foreach (Factura f in facturi)
                    {
                        sw.WriteLine(f.ConversieLaSirPentruFisier());
                    }
                }
            }
            return gasit;
        }
    }
}
