using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class Tokenizer
    {
        public Token GetNextToken()
        {
            Token returnToken = null;
            int tokenEnd = 1;

            while(tokenEnd < sourceString.Length)
            {
                string checkString = sourceString.Substring(0, tokenEnd);

                Console.WriteLine(checkString.ToUpper());
                Console.ReadLine();

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
                    returnToken = new Token(TokenType.OPERATOR);
                    break;
                }
                else if(checkString == ":=")
                {
                    returnToken = new Token(TokenType.ASSIGNMENT);
                    break;
                }
                
                //Integers
                bool convertedOnce = false;
                while (tokenEnd < sourceString.Length)
                {
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

                tokenEnd++;
            }

            if(tokenEnd < sourceString.Length)
            {
                sourceString = sourceString.Substring(tokenEnd);
            }
            else
            {
                returnToken = new Token(TokenType.ERROR);
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

            Console.WriteLine(this.sourceString);
        }
    }
}
