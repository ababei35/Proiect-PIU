using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele;

namespace NivelStocareData
{
    public interface IStocareFacturi
    {
        void AdaugaFactura(Factura factura);
        List<Factura> GetFacturi();
        Factura GetFacturaDupaNumeComplet(string nume, string prenume);
        List<Factura> GetFacturiDupaProdus(string numeProdus);
        bool ModificaFactura(Factura facturaActualizata);
    }
}
