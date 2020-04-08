using System.Collections.Generic;
using System;
using Lexer.Objects;

namespace Parser.Objects.Nodes
{
    public class ProgramNode : AstNode
    {
        public List<StatementNode> Statements = new List<StatementNode>();
        public List<FunctionDefinitonNode> FunctionDefinitons = new List<FunctionDefinitonNode>();
        public FunctionLoopNode LoopFunction;
        public ProgramNode()
        {
            this.Type = TokenType.PROG;
        }

        public override void Accept(Visitor visitor) {
            visitor.Visit(this);
        }
    }
}