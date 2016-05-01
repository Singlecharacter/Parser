using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Parser
{
    public enum TokenType
    {
        BEGIN,
        END,
        READ,
        WRITE,
        INT,
        OPERATOR,
        IDENTIFIER,
        OPEN_PAREN,
        CLOSE_PAREN,
        SEMICOLON,
        COMMA,
        ASSIGNMENT,
        ERROR
    }

    /// <summary>
    /// Token represents a singular piece of the grammar; i.e. an identifier, keyword, or operator.
    /// </summary>
    public class Token
    {
        public TokenType type;

        /// <summary>
        /// Only used on identifier tokens, empty string on all others
        /// </summary>
        public string name;

        public Token(TokenType type, string name = "")
        {
            this.type = type;
            this.name = name;
        }

        public Token() { }
    }
}
