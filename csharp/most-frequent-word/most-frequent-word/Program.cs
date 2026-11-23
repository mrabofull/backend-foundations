using System;

namespace MostFrequentWord
{
    internal class Program
    {
        static string MostFrequentWord(string word)
        {
            if (string.IsNullOrWhiteSpace(word)) // null, empty strings,whitespace (spaces, tabs, newlines, etc.)
            {
                // string.IsNullOrEmpty(word) only empty strings and null
                return "";
            }

            string newWord = new string(word.ToLower().Where(c => !char.IsPunctuation(c)).ToArray());
            string[] split = newWord.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (split.Length == 0)
            {
                return "";
            }

            Dictionary<string, int> sub = new Dictionary<string, int>();

            foreach (string s in split)
            {
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
            Console.WriteLine(MostFrequentWord("hi hello wow wow hi hi")); // hi
            Console.WriteLine(MostFrequentWord("hi wow hello wow wow hi hi")); // hi
            Console.WriteLine(MostFrequentWord("Wow Hi  hEllo wow wow hi hi")); // wow
            Console.WriteLine(MostFrequentWord("cat dog bird")); // cat
            Console.WriteLine(MostFrequentWord("Java java JAVA python Python")); // java
            Console.WriteLine(MostFrequentWord("")); // ""
            Console.WriteLine(MostFrequentWord("Java java !JAVA python Python python? jAvA?")); // java
            Console.WriteLine(MostFrequentWord("?")); // ""
        }
    }
}
