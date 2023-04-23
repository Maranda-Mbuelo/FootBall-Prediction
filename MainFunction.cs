using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
//using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
//using System.Threading.Tasks;
//using System.Security.Policy;

namespace BetwayProject
{
    internal class Program

    {
        public static List<TeamStats>  teamStats = new List<TeamStats>();
        static async Task Main(string[] args)
        {
            string userOption = null;
            bool limit = true;

            var losing = new List<string>();

            while (limit == true)
            {

                if (userOption != "true")
                {
                    Console.WriteLine("FootBall_Prediction Programme \n\n\n");


                    Console.Write("Chose from option:  ");
                    Console.WriteLine("1.print all,  2.mostlyLosing,  3.neverLose,  4.highDrawChance,  5.mostlyWinning  ");
                    userOption = Console.ReadLine();

                    switch (userOption)
                    {
                        case "1":
                            PrintAll();
                            break;

                        case "2":
                            foreach (var x in await MostlyLosing())
                            {
                                losing.Add(x);
                            }
                            break;
                           
                        case "3":
                            await NeverLost();
                            break;
                           
                        case "4":
                            await DrawTeams();
                            break;
                           
                        case "5":
                            Console.WriteLine("Option Still under Construction");
                            break;

                        default:
                            if (userOption != "true")
                            {
                                Console.WriteLine("Invalid Option");
                            }
                            else
                            {
                                Console.WriteLine("F R O M  THE LOSING LIST: -> ");
                                foreach (var item in losing)
                                {
                                    Console.WriteLine(item);
                                }
                                Console.WriteLine("Press any Key to Exit");
                            }
                            break;
                                
                    }

                    Console.WriteLine("\n\n\n");
                }
                else
                {
                    limit = false;
                }
            }
            Console.ReadKey();
        }

        static async void PrintAll()
        {
            Materials jsonLinks = new Materials();

            HttpClient client = new HttpClient();



            try
            {

                var httpResponseMessage = await client.GetAsync(jsonLinks.url);
                string jsonResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                var jsonInfo = JsonConvert.DeserializeObject<BetwayTestClass[]>(jsonResponse);

                Console.WriteLine(jsonResponse);
                Console.WriteLine("\n\n\n");

                foreach (var item in jsonInfo)
                {
                    Console.WriteLine("Group         : " + item.Group);
                    Console.WriteLine("MatchNumber   : " + item.MatchNumber);
                    Console.WriteLine("RoundNumber   : " + item.RoundNumber);
                    Console.WriteLine("DateOfMatch   : " + item.DateUtc);
                    Console.WriteLine("Location      : " + item.Location);
                    Console.WriteLine("Home-Team     : " + item.HomeTeam);
                    Console.WriteLine("HomeTeam Score: " + item.HomeTeamScore);
                    Console.WriteLine("Away-Team     : " + item.AwayTeam);
                    Console.WriteLine("AwayTeamScore : " + item.AwayTeamScore + "\n");

                    Console.Write("           ");
                    Console.WriteLine(item.HomeTeam + " " + item.HomeTeamScore + " : " + item.AwayTeamScore + " " + item.AwayTeam);
                    Console.WriteLine("\n\n");

                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                //throw;
            }
            finally
            {
                //Dispose HTTP Client
                client.Dispose();
            }
        }

        static async Task<string[]> MostlyLosing()
        {
            Materials jsonLinks = new Materials();
            var url = jsonLinks.url;

            HttpClient httpClient = new HttpClient();

            List<string> losingHomeTeams = new List<string>();
            var myList = new List<string>();
            var duplicates = new List<string>();

            try
            {
                var httpResponseMessage = await httpClient.GetAsync(url);

                string jsonFile = await httpResponseMessage.Content.ReadAsStringAsync();

                //Deserialize the json response into a C# array of type post[]
                var jsonInfo = JsonConvert.DeserializeObject<BetwayTestClass[]>(jsonFile);


                foreach (var item in jsonInfo)
                {
                    if (item.HomeTeamScore < item.AwayTeamScore)
                    {
                        losingHomeTeams.Add(item.HomeTeam);
                    }

                }

                foreach (var item in losingHomeTeams)
                {

                    if (!myList.Contains(item))
                    {
                        myList.Add(item);
                    }
                    else
                    {
                        duplicates.Add(item);
                    }

                }

                foreach (var x in duplicates)
                {
                    Console.WriteLine(x);
                }

            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                Console.ReadKey();
                //throw;
            }
            finally
            {
                //Dispose HTTP Client
                httpClient.Dispose();
            }

            return duplicates.ToArray();
        }

        static async Task<string[]> NeverLost()
        {
            Materials jsonLinks = new Materials();
            var url = jsonLinks.url;
            HttpClient clientUser = new HttpClient();


            var winningTeams = new List<string>();
            var losers = new List<string>();
            var myList = new List<string>();
            var duplicates = new List<string>();
            

            try
            {
                var httpResponseMessage = await clientUser.GetAsync(url);
                var jsonFile = await httpResponseMessage.Content.ReadAsStringAsync();
                var json = JsonConvert.DeserializeObject<BetwayTestClass[]>(jsonFile);

                foreach (var item in json)
                {
                    if (item.AwayTeamScore > item.HomeTeamScore)
                    {
                        AddStats(item.AwayTeam, Odds.Win);
                        AddStats(item.HomeTeam, Odds.Lose);
                        winningTeams.Add(item.AwayTeam);
                        losers.Add(item.HomeTeam);

                    }
                    else if (item.AwayTeamScore < item.HomeTeamScore)
                    {
                        AddStats(item.AwayTeam, Odds.Lose);
                        AddStats(item.HomeTeam, Odds.Win);
                        winningTeams.Add(item.HomeTeam);
                        losers.Add(item.AwayTeam);
                    }

                }

                foreach (var item in winningTeams)
                {
                    if (losers.Contains(item))
                    {
                        winningTeams.Remove(item);
                    }
                }

                foreach (var item in winningTeams)
                {

                    if (!myList.Contains(item))
                    {
                        myList.Add(item);
                    }
                    else
                    {
                        duplicates.Add(item);
                    }

                }

                foreach (var item in teamStats)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
            finally
            {
                clientUser.Dispose();
            }


            return duplicates.ToArray();
        }

        private static void AddStats(string teamName, Odds odds)
        {
            var team = teamStats.FirstOrDefault(x => x.TeamName.Equals(teamName));
            if (team == null)
            {
                team = new TeamStats { TeamName = teamName, Lose = 0, Win = 0 };
                teamStats.Add(team);
            }
                
            if (odds == Odds.Win)
                team.Win = team.Win + 1;
            if (odds == Odds.Lose)
                team.Lose = team.Lose + 1;
        }

        static async Task<string[]> DrawTeams()
        {
            Materials jsonLinks = new Materials();
            var url = jsonLinks.url;

            HttpClient httpClient = new HttpClient();


            var drawingTeams = new List<string>();

            try
            {
                var httpResponseMessage = await httpClient.GetAsync(url);
                var jsonFile = await httpResponseMessage.Content.ReadAsStringAsync();
                var jsonInfo = JsonConvert.DeserializeObject<BetwayTestClass[]>(jsonFile);

                foreach (var item in jsonInfo)
                {
                    if (item.HomeTeamScore == item.AwayTeamScore)
                    {
                        drawingTeams.Add(item.AwayTeam);
                        drawingTeams.Add(item.HomeTeam);
                    }
                }

                foreach (var item in drawingTeams)
                {
                    Console.WriteLine(item);
                }
            }
            catch (Exception e)
            {
                Console.Write(e);
            }
            finally
            {
                httpClient.Dispose();
            }

            return drawingTeams.ToArray();

        }


    }

    internal class TeamStats
    {
        public string TeamName { get; set; }
        public int Win { get; set; }
        public int Lose { get; set; }

    }
}

