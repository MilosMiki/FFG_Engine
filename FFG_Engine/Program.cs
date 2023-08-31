using System;

namespace FFG_Engine 
{
    class Team : IComparable<Team>
    {
        private int quali;
        private int race;
        private int id;
        private double value; //quali with a 0,6 multiplier, and race with a 1,0
        public Team(int q, int r, int i)
        {
            this.quali = q;
            this.race = r;
            this.id = i;
            this.value = q * 0.6 + r;
        }

        public int Id { get => id; set => id = value; }
        public double Value { get => value; set => this.value = value; }
        public int Quali { get => quali; set => quali = value; }
        public int Race { get => race; set => race = value; }

        public int CompareTo(Team? t)
        {
            if (t == null) return 0;
            int ret = -1;
            if (value > t.value)
                ret = -1;
            else if (value < t.value)
                ret = 1;
            else if (value == t.value)
                ret = 0;
            return ret;
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            int numIterations = 5000;
            int num3hpT1 = 0;
            int num5hpT1 = 0;
            int num3hpT2 = 0;
            int num5hpT2 = 0;
            int numBestT1 = 0;
            int numBestT2 = 0;
            int T1better = 0;
            int T2better = 0;

            Random r = new Random();

            for (int i = 0; i < numIterations; i++)
            {
                //indexes 0-9 - other player engines (5*tier 1, 5*tier 2)
                //index 10 - player tier 1 engine
                //index 11 - player tier 2 engine
                //calculations to be done separately, but using the same data for other teams
                List<Team> teams = new List<Team>();
                //Team[] teams = new Team[12];
                for (int j = 0; j < 6; j++)
                {
                    teams.Add(new Team(r.Next(781, 786), r.Next(781, 786), j == 5 ? 10 : j));
                }
                for (int j = 5; j < 10; j++)
                {
                    teams.Add(new Team(r.Next(775, 790), r.Next(775, 790), j == 9 ? 11 : j)); 
                    //add +1 to race HP to account for $200 price difference between T1 and T2
                    //currently NOT added
                }
                teams.Sort();
                double bestValue = 0;
                bool foundOne = false;
                for (int j = 0; j < 12; j++)
                {
                    if (teams[j] == null) continue;
                    if (j == 0)
                    {
                        bestValue = teams[j].Value;
                        Console.WriteLine("Best: {0},{1},{2}", teams[j].Quali, teams[j].Race, teams[j].Value);
                    }
                    if (teams[j].Id == 10) //id = player tier 1
                    {
                        Console.WriteLine("T1: {0},{1},{2}",teams[j].Quali, teams[j].Race, teams[j].Value);
                        double gap = bestValue - teams[j].Value;
                        if (gap < 5)
                            num5hpT1++;
                        if (gap < 3)
                            num3hpT1++;
                        if (j == 0)
                            numBestT1++;
                        if (foundOne)
                            break;
                        else
                        {
                            foundOne = true;
                            T1better++;
                        }
                    }
                    if (teams[j].Id == 11) //id = player tier 2
                    {
                        Console.WriteLine("T2: {0},{1},{2}", teams[j].Quali, teams[j].Race, teams[j].Value);
                        double gap = bestValue - teams[j].Value;
                        if (gap < 5)
                            num5hpT2++;
                        if (gap < 3)
                            num3hpT2++;
                        if (j == 0)
                            numBestT2++;
                        if (foundOne)
                            break;
                        else
                        {
                            foundOne = true;
                            T2better++;
                        }
                    }
                }
            }

            Console.WriteLine("\n\nResult after 5000 seasons of FFG\n");
            Console.WriteLine("Times T1 best: {0} ({1}%)", numBestT1, numBestT1 * 100 / numIterations);
            Console.WriteLine("Times T2 best: {0} ({1}%)", numBestT2, numBestT2 * 100 / numIterations);
            Console.WriteLine();
            Console.WriteLine("Times T1 within 3hp: {0} ({1}%)", num3hpT1, num3hpT1 * 100 / numIterations);
            Console.WriteLine("Times T2 within 3hp: {0} ({1}%)", num3hpT2, num3hpT2 * 100 / numIterations);
            Console.WriteLine();
            Console.WriteLine("Times T1 within 5hp: {0} ({1}%)", num5hpT1, num5hpT1 * 100 / numIterations);
            Console.WriteLine("Times T2 within 5hp: {0} ({1}%)", num5hpT2, num5hpT2 * 100 / numIterations);
            Console.WriteLine();
            Console.WriteLine("Times T1 better than T2: {0} ({1}%)", T1better, T1better * 100 / numIterations);
            Console.WriteLine("Times T2 better than T1: {0} ({1}%)", T2better, T2better * 100 / numIterations);
            Console.ReadKey();
        }
    }
}
