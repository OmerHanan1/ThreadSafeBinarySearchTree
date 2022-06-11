using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadedBinarySearchTree
{
    class ThreadedBinarySearchTree
    {
        private Node TreeRoot;
        private ReaderWriterLockSlim CacheLockSlim = new ReaderWriterLockSlim();

        /// <summary>
        /// Default constructor
        /// </summary>
        public ThreadedBinarySearchTree() 
        {
            TreeRoot = new Node();
        }

        /// <summary>
        /// Node as argument constructor
        /// </summary>
        /// <param name="root">node for tree root</param>
        public ThreadedBinarySearchTree(Node root)
        {
            TreeRoot = root;
        }

        /// <summary>
        /// Int as argument constructor
        /// </summary>
        /// <param name="value">int for tree root value</param>
        public ThreadedBinarySearchTree(int value) 
        {
            TreeRoot = new Node(value);
        }

        /// <summary>
        /// Add num to the tree, if it already exists, do nothing
        /// </summary>
        /// <param name="num"> value of addition node </param>
        public void add(int num) 
        {
            bool isExist = search(num);
            if (isExist)
                return;

            this.CacheLockSlim.EnterWriteLock();
            try
            {
                Node currentNode = TreeRoot;
                
                while (currentNode != null)
                {
                    if (currentNode.Left == null && currentNode.Right == null)
                        break;
                    if (currentNode.Left == null && num <= currentNode.Value)
                        break;
                    if (currentNode.Right == null && num > currentNode.Value)
                        break;

                    if (currentNode.Value >= num)
                        currentNode = currentNode.Left;

                    else if (currentNode.Value < num)
                        currentNode = currentNode.Right;
                }

                if (currentNode == null)
                {
                    this.TreeRoot = new Node(num);
                    return;
                }

                if (num <= currentNode.Value)
                {
                    currentNode.Left = new Node(num);
                    currentNode.Left.Parent = currentNode;
                }
                if (num > currentNode.Value)
                {
                    currentNode.Right = new Node(num);
                    currentNode.Right.Parent = currentNode;
                }
            }
            finally 
            {
                if(this.CacheLockSlim.IsWriteLockHeld)
                    this.CacheLockSlim.ExitWriteLock(); 
            }
        }

        /// <summary>
        /// Remove num from the tree, if it doesn't exists, do nothing
        /// </summary>
        /// <param name="num"> value to remove </param>
        public void remove(int num) 
        {
            this.CacheLockSlim.EnterWriteLock();
            try
            {
                this.TreeRoot = deleteHelper(this.TreeRoot, num);
            }
            finally 
            {
                this.CacheLockSlim.ExitWriteLock(); 
            }
        }

        private Node deleteHelper(Node root, int key)
        {
            if (root == null)
                return root;

            if (key < root.Value)
                root.Left = deleteHelper(root.Left, key);
            else if (key > root.Value)
                root.Right = deleteHelper(root.Right, key);

            else
            {
                if (root.Left == null)
                    return root.Right;
                else if (root.Right == null)
                    return root.Left;

                root.Value = subTreeMinValue(root.Right);
                root.Right = deleteHelper(root.Right, root.Value);
            }
            return root;
        }

        int subTreeMinValue(Node treeRoot)
        {
            int minValue = treeRoot.Value;

            while (treeRoot.Left != null)
            {
                minValue = treeRoot.Left.Value;
                treeRoot = treeRoot.Left;
            }
            return minValue;
        }

        /// <summary>
        /// Search num in the tree and return true/false accordingly
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public bool search(int num) 
        {
            this.CacheLockSlim.EnterReadLock();
            try
            {
                Node currentNode = this.TreeRoot;

                while (currentNode != null)
                {
                    if (currentNode.Value == num)
                        return true;

                    if (currentNode.Value > num)
                        currentNode = currentNode.Left;
                    else
                        currentNode = currentNode.Right;
                }
                return false;
            }
            finally { this.CacheLockSlim.ExitReadLock(); }
        }

        /// <summary>
        /// Remove all items from tree
        /// </summary>
        public void clear() 
        {
            this.CacheLockSlim.EnterWriteLock();
            try
            {
                this.TreeRoot = null;
            }
            finally
            {
                this.CacheLockSlim.ExitWriteLock();
            }
        } 

        /// <summary>
        /// Print the values of tree from the smallest to largest in comma delimited form. For example; -15,0,1,3,5,23,40,89,201. If the tree is empty do nothing.
        /// </summary>
        public void print() 
        {
            Node currentNode = TreeRoot;
            List<String> list = new List<String>();

            static void InOrder(Node node, List<String> toDisplay) 
            {
                if (node == null)
                    return;
                InOrder(node.Left, toDisplay);

                toDisplay.Add(node.Value.ToString());
                toDisplay.Add(",");

                InOrder(node.Right, toDisplay);
            }

            InOrder(currentNode, list);

            for (int i = 0; i < list.Count-1; i++)
            {
                Console.Write(list[i]);
            }
        }
    }

    public class Node 
    {
        public int Value { get; set; }
        public Node Left { get; set; }
        public Node Right { get; set; }
        public Node Parent { get; set; }    


        public Node() 
        {
            this.Value = 0;
            this.Left = null;
            this.Right = null;
            this.Parent = null;
        }

        public Node(int value)
        {
            this.Value = value;
            this.Left = null;
            this.Right = null;
            this.Parent = null;
        }
    }
}
