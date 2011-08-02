using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CodMW2.Model;
using System.Reflection;

namespace CodMW2.Controller
{
    class Parser
    {
        private Dictionary<string, Resultado> dicResultado = new Dictionary<string, Resultado>();

        public Parser(string filePath)
        {
            parseFile(filePath);
        }
        private void parseFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            List<Partida> listPartidas = montaObjetos(lines);
            printEstatisticas(GeraEstatisticas(listPartidas), filePath);
        }

        private void printEstatisticas(string listEstatisticas, string filePath)
        {
            File.WriteAllText(Path.GetDirectoryName(filePath) + @"\resultado_" + Path.GetFileName(filePath), listEstatisticas);
        }
        private string GeraEstatisticas(List<Partida> listPartidas)
        {
            StringBuilder sb = new StringBuilder();
            List<Resultado> listFinal = new List<Resultado>();

            foreach(Partida partida in listPartidas)
            {
                partida.GetEstatisticas();
                sb.AppendLine("====================================================================");
                sb.AppendLine("Partida: " + partida.Id);
                sb.AppendLine("== Resumo ==");
                sb.AppendLine("Axis - Kills: " + partida.AxisKillCount + "\tDeaths: " + partida.AxisDeathCount + "\tScore: " + (partida.AxisKillCount - partida.AxisDeathCount) + "\tRatio: " + ((double)partida.AxisKillCount / (double)partida.AxisDeathCount));

                List<Player> listAxis = partida.PlayerList.Where(p => p.Time == TypeTime.Axis).ToList<Player>();
                List<Player> listAllies = partida.PlayerList.Where(p => p.Time == TypeTime.Allies).ToList<Player>();

                List<Player>  bestPlayerList = listAxis.OrderByDescending(p => p.Estatistica.Ratio).ToList<Player>();
                string bestPlayer = "N/A";                
                if (bestPlayerList.Count > 0)
                    bestPlayer = bestPlayerList[0].Nome;
                sb.AppendLine("Best Player: " + bestPlayer);
                
                sb.AppendLine();
                sb.AppendLine("Allies - Kills: " + partida.AlliesKillCount + "\tDeaths: " + partida.AlliesDeathCount + "\tScore: " + (partida.AlliesKillCount - partida.AlliesDeathCount) + "\tRatio: " + ((double)partida.AlliesKillCount / (double)partida.AlliesDeathCount));
                
                bestPlayerList = listAllies.OrderByDescending(p => p.Estatistica.Ratio).ToList<Player>();
                bestPlayer = "N/A";
                if (bestPlayerList.Count > 0)
                    bestPlayer = bestPlayerList[0].Nome;
                sb.AppendLine("Best Player: " + bestPlayer);
                                       
                sb.AppendLine("== == == ==");
                
                sb.AppendLine("Individuais:");
                sb.AppendLine("======== Alies ==========");
                foreach (Player p in listAllies.OrderByDescending(p => p.Estatistica.Ratio))
                {
                    sb.AppendLine(p.Nome);
                    sb.AppendLine("Kills: " + p.Estatistica.KillCount +" \tDeaths: " + p.Estatistica.DeathCount + "\t\tRatio: " + p.Estatistica.Ratio);
                    sb.AppendLine("Headshots: " + p.Estatistica.HeadshotsCount + "\tHeadshots levados: " + p.Estatistica.HeadshotsCountD);
                    sb.AppendLine("Facadas: " + p.Estatistica.MeleeCount + "\tFacadas levadas: " + p.Estatistica.MeleeCountD);
                    sb.AppendLine("Main weapon: " + p.Estatistica.MostKilledWeapon);
                    sb.AppendLine();
                    addPlayerToFinalScore(p);
                }

                sb.AppendLine("======== Axis ==========");
                foreach (Player p in listAxis.OrderByDescending(p => p.Estatistica.Ratio))
                {
                    sb.AppendLine(p.Nome);
                    sb.AppendLine("Kills: " + p.Estatistica.KillCount + " \tDeaths: " + p.Estatistica.DeathCount + "\t\tRatio: " + p.Estatistica.Ratio);
                    sb.AppendLine("Headshots: " + p.Estatistica.HeadshotsCount + "\tHeadshots levados: " + p.Estatistica.HeadshotsCountD);
                    sb.AppendLine("Facadas: " + p.Estatistica.MeleeCount + "\tFacadas levadas: " + p.Estatistica.MeleeCountD);
                    sb.AppendLine("Main weapon: " + p.Estatistica.MostKilledWeapon);
                    sb.AppendLine();
                    addPlayerToFinalScore(p);
                }
            }

                        
            StringBuilder sbFinal = new StringBuilder();
            int count = 0;
            sbFinal.AppendLine("====== Resultado Final =======");
            foreach (Resultado res in dicResultado.Values.OrderByDescending(p=> p.Ratio))
            {
                sbFinal.AppendLine(++count + " - " + res.Player.Nome + Environment.NewLine +"\t\tRatio: " + Math.Round(res.Ratio, 5) + "\tKills: " + res.Kills + "\tDeaths: " + res.Deaths + "  \tHeadshots: " + res.Headshots + "\tFacadas: " + res.Facadas);
            }
            sbFinal.AppendLine();
            
            return sbFinal.ToString() + sb.ToString();
        }
        private void addPlayerToFinalScore(Player p)
        {
            if (!dicResultado.ContainsKey(p.Nome))
            {
                dicResultado.Add(p.Nome, new Resultado { Deaths = p.Estatistica.DeathCount, Kills = p.Estatistica.KillCount, Player = p });
            }
            else
            {
                Resultado resultadoAux = dicResultado[p.Nome];
                resultadoAux.Deaths += p.Estatistica.DeathCount;
                resultadoAux.Kills += p.Estatistica.KillCount;
                resultadoAux.Headshots += p.Estatistica.HeadshotsCount;
                resultadoAux.Facadas += p.Estatistica.MeleeCount;                
                if (resultadoAux.Deaths != 0)
                    resultadoAux.Ratio = resultadoAux.Kills / resultadoAux.Deaths;
                else
                    resultadoAux.Ratio = resultadoAux.Kills;
            }            
        }
        private List<Partida> montaObjetos(string[] lines)
        {
            List<Partida> listPartida = new List<Partida>();
            Partida partida = null;
            int contPartidas = 0;

            foreach (string line in lines)
            {
                if (line.Contains("InitGame"))
                {
                    partida = new Partida() { Id = (++contPartidas).ToString() };
                    continue;
                }
                else if (line.Contains("ExitLevel:"))
                {

                    listPartida.Add(partida);
                    partida = null;
                }

                if (partida != null)
                {
                    string[] words = line.Split(';');
                    if (words[0].EndsWith("J"))
                    {
                        Evento ev = new Evento() { Tipo = TypeEvent.Join, Time = new DateTime(), PlayerA = new Player() { Id = words[1], Nome = words[3] } };
                        partida.AddEvento(ev);
                    }
                    else if (words[0].EndsWith("D") || words[0].EndsWith("K"))
                    {
                        TypeEvent typeEv = TypeEvent.Undefined;
                        if (words[0].EndsWith("D"))
                        {
                            typeEv = TypeEvent.Damage;
                        }
                        else
                        {
                            typeEv = TypeEvent.Kill;
                        }

                        Evento ev = new Evento()
                        {
                            Tipo = typeEv,
                            Time = new DateTime(),
                            PlayerA = new Player() { Id = words[1], Nome = words[4], Time = Evento.GetTime(words[3]) },
                            PlayerB = new Player() { Id = words[5], Nome = words[8], Time = Evento.GetTime(words[7]) },
                            Modificador = Evento.GetModificador(words[11]),
                            Arma = Evento.GetArma(words[9]),
                            Onde = Evento.GetOnde(words[12]),
                        };
                        partida.AddEvento(ev);
                    }
                }
            }
            return listPartida;
        }
    }
}
