using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    public class ProdusFinit
    {
        private const char SEPARATOR = ';';

        public int IdProdus { get; set; }
        public string NumeFuraj { get; set; }
        public TipFuraj Categorie { get; set; }
        public double CantitateKg { get; set; }

        public double PorumbNecesar { get; set; }
        public double SoiaNecesara { get; set; }
        public double VitamineNecesare { get; set; }

        public ProdusFinit(int id, string nume, TipFuraj categorie, double cantitate)
        {
            IdProdus = id;
            NumeFuraj = nume;
            Categorie = categorie;
            CantitateKg = cantitate;

            ActualizeazaReteta();
        }

        public void ActualizeazaReteta()
        {
            PorumbNecesar = CantitateKg * 0.60;
            SoiaNecesara = CantitateKg * 0.30;
            VitamineNecesare = CantitateKg * 0.10;
        }

        public ProdusFinit(string linieFisier)
        {
            string[] date = linieFisier.Split(SEPARATOR);
            IdProdus = Convert.ToInt32(date[0]);
            NumeFuraj = date[1];

            Enum.TryParse(date[2], out TipFuraj categorie);
            Categorie = categorie;

            CantitateKg = Convert.ToDouble(date[3]);

            ActualizeazaReteta();
        }

        public string ConversieLaSirPentruFisier()
        {
            return $"{IdProdus}{SEPARATOR}{NumeFuraj}{SEPARATOR}{(int)Categorie}{SEPARATOR}{CantitateKg}";
        }

        public string Info()
        {
            return $"[{IdProdus}] {NumeFuraj} (Cat: {Categorie}) - Stoc: {CantitateKg}kg";
        }
    }
}
