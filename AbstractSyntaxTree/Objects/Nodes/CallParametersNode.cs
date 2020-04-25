using System.Collections.Generic;
using Lexer.Objects;

namespace AbstractSyntaxTree.Objects.Nodes
{
    public class CallParametersNode : AstNode
    {
        public List<ValNode> Parameters { get; set; } = new List<ValNode>();
        public CallParametersNode(int line, int offset) : base(TokenType.CALLPARAMS, line, offset)
        {
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}