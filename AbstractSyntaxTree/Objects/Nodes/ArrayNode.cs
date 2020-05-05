using Lexer.Objects;

namespace AbstractSyntaxTree.Objects.Nodes
{
    public class ArrayNode : AstNode, IAssignment
    {
        public ArrayNode(ScannerToken token) : base(token)
        {
        }

        public override void Accept(Visitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}