namespace Manatee
{
    public class Node
    {
        public int frequency;
        public char? symbol;
        public Node right;
        public Node left;
        public int height;

        public Node(char? symbol, int frequency)
        {
            this.frequency = frequency;
            this.symbol = symbol;
            height = 0;
        }

        public Node(char? symbol, int frequency, Node left, Node right) : this(symbol, frequency)
        {
            this.right = right;
            this.left = left;
            height = Math.Max(left.height, right.height) + 1;
        }
    }


    public class Node2
    {
        public char? symbol;
        public Node2 right;
        public Node2 left;

        public Node2()
        {
            symbol = null;
        }

        private Node2(char? symbol)
        {
            this.symbol = symbol;
        }

        public Node2 Add(char? symbol, byte bit)
        {
            if (bit == 1)
            {
                if (this.left == null)
                {
                    this.left = new Node2(symbol);
                }

                return this.left;
            }
            else
            {
                if (this.right == null)
                {
                    this.right = new Node2(symbol);
                }

                return this.right;
            }

        }
    }

}
