using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GabenSimulator2013
{
    static class NameGenerator
    {
        public enum NicknameMode
        {
            Always,
            Never,
            Sometimes,
            Half
        }

        static List<string> FirstNames;
        static List<string> Nicknames;
        static List<string> Surnames;

        static Random random = new Random();

        static NicknameMode _NicknameMode;

        static NameGenerator()
        {
            FirstNames = new List<string>();
            Nicknames = new List<string>();
            Surnames = new List<string>();

            LoadNames();
        }

        public static void SetNicknameMode(NicknameMode mode)
        {
            _NicknameMode = mode;
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
            if (nick.Contains("[FIRST]"))
            {
                string[] splits =  nick.Split('[', ']');
                splits[1] = first;
                nick = string.Join("", splits);                
            }

            string last = Surnames[random.Next(Surnames.Count)];

            string final;

            switch (_NicknameMode)
            {
                case NicknameMode.Always:
                    final =  first + " \"" + nick + "\" " + last;
                    break;
                    
                case NicknameMode.Never:
                    final = first + " " + last;
                    break;

                case NicknameMode.Sometimes:
                    if (random.NextDouble() > 0.8f)
                        final = first + " \"" + nick + "\" " + last;
                    else
                        final = first + " " + last;
                    break;
                    
                case NicknameMode.Half:
                    if (random.NextDouble() > 0.5f)
                        final = first + " \"" + nick + "\" " + last;
                    else
                        final = first + " " + last;
                    break;

                default:
                    final = first +" \"" + nick + "\" " + last;
                    break;
                    
            }
            if (final.Length > 25)
            {
                string[] splits = final.Split(' ');
                int splitpoint = splits.Length / 2;
                splits[splitpoint] = splits[splitpoint].Insert(0, "\n");
                final = string.Join(" ", splits);
            }
            return final;
            
        }
    }
}
