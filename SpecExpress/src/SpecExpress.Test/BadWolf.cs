using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpecExpressTest
{
    public class BadWolf
    {
        public BadWolf()
        {
            //throw new InvalidProgramException("Bad Wolf!");
        }

        public bool IsTrue
        {
            get { return true;}
        }

        public int Max(int value)
        {
            return 10/value;
        }
    }
}
