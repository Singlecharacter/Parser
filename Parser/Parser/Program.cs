using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            args = new string[1];
            args[0] = "testProgram1";
            if(args.Length != 1) //Wrong number of command line arguments
            {
                Console.WriteLine("Usage: Parser.exe <name of file>");
            }
            else
            {
                Tokenizer tz = new Tokenizer(File.ReadAllText(args[0]));

                Token t = tz.GetNextToken();
                while(t.type != TokenType.ERROR)
                {
                    Console.WriteLine(t.type.ToString());
                    t = tz.GetNextToken();
                }

                Console.ReadLine();
            }
        }
    }
}
