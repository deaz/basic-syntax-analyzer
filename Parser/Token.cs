using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyzer
{
    // Возможные типы токенов
    public enum TokenType
    {
        Dim,
        Identifier,
        Comma,
        As,
        Type,
        EndOfLine,
        EndOfFile,
        Unknown
    };

    // Ключевые слова
    static class Keywords
    {
        public const string Dim = "dim";
        public const string As = "as";
        public const string Variant = "variant";
        public const string Integer = "integer";
        public const string Single = "single";
        public const string String = "string";
    }

    public class Token
    {
        #region Данные

        private string lexeme;      // Текст токена
        private TokenType type;     // Тип токена

        // Пложение первого символа токена в тексте
        private int line;
        private int character;

        #endregion

        #region Конструкторы

        public Token(string lexeme, TokenType type, int line, int character)
        {
            this.lexeme = lexeme;
            this.type = type;
            this.line = line;
            this.character = character;
        }

        public Token(string lexeme, int line, int character)
        {
            this.lexeme = lexeme;
            this.line = line;
            this.character = character;

            identify();
        }

        #endregion

        #region Методы

        public static TokenType GetType(string lexeme)
        {
            TokenType type;

            switch (lexeme.ToLower())
            {
                case Keywords.Dim:
                    type = TokenType.Dim;
                    break;
                case Keywords.As:
                    type = TokenType.As;
                    break;
                case Keywords.Variant:
                case Keywords.Integer:
                case Keywords.Single:
                case Keywords.String:
                    type = TokenType.Type;
                    break;
                case ",":
                    type = TokenType.Comma;
                    break;
                case "\n":
                    type = TokenType.EndOfLine;
                    break;
                default:
                    type = TokenType.Identifier;
                    break;
            }

            return type;
        }

        // Определение типа токена
        private void identify()
        {
            // Выбор типа токена по тексту
            type = GetType(lexeme);
        }

        #endregion

        #region Свойства

        public string Lexeme
        {
            get
            {
                return lexeme;
            }
        }

        public TokenType Type
        {
            get
            {
                return type;
            }
        }

        public int Line
        {
            get
            {
                return line;
            }
        }

        public int Character
        {
            get
            {
                return character;
            }
        }

        #endregion
    }
}