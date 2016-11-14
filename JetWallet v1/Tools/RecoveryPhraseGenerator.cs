using JetWallet.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetWallet.Tools
{
    public class RecoveryPhraseGenerator
    {

        private static Random rnd = new Random();

        public string Generate()
        {
            const int PHRASE_LENGTH = 7;

            string[] wordList = LowercaseWords.lowercase_words.Split('\n');
            string phrase = string.Empty;
            for (int i = 0; i < PHRASE_LENGTH; i++)
            {
                string rndWord = wordList[rnd.Next(0, wordList.Length)];
                phrase += rndWord.Trim();
                if (i < 6)
                {
                    phrase += " ";
                }
            }
            return phrase;
        }
    }
}
