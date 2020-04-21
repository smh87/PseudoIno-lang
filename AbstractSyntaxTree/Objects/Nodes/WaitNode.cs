using Lexer.Objects;

namespace AbstractSyntaxTree.Objects.Nodes
{
    public class WaitNode : StatementNode
    {
        public TimeNode TimeModifier {get;set;}
        public WaitNode(int line, int offset) : base(TokenType.WAIT, line, offset)
        {
        }
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}