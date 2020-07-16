using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace wordAnalysis
{
    /// <summary>
    /// A data structure for storing the mapping of encoded to plaintext letters.
    /// </summary>
    public class Encoding : ICloneable
    {
        public Dictionary<string, string> EncodingDictionary = new Dictionary<string, string>();

        public Encoding()
        {
            this.EncodingDictionary = new Dictionary<string, string>();
            using (StreamReader sr = new StreamReader(@"C:\Users\bgrasher.REDMOND\source\repos\wordAnalysis\wordAnalysis\Metadata\BaselineEncoding.txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] keyValue = line.Split('\t');   // \t = tab.

                    if (keyValue != null && keyValue.Length == 2)
                    {
                        this.EncodingDictionary.Add(keyValue[0], keyValue[1]);
                    }
                }
            }
        }

        /// <summary>
        /// Critical, because otherwise we pass by reference, which is not great with all the parallel processing going on.
        /// </summary>
        /// <returns>A copy of the original encoding.</returns>
        public object Clone()
        {
            return new Encoding
            {
                EncodingDictionary = new Dictionary<string, string>(this.EncodingDictionary)
            };
        }

        /// <summary>
        /// Decodes the whole sentence, used for outputting the data (since we make no assumptions about one word vs another (syntax, grammer, etc) other
        /// than the encoding pattern ermains constant (a "simple substitution cypher")
        /// </summary>
        /// <param name="sentenceToDecode">A Sentence object to be decoded.</param>
        /// <returns>The decoded sentence in string form.</returns>
        internal string Decode(Sentence sentenceToDecode)
        {
            string solution = string.Empty;

            foreach (Word w in sentenceToDecode._sentence)
            {
                foreach (char c in w.GetWord())
                {
                    solution += this.EncodingDictionary[c.ToString()];
                }
                solution += " ";
            }

            return solution.Trim();
        }

        public void SetValueFor(string key, string value)
        {
            this.EncodingDictionary[key] = value;
        }

        /// <summary>
        /// This is our main test function for the encoding.  It ensures that a potential encoding is legal given previous encodings.
        /// </summary>
        /// <param name="encoded">The encoded string</param>
        /// <param name="decoded">The candidate decoded string</param>
        /// <returns></returns>
        internal bool CouldEncodingWork(string encoded, string decoded)
        {
            // if we already have some key that decodes to this value
            if(this.EncodingDictionary.ContainsValue(decoded))
            {
                // it better be for this key.
                return this.EncodingDictionary[encoded] == decoded;
            }
            // otherwise we don't have the value set.
            else
            {
                // and it can only be set for this value if it is not set already.  (decoded values are intiialized as "*".
                return this.EncodingDictionary[encoded] == "*";
            }
        }
    }
}
