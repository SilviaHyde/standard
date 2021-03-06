using System;
using Standard;

namespace Standard.StringMetrics
{
    /// <summary>
    /// Needleman-Wunch algorithm provides an edit distance based similarity measure between two strings.
    /// </summary>
    public sealed class NeedlemanWunch : AbstractStringMetric
    {
        private AbstractSubstitutionCost dCostFunction;
        private const double defaultGapCost = 2.0;
        private const double defaultMismatchScore = 0.0;
        private const double defaultPerfectMatchScore = 1.0;
        private double estimatedTimingConstant;
        private double gapCost;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeedlemanWunch"/> class.
        /// </summary>
        public NeedlemanWunch() 
            : this(2.0, new SubCostRange0To1())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NeedlemanWunch"/> class, using the substitution cost function specified.
        /// </summary>
        /// <param name="costFunction">The substitution cost function to use.</param>
        public NeedlemanWunch(AbstractSubstitutionCost costFunction) 
            : this(2.0, costFunction)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NeedlemanWunch"/> class, using the gap cost value specified.
        /// </summary>
        /// <param name="costG">The gap cost value.</param>
        public NeedlemanWunch(double costG) 
            : this(costG, new SubCostRange0To1())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NeedlemanWunch"/> class, using the parameters specified.
        /// </summary>
        /// <param name="costG">The gap cost value.</param>
        /// <param name="costFunction">The substitution cost function to use.</param>
        public NeedlemanWunch(double costG, AbstractSubstitutionCost costFunction)
        {
            this.estimatedTimingConstant = 0.00018420000560581684;
            this.gapCost = costG;
            this.dCostFunction = costFunction;
        }

        /// <see cref="AbstractStringMetric.GetSimilarity(string, string)"/>
        public override double GetSimilarity(string firstWord, string secondWord)
        {
            if ((firstWord == null) || (secondWord == null))
                return 0.0;

            double unnormalizedSimilarity = this.GetUnnormalizedSimilarity(firstWord, secondWord);
            double num2 = Math.Max(firstWord.Length, secondWord.Length);
            double num3 = num2;

            if (this.dCostFunction.MaxCost > this.gapCost)
                num2 *= this.dCostFunction.MaxCost;
            else
                num2 *= this.gapCost;

            if (this.dCostFunction.MinCost < this.gapCost)
                num3 *= this.dCostFunction.MinCost;
            else
                num3 *= this.gapCost;

            if (num3 < 0.0)
            {
                num2 -= num3;
                unnormalizedSimilarity -= num3;
            }
            if (num2 == 0.0)
                return 1.0;

            return (1.0 - (unnormalizedSimilarity / num2));
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
        public override double GetUnnormalizedSimilarity(string firstWord, string secondWord)
        {
            if ((firstWord == null) || (secondWord == null))
                return 0.0;

            int length = firstWord.Length;
            int index = secondWord.Length;
            if (length == 0)
                return (double) index;

            if (index == 0)
                return (double) length;

            double[][] numArray = new double[length + 1][];
            for (int i = 0; i < (length + 1); i++)
            {
                numArray[i] = new double[index + 1];
            }
            for (int j = 0; j <= length; j++)
            {
                numArray[j][0] = j;
            }
            for (int k = 0; k <= index; k++)
            {
                numArray[0][k] = k;
            }
            for (int m = 1; m <= length; m++)
            {
                for (int n = 1; n <= index; n++)
                {
                    double num8 = this.dCostFunction.GetCost(firstWord, m - 1, secondWord, n - 1);
                    numArray[m][n] = MathUtility.Min((double)(numArray[m - 1][n] + this.gapCost), (double)(numArray[m][n - 1] + this.gapCost), (double)(numArray[m - 1][n - 1] + num8));
                }
            }
            return numArray[length][index];
        }

        /// <summary>
        /// Gets or sets the substitution cost function
        /// </summary>
        public AbstractSubstitutionCost DCostFunction
        {
            get { return this.dCostFunction; }
            set { this.dCostFunction = value; }
        }

        /// <summary>
        /// Gets or sets the gap cost value.
        /// </summary>
        public double GapCost
        {
            get { return this.gapCost; }
            set { this.gapCost = value; }
        }
    }
}
