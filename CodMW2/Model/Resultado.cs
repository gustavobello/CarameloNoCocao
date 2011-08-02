using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodMW2.Model
{
    class Resultado
    {
        public Player Player { get; set; }

        public double Kills { get; set; }

        public double Deaths { get; set; }

        public double Ratio { get; set; }

        public int Headshots { get; set; }

        public int Facadas { get; set; }
    }
}
