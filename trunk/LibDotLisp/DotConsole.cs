using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LibDotLisp
{
    public class DotConsole
    {
        private Interpreter minterpreter;

        public DotConsole()
        {
            minterpreter = new Interpreter();
            minterpreter.LoadFile("boot.lisp");
        }
        public void DoDefine(string str)
        {
            object r, x;
            TextReader treader;

            treader = new StringReader(str + "\n");
            r = minterpreter.Read("console", treader);
            x = minterpreter.Eval(r);           
        }
        public object Eval(string str)
        {
            object r, oresult;
            TextReader treader;

            treader = new StringReader(str + "\n");
            r = minterpreter.Read("console", treader);
            oresult = minterpreter.Eval(r);
            Console.WriteLine(oresult + " : " + oresult.GetType().Name);          
            return oresult;
        }

    }
}
