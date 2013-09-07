using System;
using System.Collections.Generic;
using System.Text;

namespace SyntaxAnalyzer
{
    public class Lexer
    {
        #region Данные

        // Словарь допустимых символов
        private const string CorrectCharacters = "abcdefghijklmnopqrstuvwxyz, \t\n";

        // Исходный код
        private string source;

        // Индекс текущего символа для просмотра
        private int index;

        // Позиция текущего символа в исходном тексте
        private int line;
        private int character;

        #endregion

        #region Конструкторы

        public Lexer(string source)
        {
            this.source = string.Copy(source);

            index = 0;

            line = 1;
            character = 1;
        }

        #endregion

        #region Методы

        // Получение следующего токена из исходного текста
        public Token GetNextToken()
        {
            StringBuilder lexeme = new StringBuilder();
            int firstCharacter = character;     // Положение первого символа токена в строке

            // Если текст закончился, то конец файла
            if (index == source.Length)
            {
                return new Token(null, TokenType.EndOfFile, line, character);
            }

            // Проход по тексту
            for (; index < source.Length; ++index)
            {
                char ch = source[index];

                // Пропуск незначащих символов
                if (ch == ' ' || ch == '\t')
                {
                    if (lexeme.Length > 0)
                    {
                        // Заканчиваем построение лексемы, если лексема уже что-то содержит
                        break;
                    }
                    else
                    {
                        // Пропуск символа перед началом лексемы
                        ++firstCharacter;
                    }
                }
                else if (ch == '\n' || ch == ',')
                {
                    if (lexeme.Length == 0)
                    {
                        // Если лексема ничего не содержит, то в качестве лексемы используется данный символ                        
                        lexeme.Append(ch);
                        ++index;    // Чтобы не считывалась одна и та же лексема при последующих вызовах

                        updateCursorOffset(ch);
                    }

                    // Если лексема уже что-то содержала, то символ выступает в качестве разделителя

                    break;
                }
                else if (isCorrectCharacter(ch))
                {
                    lexeme.Append(ch);
                }
                else
                {
                    // Найден несловарный символ
                    firstCharacter = character;
                    return new Token(ch.ToString(), TokenType.Unknown, line, firstCharacter);
                }

                updateCursorOffset(ch);
            }

            return new Token(lexeme.ToString(), line, firstCharacter);
        }

        // Устанавливает, какой следующий токен вернет метод GetNextToken(). Возвращает тип найденного токена.
        public TokenType SetNextToken(params string[] lexemes)
        {
            for (; index < source.Length; ++index)
            {
                foreach (string lexeme in lexemes)
                {
                    if (string.Compare(source, index, lexeme, 0, lexeme.Length, true) == 0)
                    {
                        return Token.GetType(lexeme);
                    }
                }

                updateCursorOffset(source[index]);
            }

            return TokenType.EndOfFile;
        }

        private bool isCorrectCharacter(char character)
        {
            foreach (char ch in CorrectCharacters)
            {
                // Сравнение символов с игнорированием регистра
                if (string.Compare(ch.ToString(), character.ToString(), true) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        // Обновляет положение курсора в исходном тексте
        private void updateCursorOffset(char character)
        {
            if (character == '\n')
            {
                ++line;
                this.character = 1;
            }
            else
            {
                ++this.character;
            }
        }

        #endregion
    }
}