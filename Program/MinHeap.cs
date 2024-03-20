namespace AssignmentFour
{
    public class Node
    {
        public string Symbol { get; set; }
        public int Frequency { get; set; }
        public Node LeftChild { get; set; }
        public Node RightChild { get; set; }

        public override string ToString()
        {
            return $"{Symbol,-10} {Frequency,5}";
        }

        public void PrintTree(string indent = "", bool last = true)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("\\-");
                indent += "  ";
            }
            else
            {
                Console.Write("|-");
                indent += "| ";
            }

            Console.WriteLine(ToString());

            if (LeftChild != null)
                LeftChild.PrintTree(indent, RightChild == null);
            if (RightChild != null)
                RightChild.PrintTree(indent, true);
        }
    }

    public class MinHeap
    {
        public List<Node> data;

        public MinHeap()
        {
            data = new List<Node>();
        }

        public void Add(Node node)
        {
            data.Add(node);
            int index = data.Count - 1;
            while (index > 0 && data[index].Frequency < data[(index - 1) / 2].Frequency)
            {
                Node temp = data[index];
                data[index] = data[(index - 1) / 2];
                data[(index - 1) / 2] = temp;
                index = (index - 1) / 2;
            }
        }

        public Node Pop()
        {
            if (data.Count == 0)
            {
                throw new InvalidOperationException("Heap is empty.");
            }

            Node root = data[0];
            data[0] = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);
            HeapifyDown(0);
            return root;
        }

        private void HeapifyDown(int index)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int smallest = index;

            if (leftChild < data.Count && data[leftChild].Frequency < data[smallest].Frequency)
            {
                smallest = leftChild;
            }

            if (rightChild < data.Count && data[rightChild].Frequency < data[smallest].Frequency)
            {
                smallest = rightChild;
            }

            if (smallest != index)
            {
                Node temp = data[index];
                data[index] = data[smallest];
                data[smallest] = temp;
                HeapifyDown(smallest);
            }
        }

        public void Print()
        {
            if (data.Count > 0)
            {
                Node root = data[0];
                root.PrintTree();
            }
        }
    }
}