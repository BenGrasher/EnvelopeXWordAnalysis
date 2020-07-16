using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace wordAnalysis
{
    /// <summary>
    /// Pulls words from the metadata files, and stores them as lists to be iterated through.
    /// </summary>
    public static class WordUtils
    {
        public static List<Word> twoLetterWords = new List<Word>();
        public static List<Word> threeLetterWords = new List<Word>();
        public static List<Word> fourLetterWords = new List<Word>();
        public static List<Word> fiveLetterWords = new List<Word>();
        public static List<Word> sixLetterWordsWithDoubles = new List<Word>();

        static bool initialized = false;

        // Would be nice if this was relative, my "." efforts did not work.
        // Switching "RealWords" to "Words" swaps between the culled (realwords) dictionary and the full (words) dictionary.
        public static string rootPath = @"C:\Users\bgrasher.REDMOND\source\repos\wordAnalysis\wordAnalysis\RealWords\";

        public static void Initialize()
        {
            PopulateWordList(rootPath + "2LetterWords.txt", twoLetterWords);
            PopulateWordList(rootPath + "3LetterWords.txt", threeLetterWords);
            PopulateWordList(rootPath + "4LetterWords.txt", fourLetterWords);
            PopulateWordList(rootPath + "5LetterWords.txt", fiveLetterWords);
            PopulateWordList(rootPath + "6LetterWords-WithDoubleLetters.txt", sixLetterWordsWithDoubles);

            initialized = true;
        }

        public static List<Word> GetWordsOfLength(int length)
        {
            if(!initialized)
            {
                Initialize();
            }

            switch (length)
            {
                case 2:
                    return twoLetterWords;
                case 3:
                    return threeLetterWords;
                case 4:
                    return fourLetterWords;
                case 5:
                    return fiveLetterWords;
                case 6:
                    return sixLetterWordsWithDoubles;
                default:
                    throw new ArgumentOutOfRangeException("only lengths from 2 to 6 inclusive are allowed");
            }
        }

        /// <summary>
        /// Handles the actual import of the word dictionary metadata files.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="wordList"></param>
        private static void PopulateWordList(string path, List<Word> wordList)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                while (!sr.EndOfStream)
                {
                    wordList.Add(new Word(sr.ReadLine()));
                }
            }
        }
    }
}
