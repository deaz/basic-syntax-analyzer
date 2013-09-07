using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyzer
{
    // Возможные состояния автомата
    enum State
    {
        A,
        B,
        C,
        D,
        E
    }

    public class Parser
    {
        #region Данные

        private Lexer lexer;

        // Текущее состояние
        private State state;

        // Ссылка на список сообщений анализатора
        List<string> messages;

        #endregion

        #region Конструкторы

        public Parser(string source, List<string> messages)
        {
            lexer = new Lexer(source);
            this.messages = messages;
        }

        #endregion

        #region Методы

        public void Start()
        {
            state = State.A;

            Token token;
            do
            {
                token = lexer.GetNextToken();
                move(token);
            }
            while (token.Type != TokenType.EndOfFile);

            messages.Add("Анализ завершён. Ошибок: " + messages.Count);
        }

        // Перевод автомата в новое состояние
        private void move(Token token)
        {
            if (token.Type == TokenType.Unknown)
            {
                unknownCharacterError(token);
                return;
            }

            switch (state)
            {
                case State.A:
                    if (token.Type == TokenType.Dim)
                    {
                        state = State.B;
                    }
                    else if (token.Type == TokenType.EndOfLine || token.Type == TokenType.EndOfFile)
                    {
                        state = State.A;
                    }
                    else
                    {
                        unexpectedTokenError(TokenType.Dim, token);
                    }
                    break;
                case State.B:
                    if (token.Type == TokenType.Identifier)
                    {
                        state = State.C;
                    }
                    else
                    {
                        unexpectedTokenError(TokenType.Identifier, token);
                    }
                    break;
                case State.C:
                    if (token.Type == TokenType.Comma)
                    {
                        state = State.B;
                    }
                    else if (token.Type == TokenType.As)
                    {
                        state = State.D;
                    }
                    else if (token.Type == TokenType.EndOfLine || token.Type == TokenType.EndOfFile)
                    {
                        state = State.A;
                    }
                    else
                    {
                        unexpectedTokenError(TokenType.Comma, token);
                    }
                    break;
                case State.D:
                    if (token.Type == TokenType.Type)
                    {
                        state = State.E;
                    }
                    else
                    {
                        unexpectedTokenError(TokenType.Type, token);
                    }
                    break;
                case State.E:
                    if (token.Type == TokenType.Comma)
                    {
                        state = State.B;
                    }
                    else if (token.Type == TokenType.EndOfLine || token.Type == TokenType.EndOfFile)
                    {
                        state = State.A;
                    }
                    else
                    {
                        unexpectedTokenError(TokenType.Comma, token);
                    }
                    break;
            }
        }

        // Обработка нахождения неожиданного токена
        private void unexpectedTokenError(TokenType expected, Token got)
        {
            StringBuilder error = new StringBuilder("Строка " + got.Line + " символ " + got.Character + ": Ожидается ");

            switch (expected)
            {
                case TokenType.Dim:
                    error.Append("Dim,");
                    break;
                case TokenType.As:
                    error.Append("As,");
                    break;
                case TokenType.Comma:
                    error.Append("запятая,");
                    break;
                case TokenType.EndOfLine:
                    error.Append("конец файла,");
                    break;
                case TokenType.Identifier:
                    error.Append("идентификатор,");
                    break;
                case TokenType.Type:
                    error.Append("название типа,");
                    break;
                default:
                    break;
            }

            switch (got.Type)
            {
                case TokenType.Identifier:
                    error.Append(" получен идентификатор '" + got.Lexeme + "'");
                    break;
                case TokenType.As:
                    error.Append(" получен As");
                    break;
                case TokenType.Comma:
                    error.Append(" получена запятая");
                    break;
                case TokenType.Dim:
                    error.Append(" получен Dim");
                    break;
                case TokenType.EndOfLine:
                    error.Append(" получен конец файла");
                    break;
                case TokenType.Type:
                    error.Append(" получено название типа " + got.Lexeme);
                    break;
                case TokenType.EndOfFile:
                    error.Append(" получен конец файла");
                    break;
                default:
                    break;
            }

            messages.Add(error.ToString());

            skipTokens();
        }

        // Обработка нахождения неизвестного символа
        private void unknownCharacterError(Token token)
        {
            string error = "Строка " + token.Line + " символ " + token.Character + ": Неизвестный символ '" + token.Lexeme + "'";
            messages.Add(error);

            skipTokens();
        }

        // Пропуск токенов до нахождения синхронизирующего символа
        private void skipTokens()
        {
            // В зависимости от состояния автомата ищется следующий токен
            TokenType type;
            if (state == State.A)
            {
                // Небольшой воркэраунд
                state = State.B;
                return;
                //type = lexer.SetNextToken("\n", ",");
            }
            else
            {
                type = lexer.SetNextToken("\n", ",");
            }

            // В зависимости от найденного токена ставится состояние
            if (type == TokenType.Comma)
            {
                state = State.C;
            }
            else
            {
                state = State.A;
            }
        }

        #endregion
    }
}
