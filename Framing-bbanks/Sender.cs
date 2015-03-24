using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS327_Framing {
    class Sender {
        public static void init() {
            bool looping = true;
            while (looping) {
                int caseSwitch;

                Console.WriteLine("\tByte Count (1)");
                Console.WriteLine("\tByte Stuffing (2)");
                Console.WriteLine("\tBit Stuffing (3)");
                Console.WriteLine("\tQuit (4)");

                while (!int.TryParse(Console.ReadLine(), out caseSwitch)) {
                    Console.WriteLine("Invalid input. Please enter the number associated with the option you want.\n");
                    Console.WriteLine("\tByte Count (1)");
                    Console.WriteLine("\tByte Stuffing (2)");
                    Console.WriteLine("\tBit Stuffing (3)");
                    Console.WriteLine("\tQuit (4)");
                }
                switch (caseSwitch) {
                    case 1:
                        Console.WriteLine("\nPlease input your frame");
                        string input = Console.ReadLine();
                        byteCount(input);
                        break;
                    case 2:
                        Console.WriteLine("\nPlease input your frame. Ex: a flag b");
                        input = Console.ReadLine();
                        byteStuffer(input);
                        break;
                    case 3:
                        Console.WriteLine("\nPlease input your frame. Ex: 011010111110100");
                        string bitString = Console.ReadLine();
                        bitStuffer(bitString);
                        break;
                    case 4:
                        looping = false;
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter the number associated with the option you want.\n");
                        break;
                }
            }
        }
        private static void byteCount(string input) {
            string[] stringArray = input.Split();
            string finalString = "";
            string partialString = "";
            foreach (string s in stringArray) {
                int stringLength = s.Length;
                if (stringLength < 10) {
                    partialString = stringLength + 1 + "" + s + " ";
                }
                else if (stringLength >= 10 && stringLength < 100) {
                    partialString = stringLength + 2 + "" + s + " ";
                }
                else {
                    partialString = stringLength + 3 + "" + s + " ";
                }
                finalString += partialString;
            }
            Console.WriteLine("\nTransmitted Frame:");
            Console.WriteLine(finalString + "\n");
        }
        private static void byteStuffer(string input) {
            string esc = "ESC";
            string flag = "FLAG";
            string[] byteArray = input.Split();
            List<string> byteList = new List<string>();
            foreach (string s in byteArray) {
                byteList.Add(s);
            }
            for(int i = 0; i < byteList.Count; i++) {
                if (byteList[i].ToUpper() == flag || byteList[i].ToUpper() == esc) {
                    byteList.Insert(i, esc);
                    i++;
                }
            }
            byteList.Insert(0, esc);
            byteList.Insert(byteList.Count, esc);
            string finalString = "";
            foreach (string s in byteList) {
                finalString += s + " ";
            }
            Console.WriteLine("\nTransmitted Frame:");
            Console.WriteLine(finalString + "\n");
        }
        private static void bitStuffer(string input) {
            char[] bitArray = input.ToCharArray();
            List<char> bitList = new List<char>();
            foreach(char c in bitArray) {
                if (c == '0' || c == '1') {
                    bitList.Add(c);
                }
                else if (c == ' ') {
                    break;
                }
                else {
                    Console.WriteLine("Only enter 0s and 1s.");
                    return;
                }
            }
            int counter = 0;
            for (int i = 0; i < bitList.Count; i++) {
                if (bitList[i] == '1') {
                    counter++;
                    if (counter == 6) {
                        bitList.Insert(i, '0');
                        counter = 0;
                    }
                }
                else {
                    counter = 0;
                }
            }
            string finalString = "";
            foreach (char c in bitList) {
                finalString += c;
            }
            Console.WriteLine("\nTransmitted Frame:");
            Console.WriteLine("111111" + finalString + "111111" + "\n");
        }
    }
}
