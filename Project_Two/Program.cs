using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Project_Two
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to NFL Summary Program! Press 'enter' to continue\n");
            Console.WriteLine("Please enter the path (without file name) you would like to read from and then press 'enter'");
            Console.WriteLine("Warning: If file directory doesn't contain a file called Super_Bowl_Project.csv no output file will be created");

            string PATH = Console.ReadLine();
            if (Directory.Exists(PATH))
            {
                Console.WriteLine("Directory exists");
            }
            else
            {
                Console.WriteLine("Directory Doesn't exist, Exiting Program");
                System.Environment.Exit(1);
            }
            //For mstring PATH = @"C:\Users\steantc\Documents\CWEB2010\Project2\super-bowl-data-analytics-program-project-2-Steantc\Super_Bowl_Project.csv";
            string line;
            string[] data;
            List<SB_Info> sbList = null;

            try
            {
                FileStream input = new FileStream(PATH + "\\Super_Bowl_Project.csv", FileMode.Open, FileAccess.Read);
                StreamReader read = new StreamReader(input);
                line = read.ReadLine();
                sbList = new List<SB_Info>();
                while (!read.EndOfStream)
                {
                    data = read.ReadLine().Split(',');
                    sbList.Add(new SB_Info(data[0], data[1], Convert.ToInt32(data[2]), data[3], data[4], data[5], Convert.ToInt32(data[6]),
                        data[7], data[8], data[9], Convert.ToInt32(data[10]), data[11], data[12], data[13], data[14]));
                }
                read.Close();
                input.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.WriteLine("please choose a file path (without file name) and press 'enter'");
            string filePath = Console.ReadLine();
            if (Directory.Exists(filePath))
            {
                Console.WriteLine("your file Super_Bowl_Info awaits");
                Console.WriteLine("Press 'enter' to continue");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Directory Doesn't exist, Exiting Program");
                System.Environment.Exit(1);
            }

            using (StreamWriter writer = new StreamWriter(filePath + "\\Super_Bowl_Info.txt", false))
            {
                Console.WriteLine("A summary of Super Bowl information\n");
                writer.WriteLine("A summary of Super Bowl information\n");
                //List of superbowl winners
                Console.WriteLine("List of all Super Bowl Winners\n");
                writer.WriteLine("List of all Super Bowl Winners\n");
                sbList.ForEach(x => Console.WriteLine($"The Winner was {x.Winner} Year: {x.Date} Winning QB: {x.QB_Winner} Coach: {x.Coach_Winner} MVP: {x.MVP} Difference in Points = {x.Winning_Pts - x.Losin_Pts}             "));//extra space for formatting
                sbList.ForEach(x => writer.WriteLine($"The Winner was {x.Winner} Year: {x.Date} Winning QB: {x.QB_Winner} Coach: {x.Coach_Winner} MVP: {x.MVP} Difference in Points = {x.Winning_Pts - x.Losin_Pts}             "));

                //List of the top 5 attended superbowls
                Console.WriteLine("\nThe Top 5 attended Super Bowls\n");
                writer.WriteLine("\nThe Top 5 attended Super Bowls\n");
                var top5Query = (from SB_Info in sbList
                                 orderby SB_Info.Attendance descending
                                 select SB_Info).Take(5);
                top5Query.ToList().ForEach(s => Console.WriteLine($"{s.Attendance} attended Super Bowl {s.SB} on {s.Date}  {s.Winner} won  {s.Loser} lost. Game was held at {s.City}, {s.State} in {s.Stadium}"));
                top5Query.ToList().ForEach(s => writer.WriteLine($"{s.Attendance} attended Super Bowl {s.SB} on {s.Date}  {s.Winner} won  {s.Loser} lost. Game was held at {s.City}, {s.State} in {s.Stadium}"));

                //State that hosted the most super bowls
                var stateMostQuery = (from SB_Info in sbList
                                      group SB_Info by SB_Info.State into nestedQuery
                                      orderby nestedQuery.Count() descending
                                      select nestedQuery).First().Count();

                var qryState = from SB_Info in sbList
                               group SB_Info by SB_Info.State into stateGroups
                               where stateGroups.Count() == stateMostQuery
                               select stateGroups;
                foreach (var outerGroup in qryState)
                {
                    Console.WriteLine($"\nThe state that has hosted the most Super Bowls is {outerGroup.Key}: \n");
                    writer.WriteLine($"\nThe state that has hosted the most superbowls is {outerGroup.Key}: \n");
                    foreach (var MostGroup in qryState)
                    {
                        foreach (var detail in MostGroup)
                        {
                            Console.WriteLine($"Super Bowl {detail.SB} { detail.City}, {detail.State} at {detail.Stadium}");
                            writer.WriteLine($"Super Bowl {detail.SB} { detail.City}, {detail.State} at {detail.Stadium}");
                        }
                    }
                }

                //MPV Winners
                Console.WriteLine("\nThe Players who have earned MVP more than once are:");
                writer.WriteLine("\nThe Players who have earned MVP more than once are:");
                var mvpQuery = from SB_Info in sbList
                               group SB_Info by SB_Info.MVP into MVPGroup
                               where MVPGroup.Count() > 1
                               orderby MVPGroup.Key
                               select MVPGroup;
                foreach (var MVPGroup in mvpQuery)
                {
                    Console.WriteLine($"\n{MVPGroup.Key} with {MVPGroup.Count()}.");
                    writer.WriteLine($"\n{MVPGroup.Key} with {MVPGroup.Count()}.");
                    foreach (var SB_Info in MVPGroup)
                    {
                        Console.WriteLine($"The winner at Super Bowl {SB_Info.SB} was the {SB_Info.Winner} and the loser was the {SB_Info.Loser}");
                        writer.WriteLine($"The winner at Super Bowl {SB_Info.SB} was the {SB_Info.Winner} and the loser was the {SB_Info.Loser}");
                    }
                }

                //Coach(s) That Lost the most super bowls
                Console.WriteLine("\nThe coaches that have lost the most Super Bowls are:\n");
                writer.WriteLine("\nThe coaches that have lost the most Super Bowls are:\n");
                var CoachLostQuery = from SB_Info in sbList
                                    .GroupBy(SB_Info => SB_Info.Coach_Loser)
                                     select new
                                     {
                                         SB_Info.Key,
                                         Most = SB_Info.Count()
                                     };
                foreach (var SB_Info in CoachLostQuery)
                {
                    if (SB_Info.Most == CoachLostQuery.Max(x => x.Most))
                    {
                        Console.WriteLine($"{SB_Info.Key} has lost {SB_Info.Most}");
                        writer.WriteLine($"{SB_Info.Key} has lost {SB_Info.Most}");
                    }
                }

                //Coach(s) That won the most super bowls
                Console.WriteLine("\nThe coaches that have won the most Super Bowls are:\n");
                writer.WriteLine("\nThe coaches that have won the most Super Bowls are:\n");
                var CoachWonQuery = from SB_Info in sbList
                                    .GroupBy(SB_Info => SB_Info.Coach_Winner)
                                    select new
                                    {
                                        SB_Info.Key,
                                        Most = SB_Info.Count()
                                    };
                foreach (var SB_Info in CoachWonQuery)
                {
                    if (SB_Info.Most == CoachWonQuery.Max(x => x.Most))
                    {
                        Console.WriteLine($"{SB_Info.Key} has won {SB_Info.Most}");
                        writer.WriteLine($"{SB_Info.Key} has won {SB_Info.Most}");
                    }
                }

                //Team(s) that lost the most super bowls
                Console.WriteLine("\nThe teams that have lost the most Super Bowls are:\n");
                writer.WriteLine("\nThe teams that have lost the most Super Bowls are:\n");
                var TeamLostQuery = from SB_Info in sbList
                                    .GroupBy(SB_Info => SB_Info.Loser)
                                    select new
                                    {
                                        SB_Info.Key,
                                        Most = SB_Info.Count()
                                    };
                foreach (var SB_Info in TeamLostQuery)
                {
                    if (SB_Info.Most == TeamLostQuery.Max(x => x.Most))
                    {
                        Console.WriteLine($"{SB_Info.Key} have Lost {SB_Info.Most}");
                        writer.WriteLine($"{SB_Info.Key} have Lost {SB_Info.Most}");
                    }
                }

                //Team(s) that won the most super bowls
                Console.WriteLine("\nThe teams that have won the most Super Bowls are:\n");
                writer.WriteLine("\nThe teams that have won the most Super Bowls are:\n");
                var TeamWonQuery = from SB_Info in sbList
                                    .GroupBy(SB_Info => SB_Info.Winner)
                                   select new
                                   {
                                       SB_Info.Key,
                                       Most = SB_Info.Count()
                                   };
                foreach (var SB_Info in TeamWonQuery)
                {
                    if (SB_Info.Most == TeamWonQuery.Max(x => x.Most))
                    {
                        Console.WriteLine($"{SB_Info.Key} have won {SB_Info.Most}");
                        writer.WriteLine($"{SB_Info.Key} have won {SB_Info.Most}");
                    }
                }

                //Greatest point differnce 
                var pointDifQuery = (from SB_Info in sbList
                                     let dif = SB_Info.Winning_Pts - SB_Info.Losin_Pts
                                     group new { SB_Info.SB } by dif into SBgroup
                                     orderby SBgroup.Key
                                     select SBgroup);
                foreach (var SBgroup in pointDifQuery)
                {
                    if (SBgroup.Key == pointDifQuery.Max(x => x.Key))
                    {
                        foreach (var SB_Info in SBgroup)
                        {
                            Console.WriteLine($"\nThe largest Point difference was { SBgroup.Key} at SuperBowl {SB_Info.SB}");
                            writer.WriteLine($"\nThe largest Point difference was { SBgroup.Key} at SuperBowl {SB_Info.SB}");
                        }
                    }
                }

                //Average Attendance for all Super Bowls
                var averageQuery = (from SB_Info in sbList
                                    select SB_Info.Attendance).Average();
                double average = averageQuery;
                Console.WriteLine($"\nAverage attendance for all Super Bowls is {average}\n");
                writer.WriteLine($"\nAverage attendance for all Super Bowls is {average}\n");
            }
        }
    }
    class SB_Info
    {
        public string Date { get; set; }
        public string SB { get; set; }
        public int Attendance { get; set; }
        public string QB_Winner { get; set; }
        public string Coach_Winner { get; set; }
        public string Winner { get; set; }
        public int Winning_Pts { get; set; }
        public string QB_Loser { get; set; }
        public string Coach_Loser { get; set; }
        public string Loser { get; set; }
        public int Losin_Pts { get; set; }
        public string MVP { get; set; }
        public string Stadium { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public SB_Info(string date, string sb, int attendance, string qB_winner, string coach_winner, string winner, int winning_pts,
            string qB_loser, string coach_loser, string loser, int losing_pts, string mvp, string stadium, string city, string state)
        {
            this.Date = date;
            this.SB = sb;
            this.Attendance = attendance;
            this.QB_Winner = qB_winner;
            this.Coach_Winner = coach_winner;
            this.Winner = winner;
            this.Winning_Pts = winning_pts;
            this.QB_Loser = qB_loser;
            this.Coach_Loser = coach_loser;
            this.Loser = loser;
            this.Losin_Pts = losing_pts;
            this.MVP = mvp;
            this.Stadium = stadium;
            this.City = city;
            this.State = state;
        }

        public override string ToString() //ToString for displying sb info
        {
            return String.Format($"Date: {Date}, SB: {SB}, Attendance: {Attendance}, QB Winner: {QB_Winner}, Coach Winner: {Coach_Winner}, Winner: {Winner}, " +
                $"QB Loser: {QB_Loser}, Coach Loser: {Coach_Loser}, Loser: {Loser}, Losing_Pts: {Losin_Pts}, MVP: {MVP}, Stadium: {Stadium}, City: {City}, State: {State}");
        }

    }
}
