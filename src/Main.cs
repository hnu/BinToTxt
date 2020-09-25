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
        if(args.Count() != 2){

            } else {
                string fname = args[0];
                string fname_out = args[1];
                ProcessFile pf = new ProcessFile();
                pf.read(fname, fname_out);
            }
    }
    }
}
