using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calculator.Tokens
{
    public class Tokenizer
    {
        public Tokenizer()
        {
            _valueTokenBuilder = new StringBuilder();
            _infixNotationTokens = new List<IToken>();
            _validOperators = new char[] { '(', ')', '+', '-', '*', '/' };
        }

        public IEnumerable<IToken> Parse(string expression)
        {
            Reset();
            foreach (char next in expression)
            {
                FeedCharacter(next);
            }
            return GetResult();
        }

        private void FeedCharacter(char next)
        {
            if (IsOperatorCharacter(next))
            {
                if (_valueTokenBuilder.Length > 0)
                {
                    var token = CreateOperandToken(_valueTokenBuilder.ToString());
                    _valueTokenBuilder.Clear();
                    _infixNotationTokens.Add(token);
                }

                var operatorToken = CreateOperatorToken(next);
                _infixNotationTokens.Add(operatorToken);
            }
            else
            {
                _valueTokenBuilder.Append(next);
            }
        }

        private bool IsOperatorCharacter(char next) => _validOperators.Contains(next);

        private IToken CreateOperandToken(string raw)
        {
            if (double.TryParse(raw, out double result))
                return new OperandToken(result);

            throw new ArgumentException($"The operand {raw} has an invalid format.");
        }


        private IToken CreateOperatorToken(char next) => next switch
        {
            '(' => new OperatorToken(OperatorType.OpeningBracket),
            ')' => new OperatorToken(OperatorType.ClosingBracket),
            '+' => new OperatorToken(OperatorType.Addition),
            '-' => new OperatorToken(OperatorType.Subtraction),
            '*' => new OperatorToken(OperatorType.Multiplication),
            '/' => new OperatorToken(OperatorType.Division),
            _ => throw new ArgumentException($"There's not a suitable operator for the char {next}")

        };

        private void Reset()
        {
            _valueTokenBuilder.Clear();
            _infixNotationTokens.Clear();
        }

        private IEnumerable<IToken> GetResult()
        {
            if(_valueTokenBuilder.Length > 0)
            {
                var token = CreateOperandToken(_valueTokenBuilder.ToString());
                _valueTokenBuilder.Clear();
                _infixNotationTokens.Add(token);
            }

            return _infixNotationTokens.ToList();
        }

        private readonly StringBuilder _valueTokenBuilder;
        private readonly List<IToken> _infixNotationTokens;
        private readonly char[] _validOperators;
    }
}
