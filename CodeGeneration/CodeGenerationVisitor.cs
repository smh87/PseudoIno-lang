using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AbstractSyntaxTree.Objects;
using AbstractSyntaxTree.Objects.Nodes;
using CodeGeneration.Exceptions;
using Contextual_analysis;
using Contextual_analysis.Exceptions;
using Lexer.Objects;
using SymbolTable;


namespace CodeGeneration
{
    public class CodeGenerationVisitor : Visitor
    {
        public static bool HasError { get; set; } = false;
        private string Header { get; set; }
        private string Declarations { get; set; }
        private string Global { get; set; }
        private string Prototypes { get; set; }
        private string Setup { get; set; }
        private string Funcs { get; set; }
        private string Loop { get; set; }

        private string FileName { get; set; }

        private List<string> PinDefs = new List<string>();

        private SymbolTableObject GlobalScope = SymbolTableBuilder.GlobalSymbolTable;
        private SymbolTableObject CurrentScope;

        public CodeGenerationVisitor(string fileName)
        {
            FileName = fileName;
        }
        public void PrintStringToFile(string content)
        {
            using (StreamWriter writer = File.AppendText(FileName))
            {
                writer.Write(content);
            }
        }

        public void PrintStringToFile()
        {
            string content = "";
            List<string> uniqueList = PinDefs.Distinct().ToList();
            foreach (var pinDef in uniqueList)
            {
                Setup += pinDef + "\n";
            }

            Setup += "}\n";
            content += Header + Global + Prototypes + Declarations + Setup + Funcs + Loop;
            using (StreamWriter writer = File.AppendText(FileName))
            {
                writer.Write(content);
            }
        }

        public void PrintToHeader(string input)
        {
            Header += input;
        }

        public void PrintToGlobal(string input)
        {
            Global += input;
        }

        public void PrintToPrototypes(string input)
        {
            Prototypes += input;
        }

        public void PrintToSetup(string input)
        {
            Setup += input;
        }

        public void PrintToFuncs(string input)
        {
            Funcs += input;
        }

        public void PrintToLoop(string input)
        {
            Loop += input;
        }

        public override object Visit(TimesNode timesNode)
        {
            //PrintStringToFile(" * ");
            return " * ";
        }

        public override object Visit(AssignmentNode assignmentNode)
        {
            string assign = "";
            if (assignmentNode.LeftHand.Type == TokenType.DPIN)
            {
                string pinDef = "pinMode(" + assignmentNode.LeftHand.Accept(this) + ", OUTPUT);";
                PinDefs.Add(pinDef);

                assign += "digitalWrite(" + assignmentNode.LeftHand.Accept(this) + ", ";
                string boolValue = assignmentNode.RightHand.Accept(this) + ")";
                assign += boolValue == " 1)" ? "HIGH)" : "LOW)";
            }
            else if (assignmentNode.LeftHand.Type == TokenType.APIN)
            {
                string pinDef = "pinMode(" + assignmentNode.LeftHand.Accept(this) + ", OUTPUT);";
                PinDefs.Add(pinDef);

                assign += "analogWrite(" + assignmentNode.LeftHand.Accept(this) + ", ";
                string boolValue = assignmentNode.RightHand.Accept(this) + ")";
                if (assignmentNode.RightHand.SymbolType.Type == TokenType.NUMERIC)
                    assign += boolValue;
                else
                    assign += boolValue == " 1)" ? "255)" : "0)";

            }
            else
            {
                assign += (string)assignmentNode.LeftHand.Accept(this);
                assign += " = ";
                assign += (string)assignmentNode.RightHand.Accept(this);
            }


            assign += ";\n";
            return assign;
        }

        public override object Visit(WaitNode waitNode)
        {
            string delay = "delay(";
            delay += waitNode.TimeAmount.Accept(this);
            delay += waitNode.TimeModifier.Accept(this);
            delay += ");\n";
            return delay;
        }

        public override object Visit(VarNode varNode)
        {
            string varNodeId = "";
            if (varNode.Declaration)
            {
                if (varNode.SymbolType.Type == TokenType.NUMERIC)
                {
                    if (varNode.SymbolType.IsFloat)
                    {
                        varNodeId += "float ";
                    }
                    else
                    {
                        varNodeId += "int ";
                    }
                }
                else if (varNode.SymbolType.Type == TokenType.BOOL)
                {
                    varNodeId += "bool ";
                }
                else if (varNode.SymbolType.Type == TokenType.STRING)
                {
                    varNodeId += "String ";
                }
            }
            varNodeId += varNode.Id;
            //PrintStringToFile(varNode.Id);
            return varNodeId;
        }

        public override object Visit(ValNode valNode)
        {
            //PrintStringToFile(valNode.Value);
            return valNode.Value;
        }

        public override object Visit(TimeSecondNode timeSecondNode)
        {
            return "*1000";
        }

        public override object Visit(TimeMinuteNode timeMinuteNode)
        {
            return "*60000";
        }

        public override object Visit(TimeMillisecondNode timeMillisecondNode)
        {
            return "";
        }

        public override object Visit(TimeHourNode timeHourNode)
        {
            return "*3600000";
        }

        public override object Visit(NumericNode numericNode)
        {
            string numeric = "";
            if (numericNode.FValue % 1 != 0)
            {
                numeric += numericNode.FValue.ToString();

            }
            else
            {
                numeric += numericNode.IValue.ToString();

            }
            return numeric;
        }

        public override object Visit(EqualNode equalNode)
        {
            //PrintStringToFile(" == ");
            return " == ";
        }

        public override object Visit(ProgramNode programNode)
        {
            PrintToHeader("#include<Arduino.h>\n");
            PrintToHeader("//code generated by the PseudoIno compiler\n");

            if (programNode.FunctionDefinitons.Any())
            {
                string funcs = "";
                programNode.FunctionDefinitons.ForEach(node =>
                {
                    if (SymbolTableObject.FunctionCalls.Any(cn => cn.Id.Id == node.Name.Id && cn.Parameters.Count == node.FunctionParameters.Count))
                        node.Parent = programNode;
                });
                programNode.FunctionDefinitons.ForEach(node =>
                {
                    if (SymbolTableObject.FunctionCalls.Any(cn => cn.Id.Id == node.Name.Id && cn.Parameters.Count == node.FunctionParameters.Count))
                        funcs += node.Accept(this);
                });
                PrintToFuncs(funcs);
            }

            Global += "void setup();\n";
            string setupString = "void setup()\n{\n";
            if (programNode.Statements.Any())
            {
                programNode.Statements.ForEach(node => node.Parent = programNode);
                foreach (var node in programNode.Statements)
                {
                    if (node.Type == TokenType.ASSIGNMENT)
                    {
                        if (((AstNode)((AssignmentNode)node).LeftHand).GetType().IsAssignableFrom(typeof(VarNode)) && ((VarNode)((AssignmentNode)node).LeftHand).Declaration)
                        {
                            Declarations += node.Accept(this);
                            continue;
                        }
                    }
                    setupString += node.Accept(this);
                }
                //programNode.Statements.ForEach(node => setupString += node.Accept(this));
            }
            //setupString += "}\n";
            PrintToSetup(setupString);
            //PrintStringToFile("void loop(){\n");
            string loopString = (string)programNode.LoopFunction.Accept(this);
            PrintToLoop(loopString);
            //PrintStringToFile("}\n");
            PrintStringToFile();
            return null;
        }

        public override object Visit(CallNode callNode)
        {
            string callString = callNode.Id.Id + "(";
            for (int i = 0; i < callNode.Parameters.Count; i++)
            {
                if (i > 0)
                {
                    callString += ", ";
                }
                callString += callNode.Parameters[i].Value;
            }
            callString += ");\n";
            return callString;
        }

        public override object Visit(AndNode andNode)
        {
            //PrintStringToFile(" && ");
            return " && ";
        }

        public override object Visit(APinNode apinNode)
        {
            return apinNode.Id;
        }

        public override object Visit(DPinNode dpinNode)
        {
            return dpinNode.Id;
        }

        public override object Visit(DivideNode divideNode)
        {
            //PrintStringToFile(" / ");
            return " / ";
        }

        public override object Visit(ForNode forNode)
        {
            string forLoop = "";
            if (forNode.From.IValue < forNode.To.IValue)
            {
                forLoop += "for(int " + forNode.CountingVariable.Id + " = " +
                                  forNode.From.IValue + ";" + " " + forNode.CountingVariable.Id + " < " +
                                  forNode.To.IValue + "; " +
                                  forNode.CountingVariable.Id + "++){\n";
            }
            else
            {
                forLoop += "for(int " + forNode.CountingVariable.Id + " = " +
                                  forNode.From.IValue + ";" + " " + forNode.CountingVariable.Id + " < " +
                                  forNode.To.IValue + "; " +
                                  forNode.CountingVariable.Id + "--){\n";
            }
            if (forNode.Statements.Any())
            {
                forNode.Statements.ForEach(node => node.Parent = forNode);
                forNode.Statements.ForEach(node => forLoop += ((string)node.Accept(this)));
            }
            forLoop += "}\n";
            return forLoop;
        }

        public override object Visit(FuncNode funcNode)
        {
            string func = "";

            switch ((funcNode.SymbolType?.Type.ToString() ?? "void"))
            {
                case "void":
                    func += "void ";
                    break;
                case "NUMERIC":
                    if (funcNode.SymbolType.IsFloat)
                    {
                        func += "float ";
                    }
                    else
                    {
                        func += "int ";
                    }
                    break;
                case "BOOL":
                    func += "bool ";
                    break;
                case "STRING":
                    func += "string ";
                    break;
                default:
                    new InvalidCodeException($"Invalid return type in function {funcNode.Name.Id} at {funcNode.Line}:{funcNode.Offset}");
                    break;
            }

            //funcNode.Name.Accept(this);
            func += funcNode.Name.Id + "(";

            funcNode.FunctionParameters.ForEach(node => func += findFuncInputparam(node, funcNode) + node.Accept(this) + (funcNode.FunctionParameters.IndexOf(node) < funcNode.FunctionParameters.Count - 1 ? ", " : " "));

            func += ")";
            Global += func + ";";
            Global += "\n";
            func += "\n{\n";
            if (funcNode.Statements.Any())
            {
                funcNode.Statements.ForEach(node => node.Parent = funcNode);
                funcNode.Statements.ForEach(node => func += node.Accept(this));
            }
            func += "\n}\n";
            return func;
        }

        private string findFuncInputparam(VarNode functionsParam, FuncNode function)
        {
            VarNode param = function.FunctionParameters.Find(x => x.Id == functionsParam.Id);

            if (param.SymbolType.IsFloat)
            {
                return "float ";
            }
            else if (param.SymbolType.Type == TokenType.BOOL)
            {
                return "bool ";
            }
            else if (!param.SymbolType.IsFloat)
            {
                return "int ";
            }
            else if (param.SymbolType.Type == TokenType.STRING)
            {
                return "String ";
            }
            else
            {
                new InvalidCodeException($"The function{function.Name} input parameter {functionsParam.Id} at {functionsParam.Line}:{functionsParam.Offset} is not used.");
                return "";
            }


        }


        public override object Visit(GreaterNode greaterNode)
        {
            //PrintStringToFile(" > ");
            return " > ";
        }

        public override object Visit(GreaterOrEqualNode greaterNode)
        {
            //PrintStringToFile(" >= ");
            return " >= ";
        }

        public override object Visit(IfStatementNode ifStatementNode)
        {
            string ifNode = "";
            ifNode += "if(";
            ifNode += ifStatementNode.Expression?.Accept(this);
            ifNode += "){\n";
            if (ifStatementNode.Statements.Any())
            {
                ifStatementNode.Statements.ForEach(node => node.Parent = ifStatementNode);

                ifStatementNode.Statements.ForEach(node => ifNode += node.Accept(this));

            }
            ifNode += "\n}\n";
            return ifNode;
        }

        public override object Visit(LessNode lessNode)
        {
            //PrintStringToFile(" < ");
            return " < ";
        }

        public override object Visit(LessOrEqualNode lessNode)
        {
            //PrintStringToFile(" <= ");
            return " <= ";
        }

        public override object Visit(PlusNode plusNode)
        {
            //PrintStringToFile(" + ");
            return " + ";
        }

        public override object Visit(MinusNode minusNode)
        {
            //PrintStringToFile(" - ");
            return " - ";
        }

        public override object Visit(ModuloNode moduloNode)
        {
            //PrintStringToFile(" % ");
            return " % ";
        }

        public override object Visit(OrNode orNode)
        {
            //PrintStringToFile(" || ");
            return " || ";
        }

        public override object Visit(StringNode stringNode)
        {
            //PrintStringToFile($" {stringNode.Value} ");
            return $" {stringNode.Value} ";
        }

        public override object Visit(WhileNode whileNode)
        {
            string whileString = "";
            whileString += "while(";
            whileString += whileNode.Expression.Accept(this);
            whileString += "){\n";
            if (whileNode.Statements.Any())
            {
                whileNode.Statements.ForEach(node => node.Parent = whileNode);
                whileNode.Statements.ForEach(node => whileString += node.Accept(this));
            }
            whileString += "}\n";
            return whileString;
        }

        public override object Visit(ElseStatementNode elseStatement)
        {
            if (elseStatement.Statements.Any())
            {
                string elseString = "else{\n";
                elseStatement.Statements.ForEach(node => node.Parent = elseStatement);
                elseStatement.Statements.ForEach(node => elseString += node.Accept(this));
                elseString += "\n}\n";
                return elseString;
            }

            return "";
        }

        public override object Visit(ElseifStatementNode elseifStatementNode)
        {
            string elseif = "else if (";
            //elseifStatementNode.Val?.Accept(this);
            elseif += elseifStatementNode.Expression?.Accept(this);
            elseif += "){\n";
            if (elseifStatementNode.Statements.Any())
            {
                elseifStatementNode.Statements.ForEach(node => node.Parent = elseifStatementNode);
                elseifStatementNode.Statements.ForEach(node => elseif += node.Accept(this));
            }
            elseif += ("\n}\n");

            return elseif;
        }

        public override object Visit(ReturnNode returnNode)
        {
            string ret = "return ";
            ret += returnNode.ReturnValue.Accept(this);
            ret += ";";
            return ret;
        }

        public override object Visit(ExpressionTerm expressionTermNode)
        {
            string exp = "";
            exp += expressionTermNode.LeftHand?.Accept(this);
            exp += expressionTermNode.Operator?.Accept(this);
            exp += expressionTermNode.RightHand?.Accept(this);
            return exp;
        }

        public override object Visit(BinaryExpression noParenExpression)
        {
            string exp = "";
            exp += noParenExpression.LeftHand.Accept(this);
            exp += noParenExpression.Operator?.Accept(this);
            exp += noParenExpression.RightHand?.Accept(this);
            return exp;
        }

        public override object Visit(ParenthesisExpression parenthesisExpression)
        {
            string exp = "(";
            exp += parenthesisExpression.LeftHand.Accept(this);
            exp += parenthesisExpression.Operator.Accept(this);
            exp += parenthesisExpression.RightHand.Accept(this);
            exp += ")";
            return exp;
        }

        public override object Visit(BoolNode boolNode)
        {
            string boolVal = "";
            if (boolNode.Value)
                boolVal += " 1";
            else
                boolVal += " 0";

            return boolVal;
        }
    }
}