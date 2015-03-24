using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS327_Framing {
    class Program {
        private static void Main(string[] args) {
            bool looping = true;
            while (looping) {
                Console.WriteLine("Would you like to Send or Receive?");
                Console.WriteLine("\tSend (1)");
                Console.WriteLine("\tReceive (2)");
                Console.WriteLine("\tQuit (3)");
                int caseSwitch;
                while (!int.TryParse(Console.ReadLine(), out caseSwitch)) {
                    Console.WriteLine("Incorrect input. Please enter the number associated with the option you want.\n");
                    Console.WriteLine("Would you like to Send or Receive?");
                    Console.WriteLine("\tSend (1)");
                    Console.WriteLine("\tReceive (2)");
                    Console.WriteLine("\tQuit (3)");
                }
                switch (caseSwitch) {
                    case 1:
                        Sender.init();
                        break;
                    case 2:
                        Receiver.init();
                        break;
                    case 3:
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Incorrect Input. Please enter the number associated with the option you want.\n");
                        break;
                }
            }
        }
    }
}
