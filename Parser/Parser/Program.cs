using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Parser
{
    #region GrammarClasses
    class GrammarSymbol{}

    class Terminal : GrammarSymbol
    {
        public Token matchesToken;

        public Terminal(Token matchesToken)
        {
            this.matchesToken = matchesToken;
        }
    }

    class Nonterminal : GrammarSymbol
    {
        public Transition[] transitions;

        public Nonterminal(params Transition[] transitions)
        {
            this.transitions = transitions;
        }
    }

    class Transition
    {
        public GrammarSymbol[] symbols;

        public Transition(params GrammarSymbol[] symbols)
        {
            this.symbols = symbols;
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            /*args = new string[1];
            args[0] = "testProgram1";*/
            if(args.Length != 1) //Wrong number of command line arguments
            {
                Console.WriteLine("Usage: Parser.exe <name of file>");
            }
            else
            {
                #region GrammarSetup

                //Terminals
                Terminal grammarOperator = new Terminal(new Token(TokenType.OPERATOR));
                Terminal identifier = new Terminal(new Token(TokenType.IDENTIFIER));
                Terminal integer = new Terminal(new Token(TokenType.INT));
                Terminal openParen = new Terminal(new Token(TokenType.OPEN_PAREN));
                Terminal closeParen = new Terminal(new Token(TokenType.CLOSE_PAREN));
                Terminal begin = new Terminal(new Token(TokenType.BEGIN));
                Terminal end = new Terminal(new Token(TokenType.END));
                Terminal read = new Terminal(new Token(TokenType.READ));
                Terminal write = new Terminal(new Token(TokenType.WRITE));
                Terminal comma = new Terminal(new Token(TokenType.COMMA));
                Terminal semicolon = new Terminal(new Token(TokenType.SEMICOLON));
                Terminal assignment = new Terminal(new Token(TokenType.ASSIGNMENT));

                //Nonterminals
                Nonterminal ident = new Nonterminal(new Transition(identifier));
                Nonterminal op = new Nonterminal(new Transition(grammarOperator));
                Nonterminal factor = new Nonterminal(new Transition(integer), new Transition(ident), new Transition(openParen, new Nonterminal());

                #endregion

                tz = new Tokenizer(File.ReadAllText(args[0]));

            }
        }

        static Tokenizer tz;

        static void Parse(GrammarSymbol symbol)
        {
            if(symbol is Nonterminal)
            {
                Nonterminal n = symbol as Nonterminal;
                foreach(Transition t in n.transitions)
                {
                    foreach(GrammarSymbol g in t.symbols)
                    {
                        Parse(g);
                    }
                }
            }
            else
            {
                Terminal term = symbol as Terminal;

                
            }
        }
    }
}
