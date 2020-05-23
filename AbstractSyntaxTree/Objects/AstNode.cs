
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Lexer.Objects;
using System;

namespace AbstractSyntaxTree.Objects
{
    public abstract class AstNode
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public TypeContext SymbolType { get; set; }

        // TODO Pass these from the scanner Token
        public int Line { get; set; }
        public int Offset { get; set; }

        public bool Visited { get; set; }

        public AstNode Parent { get; set; }

        public AstNode(TokenType type, int line, int offset)
        {
            this.Type = type;
            this.Line = line;
            this.Offset = offset;
            this.Visited = false;
        }

        public AstNode(ScannerToken token)
        {
            this.Value = token.Value;
            this.Type = token.Type;
            this.Line = token.Line;
            this.Offset = token.Offset;
            this.SymbolType = token.SymbolicType;
            this.Visited = false;
        }

        public AstNode(TokenType type, ScannerToken token)
        {
            this.Type = type;
            this.Value = token.Value;
            this.Line = token.Line;
            this.Offset = token.Offset;
            this.SymbolType = token.SymbolicType;
            this.Visited = false;
        }

        public override string ToString() => $"Type={Type}";

        public bool IsType(Type type)
        {
            return this.GetType().IsAssignableFrom(type);
        }
        public abstract object Accept(Visitor visitor);
    }
}