namespace Huffman
{
    public class Node
    {
        public string Symbol;
        public int Frequency;
        public Node LeftChild;
        public Node RightChild;

        public List<int> Search(string symbol, List<int> prevPath)
        {
            if (RightChild == null && LeftChild == null)
            {
                if (symbol == this.Symbol)
                {
                    return prevPath;
                }

                return null;
            }

            List<int> path = null;
            if (LeftChild != null)
            {
                var leftPath = new List<int>(prevPath);
                leftPath.Add(0);
                path = LeftChild.Search(symbol, leftPath);
            }

            if (path != null)
            {
                return path;
            }

            if (RightChild != null)
            {
                var rightPath = new List<int>(prevPath);
                rightPath.Add(1);
                path = RightChild.Search(symbol, rightPath);
            }

            return path;
        }
    }

    public class MinHeap
    {
        public List<Node> data = new List<Node>();

        public void Add(Node node)
        {
            data.Add(node);
            int i = data.Count - 1;
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (data[parent].Frequency <= data[i].Frequency)
                    break;
                Swap(parent, i);
                i = parent;
            }
        }

        public Node Pop()
        {
            if (data.Count == 0)
                return null;

            Node root = data[0];
            data[0] = data[data.Count - 1];
            data.RemoveAt(data.Count - 1);

            MinHeapify(0);

            return root;
        }

        private void MinHeapify(int i)
        {
            int left = 2 * i + 1;
            int right = 2 * i + 2;
            int smallest = i;

            if (left < data.Count && data[left].Frequency < data[smallest].Frequency)
                smallest = left;

            if (right < data.Count && data[right].Frequency < data[smallest].Frequency)
                smallest = right;

            if (smallest != i)
            {
                Swap(i, smallest);
                MinHeapify(smallest);
            }
        }

        private void Swap(int i, int j)
        {
            Node temp = data[i];
            data[i] = data[j];
            data[j] = temp;
        }

        public void Print()
        {
            foreach (var node in data)
            {
                Console.WriteLine($"Symbol: {node.Symbol}, Frequency: {node.Frequency}");
            }

            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var testFile = "D:\\Algotitms and data structures\\4555555\\assignment-4\\Program\\text";
            var text = File.ReadAllText(testFile);
            var freqChar = new Dictionary<char, int>();
            var minHeap = new MinHeap();

            foreach (var letter in text)
            {
                
                if (freqChar.ContainsKey(letter))
                    freqChar[letter] += 1;
                else
                    freqChar[letter] = 1;
            }

            foreach (var pair in freqChar)
            {
                var key = pair.Key.ToString();

                var node = new Node()
                {
                    Symbol = key,
                    Frequency = pair.Value,
                    LeftChild = null,
                    RightChild = null
                };
                minHeap.Add(node);
            }

            minHeap.Print();

            while (minHeap.data.Count > 1)
            {
                var m1 = minHeap.Pop();
                var m2 = minHeap.Pop();
                var m1_m2 = new Node()
                {
                    Symbol = m1.Symbol + "+" + m2.Symbol,
                    Frequency = m1.Frequency + m2.Frequency,
                    LeftChild = m1,
                    RightChild = m2
                };
                minHeap.Add(m1_m2);
                minHeap.Print();
            }

            if (minHeap.data.Count > 0)
            {
                var root = minHeap.Pop();
                if (root != null)
                {
                    foreach (var pair in freqChar)
                    {
                        var path = new List<int>();
                        path = root.Search(pair.Key.ToString(), path);
                        Console.Write(
                            $"{pair.Key.ToString().Replace("\n", "\\n").Replace("\r", "\\r").Replace(" ", "_")}: ");
                        if (path != null)
                            foreach (var bit in path)
                            {
                                Console.Write(bit);
                            }
                        Console.WriteLine();
                    }
                }
            }
        }
    }
}