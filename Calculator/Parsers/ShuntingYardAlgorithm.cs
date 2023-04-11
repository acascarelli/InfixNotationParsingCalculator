using Calculator.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Calculator.Parsers
{
    public class ShuntingYardAlgorithm
    {
        public ShuntingYardAlgorithm()
        {
            _operatorStack = new Stack<OperatorToken>();
            _postfixNotationTokens = new List<IToken>();
        }

        public IEnumerable<IToken> Apply(IEnumerable<IToken> infixnotationTokens)
        {
            Reset();
            foreach (var token in infixnotationTokens)
            {
                ProcessToken(token);
            }
            return GetResult();
        }

        private void ProcessToken(IToken token)
        {
            switch (token)
            {
                case OperandToken operandToken:
                    StoreOperand(operandToken);
                    break;
                case OperatorToken operatorToken:
                    ProcessOperator(operatorToken);
                    break;
                default:
                    var exMessage = $"An unknown token type: {token.GetType()}.";
                    throw new ArgumentException(exMessage);
            }
        }

        private void StoreOperand(OperandToken operandToken)
        {
            _postfixNotationTokens.Add(operandToken);
        }

        private void ProcessOperator(OperatorToken operatorToken)
        {
            switch (operatorToken.OperatorType)
            {
                case OperatorType.OpeningBracket:
                    PushOpeningBracket(operatorToken);
                    break;
                case OperatorType.ClosingBracket:
                    PushClosingBracket();
                    break;
                default:
                    PushOperator(operatorToken);
                    break;
            }
        }

        private void PushOpeningBracket(OperatorToken operatorToken)
        {
            _operatorStack.Push(operatorToken);
        }

        private void PushClosingBracket()
        {
            bool openingBracketFound = false;

            while (_operatorStack.Count > 0)
            {
                var stackOperatorToken = _operatorStack.Pop();
                if (stackOperatorToken.OperatorType == OperatorType.OpeningBracket)
                {
                    openingBracketFound = true;
                    break;
                }

                _postfixNotationTokens.Add(stackOperatorToken);
            }

            if (!openingBracketFound)
            {
                throw new ArgumentException("An unexpected closing bracket.");
            }
        }

        private void PushOperator(OperatorToken operatorToken)
        {
            var operatorPriority = GetOperatorPriority(operatorToken);

            while (_operatorStack.Count > 0)
            {
                var stackOperatorToken = _operatorStack.Peek();
                if (stackOperatorToken.OperatorType == OperatorType.OpeningBracket)
                {
                    break;
                }

                var stackOperatorPriority = GetOperatorPriority(stackOperatorToken);
                if (stackOperatorPriority < operatorPriority)
                {
                    break;
                }

                _postfixNotationTokens.Add(_operatorStack.Pop());
            }

            _operatorStack.Push(operatorToken);
        }

        private static int GetOperatorPriority(OperatorToken operatorToken)
        {
            switch (operatorToken.OperatorType)
            {
                case OperatorType.OpeningBracket:
                    return 0;
                case OperatorType.Addition:
                case OperatorType.Subtraction:
                    return 1;
                case OperatorType.Multiplication:
                case OperatorType.Division:
                    return 2;
                default:
                    var exMessage = "An unexpected action for the operator: " +
                        $"{operatorToken.OperatorType}.";
                    throw new ArgumentException(exMessage);
            }
        }

        private List<IToken> GetResult()
        {
            while (_operatorStack.Count > 0)
            {
                var token = _operatorStack.Pop();
                if (token.OperatorType == OperatorType.OpeningBracket)
                {
                    throw new ArgumentException("A redundant opening bracket.");
                }
                _postfixNotationTokens.Add(token);
            }
            return _postfixNotationTokens.ToList();
        }

        private void Reset()
        {
            _operatorStack.Clear();
            _postfixNotationTokens.Clear();
        }

        private readonly Stack<OperatorToken> _operatorStack;
        private readonly List<IToken> _postfixNotationTokens;
    }
}
