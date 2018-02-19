using System;
using System.Text.RegularExpressions;
using Standard.Data.Markdown.Matchers;

namespace Standard.Data.Markdown
{

    public class GfmFencesBlockRule : IMarkdownRule
    {
        private static readonly Matcher _EndFences =
            Matcher.NewLine.RepeatAtLeast(1) +
            Matcher.WhiteSpace.Repeat(0, 3) +
            Matcher.BackReference("flag").RepeatAtLeast(3).CompareLength(LengthComparison.GreaterThanOrEquals, "flagLength") +
            Matcher.WhiteSpacesOrEmpty +
            (Matcher.NewLine | Matcher.EndOfString);
        private static readonly Matcher _FencesMatcher =
            Matcher.WhiteSpacesOrEmpty.ToGroup("spaces") +
            (Matcher.Char('`').ToGroup("flag").RepeatAtLeast(3) | Matcher.Char('~').ToGroup("flag").RepeatAtLeast(3)).ToGroup("flagLength") +
            Matcher.WhiteSpacesOrEmpty +
            Matcher.AnyCharNotIn(' ', '\n').RepeatAtLeast(0).ToGroup("lang") +
            Matcher.WhiteSpacesOrEmpty + Matcher.NewLine +
            (
                _EndFences.ToNegativeTest() +
                (Matcher.AnyStringInSingleLine | Matcher.NewLine.RepeatAtLeast(1))
            ).RepeatAtLeast(0).ToGroup("code") +
            (_EndFences | Matcher.EndOfString);

        public virtual string Name => "Fences";

        public virtual Matcher FencesMatcher => null;

        [Obsolete("Please use FencesMatcher.", true)]
        public virtual Regex Fences => Regexes.Block.Gfm.Fences;

        public IMarkdownToken TryMatch(IMarkdownParser parser, IMarkdownParsingContext context)
        {
            var match = context.Match(_FencesMatcher);
            if (match?.Length > 0)
            {
                var sourceInfo = context.Consume(match.Length);
                var code = match["code"].GetValue();
                if (match["spaces"].Count > 0)
                    code = Regex.Replace(code, "^ {1," + match["spaces"].Count.ToString() + "}", string.Empty, RegexOptions.Multiline);

                return new MarkdownCodeBlockToken(this, parser.Context, code, match["lang"].GetValue(), sourceInfo);
            }
            return null;
        }
    }
}
