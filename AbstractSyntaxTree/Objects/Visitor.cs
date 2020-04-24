using System;
using System.Linq;
using AbstractSyntaxTree.Objects;
using AbstractSyntaxTree.Objects.Nodes;

namespace AbstractSyntaxTree.Objects
{
    public abstract class Visitor
    {
        public void Visit(BeginNode beginNode)
        {
            beginNode.LoopNode.Accept(this);

        }

        internal void Visit(TimeNode timeNode)
        {
            timeNode.Accept(this);
        }
        public void Visit(TimesNode timesNode)
        {

        }

        public void Visit(FunctionLoopNode loopFnNode)
        {
            if (loopFnNode.Statements.Any()) {
                loopFnNode.Statements.ForEach(stmnt => stmnt.Accept(this));
            }
        }

        internal void Visit(AssignmentNode assignmentNode)
        {
            assignmentNode.LeftHand.Accept(this);
            assignmentNode.RightHand.Accept(this);
            if (assignmentNode.ExpressionHand != null)
                assignmentNode.ExpressionHand.Accept(this);
        }

        internal void Visit(FunctionDefinitonNode functionDefinitonNode)
        {
            functionDefinitonNode.Accept(this);
        }

        public void Visit(StatementNode statementNode)
        {
           statementNode.Accept(this);
        }

        public void Visit(WithNode withNode)
        {
            withNode.Accept(this);
        }

        public void Visit(WaitNode waitNode)
        {
            
            waitNode.TimeAmount.Accept(this);
            waitNode.TimeModifier.Accept(this); 
        }

        public void Visit(VarNode varNode)
        {

        }

        public void Visit(ValNode valNode)
        {
            valNode.Accept(this);
        }

        public void Visit(TimeSecondNode timeSecondNode)
        {
            //timeSecondNode.Accept(this);
        }

        public void Visit(TimeMinuteNode timeMinuteNode)
        {
            //timeMinuteNode.Accept(this);
        }

        public void Visit(TimeMillisecondNode timeMillisecondNode)
        {
            //timeMillisecondNode.Accept(this);
        }

        public void Visit(TimeHourNode timeHourNode)
        {
           // timeHourNode.Accept(this);

        }

        public void Visit(RightParenthesisNode rightParenthesisNode)
        {
            rightParenthesisNode.Accept(this);
        }

        public void Visit(NumericNode numericNode)
        {
 
            numericNode.Accept(this);

        }

        public void Visit(NewlineNode newlineNode)
        {
            newlineNode.Accept(this);
        }

        public void Visit(LeftParenthesisNode leftParenthesisNode)
        {
            leftParenthesisNode.Accept(this);
        }

        public void Visit(InNode inNode)
        {
            inNode.Accept(this);
        }
        public void Visit(EqualNode equalNode)
        {
            equalNode.Accept(this);
        }

        public void Visit(EqualsNode equalsNode)
        {
            equalsNode.Accept(this);
        }

        public void Visit(EOFNode eOFNode)
        {
            eOFNode.Accept(this);
        }
        
        public void Visit(EpsilonNode epsilonNode)
        {
            epsilonNode.Accept(this);
        }

        public void Visit(DoNode doNode)
        {
            doNode.Accept(this);
        }

        public void Visit(ProgramNode programNode)
        {
            if (programNode.FunctionDefinitons.Any()) {
                programNode.FunctionDefinitons.ForEach(node => node.Accept(this));
            }
            if (programNode.Statements.Any()) {
                programNode.Statements.ForEach(node => node.Accept(this));
            }
            programNode.LoopFunction.Accept(this);
        }

        public void Visit(CallNode callNode)
        {
            callNode.VarNode.Accept(this);
            callNode.RightHand.Accept(this);
            callNode.Accept(this);
        }

        public void Visit(EndNode endNode)
        {
            endNode.Accept(this);
        }
        public void Visit(AndNode andNode)
        {
            //andNode.Accept(this);
        }
        public void Visit(PinNode pinNode)
        {
            pinNode.Accept(this);
        }
        public void Visit(APinNode apinNode)
        {
            
        }
        public void Visit(DPinNode dpinNode)
        {

        }
        public void Visit(OperatorNode operatorNode)
        {
            operatorNode.Accept(this);
        }
        public void Visit(BoolOperatorNode boolOperatorNode)
        {
            boolOperatorNode.Accept(this);
        }
        public void Visit(CallParametersNode callParametersNode)
        {
            callParametersNode.ValNode.Accept(this);
            callParametersNode.RightHand.Accept(this);
            callParametersNode.Accept(this);
        }
        public void Visit(DivideNode divideNode)
        {
            
        }
        public void Visit(ExpressionNode expressionNode)
        {
            expressionNode.Operator.Accept(this);
            expressionNode.Value.Accept(this);
            expressionNode.Expression.Accept(this);
            expressionNode.Accept(this);
        }
        //TODO change expressionnode to rangenode
        public void Visit(ForNode forNode)
        {
            forNode.ValNode.Accept(this);
            forNode.ExpressionNode.Accept(this);
            if (forNode.Statements.Any())
            {
                forNode.Statements.ForEach(node => node.Accept(this));
            }
            forNode.Accept(this);
        }
        public void Visit(FuncNode funcNode)
        {
            if (funcNode.Statements.Any())
            {
                funcNode.Statements.ForEach(node => node.Accept(this));
            }
            funcNode.Accept(this);
        }
        public void Visit(GreaterNode greaterNode)
        {
            greaterNode.OrEqualNode.Accept(this);
            greaterNode.Accept(this);
        }
        public void Visit(IfStatementNode ifStatementNode)
        {
            ifStatementNode.Val.Accept(this);
            ifStatementNode.Expression.Accept(this);
            if (ifStatementNode.Statements.Any())
            {
                ifStatementNode.Statements.ForEach(node => node.Accept(this));
            }
            ifStatementNode.Accept(this);
        }
        public void Visit(LessNode lessNode)
        {
            lessNode.OrEqualNode.Accept(this);
            lessNode.Accept(this);
        }
        public void Visit(LoopNode loopNode)
        {
            loopNode.Accept(this);
        }
        public void Visit(MathOperatorNode mathOperatorNode)
        {
            mathOperatorNode.Accept(this);
        }
        public void Visit(PlusNode plusNode)
        {

        }
        public void Visit(MinusNode minusNode)
        {
            
        }
        public void Visit(ModuloNode moduloNode)
        {

        }
        public void Visit(OrNode orNode)
        {
            orNode.Accept(this);
        }
        public void Visit(StringNode stringNode)
        {
            stringNode.Accept(this);
        }
        public void Visit(WhileNode whileNode)
        {
            whileNode.ValNode.Accept(this);
            whileNode.ExpressionNode.Accept(this);
            if (whileNode.Statements.Any())
            {
                whileNode.Statements.ForEach(node => node.Accept(this));
            }
            whileNode.Accept(this);
        }
    }
}