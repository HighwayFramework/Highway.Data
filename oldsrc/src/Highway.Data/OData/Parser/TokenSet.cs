namespace Highway.Data.OData.Parser
{
    internal class TokenSet
    {
        public TokenSet()
        {
            Left = string.Empty;
            Right = string.Empty;
            Operation = string.Empty;
        }

        public string Left { get; set; }
        public string Operation { get; set; }
        public string Right { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Left, Operation, Right);
        }
    }
}