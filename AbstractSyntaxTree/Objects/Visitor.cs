using System;
using System.Linq;
using AbstractSyntaxTree.Objects;
using AbstractSyntaxTree.Objects.Nodes;
namespace AbstractSyntaxTree.Objects
{
    public abstract class Visitor
    {
        public abstract object Visit(BeginNode beginNode);
        public abstract object Visit(TimeNode timeNode);
        public abstract object Visit(DeclParametersNode declParametersNode);
        public abstract object Visit(TimesNode timesNode);
        public abstract object Visit(FunctionLoopNode loopFnNode);
        public abstract object Visit(AssignmentNode assignmentNode);
        public abstract object Visit(StatementNode statementNode);
        public abstract object Visit(WithNode withNode);
        public abstract object Visit(WaitNode waitNode);
        public abstract object Visit(VarNode varNode);
        public abstract object Visit(ValNode valNode);
        public abstract object Visit(TimeSecondNode timeSecondNode);
        public abstract object Visit(TimeMinuteNode timeMinuteNode);
        public abstract object Visit(TimeMillisecondNode timeMillisecondNode);
        public abstract object Visit(TimeHourNode timeHourNode);
        public abstract object Visit(RightParenthesisNode rightParenthesisNode);
        public abstract object Visit(NumericNode numericNode);
        public abstract object Visit(NewlineNode newlineNode);
        public abstract object Visit(LeftParenthesisNode leftParenthesisNode);
        public abstract object Visit(InNode inNode);
        public abstract object Visit(EqualNode equalNode);
        public abstract object Visit(EqualsNode equalsNode);
        public abstract object Visit(EOFNode eOFNode);
        public abstract object Visit(EpsilonNode epsilonNode);
        public abstract object Visit(DoNode doNode);
        public abstract object Visit(ProgramNode programNode);
        public abstract object Visit(CallNode callNode);
        public abstract object Visit(EndNode endNode);
        public abstract object Visit(AndNode andNode);
        public abstract object Visit(PinNode pinNode);
        public abstract object Visit(APinNode apinNode);
        public abstract object Visit(DPinNode dpinNode);
        public abstract object Visit(OperatorNode operatorNode);
        public abstract object Visit(BoolOperatorNode boolOperatorNode);
        public abstract object Visit(CallParametersNode callParametersNode);
        public abstract object Visit(DivideNode divideNode);
        public abstract object Visit(ExpressionNode expressionNode);
        public abstract object Visit(ForNode forNode);
        public abstract object Visit(FuncNode funcNode);
        public abstract object Visit(GreaterNode greaterNode);
        public abstract object Visit(IfStatementNode ifStatementNode);
        public abstract object Visit(LessNode lessNode);
        public abstract object Visit(LoopNode loopNode);
        public abstract object Visit(MathOperatorNode mathOperatorNode);
        public abstract object Visit(PlusNode plusNode);
        public abstract object Visit(MinusNode minusNode);
        public abstract object Visit(ModuloNode moduloNode);
        public abstract object Visit(OrNode orNode);
        public abstract object Visit(StringNode stringNode);
        public abstract object Visit(WhileNode whileNode);
        public abstract object Visit(ElseStatementNode elseStatement);
        public abstract object Visit(ElseifStatementNode elseifStatementNode);
        public abstract object Visit(RangeNode rangeNode);
        public abstract object Visit(ReturnNode returnNode);
    }
}