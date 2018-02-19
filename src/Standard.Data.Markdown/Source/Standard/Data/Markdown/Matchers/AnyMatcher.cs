namespace Standard.Data.Markdown.Matchers
{
    internal sealed class AnyMatcher : Matcher
    {
        private readonly Matcher[] _inners;

        public AnyMatcher(Matcher[] inners)
        {
            _inners = inners;
        }

        public override int Match(MatchContent content)
        {
            bool matchSuccess = false;
            foreach (var m in _inners)
            {
                var c = m.Match(content);
                if (c > 0)
                    return c;

                if (c == 0)
                    matchSuccess = true;
            }
            return matchSuccess ? 0 : NotMatch;
        }

        internal Matcher[] Inners => _inners;

        public override string ToString()
        {
            return "(" + string.Join<Matcher>("|", _inners) + ")";
        }
    }
}
