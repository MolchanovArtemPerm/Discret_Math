using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator_Mnoj
{
    internal class Operation
    {
        public List<int> Nums { get; internal set; }
        public char Name { get; internal set; }
        public Operation(List<int> nums, char name)
        {
            Nums = new List<int>(nums);
            RemoveDuplicates(Nums);
            Nums.Sort();
            Name = name;
        }
        public Operation(Operation s)
        {
            Nums = new List<int>(s.Nums);
            RemoveDuplicates(Nums);
            Nums.Sort();
            Name = s.Name;
        }

        // Удаление дубликатов
        public static void RemoveDuplicates(List<int> l)
        {
            for (int i = 0; i < (l.Count - 1); i++)
            {
                for (int j = i + 1; j < l.Count; ++j)
                {
                    if (l[i] == l[j])
                    {
                        l.RemoveAt(j--);
                    }
                }
            }
        }

        // Объединение
        public Operation GetUnion(Operation s2, char Name)
        {
            return new Operation(GetUnion(this.Nums, s2.Nums), Name);
        }
        public static List<int> GetUnion(List<int> l1, List<int> l2)
        {
            List<int> result = new List<int>(l1);
            result.AddRange(l2);
            RemoveDuplicates(result);
            return result;
        }

        // Пересечение
        public Operation GetIntersection(Operation s2, char Name)
        {
            return new Operation(GetIntersection(this.Nums, s2.Nums), Name);
        }
        public static List<int> GetIntersection(List<int> l1, List<int> l2)
        {
            List<int> result = new List<int>();
            bool isFind;
            for (int i = 0; i < l1.Count; i++)
            {
                isFind = false;
                for (int j = 0; j < l2.Count && !isFind; ++j)
                {
                    if (l1[i] == l2[j])
                    {
                        result.Add(l1[i]);
                        isFind = true;
                    }
                }
            }
            RemoveDuplicates(result);
            return result;
        }

        // Разность
        public Operation GetDifferenceWith(Operation s2, char Name)
        {
            return new Operation(GetDifferenceWith(this.Nums, s2.Nums), Name);
        }
        public static List<int> GetDifferenceWith(List<int> l1, List<int> l2)
        {
            List<int> result = new List<int>(l1);

            for (int j = 0; j < l2.Count; ++j)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i] == l2[j]) 
                    { 
                        result.RemoveAt(i); 
                    }
                }
            }
            RemoveDuplicates(result);
            return result;
        }

        // Симметрическая разность
        public Operation GetSymmetricDifference(Operation s2, char Name)
        {
            return new Operation(GetSymmetricDifference(this.Nums, s2.Nums), Name);
        }
        public static List<int> GetSymmetricDifference(List<int> l1, List<int> l2)
        {
            return GetDifferenceWith(GetUnion(l1, l2), GetIntersection(l1, l2));
        }

        // Вхождение в подмножество
        public bool isSubsetOf(Operation s)
        {
            return isSubsetOf(Nums, s.Nums);
        }
        public static bool isSubsetOf(List<int> sub, List<int> super)
        {
            if (GetDifferenceWith(sub, super).Count > 0)
            {
                return false;
            }
            else 
            { 
                return true; 
            }
        }
        public override string ToString()
        {
            if (Nums.Count > 0)
            {
                string result = Name + ": { ";
                for (int i = 0; i < Nums.Count - 1; i++)
                {
                    result += Nums[i].ToString() + ", ";
                }
                result += Nums.Last().ToString() + " }";
                return result;
            }
            else
            {
                return Name + ": Пусто";
            }
        }
    }
}
