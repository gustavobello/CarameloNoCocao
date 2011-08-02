using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodMW2.Model
{
    class Player
    {
        public string Nome { get; set; }
        public string Id { get; set; }
        public TypeTime Time { get; set; }
        public Estatistica Estatistica { get; set; }
    }
    enum TypeTime
    {
        Undefined,
        Allies,
        Axis
    }
}
