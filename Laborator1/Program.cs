using System;
using System.Collections.Generic;

public class ProdusFinit
{
    public string NumeFuraj;
    public double CantitateKg;
    public double PorumbNecesar;
    public double SoiaNecesara;
    public double VitamineNecesare;

    public ProdusFinit(string nume, double cantitate)
    {
        NumeFuraj = nume;
        CantitateKg = cantitate;
        PorumbNecesar = cantitate * 0.60;
        SoiaNecesara = cantitate * 0.30;
        VitamineNecesare = cantitate * 0.10;
    }

    public void AfiseazaRezultat()
    {
        Console.WriteLine("Pentru " + CantitateKg + " kg de " + NumeFuraj + " s-au folosit:");
        Console.WriteLine("- Porumb: " + PorumbNecesar + " kg");
        Console.WriteLine("- Soia: " + SoiaNecesara + " kg");
        Console.WriteLine("- Vitamine: " + VitamineNecesare + " kg");
    }
}

public class Factura
{
    public string NumeCumparator;
    public string Adresa;
    public double CantitateCumparata;

    public Factura(string nume, string adresa, double cantitate)
    {
        NumeCumparator = nume;
        Adresa = adresa;
        CantitateCumparata = cantitate;
    }

    public void AfiseazaFactura()
    {
        Console.WriteLine("-----Factură fiscală:");
        Console.WriteLine("Cumpărător: " + NumeCumparator);
        Console.WriteLine("Adresă: " + Adresa);
        Console.WriteLine("Cantitate achiziționată: " + CantitateCumparata + " kg");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Introdu numele furajului produs : ");
        string numeProdus = Console.ReadLine();
        Console.Write("Introdu cantitatea produsa in kg : ");
        double cantitateProdusa = Convert.ToDouble(Console.ReadLine());

        ProdusFinit produsulMeu = new ProdusFinit(numeProdus, cantitateProdusa);
        produsulMeu.AfiseazaRezultat();
        Console.WriteLine("----------------------------------\n");

        List<Factura> dosarFacturi = new List<Factura>();
        string raspuns = "da";

        while (raspuns.ToLower() == "da")
        {
            Console.Write("Introdu numele cumpărătorului: ");
            string numeClient = Console.ReadLine();

            Console.Write("Introdu adresa cumpărătorului: ");
            string adresaClient = Console.ReadLine();

            Console.Write("Introdu cantitatea cumpărată în kg: ");
            double cantitateVanduta = Convert.ToDouble(Console.ReadLine());

            if (cantitateVanduta <= produsulMeu.CantitateKg)
            {
                produsulMeu.CantitateKg = produsulMeu.CantitateKg - cantitateVanduta;

                Factura facturaNoua = new Factura(numeClient, adresaClient, cantitateVanduta);
                dosarFacturi.Add(facturaNoua);

                Console.WriteLine("Vânzare reusită! ");
            }
            else
            {
                Console.WriteLine("Eroare! Nu avem suficient stoc.");
            }
            Console.Write("\nDoresti să mai introduci o factură? (da/nu): ");
            raspuns = Console.ReadLine();
            Console.WriteLine();
        }

        Console.WriteLine("-----CĂUTARE -----");
        string vreauSaCaut = "da";

        while (vreauSaCaut.ToLower() == "da")
        {
            Console.Write("Introdu numele clientului pe care vrei să-l cauti: ");
            string numeCautat = Console.ReadLine();

            bool facturaGasita = false;

            Console.WriteLine("\nRezultatele căutării:");
            foreach (Factura f in dosarFacturi)
            {
                if (f.NumeCumparator.ToLower() == numeCautat.ToLower())
                {
                    f.AfiseazaFactura();
                    facturaGasita = true;
                }
            }

            if (facturaGasita == false)
            {
                Console.WriteLine("Nu s-a găsit nicio factură pentru clientul: " + numeCautat);
            }

            Console.Write("\nDoresti să cauti alt client? (da/nu): ");
            vreauSaCaut = Console.ReadLine();
            Console.WriteLine();
        }
        Console.ReadLine();
    }
}