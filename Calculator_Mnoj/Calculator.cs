using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator_Mnoj
{
    internal class Calculator
    {
        public const int MAX_SETS_COUNT = 10;
        public const int MAX_NUM_COUNT = 30;
        public const char NOVALUE = (char)0;
        public List<Operation> Operations { get; set; }
        public Calculator()
        {
            Operations = new List<Operation>();
            Display();
            Event ev;
            bool isUniversum;
            Program.ColorDisplay("Задайте универсум с помощью команд 6 -n U (для ручного заполнения) или 7 -n U (для автоматического заполнения)", ConsoleColor.Green);
            do
            {
                Console.Write("\nВаш выбор: ");
                string command = Console.ReadLine();
                ev = parse(command);
                isUniversum = (ev.operation != evOperation.op_auto_input || ev.operation != evOperation.op_hand_input) && ev.resName != 'U';
                if (!isUniversum)
                {
                    handleEvent(ev);
                }
                else
                {
                    Program.ColorDisplay("Неправильный формат вводимых данных, повторите ввод\n", ConsoleColor.Red);
                }
            } while (isUniversum);

            Program.ColorDisplay("Нажмите любую клавишу для продолжения...", ConsoleColor.Green);
            Console.ReadKey();
            Console.Clear();
        }
        public void Execute()
        {
            Event ev;
            while (true)
            {
                Display();
                Console.Write("\nВаш выбор: ");
                string command = Console.ReadLine();
                ev = parse(command);
                handleEvent(ev);
                Program.ColorDisplay("Нажмите любую клавишу для продолжения...", ConsoleColor.Green);
                Console.ReadKey();
                Console.Clear();
            }
        }
        public void handleEvent(Event ev)
        {
            switch (ev.operation)
            {
                case evOperation.op_union:
                    Operation union = new Operation(Operation.GetUnion(ev.arg1.Nums, ev.arg2.Nums), NOVALUE);
                    union.Nums.Sort();
                    Console.WriteLine("Результат" + union.ToString());
                    if (ev.resName != NOVALUE)
                    {
                        if (Operations.Count < MAX_SETS_COUNT)
                        {
                            union.Name = ev.resName;
                            Operations.Add(union);
                        }
                        else
                        {
                            Calculator_Mnoj.Program.ColorDisplay($"Превышен максимальный лимит: {MAX_SETS_COUNT}", ConsoleColor.Red);
                        }
                    }
                    break;
                case evOperation.op_intersect:
                    Operation intersect = new Operation(Operation.GetIntersection(ev.arg1.Nums, ev.arg2.Nums), NOVALUE);
                    intersect.Nums.Sort();
                    Console.WriteLine("Результат" + intersect.ToString());
                    if (ev.resName != NOVALUE)
                    {
                        if (Operations.Count < MAX_SETS_COUNT)
                        {
                            intersect.Name = ev.resName;
                            Operations.Add(intersect);
                        }
                        else
                        {
                            Calculator_Mnoj.Program.ColorDisplay($"Превышен максимальный лимит: {MAX_SETS_COUNT}", ConsoleColor.Red);
                        }
                    }
                    break;
                case evOperation.op_diff:
                    Operation diff = new Operation(Operation.GetDifferenceWith(ev.arg1.Nums, ev.arg2.Nums), NOVALUE);
                    diff.Nums.Sort();
                    Console.WriteLine("Результат" + diff.ToString());
                    if (ev.resName != NOVALUE)
                    {
                        if (Operations.Count < MAX_SETS_COUNT)
                        {
                            diff.Name = ev.resName;
                            Operations.Add(diff);
                        }
                        else
                        {
                            Calculator_Mnoj.Program.ColorDisplay($"Превышен максимальный лимит: {MAX_SETS_COUNT}", ConsoleColor.Red);
                        }
                    }
                    break;
                case evOperation.op_symmetric_diff:
                    Operation symmetric_diff = new Operation(Operation.GetSymmetricDifference(ev.arg1.Nums, ev.arg2.Nums), NOVALUE);
                    symmetric_diff.Nums.Sort();
                    Console.WriteLine("Результат" + symmetric_diff.ToString());
                    if (ev.resName != NOVALUE)
                    {
                        if (Operations.Count < MAX_SETS_COUNT)
                        {
                            symmetric_diff.Name = ev.resName;
                            Operations.Add(symmetric_diff);
                        }
                        else
                        {
                            Calculator_Mnoj.Program.ColorDisplay($"Превышен максимальный лимит: {MAX_SETS_COUNT}", ConsoleColor.Red);
                        }
                    }
                    break;
                case evOperation.op_subset_of:
                    Console.WriteLine("Результат: " + ev.arg1.isSubsetOf(ev.arg2));
                    break;
                case evOperation.op_hand_input:
                    if (Operations.Count < MAX_SETS_COUNT)
                    {
                        HandInput(ev.resName);
                    }
                    else
                    {
                        Calculator_Mnoj.Program.ColorDisplay($"Превышен максимальный лимит: {MAX_SETS_COUNT}\n", ConsoleColor.Red);
                    }
                    break;
                case evOperation.op_auto_input:
                    if (Operations.Count < MAX_SETS_COUNT)
                    {
                        AutoInput(ev.resName);
                    }
                    else
                    {
                        Calculator_Mnoj.Program.ColorDisplay($"Превышен максимальный лимит: {MAX_SETS_COUNT}\n", ConsoleColor.Red);
                    }
                    break;
                case evOperation.op_delete:
                    if (ev.resName != 'U')
                    {
                        Operations.Remove(Operations.Find((Operation s) => s.Name == ev.resName));
                    }
                    else
                    {
                        Calculator_Mnoj.Program.ColorDisplay("Невозможно удалить универсум\n", ConsoleColor.Red);
                    }
                    break;
                case evOperation.op_error:
                    Calculator_Mnoj.Program.ColorDisplay("Неправильный ввод команды, повторите ввод\n", ConsoleColor.Red);
                    break;
            }
        }
        public void Display()
        {
            Console.WriteLine($"\nМаксимум сохранённых множеств: {MAX_SETS_COUNT}" +
                $"\nМаксимум чисел в множестве: {MAX_NUM_COUNT}\n" +
                "\nДоступные команды:\n" +
                "1. Объединение\t\tФормат: 1 -l (имя левого операнда) -r (имя правого операнда) -n (имя для сохранения результата)\n" +
                "2. Пересечение\t\tФормат: 2 -l (имя левого операнда -r (имя правого операнда) -n (имя для сохранения результата)\n" +
                "3. Разность\t\tФормат: 3 -l (имя левого операнда) -r (имя правого операнда) -n (имя для сохранения результата)\n" +
                "4. Симм. разность\tФормат: 4 -l (имя левого операнда) -r (имя правого операнда) -n (имя для сохранения результата)\n" +
                "5. Подмножество\t\tФормат: 5 -l (имя левого операнда) -r (имя правого операнда)\n" +
                "6. Ручной ввод\t\tФормат: 6 -n (имя для сохранения результата)\n" +
                "7. Автоматический ввод\tФормат: 7 -n (имя для сохранения результата)\n" +
                "8. Удаление\t\tФормат: 8 -d (имя удаляемого множества)\n");
            Program.ColorDisplay("Сохранённые множества:\n", ConsoleColor.DarkYellow);
            for (int i = 0; i < Operations.Count; i++)
            {
                Console.WriteLine(Operations[i]);
            }
        }
        public void HandInput(char Name)
        {
            List<int> nums = new List<int>();

            if (Name != 'U')
            {
                Operation U = Operations.Find((Operation s) => s.Name == 'U');
                int count = Program.GetInt("Введите кол-во элементов множества: ", (int num) => (num >= 1 && num <= MAX_NUM_COUNT && U.Nums.Count >= num));
                for (int i = 0, k = 1; i < count; i++, k++)
                {
                    nums.Add(Program.GetInt($"Введите [{k}] элемент множества: ", (int num) => U.Nums.Contains(num)));
                }
            }
            else
            {
                int count = Program.GetInt("Введите кол-во элементов множества: ", (int num) => (num >= 1 && num <= MAX_NUM_COUNT));
                for (int i = 0, k = 1; i < count; i++,k++)
                {
                    nums.Add(Program.GetInt($"Введите [{k}] элемент множества: ", (int num) => true));
                }
            }
            Operation result = new Operation(nums, Name);
            Operations.Add(result);
        }
        public void AutoInput(char Name)
        {
            List<int> nums = new List<int>();
            if (Name != 'U')
            {
                Operation U = Operations.Find((Operation s) => s.Name == 'U');
                int left = Program.GetInt("Введите левую границу чисел множества: ", (int num) => U.Nums.Contains(num));
                int right = Program.GetInt("Введите правую границу чисел множества: ", (int num) => U.Nums.Contains(num) && num <= left + MAX_NUM_COUNT && num > left);
                for (int i = left; i <= right; i++)
                {
                    if (U.Nums.Contains(i)) nums.Add(i);
                }
            }
            else
            {
                int left = Program.GetInt("Введите левую границу чисел множества: ", (int num) => true);
                int right = Program.GetInt("Введите правую границу чисел множества: ", (int num) => num <= left + MAX_NUM_COUNT && num > left);
                for (int i = left; i <= right; i++)
                {
                    nums.Add(i);
                }
            }
            Operation result = new Operation(nums, Name);
            Operations.Add(result);
        }
        public class Event
        {
            internal Operation arg1;
            internal Operation arg2;
            internal char resName; 
            internal evOperation operation;
        }
        public Event parse(string command)
        {
            Event ev = new Event();
            ev.resName = NOVALUE;
            command = removeSpace(command);                      
            if (command.Length >= 3)
            {
                // Распознавание команды
                switch (command.Substring(0, 1))
                {
                    case "1":
                        ev.operation = evOperation.op_union;
                        break;
                    case "2":
                        ev.operation = evOperation.op_intersect;
                        break;
                    case "3":
                        ev.operation = evOperation.op_diff;
                        break;
                    case "4":
                        ev.operation = evOperation.op_symmetric_diff;
                        break;
                    case "5":
                        ev.operation = evOperation.op_subset_of;
                        break;
                    case "6":
                        ev.operation = evOperation.op_hand_input;
                        break;
                    case "7":
                        ev.operation = evOperation.op_auto_input;
                        break;
                    case "8":
                        ev.operation = evOperation.op_delete;
                        break;
                    default:
                        ev.operation = evOperation.op_error;
                        break;
                }
                if ((int)ev.operation >= 0 && (int)ev.operation <= 3)
                {
                    getR(command, ev);
                    getL(command, ev);
                    if (getArg(command, "-n") != NOVALUE)
                    {
                        getN(command, ev);
                    }
                }
                else if ((int)ev.operation == 4)
                {
                    getR(command, ev);
                    getL(command, ev);
                }
                else if ((int)ev.operation >= 5 && (int)ev.operation <= 6)
                {
                    getN(command, ev);
                }
                else if ((int)ev.operation == 7)
                {
                    getD(command, ev);
                }
            }
            else
            {
                ev.operation = evOperation.op_error;
            }
            return ev;
        }
        private void getR(string command, Event ev)
        {
            char r_name = getArg(command, "-r");
            if (isCapitalLetter(r_name) && (Operations.Find((Operation s) => s.Name == r_name) != null))
            {
                ev.arg2 = Operations.Find((Operation s) => s.Name == r_name);
            }
            else
            {
                ev.operation = evOperation.op_error;
            }
        }
        private void getL(string command, Event ev)
        {
            char l_name = getArg(command, "-l");
            if (isCapitalLetter(l_name) && (Operations.Find((Operation s) => s.Name == l_name) != null))
            {
                ev.arg1 = Operations.Find((Operation s) => s.Name == l_name);
            }
            else
            {
                ev.operation = evOperation.op_error;
            }
        }
        private void getN(string command, Event ev)
        {
            char s_name = getArg(command, "-n");
            if (isCapitalLetter(s_name) && Operations.Find((Operation s) => s.Name == s_name) == null)
            {
                ev.resName = s_name;
            }
            else
            {
                ev.operation = evOperation.op_error;
            }
        }
        private void getD(string command, Event ev)
        {
            char d_name = getArg(command, "-d");
            if (isCapitalLetter(d_name) && Operations.Find((Operation s) => s.Name == d_name) != null)
            {
                ev.resName = d_name;
            }
            else
            {
                ev.operation = evOperation.op_error;
            }
        }
        private static char getArg(string command, string prefix)
        {
            int arg = command.IndexOf(prefix);
            if (arg != -1 && command.Length > arg + 2)
            {
                return command.Substring(arg + 2, 1)[0];
            }
            else
            {
                return NOVALUE;
            }
        }
        private static bool isCapitalLetter(char letter)
        {
            if ((int)letter >= 65 && (int)letter <= 90)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private static string removeSpace(string command)
        {
            for (int i = 0; i < command.Length; i++)
            {
                if (command[i] == ' ')
                {
                    command = command.Remove(i--, 1);
                }
            }
            return command;
        }
        public enum evOperation
        {
            op_union = 0,
            op_intersect = 1,
            op_diff = 2,
            op_symmetric_diff = 3,
            op_subset_of = 4,
            op_hand_input = 5,
            op_auto_input = 6,
            op_delete = 7,
            op_error = 100
        }
    }
}
