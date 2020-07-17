using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace wordAnalysis
{
    /// <summary>
    /// Stores the letters in the words, and the logic for whether an encoding could work for the word in question.
    /// </summary>
    public class Word
    {
        string _encodedWord = string.Empty;
        public List<Word> wordsThatMatchThisPattern;
        public bool ignored;

        public Word(string encodedWord, bool ignored = false)
        {
            this._encodedWord = encodedWord;
            this.ignored = ignored;
        }

        public string GetWord()
        {
            return this._encodedWord;
        }

        /// <summary>
        /// Note, this will only function on 10 or less letter words.
        /// </summary>
        /// <returns></returns>
        public string BasicPattern()
        {
            string pattern = string.Empty;
            string word = this.GetWord();
            

            foreach(char letter in word)
            {
                pattern += (word.IndexOf(letter));
            }

            return pattern;
        }

        /// <summary>
        /// Extremely simple pattern matching.  Used to give a baseline for iteration and the more complex comparisons.
        /// </summary>
        public void DetermineWordsThatMatchThisPattern()
        {
            List<Word> allWords = WordUtils.GetWordsOfLength(this.GetWord().Length);

            this.wordsThatMatchThisPattern = allWords.Where(w => w.BasicPattern() == this.BasicPattern()).ToList<Word>();
        }

        /// <summary>
        /// The more complex pattern matching.  Given an encoding, could a word be the decoding of the currently encoded word.  And what are all these words.
        /// Drives the main recursion.
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public List<Encoding> EncodingsThatMatch(Encoding encoding)
        {
            List<Encoding> encodings = new List<Encoding>();

            // for each potential word
            foreach(Word w in this.wordsThatMatchThisPattern)
            {
                Encoding testEncoding = (Encoding)encoding.Clone();

                bool allLettersWork = true;

                // for each letter in the word.
                for(int i = 0; i < this._encodedWord.Length; i++)
                {
                    string encodedLetter = this.GetWord()[i].ToString();
                    string candidateDecodedLetter = w.GetWord()[i].ToString();

                    // if the encoding can't work
                    if (!testEncoding.CouldEncodingWork(encodedLetter, candidateDecodedLetter))
                    {
                        // stop trying to encode this and go to the next candidate
                        allLettersWork = false;
                        break;
                    }
                    else // if the encoding can work, set the value.
                    {
                        testEncoding.SetValueFor(encodedLetter, candidateDecodedLetter);
                    }
                }

                // Once we test all the letters for this word, add this word to the candidates for recursion if all tests passed.
                if(allLettersWork)
                {
                    encodings.Add(testEncoding);
                }
            }

            return encodings;
        }
    }
}
