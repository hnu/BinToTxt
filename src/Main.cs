using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestoreOriginal
{
    class Program
    {
    static void Main(string[] args)
    {
        if(args.Count() != 1){

            } else {
                string fname = args[0];
                ProcessFile pf = new ProcessFile();
                pf.read(fname);
            }
    }
    }
}
