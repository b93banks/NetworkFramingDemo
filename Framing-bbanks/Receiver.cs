using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS327_Framing {
    class Receiver {
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
                        Console.WriteLine("\nPlease input the received frame");
                        string input = Console.ReadLine();
                        byteCount(input);
                        break;
                    case 2:
                        Console.WriteLine("\nPlease input your frame. Ex: flag a flag b flag");
                        input = Console.ReadLine();
                        byteStuffer(input);
                        break;
                    case 3:
                        Console.WriteLine("\nPlease input your frame. Ex: 111111011011111011111011111010010111111");
                        input = Console.ReadLine();
                        bitStuffer(input);
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
            foreach (string s in stringArray) {
                char[] bitArray = s.ToCharArray();
                int num;
                string partialString = "";
                if (!int.TryParse(bitArray[0].ToString(), out num)) {
                    Console.WriteLine("The received frame has an error. First character in frame isn't a number.\n");
                    return;
                }
                else if (num != bitArray.Count()) {
                    Console.WriteLine("The received frame has an error.\n");
                    return;
                }
                else {
                    for (int i = 1; i < bitArray.Count(); i++) {
                        partialString += bitArray[i];
                    }
                }
                finalString += partialString + " ";
            }
            Console.WriteLine(finalString + "\n");
        }
        private static void byteStuffer(string input) {
            string esc = "ESC";
            string flag = "FLAG";
            string finalString = "";
            string[] byteArray = input.Split();
            List<string> byteList = new List<string>();
            foreach (string s in byteArray) {
                byteList.Add(s);
            }
            if (checkFlags(byteList)) {
                for (int i = 0; i < byteList.Count; i++) {
                    if (byteList[i].ToUpper() == esc) {
                        if (byteList[i - 1].ToUpper() == esc) {
                            byteList[i - 1] = "";
                        }
                    }
                    if (byteList[i].ToUpper() == flag) {
                        if (byteList[i - 1].ToUpper() == esc) {
                            byteList[i - 1] = "";
                        }
                        else {
                            Console.WriteLine("Received frame has an error. There was no esc for flag.\n");
                            return;
                        }
                    }
                }
            }
            else {
                Console.WriteLine("Received frame has an error. It didn't pass the flag check.\n");
                return;
            }
            foreach (string s in byteList) {
                finalString += s + " ";
            }
            Console.WriteLine(finalString + "\n");
        }
        //private static bool checkByteFlags(string input) {
        //    int counter = 0;
        //    if (input.StartsWith("esc") && input.EndsWith("esc")) {
        //        input = input.Substring(3, input.Length - 3);
        //        for (int i = 0; i < input.Length; i++) {
        //            if (input.Substring(i, 3) == "esc" || input.Substring(i, 4) == "flag") {
        //                counter++;
        //            }
        //        }
        //        if (counter % 2 == 0) {
        //            return true;
        //        }
        //        else {
        //            return false;
        //        }
        //    }
        //    else {
        //        return false;
        //    }
        //}
        private static bool checkFlags(List<string> list) {
            int counter = 0;
            string esc = "ESC";
            string flag = "FLAG";
            int lastItem = list.Count - 1;
            if (list[0].ToUpper() != esc || list[lastItem].ToUpper() != esc) {
                return false;
            }
            else {
                list[0] = "";
                list[lastItem] = "";
                for (int i = 0; i < list.Count; i++) {
                    if (list[i].ToUpper() == esc || list[i].ToUpper() == flag) {
                        counter++;
                    }
                }
                if (counter % 2 == 0) {
                    return true;
                }
                else {
                    return false;
                }
            }
        }
        private static void bitStuffer(string input) {
            if (input.Contains("[^0-1]")) {
                Console.WriteLine("Received frame has an error. Not just 0s and 1s.\n");
            }
            else {
                if (checkBitBookendFlags(input)) {
                    input = removeBookendFlags(input);
                    if (input.Contains("111111")) {
                        Console.WriteLine("Received frame has an error.\n");
                        return;
                    }
                    else {
                        if (input.Contains("111110")) {
                            input = input.Replace("111110", "11111");
                            Console.WriteLine(input);
                        }
                    }
                }
                else {
                    Console.WriteLine("Received frame has an error. The bookend flags were incorrect.");
                }
            }
        }
        private static bool checkBitBookendFlags(string input) {
            if (input.StartsWith("111111") && input.EndsWith("111111")) {
                return true;
            }
            else {
                return false;
            }
        }
        private static string removeBookendFlags(string input) {
            string trimmedString = input.Substring(6, input.Length - 12);
            return trimmedString;
        }
        private static void removeFlags(List<char> list) {
            int flagSize = 6;
            int count = list.Count + 1;
            for (int i = 0; i < flagSize; i++) {
                list.RemoveAt(0);
            }
            for (int i = 0; i < flagSize; i++) {
                list.RemoveAt(list.Count - 1);
            }
        }
        private static bool checkAndRemoveZeros(List<char> list) {
            int counter = 0;
            for (int i = 0; i < list.Count; i++) {
                if (list[i] == '1') {
                    counter++;
                    if (counter == 5) {
                        if (list[i + 1] == '0') {
                            list.RemoveAt(i + 1);
                            counter = 0;
                        }
                        else {
                            return false;
                        }
                    }
                }
                else {
                    counter = 0;
                }
            }
            return true;
        }
    }
}
