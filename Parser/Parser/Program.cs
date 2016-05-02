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

        public string name = "";

        public Nonterminal(params Transition[] transitions)
        {
            this.transitions = transitions;
        }

        public void AddTransition(Transition t)
        {
            Transition[] tempTransitions = new Transition[transitions.Length+1];
            
            for(int i = 0; i < transitions.Length; i++)
            {
                tempTransitions[i] = transitions[i];
            }

            tempTransitions[tempTransitions.Length - 1] = t;

            transitions = tempTransitions;
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
                Nonterminal expr = new Nonterminal();
                expr.name = "expr";

                Nonterminal ident = new Nonterminal(new Transition(identifier));
                ident.name = "ident";
                Nonterminal op = new Nonterminal(new Transition(grammarOperator));
                op.name = "op";
                Nonterminal factor = new Nonterminal(new Transition(openParen, expr, closeParen), new Transition(integer), new Transition(ident));
                factor.name = "factor";

                Nonterminal expr_tail = new Nonterminal();
                expr_tail.name = "expr_tail";
                expr_tail.AddTransition(new Transition(op, factor, expr_tail));
                expr_tail.AddTransition(new Transition());
                expr.AddTransition(new Transition(factor, expr_tail));
                expr.AddTransition(new Transition());

                Nonterminal expr_list_tail = new Nonterminal();
                expr_list_tail.name = "expr_list_tail";
                expr_list_tail.AddTransition(new Transition(comma, expr, expr_list_tail));
                expr_list_tail.AddTransition(new Transition());
                Nonterminal expr_list = new Nonterminal(new Transition(expr, expr_list_tail));
                expr_list.name = "expr_tail";

                Nonterminal id_list_tail = new Nonterminal();
                id_list_tail.name = "id_list_tail";
                id_list_tail.AddTransition(new Transition(comma, ident, id_list_tail));
                id_list_tail.AddTransition(new Transition());

                Nonterminal id_list = new Nonterminal(new Transition(ident, id_list_tail));
                id_list.name = "id_list";

                Nonterminal stmt = new Nonterminal(new Transition(ident, assignment, expr, semicolon), new Transition(read, openParen, id_list, closeParen, semicolon), new Transition(write, openParen, expr_list, closeParen, semicolon));
                stmt.name = "stmt";

                Nonterminal stmt_list_tail = new Nonterminal();
                stmt_list_tail.name = "stmt_list_tail";
                stmt_list_tail.AddTransition(new Transition(stmt, stmt_list_tail));
                stmt_list_tail.AddTransition(new Transition());
                Nonterminal stmt_list = new Nonterminal(new Transition(stmt, stmt_list_tail));
                stmt_list.name = "stmt_list";

                Nonterminal program = new Nonterminal(new Transition(begin, stmt_list, end));
                program.name = "program";

                #endregion

                tz = new Tokenizer(File.ReadAllText(args[0]));

                Parse(program);

                if(errorEncountered)
                {
                    Console.WriteLine(tz.GetNextToken().name);
                }
                else if(tz.sourceString.Length != 0)
                {
                    switch(tz.GetNextToken().type)
                    {
                        case TokenType.ASSIGNMENT:
                            Console.WriteLine("Error 2: Unexpected assignment at position {0}.", tz.currentPosition);
                            break;

                        case TokenType.BEGIN:
                            Console.WriteLine("Error 3: Unexpected BEGIN at position {0}.", tz.currentPosition);
                            break;

                        case TokenType.CLOSE_PAREN:
                            Console.WriteLine("Error 4: Unexpected ) at position {0}.", tz.currentPosition);
                            break;

                        case TokenType.COMMA:
                            Console.WriteLine("Error 5: Unexpected , at position {0}.", tz.currentPosition);
                            break;

                        case TokenType.IDENTIFIER:
                            Console.WriteLine("Error 6: Unexpected identifier {0} at position {1}.", tz.GetNextToken().name, tz.currentPosition);
                            break;

                        case TokenType.INT:
                            Console.WriteLine("Error 7: Unexpected integer at position {0}.", tz.currentPosition);
                            break;

                        case TokenType.OPEN_PAREN:
                            Console.WriteLine("Error 8: Unexpected ( at position {0}.", tz.currentPosition);
                            break;

                        case TokenType.OPERATOR:
                            Console.WriteLine("Error 9: Unexpected {0} at position {1}.", tz.GetNextToken().name, tz.currentPosition);
                            break;

                        case TokenType.READ:
                            Console.WriteLine("Error 10: Unexpected read at position {0}.", tz.currentPosition);
                            break;

                        case TokenType.SEMICOLON:
                            Console.WriteLine("Error 11: Unexpected ; at position {0}.", tz.currentPosition);
                            break;

                        case TokenType.WRITE:
                            Console.WriteLine("Error 12: Unexpected write at position {0}.", tz.currentPosition);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("File parsed successfully with no errors.");
                }
            }
        }

        static Tokenizer tz;
        static bool wrongPath;
        static bool errorEncountered = false;

        static void Parse(GrammarSymbol symbol)
        {
            if (!errorEncountered)
            {
                if (symbol is Nonterminal)
                {
                    Nonterminal n = symbol as Nonterminal;
                    /*Console.WriteLine(n.name);
                    Console.ReadLine();*/

                    int currentPaths = 0;
                    foreach (Transition t in n.transitions)
                    {
                        currentPaths++;
                        //Console.WriteLine("I am {0} paths deep in {1}.", currentPaths, n.name);
                        wrongPath = false;
                        foreach (GrammarSymbol g in t.symbols)
                        {
                            Parse(g);
                            if (wrongPath)
                            {
                                break;
                            }
                        }

                        if (!wrongPath)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    Terminal term = symbol as Terminal;

                    Token t = tz.GetNextToken();
                    if(t.type == TokenType.ERROR)
                    {
                        errorEncountered = true;
                    }
                    else
                    {
                        /*Console.WriteLine("{0} vs {1}", t.type.ToString(), term.matchesToken.type.ToString());
                        Console.ReadLine();*/

                        if (t.type != term.matchesToken.type)
                        {
                            wrongPath = true;
                        }
                        else
                        {
                            tz.GetNextToken(true);
                        }
                    }
                }
            }
        }
    }
}
