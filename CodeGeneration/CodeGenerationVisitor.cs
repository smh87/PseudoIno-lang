﻿using System;
using System.IO;
using System.Linq;
using AbstractSyntaxTree.Objects;
using AbstractSyntaxTree.Objects.Nodes;
using Lexer.Objects;
using SymbolTable;


namespace CodeGeneration
{
    public class CodeGenerationVisitor : Visitor
    {
        private SymbolTableObject GlobalScope = SymbolTableBuilder.GlobalSymbolTable;
        private SymbolTableObject CurrentScope;
        public void PrintStringToFile(string content)
        {
            using (StreamWriter writer = File.AppendText("Codegen_output.cpp"))
            {
                writer.Write(content);
            }
        }

        public void PrintHeader()
        {
            PrintStringToFile("#include<Arduino.h>\n");
            PrintStringToFile("//code generated by the PseudoIno");
        }
        public override object Visit(BeginNode beginNode)
        {
            beginNode.LoopNode.Accept(this);
            return null;
        }

        public override object Visit(TimeNode timeNode)
        {
            return null;
        }

        public override object Visit(DeclParametersNode declParametersNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(TimesNode timesNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(FunctionLoopNode loopFnNode)
        {
            if (loopFnNode.Statements.Any())
            {
                loopFnNode.Statements.ForEach(stmnt => stmnt.Accept(this));
            }
            return null;
        }

        public override object Visit(FollowTermNode followTermNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(AssignmentNode assignmentNode)
        {
            assignmentNode.LeftHand.Accept(this);
            assignmentNode.Operator.Accept(this);
            assignmentNode.RightHand.Accept(this);
            PrintStringToFile("\n");
            return null;
        }

        public override object Visit(StatementNode statementNode)
        {
            return null;
        }

        public override object Visit(WithNode withNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(WaitNode waitNode)
        {
            PrintStringToFile("_delay_ms(");
            waitNode.TimeAmount.Accept(this);
            waitNode.TimeModifier.Accept(this);
            switch (waitNode.TimeModifier.Type)
            {
                case TokenType.TIME_HR:
                    PrintStringToFile("*3600000");
                    break;
                case TokenType.TIME_MIN:
                    PrintStringToFile("*60000");
                    break;
                case TokenType.TIME_SEC:
                    PrintStringToFile("*1000");
                    break;
                case TokenType.TIME_MS:
                    break;
                default:
                    throw new Exception("Invalid time exception. Time parameter not specified.");
            }
            PrintStringToFile(")");
            return null;
        }

        public override object Visit(VarNode varNode)
        {
            PrintStringToFile(varNode.Id);
            return null;
        }

        public override object Visit(ValNode valNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(TimeSecondNode timeSecondNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(TimeMinuteNode timeMinuteNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(TimeMillisecondNode timeMillisecondNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(TimeHourNode timeHourNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(RightParenthesisNode rightParenthesisNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(NumericNode numericNode)
        {
            if (numericNode.FValue % 1!=0)
            {
                PrintStringToFile(numericNode.FValue.ToString());
                
            }
            else
            {
                PrintStringToFile(numericNode.IValue.ToString());

            }
            return null;
        }

        public override object Visit(NewlineNode newlineNode)
        {
            PrintStringToFile("\n");
            return null;
        }

        public override object Visit(LeftParenthesisNode leftParenthesisNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(InNode inNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(EqualNode equalNode)
        {
            PrintStringToFile(" = ");
            return null;
        }

        public override object Visit(EqualsNode equalsNode)
        {
            PrintStringToFile(" == ");
            return null;
        }

        public override object Visit(EOFNode eOFNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(EpsilonNode epsilonNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(DoNode doNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(ProgramNode programNode)
        {
            PrintHeader();
            if (programNode.FunctionDefinitons.Any())
            {
                foreach (var functionDefiniton in programNode.FunctionDefinitons)
                {
                    
                }
                programNode.FunctionDefinitons.ForEach(node => node.Parent = programNode);
                programNode.FunctionDefinitons.ForEach(node => node.Accept(this));
            }
            PrintStringToFile("void setup()\n{\n");
            if (programNode.Statements.Any())
            {
                programNode.Statements.ForEach(node => node.Parent = programNode);
                programNode.Statements.ForEach(node => node.Accept(this));
            }
            PrintStringToFile("}\n");
            PrintStringToFile("void loop(){\n");
            programNode.LoopFunction.Accept(this);
            PrintStringToFile("}\n");
            return null;
        }

        public override object Visit(CallNode callNode)
        {
            PrintStringToFile(callNode.Id.Id);
            PrintStringToFile("(");
            for (int i = 0; i < callNode.Parameters.Count - 1; i++)
            {
                if (i > 0)
                {
                    PrintStringToFile(", ");
                }
                PrintStringToFile(callNode.Parameters[i].Value);
            }
            PrintStringToFile(");\n");
            return null;
        }

        public override object Visit(EndNode endNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(AndNode andNode)
        {
            PrintStringToFile(" && ");
            return null;
        }

        public override object Visit(PinNode pinNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(APinNode apinNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(DPinNode dpinNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(OperatorNode operatorNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(BoolOperatorNode boolOperatorNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(CallParametersNode callParametersNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(DivideNode divideNode)
        {
            PrintStringToFile(" / ");
            return null;
        }

        public override object Visit(ExpressionNode expressionNode)
        {
            expressionNode.LeftHand.Accept(this);
            expressionNode.Operator.Accept(this);
            expressionNode.RightHand.Accept(this);
            return null;
        }

        public override object Visit(ForNode forNode)
        {
            if (forNode.From.IValue < forNode.To.IValue)
            {
                PrintStringToFile("for(int " + forNode.CountingVariable.Id + " = " +  
                                  forNode.From.IValue + ";" + " " + forNode.CountingVariable.Id + " < " + 
                                  forNode.To.IValue + "; " + 
                                  forNode.CountingVariable.Id + "++;){\n");
            }
            else
            {
                PrintStringToFile("for(int " + forNode.CountingVariable.Id + " = " +  
                                  forNode.From.IValue + ";" + " "+ forNode.CountingVariable.Id + " < " + 
                                  forNode.To.IValue + "; " + 
                                  forNode.CountingVariable.Id + "--;){\n");
            }
            if (forNode.Statements.Any())
            {
                forNode.Statements.ForEach(node => node.Parent = forNode);
                forNode.Statements.ForEach(node => node.Accept(this));
            }
            PrintStringToFile("}");
            return null;
        }

        public override object Visit(FuncNode funcNode)
        {
            //TODO function type Return er en expression bruge noget f.eks. funcNode.Statements.Any(statment => statment.Type == Lexer.Objects.TokenType.RETURN);
            //SymbolTableObject typeFunc = SymbolTableBuilder.GlobalSymbolTable.FindChild($"{funcNode.Type}_{funcNode.Line}");
            //funcNode.Statements.Last().
            //funcNode.Statements.Last().Type
            
            PrintStringToFile(" ");
            funcNode.Name.Accept(this);
            PrintStringToFile("(");
            //TODO lav functions paramenter med type symboltable
            funcNode.FunctionParameters.ForEach(node => node.Accept(this));
            PrintStringToFile(")\n{");
            if (funcNode.Statements.Any())
            {
                funcNode.Statements.ForEach(node => node.Parent = funcNode);
                funcNode.Statements.ForEach(node => node.Accept(this));
            }
            PrintStringToFile("\n}\n");
            return null;
        }

        public override object Visit(GreaterNode greaterNode)
        {
            PrintStringToFile(" > ");
            return null;
        }

        public override object Visit(GreaterOrEqualNode greaterNode)
        {
            PrintStringToFile(" >= ");
            return null;
        }

        public override object Visit(IfStatementNode ifStatementNode)
        {
            PrintStringToFile("if(");
            ifStatementNode.Expression?.Accept(this);
            PrintStringToFile("){\n");
            if (ifStatementNode.Statements.Any())
            {
                ifStatementNode.Statements.ForEach(node => node.Parent = ifStatementNode);

                ifStatementNode.Statements.ForEach(node => node.Accept(this));

            }
            PrintStringToFile("\n}\n");
            return null;
        }

        public override object Visit(LessNode lessNode)
        {
            PrintStringToFile(" < ");
            return null;
        }

        public override object Visit(LessOrEqualNode lessNode)
        {
            PrintStringToFile(" <= ");
            return null;
        }

        public override object Visit(LoopNode loopNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(MathOperatorNode mathOperatorNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(PlusNode plusNode)
        {
            PrintStringToFile(" + ");
            return null;
        }

        public override object Visit(MinusNode minusNode)
        {
            PrintStringToFile(" - ");
            return null;
        }

        public override object Visit(ModuloNode moduloNode)
        {
            PrintStringToFile(" % ");
            return null;
        }

        public override object Visit(OrNode orNode)
        {
            PrintStringToFile(" || ");
            return null;
        }

        public override object Visit(StringNode stringNode)
        {
            PrintStringToFile($" {stringNode.Value} ");
            return null;
        }

        public override object Visit(WhileNode whileNode)
        {
            PrintStringToFile("while(");
            whileNode.Expression.Accept(this);
            PrintStringToFile("){\n");
            if (whileNode.Statements.Any())
            {
                whileNode.Statements.ForEach(node => node.Parent = whileNode);
                whileNode.Statements.ForEach(node => node.Accept(this));
            }
            PrintStringToFile("}\n");
            return null;
        }

        public override object Visit(ElseStatementNode elseStatement)
        {
            PrintStringToFile("else{\n");
            if (elseStatement.Statements.Any())
            {
                elseStatement.Statements.ForEach(node => node.Parent = elseStatement);
                elseStatement.Statements.ForEach(node => node.Accept(this));
            }

            PrintStringToFile("\n}\n");
            return null;
        }

        public override object Visit(ElseifStatementNode elseifStatementNode)
        {
            PrintStringToFile("else if (");
            //elseifStatementNode.Val?.Accept(this);
            elseifStatementNode.Expression?.Accept(this);
            PrintStringToFile("){\n");
            if (elseifStatementNode.Statements.Any())
            {
                elseifStatementNode.Statements.ForEach(node => node.Parent = elseifStatementNode);
                elseifStatementNode.Statements.ForEach(node => node.Accept(this));
            }
            PrintStringToFile("\n}\n");

            return null;
        }

        public override object Visit(RangeNode rangeNode)
        {
            throw new NotImplementedException();
        }

        public override object Visit(ReturnNode returnNode)
        {
            PrintStringToFile("return ");
            returnNode.ReturnValue.Accept(this);
            PrintStringToFile(";");
            return null;
        }

        public override object Visit(ExpressionTerm expressionTermNode)
        {
            expressionTermNode.LeftHand.Accept(this);
            expressionTermNode.Operator.Accept(this);
            expressionTermNode.RightHand.Accept(this);
            return null;
        }

        public override object Visit(NoParenExpression noParenExpression)
        {
            noParenExpression.LeftHand.Accept(this);
            noParenExpression.Operator.Accept(this);
            noParenExpression.RightHand.Accept(this);
            return null;
        }

        public override object Visit(ParenthesisExpression parenthesisExpression)
        {
            PrintStringToFile("(");
            parenthesisExpression.LeftHand.Accept(this);
            parenthesisExpression.Operator.Accept(this);
            parenthesisExpression.RightHand.Accept(this);
            PrintStringToFile(")");
            return null;
        }

        public override object Visit(BoolNode boolNode)
        {
            if (boolNode.Value)
                PrintStringToFile(" 1");
            else
                PrintStringToFile(" 0");

            return null;
        }
    }
}
