using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodMW2.Model
{
    class Evento
    {
        public DateTime Time { get; set; }
        public TypeEvent Tipo { get; set; }
        public Player PlayerA { get; set; }
        public Player PlayerB { get; set; }
        public TypeModificador Modificador { get; set; }
        //TODO: melhora as armas
        public Arma Arma { get; set; }
        public TypeOnde Onde { get; set; }
        
        public static TypeModificador GetModificador(string mod)
        {
            switch (mod)
            {
                case "MOD_PISTOL_BULLET": return TypeModificador.Pistol;
                case "MOD_RIFLE_BULLET": return TypeModificador.Rifle;
                case "MOD_HEAD_SHOT": return TypeModificador.HeadShot;
                case "MOD_MELEE": return TypeModificador.Melee;
                case "MOD_GRENADE_SPLASH": return TypeModificador.Granade;
                default: return TypeModificador.Undefined;
            }
        }
        public static TypeTime GetTime(string time)
        {
            if (time == "allies") return TypeTime.Allies;
            if (time == "axis") return TypeTime.Axis;
            return TypeTime.Undefined;
        }
        public static TypeOnde GetOnde(string onde)
        {
            switch (onde)
            {
                case "left_arm_upper": return TypeOnde.LeftArmUpper;
                case "right_arm_upper": return TypeOnde.RightArmUpper;
                case "left_arm_lower": return TypeOnde.LeftArmLower;
                case "right_arm_lower": return TypeOnde.RightArmLower;
                case "torso_lower": return TypeOnde.TorsoLower;
                case "torso_upper": return TypeOnde.TorsoUpper;
                case "left_leg_upper": return TypeOnde.LeftLegUpper;
                case "right_leg_upper": return TypeOnde.RightLegUpper;
                case "left_leg_lower": return TypeOnde.LeftLegLower;
                case "right_leg_lower": return TypeOnde.RightLegLower;
                case "head": return TypeOnde.Head;
                default: return TypeOnde.Undefined;
            }
        }
        public static Arma GetArma(string wep)
        {
            if (wep != "none")
            {
                string nome = wep.Split('_')[0];
                string modificador = wep.Split('_')[1];
                return new Arma() { FullName = wep, Modificador = modificador, Nome = nome }; 
            }
            return new Arma() { FullName = wep, Modificador = wep, Nome = wep };             
        }
    }

    internal enum TypeEvent
    {
        Undefined,
        Join,
        Exit,
        Kill,
        Damage,
    }
    internal enum TypeModificador
    {
        Undefined,
        Pistol,
        Rifle,
        Melee,
        Granade,
        HeadShot
    }

    internal enum TypeOnde
    {
        Undefined,
        LeftArmUpper,
        LeftArmLower,
        RightArmUpper,
        RightArmLower,
        LeftLegUpper,
        LeftLegLower,
        RightLegUpper,
        RightLegLower,
        TorsoLower,
        TorsoUpper,
        Head,
    }
}
