using System;
using Standard;

namespace Standard.StringMetrics
{
    /// <summary>
    /// Jaro-Winkler algorithm provides a similarity measure between two strings allowing character transpositions to a degree, adjusting the weighting for common prefixes.
    /// </summary>
    public sealed class JaroWinkler : AbstractStringMetric
    {
        private double estimatedTimingConstant = 4.3420001020422205E-05;
        private AbstractStringMetric jaroStringMetric = new Jaro();
        private const int minPrefixTestLength = 4;
        private const double prefixAdustmentScale = 0.10000000149011612;

        private static int GetPrefixLength(string firstWord, string secondWord)
        {
            if ((firstWord == null) || (secondWord == null))
                return 4;

            int num = MathUtility.Min(4, firstWord.Length, secondWord.Length);
            for (int i = 0; i < num; i++)
            {
                if (firstWord[i] != secondWord[i])
                    return i;
            }
            return num;
        }

        /// <see cref="AbstractStringMetric.GetSimilarity(string, string)"/>
        public override double GetSimilarity(string firstWord, string secondWord)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                double similarity = this.jaroStringMetric.GetSimilarity(firstWord, secondWord);
                int prefixLength = GetPrefixLength(firstWord, secondWord);
                return (similarity + ((prefixLength * 0.10000000149011612) * (1.0 - similarity)));
            }
            return 0.0;
        }

        /// <see cref="AbstractStringMetric.GetSimilarityExplained(string, string)"/>
        /// <remarks>
        /// This method is not implement. Attempting to use this method will result in a <see cref="NotImplementedException"/>.
        /// </remarks>
        public override string GetSimilarityExplained(string firstWord, string secondWord)
        {
            throw new NotImplementedException();
        }

        /// <see cref="AbstractStringMetric.GetSimilarityTimingEstimated(string, string)"/>
        public override double GetSimilarityTimingEstimated(string firstWord, string secondWord)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                double length = firstWord.Length;
                double num2 = secondWord.Length;
                return ((length * num2) * this.estimatedTimingConstant);
            }
            return 0.0;
        }

        /// <see cref="AbstractStringMetric.GetUnnormalizedSimilarity(string, string)"/>
        /// <remarks>
        /// This method does the same thing as <see cref="GetSimilarity(string, string)"/>.
        /// </remarks>
        public override double GetUnnormalizedSimilarity(string firstWord, string secondWord)
        {
            return this.GetSimilarity(firstWord, secondWord);
        }
    }
}
