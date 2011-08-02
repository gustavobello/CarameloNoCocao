using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodMW2.Controller;

namespace CodMW2
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program(args);
        }
        public Program(string[] args)
        {
            foreach (string arg in args)
            {
                Parser p = new Parser(arg);
            }
        }
    }
}
