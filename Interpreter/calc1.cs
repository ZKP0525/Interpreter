using System;

namespace Interpreter
{
    // Token types
    enum TokenType
    {
        INTEGER,
        PLUS,
        EOF
    }

// Token class
    class Token
    {
        public TokenType Type { get; }
        public object Value { get; }

        public Token(TokenType type, object value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"Token({Type}, {Value})";
        }
    }

// Interpreter class
    class Interpreter
    {
        private readonly string text;
        private int pos;
        private Token currentToken;

        public Interpreter(string text)
        {
            this.text = text;
            pos = 0;
            currentToken = null;
        }

        private void Error()
        {
            throw new Exception("Error parsing input");
        }

        private Token GetNextToken()
        {
            if (pos > text.Length - 1)
            {
                return new Token(TokenType.EOF, null);
            }

            char currentChar = text[pos];

            if (char.IsDigit(currentChar))
            {
                Token token = new Token(TokenType.INTEGER, int.Parse(currentChar.ToString()));
                pos++;
                return token;
            }

            if (currentChar == '+')
            {
                Token token = new Token(TokenType.PLUS, currentChar);
                pos++;
                return token;
            }

            Error();
            return null; // Unreachable code, just to satisfy the compiler
        }

        private void Eat(TokenType tokenType)
        {
            if (currentToken.Type == tokenType)
            {
                currentToken = GetNextToken();
            }
            else
            {
                Error();
            }
        }

        public int Expr()
        {
            currentToken = GetNextToken();

            Token left = currentToken;
            Eat(TokenType.INTEGER);

            Token op = currentToken;
            Eat(TokenType.PLUS);

            Token right = currentToken;
            Eat(TokenType.INTEGER);

            return (int)left.Value + (int)right.Value;
        }
    }

    class Program
    {
        static void Main()
        {
            while (true)
            {
                try
                {
                    Console.Write("calc> ");
                    string text = Console.ReadLine();

                    if (string.IsNullOrEmpty(text))
                    {
                        continue;
                    }

                    Interpreter interpreter = new Interpreter(text);
                    int result = interpreter.Expr();
                    Console.WriteLine(result);
                }
                catch (Exception)
                {
                    break;
                }
            }
        }
    }
}