using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator_Mnoj
{
    class Program
    {
        static void Main()
        {
            Calculator calc = new Calculator();
            calc.Execute();
        }
        // Цветной вывод 
        public static void ColorDisplay(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        public static int GetInt(string inputMessage, Predicate<int> condition)
        {
            int result;
            bool isCorrect;
            do
            {
                Console.Write(inputMessage);
                isCorrect = int.TryParse(Console.ReadLine(), out result) && condition(result);
                if (!isCorrect)
                {
                    ColorDisplay("Неправильный формат вводимых данных, повторите ввод =>\n", ConsoleColor.Red);
                }
            } while (!isCorrect);
            return result;
        }
    }
}
