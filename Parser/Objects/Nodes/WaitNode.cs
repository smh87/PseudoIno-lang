using Lexer.Objects;

namespace Parser.Objects.Nodes
{
    public class WaitNode : AstNode
    {
        public WaitNode(int line, int offset) : base(TokenType.WAIT, line, offset)
        {
        }
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}