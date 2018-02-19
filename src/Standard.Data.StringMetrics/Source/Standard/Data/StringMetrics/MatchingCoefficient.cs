using System;
using System.Collections.ObjectModel;

namespace Standard.Data.StringMetrics
{
    /// <summary>
    /// Matching Coefficient algorithm provides a similarity measure between two strings.
    /// </summary>
    public sealed class MatchingCoefficient : AbstractStringMetric
    {
        private const double defaultMismatchScore = 0.0;
        private double estimatedTimingConstant;
        private ITokenizer tokenizer;
        private TokenizerUtility<string> tokenUtility;

        public MatchingCoefficient() : this(new TokenizerWhitespace())
        {
        }

        public MatchingCoefficient(ITokenizer tokenizerToUse)
        {
            this.estimatedTimingConstant = 0.00019999999494757503;
            this.tokenizer = tokenizerToUse;
            this.tokenUtility = new TokenizerUtility<string>();
        }

        private double GetActualSimilarity(Collection<string> firstTokens, Collection<string> secondTokens)
        {
            this.tokenUtility.CreateMergedList(firstTokens, secondTokens);
            int num = 0;
            foreach (string str in firstTokens)
            {
                if (secondTokens.Contains(str))
                    num++;
            }
            return (double)num;
        }

        public override double GetSimilarity(string firstWord, string secondWord)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                double unnormalizedSimilarity = this.GetUnnormalizedSimilarity(firstWord, secondWord);
                int num2 = Math.Max(this.tokenUtility.FirstTokenCount, this.tokenUtility.SecondTokenCount);
                return (unnormalizedSimilarity / ((double)num2));
            }
            return 0.0;
        }

        public override string GetSimilarityExplained(string firstWord, string secondWord)
        {
            throw new NotImplementedException();
        }

        public override double GetSimilarityTimingEstimated(string firstWord, string secondWord)
        {
            if ((firstWord != null) && (secondWord != null))
            {
                double count = this.tokenizer.Tokenize(firstWord).Count;
                double num2 = this.tokenizer.Tokenize(secondWord).Count;
                return ((num2 * count) * this.estimatedTimingConstant);
            }
            return 0.0;
        }

        public override double GetUnnormalizedSimilarity(string firstWord, string secondWord)
        {
            Collection<string> firstTokens = this.tokenizer.Tokenize(firstWord);
            Collection<string> secondTokens = this.tokenizer.Tokenize(secondWord);
            return this.GetActualSimilarity(firstTokens, secondTokens);
        }
    }
}
