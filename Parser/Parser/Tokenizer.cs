using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class Tokenizer
    {
        private int currentPosition;

        public Token GetNextToken()
        {
            if(sourceString.Length < 1)
            {
                return new Token(TokenType.ERROR, "Error 0: Unexpected end of file.");
            }
            Token returnToken = null;
            int tokenEnd = 1;

            //Integers
            bool convertedOnce = false;
            while (tokenEnd < sourceString.Length)
            {
                string checkString = sourceString.Substring(0, tokenEnd);

                try
                {
                    Convert.ToInt32(checkString);

                    tokenEnd++;

                    checkString = sourceString.Substring(0, tokenEnd);

                    convertedOnce = true;
                }
                catch (Exception e)
                {
                    if (convertedOnce)
                    {
                        tokenEnd--;

                        checkString = sourceString.Substring(0, tokenEnd);

                        returnToken = new Token(TokenType.INT, checkString);
                    }

                    break;
                }
            }

            if (returnToken == null)
            {
                tokenEnd = 1;
            }

            //Identifiers
            bool startsWithLetter = Char.IsLetter(sourceString[0]);
            if(startsWithLetter)
            {
                while(tokenEnd < sourceString.Length)
                {
                    string checkString = sourceString.Substring(0, tokenEnd);

                    if ((!Char.IsLetterOrDigit(checkString, checkString.Length - 1) && checkString[checkString.Length - 1] != '_')
                        && checkString.ToUpper() != "READ"
                        && checkString.ToUpper() != "WRITE"
                        && checkString.ToUpper() != "BEGIN"
                        && checkString.ToUpper() != "END")
                    {
                        tokenEnd--;
                        returnToken = new Token(TokenType.IDENTIFIER, sourceString.Substring(0,tokenEnd));
                        break;
                    }
                    else if (checkString.ToUpper() == "READ" || checkString.ToUpper() == "WRITE" || checkString.ToUpper() == "BEGIN" || checkString.ToUpper() == "END")
                    {
                        break;
                    }

                    tokenEnd++;
                }
            }

            if (returnToken == null)
            {
                tokenEnd = 1;
            }

            while(tokenEnd <= sourceString.Length && returnToken == null)
            {
                string checkString = sourceString.Substring(0, tokenEnd);

                //Predefined symbols and keywords
                if(checkString.ToUpper() == "READ")
                {
                    returnToken = new Token(TokenType.READ);
                    break;
                }
                else if(checkString.ToUpper() == "WRITE")
                {
                    returnToken = new Token(TokenType.WRITE);
                    break;
                }
                else if(checkString.ToUpper() == "BEGIN")
                {
                    returnToken = new Token(TokenType.BEGIN);
                    break;
                }
                else if(checkString.ToUpper() == "END")
                {
                    returnToken = new Token(TokenType.END);
                    break;
                }
                else if(checkString == "(")
                {
                    returnToken = new Token(TokenType.OPEN_PAREN);
                    break;
                }
                else if(checkString == ")")
                {
                    returnToken = new Token(TokenType.CLOSE_PAREN);
                    break;
                }
                else if(checkString == ",")
                {
                    returnToken = new Token(TokenType.COMMA);
                    break;
                }
                else if(checkString == ";")
                {
                    returnToken = new Token(TokenType.SEMICOLON);
                    break;
                }
                else if(checkString == "+" || checkString == "-")
                {
                    returnToken = new Token(TokenType.OPERATOR, checkString);
                    break;
                }
                else if(checkString == ":=")
                {
                    returnToken = new Token(TokenType.ASSIGNMENT);
                    break;
                }

                tokenEnd++;
            }

            if(tokenEnd < sourceString.Length)
            {
                sourceString = sourceString.Substring(tokenEnd);
                currentPosition += tokenEnd;
            }
            else if(returnToken == null)
            {
                returnToken = new Token(TokenType.ERROR, "Error 1: Unidentified token at position " + currentPosition);
            }
            else
            {
                sourceString = "";
            }

            return returnToken;
        }

        private string sourceString;

        public Tokenizer(string source)
        {
            this.sourceString = source;
            this.sourceString = this.sourceString.Replace(" ", "");
            this.sourceString = this.sourceString.Replace("\n", "");
            this.sourceString = this.sourceString.Replace("\r", "");

            currentPosition = 0;
        }
    }
}
