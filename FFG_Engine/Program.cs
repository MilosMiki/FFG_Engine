using System;

namespace FFG_Engine 
{
    class Team : IComparable<Team>
    {
        private int quali;
        private int race;
        private int id;
        private double value; //quali with a 0,6 multiplier, and race with a 1,0
        private int tier;
        private int cof;
        public Team(int q, int r, int c, int i, int tier)
        {
            this.quali = q;
            this.race = r;
            this.id = i;
            this.cof = c;
            this.value = q * 0.6 + r + ((c/100)*1.33)*-0.3;
            this.tier = tier;
        }

        public int Id { get => id; set => id = value; }
        public double Value { get => value; set => this.value = value; }
        public int Quali { get => quali; set => quali = value; }
        public int Race { get => race; set => race = value; }
        public int Tier { get => tier; set => tier = value; }
        public int Cof { get => cof; set => cof = value; }

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
            //if the player's engine pick would be within 3hp of the best engine, for the particular instance of FFG
            //using a 0.6x multiplier for quali HP, and 1.0 for race HP
            int num3hpT1 = 0;
            int num5hpT1 = 0;
            int num3hpT2 = 0;
            //if the player's engine pick would be within 5hp of the best engine, for the particular instance of FFG
            int num5hpT2 = 0;
            int num3hpT3 = 0;
            int num5hpT3 = 0;
            //shows if the player's pick would have been the best engine of that season, only one engine per season is added
            //so if both T1+T2 are better than all other engines, then it will only add to either T1/T2 depending on which is better
            int numBestT1 = 0;
            int numBestT2 = 0;
            int numBestT3 = 0;
            //shows the best pick for the player, for the particular instance of FFG
            int T1better = 0;
            int T2better = 0;
            int T3better = 0;
            //shows the best engines for the particular instance of FFG
            int numBestT1Overall = 0;
            int numBestT2Overall = 0;
            int numBestT3Overall = 0;

            Random r = new Random();

            for (int i = 0; i < numIterations; i++)
            {
                //indexes 0-9 - other player engines (4*tier 1, 4*tier 2,2*tier3)
                //index 10 - player tier 1 engine
                //index 11 - player tier 2 engine
                //calculations to be done separately, but using the same data for other teams
                List<Team> teams = new List<Team>();
                //Team[] teams = new Team[12];
                for (int j = 0; j < 5; j++)
                {
                    teams.Add(new Team(r.Next(781, 786), r.Next(781, 786), r.Next(4000, 4800), j == 4 ? 10 : j, 1));
                    //remove -1 from race HP to account for $200 price difference between T1 and T2/T3
                    //currently NOT removed
                }
                for (int j = 4; j < 9; j++)
                {
                    teams.Add(new Team(r.Next(775, 790), r.Next(775, 790),r.Next(3800, 5500), j == 8 ? 11 : j, 2));
                }
                for (int j = 8; j < 11; j++)
                {
                    teams.Add(new Team(r.Next(770, 790), r.Next(770, 790), r.Next(4600, 6000), j == 10 ? 12 : j, 3));
                    //add +1 to quali HP to account for price difference between T1&T2/T3
                    //will not be 100% accurate, but as close as you can get
                    //currently NOT added
                }
                teams.Sort();
                double bestValue = 0;
                bool foundOne = false;
                bool foundTwo = false;
                for (int j = 0; j < 12; j++)
                {
                    if (teams[j] == null) continue;
                    if (j == 0)
                    {
                        switch (teams[j].Tier)
                        {
                            case 1: numBestT1Overall++; break;
                            case 2: numBestT2Overall++; break;
                            case 3: numBestT3Overall++; break;
                        }
                        bestValue = teams[j].Value;
                        Console.WriteLine("Best: {0},{1},{2},{3}", teams[j].Quali, teams[j].Race, teams[j].Cof, (int)teams[j].Value);
                    }
                    if (teams[j].Id == 10) //id = player tier 1
                    {
                        Console.WriteLine("T1: {0},{1},{2},{3}", teams[j].Quali, teams[j].Race, teams[j].Cof, (int)teams[j].Value);
                        double gap = bestValue - teams[j].Value;
                        if (gap < 5)
                            num5hpT1++;
                        if (gap < 3)
                            num3hpT1++;
                        if (j == 0)
                            numBestT1++;
                        if (foundOne)
                        {
                            if (foundTwo) break;
                            else foundTwo = true;
                        }
                        else
                        {
                            foundOne = true;
                            T1better++;
                        }
                    }
                    if (teams[j].Id == 11) //id = player tier 2
                    {
                        Console.WriteLine("T2: {0},{1},{2},{3}", teams[j].Quali, teams[j].Race, teams[j].Cof, (int)teams[j].Value);
                        double gap = bestValue - teams[j].Value;
                        if (gap < 5)
                            num5hpT2++;
                        if (gap < 3)
                            num3hpT2++;
                        if (j == 0)
                            numBestT2++;
                        if (foundOne)
                        {
                            if (foundTwo) break;
                            else foundTwo = true;
                        }
                        else
                        {
                            foundOne = true;
                            T2better++;
                        }
                    }
                    if (teams[j].Id == 12) //id = player tier 3
                    {
                        Console.WriteLine("T3: {0},{1},{2},{3}", teams[j].Quali, teams[j].Race, teams[j].Cof, (int)teams[j].Value);
                        double gap = bestValue - teams[j].Value;
                        if (gap < 5)
                            num5hpT3++;
                        if (gap < 3)
                            num3hpT3++;
                        if (j == 0)
                            numBestT3++;
                        if (foundOne)
                        {
                            if (foundTwo) break;
                            else foundTwo = true;
                        }
                        else
                        {
                            foundOne = true;
                            T3better++;
                        }
                    }
                }
            }

            Console.WriteLine("\n\nResult after 5000 seasons of FFG\n");
            Console.WriteLine("Times T1 best overall: {0} ({1}%)", numBestT1Overall, numBestT1Overall * 100 / numIterations);
            Console.WriteLine("Times T2 best overall: {0} ({1}%)", numBestT2Overall, numBestT2Overall * 100 / numIterations);
            Console.WriteLine("Times T3 best overall: {0} ({1}%)", numBestT3Overall, numBestT3Overall * 100 / numIterations);
            Console.WriteLine("The engines are split 4/4/2 for tiers 1,2,3\nrespectively, including the player's best pick");
            Console.WriteLine();
            Console.WriteLine("Times player T1 best on the grid: {0} ({1}%)", numBestT1, numBestT1 * 100 / numIterations);
            Console.WriteLine("Times player T2 best on the grid: {0} ({1}%)", numBestT2, numBestT2 * 100 / numIterations);
            Console.WriteLine("Times player T3 best on the grid: {0} ({1}%)", numBestT3, numBestT3 * 100 / numIterations);
            Console.WriteLine();
            Console.WriteLine("Times T1 within 3hp: {0} ({1}%)", num3hpT1, num3hpT1 * 100 / numIterations);
            Console.WriteLine("Times T2 within 3hp: {0} ({1}%)", num3hpT2, num3hpT2 * 100 / numIterations);
            Console.WriteLine("Times T3 within 3hp: {0} ({1}%)", num3hpT3, num3hpT3 * 100 / numIterations);
            Console.WriteLine();
            Console.WriteLine("Times T1 within 5hp: {0} ({1}%)", num5hpT1, num5hpT1 * 100 / numIterations);
            Console.WriteLine("Times T2 within 5hp: {0} ({1}%)", num5hpT2, num5hpT2 * 100 / numIterations);
            Console.WriteLine("Times T3 within 5hp: {0} ({1}%)", num5hpT3, num5hpT3 * 100 / numIterations);
            Console.WriteLine();
            Console.WriteLine("Times T1 best choice: {0} ({1}%)", T1better, T1better * 100 / numIterations);
            Console.WriteLine("Times T2 best choice: {0} ({1}%)", T2better, T2better * 100 / numIterations);
            Console.WriteLine("Times T3 best choice: {0} ({1}%)", T3better, T3better * 100 / numIterations);
            Console.ReadKey();
        }
    }
}
