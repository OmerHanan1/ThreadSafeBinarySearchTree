using System;

namespace ThreadedBinarySearchTree
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ThreadedBinarySearchTree tree = new ThreadedBinarySearchTree(5);
            tree.add(5);
            tree.add(2);
            tree.add(8);
            tree.add(8); // Test (1)
            tree.add(1);
            tree.add(3);
            tree.add(6);
            tree.add(9);

            Console.WriteLine(tree.search(1));
            Console.WriteLine(tree.search(9)); // Test (2)

            tree.print();
            Console.WriteLine();

            tree.remove(5);
            tree.print();
            Console.WriteLine();

            tree.remove(2);
            tree.print();
            Console.WriteLine();

            tree.remove(8);
            tree.print();
            Console.WriteLine();

            tree.remove(6);
            tree.print();
            Console.WriteLine();


            tree.remove(-1); // Test (3)
            tree.print();
            Console.WriteLine();
        }
    }
}
