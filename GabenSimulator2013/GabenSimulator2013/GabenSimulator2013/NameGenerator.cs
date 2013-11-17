using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GabenSimulator2013
{
    static class NameGenerator
    {
        static List<string> FirstNames;
        static List<string> Nicknames;
        static List<string> Surnames;

        static Random random = new Random();

        static NameGenerator()
        {
            FirstNames = new List<string>();
            Nicknames = new List<string>();
            Surnames = new List<string>();

            LoadNames();
        }

        private static void LoadNames()
        {
            StreamReader reader = new StreamReader("FirstNames.txt");
            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    FirstNames.Add(reader.ReadLine());
                }
            }
            reader = new StreamReader("Nicknames.txt");
            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    Nicknames.Add(reader.ReadLine());
                }
            }
            reader = new StreamReader("Surnames.txt");
            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    Surnames.Add(reader.ReadLine());
                }
            }
        }

        public static string GetName()
        {            
            string first = FirstNames[random.Next(FirstNames.Count)];
            string nick = Nicknames[random.Next(Nicknames.Count)];
            string last = Surnames[random.Next(Surnames.Count)];

            return first + " \"" + nick + "\" " + last;
        }
    }
}
