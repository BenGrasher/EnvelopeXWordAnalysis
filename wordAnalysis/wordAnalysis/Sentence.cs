using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace wordAnalysis
{
    /// <summary>
    /// Holds all the words together, and manages the decoding trigger for the sentence.
    /// </summary>
    class Sentence
    {
        public List<Word> _sentence = new List<Word>();
        public Encoding encoding = new Encoding();

        // The candidate solutions in string form.
        public List<string> decodedSentences = new List<string>();
        
        // Progress trackers.
        int firstLevelStarted = 0;
        int firstLevelFinished = 0;
        int rootLevel = 0;
        int progressTotal = 0;

        public Sentence()
        {
        }

        /// <summary>
        /// Used when building the initial sentence.
        /// </summary>
        /// <param name="word"></param>
        public void AddWord(Word word)
        { 
            this._sentence.Add(word);
        }

        /// <summary>
        /// A thinning function.  Certain words cannot be translations using simple substitution from some encodings.
        /// "AAB" for example cannot be "BAR" and "ABC" cannot be "FOO".
        /// 
        /// This is a very basic thinning, it does not consider for example that "ABC" may already have some of it's letters encoded.  
        /// It's possible this could be improved, but it is important to not simply reimpliment the recursion, that shouldn't save any time.
        /// </summary>
        public void DetermineWordsThatMatchThisPattern()
        {
            foreach(Word w in this._sentence)
            {
                w.DetermineWordsThatMatchThisPattern();
            }
        }

        /// <summary>
        /// Entry point for the main recursion fucntion.
        /// </summary>
        public void TestAllPatterns()
        {
            this.rootLevel = this._sentence.FindIndex(w => w.ignored == false);
            this.progressTotal = this._sentence[this.rootLevel].wordsThatMatchThisPattern.Count;
            this.RecursivelyTestAllEncodings(0, this.encoding);
        }

        private void RecursivelyTestAllEncodings(int level, Encoding testEncoding)
        {
            // Keeps track of which word we are looking at currently.
            int nextLevel = level + 1;

            // If we are at the end of the sentence.
            if(level >= this._sentence.Count)
            {
                // Store the decoded sentences (makes for easy debugging)
                this.decodedSentences.Add(testEncoding.Decode(this));
            }
            else
            {
                List<Encoding> encodingsThatMightWork = new List<Encoding>();

                if (this._sentence[level].ignored)
                {
                    // just pass this encoding down to the next level.
                    encodingsThatMightWork.Add(testEncoding);
                }
                else
                {
                   encodingsThatMightWork = this._sentence[level].EncodingsThatMatch(testEncoding);
                }

                // If we are on the root/first level.  This code doesn't really do anything different than what we do below, but it does:
                // 1) Limit the parallelization of this recursion.
                // 2) Allow us to "track progress".  (though not all words are created equal)
                if(level == this.rootLevel)
                {
                    Parallel.ForEach(encodingsThatMightWork, e =>
                    {
                        this.firstLevelStarted += 1;
                        this.RecursivelyTestAllEncodings(nextLevel, e);
                        this.firstLevelFinished += 1;
                    });
                }
                else
                {
                    foreach (Encoding e in encodingsThatMightWork)
                    {
                        this.RecursivelyTestAllEncodings(nextLevel, e);
                    }
                }
            }

            return;
        }
    }
}
