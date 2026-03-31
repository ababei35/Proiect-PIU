using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarieModele
{
    public class StocMateriePrima
    {
        private const char SEPARATOR = ';';

        public double Porumb { get; set; }
        public double Soia { get; set; }
        public double Vitamine { get; set; }

        public StocMateriePrima()
        {
            Porumb = 0;
            Soia = 0;
            Vitamine = 0;
        }

        public StocMateriePrima(string linieFisier)
        {
            string[] date = linieFisier.Split(SEPARATOR);
            Porumb = Convert.ToDouble(date[0]);
            Soia = Convert.ToDouble(date[1]);
            Vitamine = Convert.ToDouble(date[2]);
        }

        public string ConversieLaSirPentruFisier()
        {
            return $"{Porumb}{SEPARATOR}{Soia}{SEPARATOR}{Vitamine}";
        }

        public string Info()
        {
            return $"Stoc Siloz: {Porumb} kg Porumb | {Soia} kg Soia | {Vitamine} kg Vitamine";
        }
    }
}