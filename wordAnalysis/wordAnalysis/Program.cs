using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Runtime.CompilerServices;

namespace wordAnalysis
{
    class Program
    {
        static Sentence sentenceToDecode = new Sentence();
        public static string fileName = @".\IgnoreNumberAndSeventhWord.txt";

        static void Main(string[] args)
        {
            SetupSentenceToBeDecoded();

            WordUtils.Initialize();

            sentenceToDecode.DetermineWordsThatMatchThisPattern();

            sentenceToDecode.TestAllPatterns();

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                foreach (string sentence in sentenceToDecode.decodedSentences)
                {
                    Console.WriteLine(sentence);
                    sw.WriteLine(sentence);
                }
            }
        }

        /// <summary>
        /// The english-character translated form of the words, from the wingdings-like form of the encoded characters.
        /// The "A" here is a simplification of that first character, not an actual A, though clearly (if this is simple substitution) a arbitrary translation of whatever
        /// the author intended the substitution to be to some other encoding does not "encode it any more" or cause it to be any less valid.
        /// 
        /// Due to the arbitrary nature of this encoding, this could in theory map A=A (not allowed in traditional cryptography puzzles)
        /// 
        /// A methodolgoy of solving this as a "6 word" puzzle is to simply comment out the word you don't want to consider.  We could instead do this at a higher level,
        /// which would allow us to decode something like a name automatically, since it might actually translate into a human-readable name.
        /// </summary>
        public static void SetupSentenceToBeDecoded()
        {
            sentenceToDecode.AddWord(new Word("ABCD"));
            sentenceToDecode.AddWord(new Word("ECFFGH"));
            sentenceToDecode.AddWord(new Word("IJKB"));
            sentenceToDecode.AddWord(new Word("LK"));
            sentenceToDecode.AddWord(new Word("PM", true)); // <== the likely number.
            sentenceToDecode.AddWord(new Word("JLCNC"));
            sentenceToDecode.AddWord(new Word("JGO",true));
        }
    }
}
