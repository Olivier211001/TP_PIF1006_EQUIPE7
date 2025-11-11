using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIF1006_tp1
{
    public class Automate
    {
        public State InitialState { get; private set; }
        public State CurrentState { get; private set; }
        public List<State> States { get; private set; }
        public bool IsValid { get; private set; }

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

            return isValid;
        }

        public void Reset()
        {
            // Vous devez faire du code pour indiquer ce que signifie réinitialiser l'automate avant chaque validation.
        }

        public override string ToString()
        {
            // Vous devez modifier cette partie de sorte à retourner un équivalent string qui décrit tous les états et
            // la table de transitions de l'automate.
            return base.ToString(); // On ne retournera donc pas le ToString() par défaut
        }
    }
}
