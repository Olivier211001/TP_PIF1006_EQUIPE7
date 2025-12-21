using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PIF1006_tp1
{
    public class Automate
    {
        public State InitialState { get; private set; }
        public State CurrentState { get; private set; }
        public List<State> States { get; private set; }
        public bool IsValid { get; private set; }

        private List<string> erreurs = new List<string>();

        public Automate(string filePath)
        {
            States = new List<State>();
            LoadFromFile(filePath);
        }

        private void LoadFromFile(string filePath)
        {
            // Vous devez pouvoir charger à partir d'un fichier quelconque.  Cela peut être un fichier XML, JSON, texte, binaire, ...
            // P.ex. avec un fichier texte, vous pouvoir balayer ligne par ligne et interprété en séparant chaque ligne en un tableau de strings
            // dont le premier représente l'action, et la suite les arguments. L'équivalent de l'automate décrit manuellement dans la classe
            // Program pourrait être:
            //  state s0 1 1
            //  state s1 0 0
            //  state s2 0 0
            //  state s3 1 0
            //  transition s0 0 s1
            //  transition s0 1 s0
            //  transition s1 0 s1
            //  transition s1 1 s2
            //  transition s2 0 s1
            //  transition s3 1 s3
            //
            // Dans une boucle, on prend les lignes une par une:
            //   - Si le 1er terme est "state", on prend les arguments et on crée un état du même nom
            //     et on l'ajoute à une liste d'état; les 2 et 3e argument représentent alors si c'est un état final, puis si c'est l'état initial
            //   - Si c'est "transition" on cherche dans la liste d'état l'état qui a le nom en 1er argument et on ajoute la transition avec les 2 autres
            //     arguments à sa liste
            // 
            // Considérez que:
            //   - S'il y a d'autres termes, les lignes pourraient être ignorées;
            //   - Si l'état n'est pas trouvé dans la liste (p.ex. l'état est référencé mais n'existe pas (encore)), la transition est ignorée
            //   - Après lecture du fichier:
            //          - si l'automate du fichier n'est pas déterministe (vous devrez penser à comment vérifier cela -> l'état et la transition
            //            en défaut doit être indiquée à l'utilisateur), OU
            //          - si l'automate n'a aucun état, ou aucun état initial
            //     l'automate est considéré comme "invalide" (la propriété IsValid doit alors valoir faux)
            //   - Lorsque des lignes (ou l'automate) sont ignorées ou à la fin l'automate rejeté, cela doit être indiqué à l'utilisateur
            //     à la console avec la ligne/raison du "rejet".

            ///////////////////////////////////////////////////////////////code de la solution ICI///////////////////////////////////////////////////////////////////
            // On valide si le fichier est vide
            erreurs.Clear();
            States.Clear();
            InitialState = null;
            CurrentState = null;
            IsValid = true;

            if (string.IsNullOrWhiteSpace(filePath)){
                erreurs.Add("filePath est vide.");
                IsValid = false;
                PrintErreurs();
                return;
            }
            // On valide si le fichier est introuvables    
            if (!File.Exists(filePath)){
                erreurs.Add("Fichier introuvable.");
                IsValid = false;
                PrintErreurs();
                return;
            }
            // setter les états
            setStates(filePath);
            // setter les transitions
            setTransitions(filePath);

                     if (States == null || States.Count == 0)
            {
                erreurs.Add("Automate invalide : aucun état défini.");
                IsValid = false;
            }

            if (InitialState == null)
            {
                erreurs.Add("Automate invalide : aucun état initial défini.");
                IsValid = false;
            }

            if (IsValid)
            {
                CurrentState = InitialState;
            }

            Console.WriteLine("DEBUG States.Count = " + States.Count);
            Console.WriteLine("DEBUG InitialState = " + (InitialState == null ? "null" : InitialState.Name));
            Console.WriteLine("DEBUG IsValid = " + IsValid);

            PrintErreurs();
        }

        private void PrintErreurs()
        {
            if (erreurs.Count == 0) return;
            Console.WriteLine("=== Résumé du chargement de l'automate ===");
            foreach (var e in erreurs)
                Console.WriteLine(e);
            Console.WriteLine("==========================================");
        }

        private void setStates(string filePath){
            foreach (var line in File.ReadAllLines(filePath)){
                string premierTerme = "";
                // code pour enlever les espaces au début et à la fin 
                string trimmed = line.Trim();
                // code pour ignorer les espaces vides 
                if (string.IsNullOrEmpty(trimmed))
                    continue;
                // code pour séparer les termes dans la ligne 
                string[] parts = trimmed.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                // trouver le premier terme
                if (parts.Length > 0){
                    premierTerme = parts[0];
                }
                // premierTerme is state ? 
                if (premierTerme.Equals("state", StringComparison.OrdinalIgnoreCase)){
                    // État final ? 
                    if (parts.Length < 4)
                    {
                        erreurs.Add("ligne state invalide");
                        IsValid = false;
                        continue;
                    }

                    bool isFinal = parts[2] == "1";
                    bool isInit = parts[3] == "1";

                    State existing = States.FirstOrDefault(s => s.Name == parts[1]);

                    if (existing == null){
                        State newState = new State(parts[1], isFinal); // création de l'état 
                        if (isInit) // État initial ? 
                        {
                            if (InitialState != null && InitialState != newState)
                            {
                                erreurs.Add("plusieurs états initiaux détectés.");
                                IsValid = false;
                            }
                            InitialState = newState;
                        }
                        States.Add(newState); // ajout dans la liste d'états 
                    }
                    else{
                        if (isInit)
                        {
                            if (InitialState != null && InitialState != existing)
                            {
                                erreurs.Add("plusieurs états initiaux détectés.");
                                IsValid = false;
                            }
                            InitialState = existing;
                        }
                    }
                }
            }
        }

        private void setTransitions(string filePath){
            foreach (var line in File.ReadAllLines(filePath)){
                string premierTerme = "";
                // code pour enlever les espaces au début et à la fin 
                string trimmed = line.Trim();
                // code pour ignorer les espaces vides 
                if (string.IsNullOrEmpty(trimmed))
                    continue;
                // code pour séparer les termes dans la ligne 
                string[] parts = trimmed.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                // trouver le premier terme
                if (parts.Length > 0){
                    premierTerme = parts[0];
                }
                // premierTerme is transition ? 
                if (premierTerme.Equals("transition", StringComparison.OrdinalIgnoreCase)){
                    if (parts.Length < 4 || parts[2].Length != 1)
                    {
                        erreurs.Add("transition invalide.");
                        IsValid = false;
                        continue;
                    }

                    char symbole = parts[2][0];
                    if (symbole != '0' && symbole != '1')
                    {
                        erreurs.Add("symbole de transition invalide (doit être 0 ou 1).");
                        continue;
                    }

                    State stateTransit = States.Where(s => s.Name == parts[3]).FirstOrDefault();
                    State source = States.Where(s => s.Name == parts[1]).FirstOrDefault();

                    if (source == null || stateTransit == null)
                    {
                        erreurs.Add("transition ignorée (état source ou destination inexistant).");
                        continue;
                    }

                    if (source.Transitions.Any(t => t.Input == symbole))
                    {
                        erreurs.Add("automate non déterministe (deux transitions avec le même symbole pour un même état).");
                        IsValid = false;
                        continue;
                    }

                    source.Transitions.Add(new Transition(symbole, stateTransit));
                }
            }
        }

      public bool Validate(string input)
{
    bool isValid = true;
    Reset();

    // Vous devez transformer l'input en une liste / un tableau de caractères (char) et les lire un par un;
    // L'automate doit maintenant à jour son "CurrentState" en suivant les transitions et en respectant l'input.
    // Considérez que l'automate est déterministe et que même si dans les faits on aurait pu mettre plusieurs
    // transitions possibles pour un état et un input donné, le 1er trouvé dans la liste est le chemin emprunté.
    // Si aucune transition n'est trouvé pour un état courant et l'input donné, cela doit retourner faux;
    // Si tous les caractères ont été pris en compte, on vérifie si l'état courant est final ou non et on retourne
    // vrai ou faux selon.

    // VOUS DEVEZ OBLIGATOIREMENT AFFICHER la suite des états actuel, input lu, et état transité pour qu'on puisse
    // suivre le déroulement de l'analyse.

    if (!IsValid || CurrentState == null)
        return false;

    if (string.IsNullOrEmpty(input))
    {
        Console.WriteLine("État initial : " + CurrentState.Name + " (final = " + CurrentState.IsFinal + ")");
        return CurrentState.IsFinal;
    }

    foreach (char c in input)
    {
        if (c != '0' && c != '1')
        {
            Console.WriteLine("Symbole invalide '" + c + "'.");
            return false;
        }

        var transition = CurrentState.Transitions.FirstOrDefault(t => t.Input == c);
        if (transition == null)
        {
            Console.WriteLine("Aucune transition pour l'état '" + CurrentState.Name + "' avec l'input '" + c + "'.");
            isValid = false;
            break;
        }

        Console.WriteLine("État courant : " + CurrentState.Name + ", input lu : '" + c + "', état transité : " + transition.TransiteTo.Name);
        CurrentState = transition.TransiteTo;
    }

    if (!isValid)
        return false;

    Console.WriteLine("État final atteint : " + CurrentState.Name + " (final = " + CurrentState.IsFinal + ")");
    return CurrentState.IsFinal;
}

        public void Reset()
        {
            // Vous devez faire du code pour indiquer ce que signifie réinitialiser l'automate avant chaque validation.
            CurrentState = InitialState;
        }

        public override string ToString()
        {
            if(States == null || States.Count == 0)
            {
                return "aucun state présent";
            }
            
            // meilleur méthode que j'ai trouvé avec un stringBuilder :)
            var printAutomate = new StringBuilder();

            foreach (var state in States)
            {
                if (state.Name.Equals(InitialState.Name))
                {
                    printAutomate.AppendLine("[" + state.ToString() + "]");
                }
                else
                {
                    printAutomate.AppendLine(state.ToString());
                }
                foreach(Transition transition in state.Transitions)
                {
                    printAutomate.AppendLine(" ");
                    printAutomate.AppendLine(" " + transition.ToString());
                }
                printAutomate.AppendLine(" ");
            }
            return printAutomate.ToString();
        }
    }
}
