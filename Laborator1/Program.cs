using System;

namespace HomeworkAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Introduceti numărul de ore lucrate: ");
            int oreLucrate = int.Parse(Console.ReadLine());
            Console.Write("Introduceți tariful pe ora: ");
            double tarifPeOra = double.Parse(Console.ReadLine());
            double salariu = (double)oreLucrate * tarifPeOra;
            Console.WriteLine($"\nSalariul calculat este: {salariu} lei");
            if (salariu > 3000)
            {
                Console.WriteLine("Salariu mare");
            }
            else
            {
                Console.WriteLine("Ati lucrat prea putine ore pentru a avea un salariu mare!");
            }
            Console.ReadKey();
        }
    }
}