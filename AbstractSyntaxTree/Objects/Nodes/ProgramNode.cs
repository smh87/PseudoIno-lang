using System.Collections.Generic;
using System;
using Lexer.Objects;

namespace AbstractSyntaxTree.Objects.Nodes
{
    public class ProgramNode : AstNode, IScope
    {
        public List<StatementNode> Statements { get; set; }
        public List<FuncNode> FunctionDefinitons = new List<FuncNode>();
        public FuncNode LoopFunction;
        public ProgramNode(int line, int offset) : base(TokenType.PROG, line, offset)
        {
            this.Statements = new List<StatementNode>();
        }

        public override void Accept(Visitor visitor)
        {
            visitor.Visit(this);
        }
    }
}