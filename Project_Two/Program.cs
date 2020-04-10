using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.Runtime.Serialization;

namespace Project_Two
{
    class Program
    {
        private static object mvpQuery;

        static void Main(string[] args)
        {

            const string PATH = @"C:\Users\steantc\Documents\CWEB2010\Project2\super-bowl-data-analytics-program-project-2-Steantc\Super_Bowl_Project.csv";

            //FileStream input;
            //StreamReader read;
            string line;
            string[] data;
            List<SB_Info> sbList = null;

            //try
            //{
                FileStream input = new FileStream(PATH, FileMode.Open, FileAccess.Read);
                StreamReader read = new StreamReader(input);
                line = read.ReadLine(); //primer
                sbList = new List<SB_Info>();

                //Looping structure that's going to read in all of my records
                while (!read.EndOfStream)
                {
                    //Our objective is to read in the records and create object instances
                    data = read.ReadLine().Split(',');
                    sbList.Add(new SB_Info(data[0], data[1], Convert.ToInt32(data[2]), data[3], data[4], data[5], Convert.ToInt32(data[6]), 
                        data[7], data[8], data[9], Convert.ToInt32(data[10]), data[11], data[12], data[13], data[14]));
                    //Console.WriteLine(sbList[sbList.Count - 1]);

                    //line = read.ReadLine(); //primer


                }

                read.Close();
                input.Close();



            //Console.WriteLine
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //List of superbowl winners
            sbList.ForEach(x => Console.WriteLine($"The Winner was {x.Winner} Year: {x.Date} Winning QB: {x.QB_Winner} Coach: {x.Coach_Winner} MVP: {x.MVP} Difference in Points = {x.Winning_Pts - x.Losin_Pts}\n"));

            var top5Query = from SB_Info in sbList
                           group SB_Info by SB_Info.Attendance into AttendanceGroup
                           orderby AttendanceGroup.Key
                           select AttendanceGroup;
            foreach (var AttendaceGroup in top5Query)
            {
                if (AttendaceGroup.Key == top5Query.Max(x => x.Key))
                {
                    Console.WriteLine($"Top 5 {AttendaceGroup.Key}\n");
                }
            }

            //Greatest point diffence 
            var pointDifQuery = (from SB_Info in sbList
                                 let dif = SB_Info.Winning_Pts - SB_Info.Losin_Pts
                                 group new { SB_Info.SB } by dif into SBgroup
                                 orderby SBgroup.Key
                                 select SBgroup);
            foreach (var SBgroup in pointDifQuery)
            {
                if (SBgroup.Key == pointDifQuery.Max(x=>x.Key))
                {
                    Console.WriteLine($"The largest Point difference was {SBgroup.Key}");
                    foreach (var SB_Info in SBgroup)
                    {
                        Console.WriteLine($"at SuperBowl {SB_Info.SB}");
                    }
                }
            }

        //test for state

            var StateQuery = from SB_Info in sbList
                             group SB_Info by SB_Info.State into StateGroup
                             select new
                             {
                                 StateGroup.Key,
                                 Most = StateGroup.Count()
                             };
            foreach (var StateGroup in StateQuery)
            {
                if (StateGroup.Most == StateQuery.Max(x => x.Most))
                {
                    Console.WriteLine($"{StateGroup.Key} has hosted {StateGroup.Most}");
                    foreach (var SB_Info in StateGroup)
                    {
                        Console.WriteLine($"The stadium is {SB_Info.Stadium} in {SB_Info.City}");
                    }
                }
            }
            //The state that has hosted the most superbowls (Still needs to display City and stadium
            //var StateQuery = from SB_Info in sbList
            //                    .GroupBy(SB_Info => SB_Info.State)
            //                    select new
            //                    {
            //                        SB_Info.Key,
            //                        Most = SB_Info.Count()
            //                    };
            //foreach (var SB_Info in StateQuery)
            //{
            //    if (SB_Info.Most == StateQuery.Max(x => x.Most))
            //    {
            //        Console.WriteLine($"{SB_Info.Key} has hosted {SB_Info.Most}\n");
            //    }
            //}
            //MPV Winners (still needs to show winning team and loseing team)
            var mvpQuery = from SB_Info in sbList
                           group SB_Info by SB_Info.MVP into MVPGroup
                           where MVPGroup.Count() > 1
                           orderby MVPGroup.Key
                           select MVPGroup;
            foreach (var MVPGroup in mvpQuery)
            {
                Console.WriteLine($"{MVPGroup.Key} has {MVPGroup.Count()}.");
                foreach (var SB_Info in MVPGroup)
                {
                    Console.WriteLine($"The winner at SuperBowl {SB_Info.SB} was {SB_Info.Winner} and the loser was {SB_Info.Loser}");
                }
            }

            //Coach(s) That Lost the most superbowls
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
                    Console.WriteLine($"{SB_Info.Key} lost {SB_Info.Most}\n");
                }
            }

            //Coach(s) That won the most superbowls
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
                    Console.WriteLine($"{SB_Info.Key} won {SB_Info.Most}\n");
                }
            }
            //Team(s) that lost the most superbowls
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
                    Console.WriteLine($"{SB_Info.Key} Lost {SB_Info.Most}\n");
                }
            }
            //Team(s) that won the most superbowls
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
                    Console.WriteLine($"{SB_Info.Key} won {SB_Info.Most}\n");
                }
            }
            //Average Attendance for all Super Bowls
            var averageQuery = (from SB_Info in sbList
                                select SB_Info.Attendance).Average();
            double average = averageQuery;
            Console.WriteLine($"Average Attendance is {average}\n");

            //foreach (int topn in top5Query.Max()) 
            //{
            //    Console.WriteLine(topn);
            //}
            //var top5Query = sbList.Where<SB_Info>(from SB_Info.)



        }
    }
    //[Serializable]
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
            this.QB_Winner = QB_Winner;
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
