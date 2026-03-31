using System;
using System.Collections.Generic;
using LibrarieModele;
using NivelStocareData;

namespace Laborator1
{
    class Program
    {
        static void Main(string[] args)
        {
            IStocareFacturi adminFacturi = new AdministrareFacturiFisierText("FacturiFinal.txt");
            AdministrareStocuriFisiere adminStocuri = new AdministrareStocuriFisiere();

            StocMateriePrima siloz = adminStocuri.GetMateriaPrima();
            List<ProdusFinit> depozitFuraje = adminStocuri.GetFuraje();

            int contorFacturi = adminFacturi.GetFacturi().Count + 1;
            int contorProduse = depozitFuraje.Count + 1;

            string optiune;
            do
            {
                Console.Clear();
                Console.WriteLine("=== SISTEM GESTIUNE INTEGRAT: FABRICA DE FURAJE ===");
                Console.WriteLine("    --- MATERIE PRIMA ---");
                Console.WriteLine("1. Afisare stoc materie prima (Siloz)");
                Console.WriteLine("2. Aprovizionare materie prima");
                Console.WriteLine("\n    --- PRODUCTIE ---");
                Console.WriteLine("3. Productie furaj nou");
                Console.WriteLine("4. Afisare stoc produse finite (Saci furaje)");
                Console.WriteLine("\n    --- FACTURARE ---");
                Console.WriteLine("5. Emite factura noua (Vanzare)");
                Console.WriteLine("6. Modifica factura existenta (Schimbare cantitate)");
                Console.WriteLine("7. Afiseaza arhiva facturi");
                Console.WriteLine("X. Inchide aplicatia");
                Console.WriteLine("===================================================");
                Console.Write("Alege o optiune: ");

                optiune = Console.ReadLine()?.ToUpper() ?? string.Empty;

                switch (optiune)
                {
                    case "1":
                        Console.WriteLine("\n--- Stoc Siloz ---");
                        Console.WriteLine(siloz.Info());
                        break;

                    case "2":
                        Console.WriteLine("\n--- Aprovizionare ---");
                        Console.Write("Cati kg de PORUMB ai primit? ");
                        siloz.Porumb += CitireNumarSigur();

                        Console.Write("Cati kg de SOIA ai primit? ");
                        siloz.Soia += CitireNumarSigur();

                        Console.Write("Cati kg de VITAMINE ai primit? ");
                        siloz.Vitamine += CitireNumarSigur();

                        adminStocuri.SalveazaMateriaPrima(siloz);
                        Console.WriteLine("Siloz actualizat si salvat cu succes!");
                        break;

                    case "3":
                        Console.WriteLine("\n--- Productie ---");
                        Console.Write("Numele furajului pe care vrei sa-l produci: ");
                        string numeFuraj = Console.ReadLine();

                        Console.WriteLine("Alege categoria (1-Pasari, 2-Porcine, 3-Bovine, 4-Pesti): ");
                        TipFuraj categorie;
                        while (!Enum.TryParse(Console.ReadLine(), out categorie) || !Enum.IsDefined(typeof(TipFuraj), categorie))
                        {
                            Console.WriteLine("Categorie invalida! Mai incearca:");
                        }

                        Console.Write("Cati kg vrei sa produci? ");
                        double cantitateDeProdus = CitireNumarSigur();

                        double necesarPorumb = cantitateDeProdus * 0.60;
                        double necesarSoia = cantitateDeProdus * 0.30;
                        double necesarVitamine = cantitateDeProdus * 0.10;

                        if (siloz.Porumb >= necesarPorumb && siloz.Soia >= necesarSoia && siloz.Vitamine >= necesarVitamine)
                        {
                            siloz.Porumb -= necesarPorumb;
                            siloz.Soia -= necesarSoia;
                            siloz.Vitamine -= necesarVitamine;
                            adminStocuri.SalveazaMateriaPrima(siloz);

                            ProdusFinit produsExistent = depozitFuraje.Find(p => p.NumeFuraj.ToLower() == numeFuraj.ToLower());
                            if (produsExistent != null)
                            {
                                produsExistent.CantitateKg += cantitateDeProdus;
                                produsExistent.ActualizeazaReteta();
                            }
                            else
                            {
                                ProdusFinit produsNou = new ProdusFinit(contorProduse++, numeFuraj, categorie, cantitateDeProdus);
                                depozitFuraje.Add(produsNou);
                            }
                            adminStocuri.SalveazaFurajele(depozitFuraje);
                            Console.WriteLine($"\nSucces! Am produs {cantitateDeProdus}kg de {numeFuraj}.");
                        }
                        else
                        {
                            Console.WriteLine("\nEROARE: Materie prima insuficienta in siloz!");
                            if (siloz.Porumb < necesarPorumb) Console.WriteLine($"- Lipsesc {necesarPorumb - siloz.Porumb} kg de Porumb.");
                            if (siloz.Soia < necesarSoia) Console.WriteLine($"- Lipsesc {necesarSoia - siloz.Soia} kg de Soia.");
                            if (siloz.Vitamine < necesarVitamine) Console.WriteLine($"- Lipsesc {necesarVitamine - siloz.Vitamine} kg de Vitamine.");
                        }
                        break;

                    case "4":
                        Console.WriteLine("\n--- Stoc Depozit Furaje ---");
                        if (depozitFuraje.Count == 0) Console.WriteLine("Depozitul este gol.");
                        foreach (ProdusFinit p in depozitFuraje) Console.WriteLine(p.Info());
                        break;

                    case "5":
                        Console.WriteLine("\n--- Emitere Factura ---");
                        Console.Write("Ce furaj vrea clientul? ");
                        string produsCautat = Console.ReadLine();

                        ProdusFinit produsDeVanzare = depozitFuraje.Find(p => p.NumeFuraj.ToLower() == produsCautat.ToLower());

                        if (produsDeVanzare == null || produsDeVanzare.CantitateKg == 0)
                        {
                            Console.WriteLine("Eroare: Produs inexistent sau stoc epuizat!");
                        }
                        else
                        {
                            Console.Write($"Cate kg doreste? (Stoc: {produsDeVanzare.CantitateKg}kg): ");
                            double kgVandute = CitireNumarSigur();

                            if (kgVandute <= produsDeVanzare.CantitateKg)
                            {
                                Console.Write("Nume client: ");
                                string numeClient = Console.ReadLine();
                                Console.Write("Prenume client: ");
                                string prenumeClient = Console.ReadLine();
                                Console.Write("Telefon: ");
                                string telefonClient = Console.ReadLine();
                                Console.Write("Adresa: ");
                                string adresaClient = Console.ReadLine();

                                produsDeVanzare.CantitateKg -= kgVandute;
                                produsDeVanzare.ActualizeazaReteta();
                                adminStocuri.SalveazaFurajele(depozitFuraje);

                                Factura f = new Factura(contorFacturi++, numeClient, prenumeClient, telefonClient, adresaClient, produsDeVanzare.NumeFuraj, kgVandute);
                                adminFacturi.AdaugaFactura(f);

                                Console.WriteLine("\nFactura emisa! Stocul a fost scazut.");
                            }
                            else
                            {
                                Console.WriteLine("Eroare: Stoc insuficient!");
                            }
                        }
                        break;

                    case "6":
                        Console.WriteLine("\n--- Modificare Factura Existenta ---");
                        Console.Write("Introdu Numele de familie al clientului: ");
                        string numeCautat = Console.ReadLine();
                        Console.Write("Introdu Prenumele clientului: ");
                        string prenumeCautat = Console.ReadLine();

                        Factura facturaDeModificat = adminFacturi.GetFacturaDupaNumeComplet(numeCautat, prenumeCautat);

                        if (facturaDeModificat != null)
                        {
                            Console.WriteLine($"\nAm gasit factura: A cumparat {facturaDeModificat.CantitateCumparata} kg de {facturaDeModificat.ProdusCumparat}.");
                            Console.Write("Introdu NOUA cantitate totala dorita (kg): ");
                            double cantitateNoua = CitireNumarSigur();

                            ProdusFinit produs = depozitFuraje.Find(p => p.NumeFuraj.ToLower() == facturaDeModificat.ProdusCumparat.ToLower());

                            double diferenta = cantitateNoua - facturaDeModificat.CantitateCumparata;

                            if (diferenta > 0)
                            {
                                if (produs != null && produs.CantitateKg >= diferenta)
                                {
                                    produs.CantitateKg -= diferenta;
                                    facturaDeModificat.CantitateCumparata = cantitateNoua;
                                    Console.WriteLine($"\nSucces! Am adaugat {diferenta} kg pe factura.");
                                }
                                else
                                {
                                    Console.WriteLine("\nEroare: Nu avem suficient stoc in depozit pentru a mari factura!");
                                    break;
                                }
                            }
                            else if (diferenta < 0)
                            {
                                if (produs != null)
                                {
                                    produs.CantitateKg += Math.Abs(diferenta); 
                                    facturaDeModificat.CantitateCumparata = cantitateNoua;
                                    Console.WriteLine($"\nSucces! Am redus factura si am returnat {Math.Abs(diferenta)} kg in stoc.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("\nCantitatea este identica. Nu am modificat nimic.");
                                break;
                            }

                            adminStocuri.SalveazaFurajele(depozitFuraje);
                            adminFacturi.ModificaFactura(facturaDeModificat);
                        }
                        else
                        {
                            Console.WriteLine("\nNu am gasit nicio factura pentru acest client!");
                        }
                        break;

                    case "7":
                        Console.WriteLine("\n--- Arhiva Facturi ---");
                        List<Factura> arhivafacturi = adminFacturi.GetFacturi();
                        if (arhivafacturi.Count == 0) Console.WriteLine("Nu exista nicio factura emisa.");
                        foreach (Factura f in arhivafacturi) Console.WriteLine(f.Info());
                        break;

                    case "X":
                        Console.WriteLine("Datele sunt salvate automat. O zi buna!");
                        break;

                    default:
                        Console.WriteLine("Optiune inexistenta!");
                        break;
                }

                if (optiune != "X")
                {
                    Console.WriteLine("\nApasa ENTER pentru a te intoarce la meniu...");
                    Console.ReadLine();
                }

            } while (optiune != "X");
        }

        static double CitireNumarSigur()
        {
            double numar;
            while (!double.TryParse(Console.ReadLine(), out numar) || numar < 0)
            {
                Console.Write("Eroare! Introdu un numar valid si pozitiv: ");
            }
            return numar;
        }
    }
}