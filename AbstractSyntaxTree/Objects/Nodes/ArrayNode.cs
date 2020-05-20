using System.Collections.Generic;
using Lexer.Objects;

namespace AbstractSyntaxTree.Objects.Nodes
{
    public class ArrayNode : AstNode, IAssignment, IExpr
    {
        public List<NumericNode> Dimensions {get;set;} = new List<NumericNode>();
        public VarNode ActualId {get;set;}
        public bool HasType {get;set;} = false;
        public bool HasBeenAccessed {get;set;} = false;

        #region Not implemented
        public ITerm LeftHand { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public OperatorNode Operator { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public IExpr RightHand { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        #endregion

        public ArrayNode(int line, int offset) : base(TokenType.ARR, line, offset)
        {
        }

        public override object Accept(Visitor visitor)
        {
            return visitor.Visit(this);
        }
    }
}