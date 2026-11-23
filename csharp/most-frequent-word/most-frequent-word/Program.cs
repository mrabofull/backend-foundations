using System;

namespace MostFrequentWord
{
    internal class Program
    {
        static string MostFrequentWord(string word)
        {
            string newWord = word.ToLower();
            //string[] split = newWord.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string[] split = newWord.Split(' ');

            //Hashtable ht = new Hashtable();
            Dictionary<string, int> sub = new Dictionary<string, int>();

            foreach (string s in split)
            {
                //if (ht.ContainsKey(s))
                //{
                //    ht[s] = (int)ht[s] + 1;
                //}
                //else
                //{
                //    ht.Add(s, 1);
                //}
                if (sub.ContainsKey(s))
                {
                    sub[s] += 1;
                }
                else
                {
                    sub.Add(s, 1);
                }
            }

            string maxStr = split[0];
            //foreach (string s in ht.Keys)
            //{
            //    if ((int)ht[s] > (int)ht[maxStr])
            //    {
            //        maxStr = s;
            //    }
            //}

            foreach (string s in sub.Keys)
            {
                if (sub[s] > sub[maxStr])
                {
                    maxStr = s;
                }
            }
            return maxStr;
        }

        static void Main(string[] args)
        {
            // Write a function that takes a string and returns the most
            // frequent word, ignoring case and punctuation.
            Console.WriteLine(MostFrequentWord("hi hello wow wow hi hi"));
            Console.WriteLine(MostFrequentWord("hi wow hello wow wow hi hi"));
            Console.WriteLine(MostFrequentWord("Wow Hi  hEllo wow wow hi hi"));
            Console.WriteLine(MostFrequentWord("cat dog bird"));
            Console.WriteLine(MostFrequentWord("Java java JAVA python Python"));
        }
    }
}
