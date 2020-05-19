using Lexer.Objects;

namespace AbstractSyntaxTree.Objects.Nodes
{
    public abstract class MathOperatorNode : OperatorNode
    {
        public MathOperatorNode(ScannerToken token) : base(token)
        {
        }

        public abstract override object Accept(Visitor visitor);
    }
}