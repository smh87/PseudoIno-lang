using Lexer.Objects;

namespace Parser.Objects.Nodes
{
    public class EOFNode : AstNode
    {
        public EOFNode(int line, int offset) : base(TokenType.EOF, line, offset)
        {
        }
        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}