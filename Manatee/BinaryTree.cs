namespace BinTreeClass
{
    public class BinaryTree
    {
        Node tree;

        public class Node
        {
            public int frequency;
            public int symbol;
            public Node left;
            public Node right;

            public Node() { }

            public Node(int frequency, int symbol)
            {
                this.frequency = frequency;
                this.symbol = symbol;
            }


            public static void Add(ref Node node, int frequency, int symbol)
            {
                if (node == null)
                {
                    node = new Node(frequency, symbol);
                }
                else
                {
                    if (node.frequency > frequency)
                    {
                        Add(ref node.left, frequency, symbol);
                    }
                    else
                    {
                        Add(ref node.right, frequency, symbol);
                    }
                }
            }


            public static void AddNode(ref Node node, Node leaf)
            {
                if (node == null)
                {
                    node = leaf;
                }
                else
                {
                    if (node.frequency > leaf.frequency || node.right != null && node.right.symbol == -1)
                    {
                        AddNode(ref node.left, leaf);
                    }
                    else
                    {
                        Console.WriteLine(node.frequency + " " + (char)node.symbol);
                        AddNode(ref node.right, leaf);
                    }

                }
            }



            public static void Inorder(Node node)
            {
                if (node != null)
                {
                    Inorder(node.left);
                    Console.WriteLine(node.symbol + " " + node.frequency);
                    Inorder(node.right);
                }
            }


            public static Node FindMinByFrequencyAndDelIt(ref Node tree)
            {
                Node node = tree;
                Node prev = tree;

                while (node.left != null)
                {
                    prev = node;
                    node = node.left;

                    if (node.symbol == -1)
                    {
                        break;
                    }
                }

                if (node == tree)
                {
                    tree = tree.right;
                }
                else
                {
                    prev.left = node.right == null ? null : node.right;
                }

                if (node.symbol != -1)
                {
                    node.right = null;
                    node.left = null;
                }

                return node;
            }

        } // конец вложенного класса


        public BinaryTree()
        {
            tree = null;
        }


        private BinaryTree(Node node)
        {
            tree = node;
        }


        public void Add(int frequency, int symbol)
        {
            Node.Add(ref tree, frequency, symbol);
        }


        public void BuildTree()
        {
            Node node;

            while (tree.symbol != -1)
            {
                node = new();

                node.left = Node.FindMinByFrequencyAndDelIt(ref tree);
                node.right = Node.FindMinByFrequencyAndDelIt(ref tree);
                node.frequency = node.left.frequency + node.right.frequency;
                node.symbol = -1;
                Node.AddNode(ref tree, node);
            }
        }

    }
}

