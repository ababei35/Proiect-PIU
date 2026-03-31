using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using LibrarieModele;

namespace NivelStocareData
{
    public class AdministrareStocuriFisiere
    {
        private string fisierMateriaPrima = "StocMateriaPrima.txt";
        private string fisierFuraje = "StocFuraje.txt";

        public StocMateriePrima GetMateriaPrima()
        {
            if (!File.Exists(fisierMateriaPrima))
            {
                File.Create(fisierMateriaPrima).Close();
                return new StocMateriePrima();
            }

            using (StreamReader sr = new StreamReader(fisierMateriaPrima))
            {
                string linie = sr.ReadLine();
                if (!string.IsNullOrEmpty(linie))
                {
                    return new StocMateriePrima(linie);
                }
            }
            return new StocMateriePrima();
        }

        public void SalveazaMateriaPrima(StocMateriePrima stoc)
        {
            using (StreamWriter sw = new StreamWriter(fisierMateriaPrima, false))
            {
                sw.WriteLine(stoc.ConversieLaSirPentruFisier());
            }
        }

        public List<ProdusFinit> GetFuraje()
        {
            List<ProdusFinit> furaje = new List<ProdusFinit>();
            if (!File.Exists(fisierFuraje))
            {
                File.Create(fisierFuraje).Close();
                return furaje;
            }

            using (StreamReader sr = new StreamReader(fisierFuraje))
            {
                string linie;
                while ((linie = sr.ReadLine()) != null)
                {
                    furaje.Add(new ProdusFinit(linie));
                }
            }
            return furaje;
        }

        public void SalveazaFurajele(List<ProdusFinit> furaje)
        {
            using (StreamWriter sw = new StreamWriter(fisierFuraje, false))
            {
                foreach (ProdusFinit furaj in furaje)
                {
                    sw.WriteLine(furaj.ConversieLaSirPentruFisier());
                }
            }
        }
    }
}