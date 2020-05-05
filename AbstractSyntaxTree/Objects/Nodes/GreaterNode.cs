using Lexer.Objects;

namespace AbstractSyntaxTree.Objects.Nodes
{
    public class GreaterNode : MathOperatorNode
    {
        public OrEqualNode OrEqualNode { get; set; }
        public GreaterNode( ScannerToken token) : base(token)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}