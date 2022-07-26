using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PageManagerApp
{
    class LinkTest

    {
        static void Main(string[] args)
        {
            var list = new DoublyLinkedList<int>();
            Console.WriteLine("1. Append Only operation. Expected output: 10,20,30");
            list.Append(10);
            list.Append(20);
            list.Append(30);
            list.Print();
            Console.WriteLine("2. Prepend Only operation. Expected output: -20,-10,0,10,20,30");
         
            list.Prepend(0);
            list.Prepend(-10);
            list.Prepend(-20);
            list.Print();

            Console.WriteLine("3. Insert Only operation. Expected output: -20,-10, -5,0,5,10,20,30");
            var findZero = list.Find(0);
            list.InsertBefore(findZero, -5);
            list.InsertAfter(findZero, 5);
            list.Print();

            Console.WriteLine("4. Remove  operation. Expected output: -20,-10,0,10,20,30");
            list.RemoveByValue(-5);
            list.RemoveByValue(5);
            list.Print();

            Console.WriteLine("5. Move  operation. Expected output: 20,-20,-10,0,10,30");

            var findTwenty = list.Find(20);
            list.MoveToFront(findTwenty);
            list.Print();

            Console.WriteLine("6. Clear All");
            list.Clear();
            list.Print();
            Console.ReadKey();

        }
    }
}
