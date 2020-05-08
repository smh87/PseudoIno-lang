using System.Collections.Generic;
using Lexer.Objects;

namespace AbstractSyntaxTree.Objects.Nodes
{
    public class WhileNode : LoopNode, IScope
    {
        public ExpressionNode Expression { get; set; }
        public List<StatementNode> Statements { get; set; }

        public WhileNode(int line, int offset) : base(TokenType.WHILE, line, offset)
        {
            Statements = new List<StatementNode>();
        }

        public override object Accept(Visitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}