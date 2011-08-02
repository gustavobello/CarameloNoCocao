using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodMW2.Model
{
    class Partida
    {
        public string Id { get; set; }
        public DateTime Data { get; set; }
        public List<Player> PlayerList = new List<Player>();
        private List<Evento> eventList = new List<Evento>();
        private List<Resultado> resultado = new List<Resultado>();

        public int AlliesKillCount = 0;
        public int AlliesDeathCount = 0;
        public int AxisKillCount = 0;
        public int AxisDeathCount = 0;
 
        public void GetEstatisticas()
        {
            foreach (Player p in PlayerList)
            {
                List<Evento> listDeaths = eventList.Where(pl => pl.PlayerA.Id == p.Id && pl.Tipo == TypeEvent.Kill).ToList<Evento>();
                List<Evento> listKills = eventList.Where(pl => pl.Tipo == TypeEvent.Kill && pl.PlayerB.Id == p.Id).ToList<Evento>();

                if (p.Time == TypeTime.Undefined)
                {
                    if (listDeaths.Count > 0) { p.Time = listDeaths[listDeaths.Count -1].PlayerA.Time; }
                    else if (listKills.Count > 0) { p.Time = listKills[listKills.Count - 1].PlayerB.Time; }
                }

                int killCount = listKills.Count();
                int deathCount = listDeaths.Count();
                double ratio = deathCount != 0 ? (double)killCount / (double)deathCount : killCount;

                List<string> listArma = new List<string>();
                listKills.ForEach(k => listArma.Add(k.Arma.Nome));
                string mostKilledWeapon = "---";
                if (killCount > 0)
                {
                    mostKilledWeapon = listArma.GroupBy(x => x).OrderByDescending(x => x.Count()).First().Key;
                }

                int headshotsCount = listKills.Count(ev => ev.Onde == TypeOnde.Head);
                       
                int meleeCount = listKills.Count(ev => ev.Modificador == TypeModificador.Melee && ev.Arma.Nome != "riotshield_mp");

                int leftArmUpperCount = listKills.Count(ev => ev.Onde == TypeOnde.LeftArmUpper);
                int leftArmLowerCount = listKills.Count(ev => ev.Onde == TypeOnde.LeftArmLower);
                int leftLegLowerCount = listKills.Count(ev => ev.Onde == TypeOnde.LeftLegLower);
                int leftLegUpperCount = listKills.Count(ev => ev.Onde == TypeOnde.LeftLegUpper);
                int rightArmUpperCount = listKills.Count(ev => ev.Onde == TypeOnde.RightArmUpper);
                int rightArmLowerCount = listKills.Count(ev => ev.Onde == TypeOnde.RightArmLower);
                int rightLegLowerCount = listKills.Count(ev => ev.Onde == TypeOnde.RightLegLower);
                int rightLegUpperCount = listKills.Count(ev => ev.Onde == TypeOnde.RightLegUpper);
                int undefinedCount = listKills.Count(ev9 => ev9.Onde == TypeOnde.Undefined);
                int torsoLowerCount = listKills.Count(ev => ev.Onde == TypeOnde.TorsoLower);
                int torsoUpperCount = listKills.Count(ev => ev.Onde == TypeOnde.TorsoUpper);

                string mostDiedWeapon = listDeaths.Max(pr => pr.Arma.Nome);
                int headshotsCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.Head);
                int meleeCountD = listDeaths.Count(ev => ev.Modificador == TypeModificador.Melee && ev.Arma.Nome != "riotshield");
                
                int leftArmUpperCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.LeftArmUpper);
                int leftArmLowerCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.LeftArmLower);
                int leftLegLowerCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.LeftLegLower);
                int leftLegUpperCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.LeftLegUpper);
                int rightArmUpperCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.RightArmUpper);
                int rightArmLowerCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.RightArmLower);
                int rightLegLowerCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.RightLegLower);
                int rightLegUpperCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.RightLegUpper);
                int undefinedCountD = listDeaths.Count(ev9 => ev9.Onde == TypeOnde.Undefined);
                int torsoLowerCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.TorsoLower);
                int torsoUpperCountD = listDeaths.Count(ev => ev.Onde == TypeOnde.TorsoUpper);

                if (p.Time == TypeTime.Allies)
                {
                    AlliesDeathCount += deathCount;
                    AlliesKillCount += killCount;
                }
                else
                {
                    AxisDeathCount += deathCount;
                    AxisKillCount += killCount;
                }

                Estatistica est = new Estatistica() { KillCount = killCount, DeathCount = deathCount, Ratio = ratio, 
                    MostKilledWeapon = mostKilledWeapon,  HeadshotsCount = headshotsCount, MeleeCount = meleeCount, LeftArmUpperCount = leftArmUpperCount, 
                    LeftArmLowerCount = leftArmLowerCount, LeftLegLowerCount = leftLegLowerCount, LeftLegUpperCount = leftLegUpperCount, RightArmUpperCount = rightArmUpperCount, 
                    RightArmLowerCount = rightArmLowerCount, RightLegLowerCount = rightLegLowerCount, RightLegUpperCount = rightLegUpperCount,
                    UndefinedCount = undefinedCount, TorsoLowerCount = torsoLowerCount, TorsoUpperCount = torsoUpperCount, MostDiedWeapon = mostDiedWeapon, HeadshotsCountD = headshotsCountD, 
                    MeleeCountD = meleeCountD, LeftArmUpperCountD = leftArmUpperCountD, LeftArmLowerCountD = leftArmLowerCountD, LeftLegLowerCountD = leftLegLowerCountD, LeftLegUpperCountD = leftLegUpperCountD, 
                    RightArmUpperCountD = rightArmUpperCountD, RightArmLowerCountD = rightArmLowerCountD, RightLegLowerCountD = rightLegLowerCountD, RightLegUpperCountD = rightLegUpperCountD, 
                    UndefinedCountD = undefinedCountD, TorsoLowerCountD = torsoLowerCountD, TorsoUpperCountD = torsoUpperCountD};

                p.Estatistica = est;
            }
         
        }
            
        public void AddEvento(Evento ev)
        {
            eventList.Add(ev);
            if (ev.Tipo == TypeEvent.Join)
            {
                if (!PlayerList.Exists(p=>p.Id == ev.PlayerA.Id))
                {
                    PlayerList.Add(ev.PlayerA);
                }
            }
        }
    }
}
