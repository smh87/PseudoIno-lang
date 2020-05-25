using Lexer.Objects;

namespace AbstractSyntaxTree.Objects.Nodes
{
    public class BoolNode : ValNode
    {
        public bool Value {get;set;}
        public BoolNode(string value , ScannerToken token) : base(token)
        {
            Value = bool.Parse(value);
        }

        /// <inheritdoc cref="AbstractSyntaxTree.Objects.AstNode.Accept(Visitor)"/>
        public override object Accept(Visitor visitor) {
            return visitor.Visit(this);
        }
    }
}