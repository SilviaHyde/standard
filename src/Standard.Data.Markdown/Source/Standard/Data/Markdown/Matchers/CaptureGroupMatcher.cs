namespace Standard.Data.Markdown.Matchers
{
    internal sealed class CaptureGroupMatcher : Matcher
    {
        private readonly string _groupName;
        private readonly Matcher _inner;

        public CaptureGroupMatcher(string groupName, Matcher inner)
        {
            _groupName = groupName;
            _inner = inner;
        }

        public override int Match(MatchContent content)
        {
            var startIndex = content.StartIndex;
            var result = _inner.Match(content);
            if (result != NotMatch)
                content.AddGroup(_groupName, startIndex, result);

            return result;
        }

        public override string ToString()
        {
            return "(?<" + _groupName + ">" + _inner + ")";
        }
    }
}
