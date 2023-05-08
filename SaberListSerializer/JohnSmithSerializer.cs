using System.Text;
using SerializerTests.Interfaces;
using SerializerTests.Nodes;

namespace SerializerTests.Implementations
{
    //Specify your class\file name and complete implementation.
    public class JohnSmithSerializer : IListSerializer
    {
        //the constructor with no parameters is required and no other constructors can be used.
        public JohnSmithSerializer()
        {
            //...
        }

        public Task<ListNode> DeepCopy(ListNode head)
        {
            if (head == null)
                return Task.FromResult(head);

            var oldNewNodeDict = new Dictionary<ListNode, ListNode>();
            
            var current = head;
            
            while (current != null)
            {
                var node = new ListNode()
                {
                    Data = current.Data
                };
                
                 oldNewNodeDict.Add(current, node);
                 
                current = current.Next;
            }
            
            current = head;
            
            while (current != null)
            {
                if (current.Next != null)
                {
                    oldNewNodeDict[current].Next = oldNewNodeDict[current.Next];
                }
                
                if (current.Previous != null)
                {
                    oldNewNodeDict[current].Previous = oldNewNodeDict[current.Previous];
                }
                
                if (current.Random != null)
                {
                    oldNewNodeDict[current].Random = oldNewNodeDict[current.Random];
                }

                current = current.Next;
            }
            
            return Task.FromResult(oldNewNodeDict.First().Value);
        }
        
        public Task<ListNode> Deserialize(Stream s)
        {
            var nodeIndexDict = new Dictionary<int, ListNode>();
            var randomIndexesList = new List<int>();
            var index = 0;

            using var br = new BinaryReader(s, Encoding.UTF8, true);
            while (br.BaseStream.Position != br.BaseStream.Length)
            {
                var random = br.ReadInt32();
                var data = br.ReadString();

                var node = new ListNode()
                {
                    Data = data,
                };

                nodeIndexDict.Add(index, node);
                randomIndexesList.Add(random);
                index++;
            }

            if (!randomIndexesList.Any())
                return null;

            if (randomIndexesList.Count == 1)
                return Task.FromResult(nodeIndexDict.Values.First());

            for (var i = 0; i < randomIndexesList.Count; i++)
            {
                var node = nodeIndexDict[i];
                var randomIndex = randomIndexesList[i];
                if (randomIndex != -1)
                    node.Random = nodeIndexDict[randomIndex];

                if (i != randomIndexesList.Count - 1)
                    node.Next = nodeIndexDict[i + 1];

                if (i != 0)
                    node.Previous = nodeIndexDict[i - 1];
            }
            
            return Task.FromResult(nodeIndexDict.Values.First());
        }

        public async Task Serialize(ListNode head, Stream s)
        {
            var nodeIndexDict = GetNodeIndexDictionary(head);
            await using var bw = new BinaryWriter(s, Encoding.UTF8, true);
            while (head != null)
            {
                var randomIndex = head.Random == null ? -1 : nodeIndexDict[head.Random];
                bw.Write(randomIndex);
                bw.Write(head.Data);
                head = head.Next;
            }
        }
        
        private static Dictionary<ListNode, int> GetNodeIndexDictionary(ListNode head)
        {
            var dict = new Dictionary<ListNode, int>();
            var index = 0;
            while (head != null)
            {
                dict.Add(head, index++);
                head = head.Next;
            }

            return dict;
        }
    }
}