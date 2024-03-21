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
            var testFile = "/Users/mac/RiderProjects/assignment-4.1/Program/text";
            var outputFilePath = "/Users/mac/RiderProjects/assignment-4.1/Program/compressed_text.txt";
            var decodedFilePath = "/Users/mac/RiderProjects/assignment-4.1/Program/decoded_text.txt";

            // Encoding part
            var text = File.ReadAllText(testFile);
            var freqChar = new Dictionary<char, int>();
            var minHeap = new MinHeap();
            var codeDictionary = new Dictionary<char, List<int>>();

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
                        codeDictionary.Add(pair.Key, path);
                        Console.Write($"{pair.Key.ToString().Replace("\n", "\\n").Replace("\r", "\\r").Replace(" ", "_")}: ");
                        if (path != null)
                            foreach (var bit in path)
                            {
                                Console.Write(bit);
                            }
                        Console.WriteLine();
                    }
                }
            }

            // Decoding part
            var encodedText = new List<int>();
            using (StreamReader reader = new StreamReader(outputFilePath))
            {
                var line = "";
                while (( line = reader.ReadLine()) != null)
                {
                    foreach (char character in line)
                    {
                        int bit = int.Parse(character.ToString());
                        encodedText.Add(bit);
                    }
                }
            }

            var decodedText = new List<char>();
            var currentCode = new List<int>();
            foreach (var bit in encodedText)
            {
                currentCode.Add(bit);
                foreach (var pair in codeDictionary)
                {
                    if (ListsEqual(currentCode, pair.Value))
                    {
                        decodedText.Add(pair.Key);
                        currentCode.Clear();
                        break;
                    }
                }
            }
            static bool ListsEqual(List<int> list1, List<int> list2)
            {
                if (list1.Count != list2.Count)
                    return false;

                for (int i = 0; i < list1.Count; i++)
                {
                    if (list1[i] != list2[i])
                        return false;
                }

                return true;
            }

            // Writing decoded text to file
            using (StreamWriter writer = new StreamWriter(decodedFilePath))
            {
                foreach (var character in decodedText)
                {
                    writer.Write(character);
                }
            }

            Console.WriteLine("Text has been decoded and written to file successfully.");

            // Writing encoded text to console
            Console.WriteLine("Encoded text:");
            foreach (var bit in encodedText)
            {
                Console.Write(bit);
            }
            Console.WriteLine();

            // Writing encoded text to file
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (var bit in encodedText)
                {
                    writer.Write(bit);
                }
            }

            Console.WriteLine("Text has been encoded and written to file successfully.");
            Console.WriteLine("Decoded text:");
            string decodedContent = File.ReadAllText(decodedFilePath);
            Console.WriteLine(decodedContent);
        }
    }
}

