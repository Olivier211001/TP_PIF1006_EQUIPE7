using System;
using System.Text;

namespace PIF1006_tp1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine("================================================");
            Console.WriteLine("Nom de l'application : TP automate PIF1006");
            Console.WriteLine("Membre de l'équipe :");
            Console.WriteLine("Olivier Lafleur, LAFO83100101");
            Console.WriteLine("Majda Lyna Lemrini, LEMM74510008");
            Console.WriteLine("Akram Adouani, ADOA13088203");
            Console.WriteLine("================================================");
            Console.WriteLine();

            Console.WriteLine("Entrez le nom du fichier d'automate (ENTER = Test.txt) : ");
            string filePath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(filePath))
            {
                filePath = "Test.txt";
            }

            Automate automate = new Automate(filePath);

            if (!automate.IsValid)
            {
                Console.WriteLine();
                Console.WriteLine("L'automate chargé est invalide. Impossible de valider des chaînes.");
                Console.WriteLine("Appuyez sur ENTER pour quitter.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Automate chargé avec succès.");
            Console.WriteLine();
            Console.WriteLine("Représentation de l'automate :");
            Console.WriteLine("--------------------------------");
            Console.WriteLine(automate.ToString());
            Console.WriteLine("--------------------------------");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Entrez une chaîne composée de 0 et 1 (ENTER pour quitter) : ");
                string input = Console.ReadLine() ?? string.Empty;

                if (string.IsNullOrEmpty(input))
                {
                    break;
                }

                Console.WriteLine();
                Console.WriteLine("=== Déroulement de l'analyse ===");

                bool accepte = automate.Validate(input);

                Console.WriteLine("================================");
                Console.WriteLine(accepte
                    ? "RÉSULTAT : CHAÎNE ACCEPTÉE"
                    : "RÉSULTAT : CHAÎNE REJETÉE");
                Console.WriteLine();
            }

            Console.WriteLine("Fin du programme. Appuyez sur ENTER pour quitter.");
            Console.ReadLine();
        }
    }
}
