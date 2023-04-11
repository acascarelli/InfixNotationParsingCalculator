using Calculator.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Parsers
{
    public class PostfixNotationCalculator
    {
        public PostfixNotationCalculator()
        {
            _operandTokenStack = new Stack<OperandToken>();
        }

        public OperandToken Calculate(IEnumerable<IToken> tokens)
        {
            Reset();
            foreach (var token in tokens)
            {
                ProcessToken(token);
            }
            return GetResults();
        }

        private void ProcessToken(IToken token)
        {
            switch (token)
            {
                case OperandToken operandToken:
                    StoreOperand(operandToken);
                    break;
                case OperatorToken operatorToken:
                    ApplyOperator(operatorToken);
                    break;
                default:
                    var exMessage = $"An unknown token type: {token.GetType()}.";
                    throw new ArgumentException(exMessage);
            }
        }

        private void StoreOperand(OperandToken operandToken)
        {
            _operandTokenStack.Push(operandToken);
        }

        private void ApplyOperator(OperatorToken operatorToken)
        {
            switch (operatorToken.OperatorType)
            {
                case OperatorType.Addition:
                    ApplyAdditionOperator();
                    break;
                case OperatorType.Subtraction:
                    ApplySubtractionOperator();
                    break;
                case OperatorType.Multiplication:
                    ApplyMultiplicationOperator();
                    break;
                case OperatorType.Division:
                    ApplyDivisionOperator();
                    break;
                default:
                    var exMessage = $"An Unknown operator type: {operatorToken.OperatorType}.";
                    throw new ArgumentException(exMessage);
            }
        }

        private void ApplyDivisionOperator()
        {
            var operands = GetBinaryOperatorArguments();
            var result = new OperandToken(operands.Item1.Value / operands.Item2.Value);
            _operandTokenStack.Push(result);
        }
        

        private void ApplyMultiplicationOperator()
        {
            var operands = GetBinaryOperatorArguments();
            var result = new OperandToken(operands.Item1.Value * operands.Item2.Value);
            _operandTokenStack.Push(result);
        }

        private void ApplySubtractionOperator()
        {
            var operands = GetBinaryOperatorArguments();
            var result = new OperandToken(operands.Item1.Value - operands.Item2.Value);
            _operandTokenStack.Push(result);
        }

        private void ApplyAdditionOperator()
        {
            var operands = GetBinaryOperatorArguments();
            var result = new OperandToken(operands.Item1.Value + operands.Item2.Value);
            _operandTokenStack.Push(result);
        }

        private Tuple<OperandToken, OperandToken> GetBinaryOperatorArguments()
        {
            if (_operandTokenStack.Count < 2)
            {
                var exMessage = "Not enought arguments for applying a binary operator.";
                throw new ArgumentException(exMessage);
            }

            var right = _operandTokenStack.Pop();
            var left = _operandTokenStack.Pop();

            return Tuple.Create(left, right);
        }

        private OperandToken GetResults()
        {
            if (_operandTokenStack.Count == 0)
            {
                var exMessage = "The expression is invalid." +
                    " Check, please, that the expression is not empty.";
                throw new ArgumentException(exMessage);
            }

            if (_operandTokenStack.Count != 1)
            {
                var exMessage = "The expression is invalid." +
                    " Check, please, that your're providing the full express and" +
                    " the tokens have a correct order.";
                throw new ArgumentException(exMessage);
            }

            return _operandTokenStack.Pop();
        }

        private void Reset()
        {
            _operandTokenStack.Clear();
        }

        private readonly Stack<OperandToken> _operandTokenStack;
    }
}
