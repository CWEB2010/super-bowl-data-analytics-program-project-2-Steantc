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
            /**Your application should allow the end user to pass end a file path for output 
            * or guide them through generating the file.
            **/
            //string Date;
            //string SB;
            //int Attendance;
            //string QB_Winner;
            //string Coach_Winner;
            //string Winner;
            //int Winning_Pts;
            //string QB_Loser;
            //string Coach_Loser;
            //string Loser;
            //int Losin_Pts;
            //string MVP;
            //string Stadium;
            //string City;
            //string State;

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

            
            Console.WriteLine
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}

            //sbList.ForEach(x => Console.WriteLine(x.ToString()));

            //IEnumerable<SB_Info> 
            //var top5Query =
            //    from SB_Info in sbList
            //    select SB_Info.Attendance;
            
            //foreach (int topn in top5Query.Take(5))
            //{
            //    Console.WriteLine(topn);
            //}



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
