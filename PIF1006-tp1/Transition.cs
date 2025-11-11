using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIF1006_tp1
{
    /// <summary>
    /// Une transition représente un tuple (input, nouvel état transité)
    /// </summary>
    public class Transition
    {
        public char Input { get; set; }
        public State TransiteTo { get; set; }

        public Transition(char input, State transiteTo)
        {
            Input = input;
            TransiteTo = transiteTo;
        }

        public override string ToString()
        {
            return base.ToString(); // Modifier ce code pour retourner une représentation plus cohérente d'une transition
        }
    }
}
