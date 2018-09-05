using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp
{
    class Program
    {
        enum Operation
        {
            Add = 1,
            Subtract,
            Multiply,
            Divide
        }
        static void Main(string[] args)
        {
            Operation op;
            op = Operation.Multiply;
            
            Console.WriteLine(op); // Add

            Console.ReadLine();
        }
    }
}
