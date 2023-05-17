using System.Buffers;
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
        
        public async Task<ListNode> Deserialize(Stream s)
        {
            var nodeIndexDict = new Dictionary<int, ListNode>();
            var randomIndexesList = new List<int>();
            var index = 0;

            var intBuffer = new byte[4];
            while (true)
            {
                var isEmpty = await ReadDataFromStream(4, intBuffer, s);
                if (isEmpty)
                {
                    break;
                }
                
                var random = BitConverter.ToInt32(intBuffer);
                
                isEmpty = await ReadDataFromStream(4, intBuffer, s);
                if (isEmpty)
                {
                    break;
                }
                
                var dataLength = BitConverter.ToInt32(intBuffer);

                var dataBuffer = ArrayPool<byte>.Shared.Rent(dataLength);
                try
                {
                    isEmpty = await ReadDataFromStream(dataLength, dataBuffer, s);
                    var data = Encoding.UTF8.GetString(dataBuffer.AsSpan(0, dataLength));
                    var node = new ListNode()
                    {
                        Data = data,
                    };

                    nodeIndexDict.Add(index, node);
                    randomIndexesList.Add(random);
                    index++;

                    if (isEmpty)
                    {
                        break;
                    }
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(dataBuffer);
                }
            }
            
            if (!randomIndexesList.Any())
                return null;

            if (randomIndexesList.Count == 1)
                return nodeIndexDict.Values.First();

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
            
            return nodeIndexDict.Values.First();
        }

        public async Task Serialize(ListNode head, Stream s)
        {
            var nodeIndexDict = GetNodeIndexDictionary(head);
            var intBuffer = new byte[4];
            while (head != null)
            {
                var randomIndex = head.Random == null ? -1 : nodeIndexDict[head.Random];
                await WriteDataToStream(randomIndex, intBuffer, s);
                await WriteDataToStream(head.Data.Length, intBuffer, s);
                await WriteDataToStream(head.Data, s);
                head = head.Next;
            }
        }
        
        private static async Task WriteDataToStream(string data, Stream stream)
        {
            var buffer = ArrayPool<byte>.Shared.Rent(data.Length);
            try
            {
                for (var i = 0; i < data.Length; i++)
                {
                    buffer[i] = (byte) data[i];
                }
                await stream.WriteAsync(buffer.AsMemory(0, data.Length));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(buffer);
            }
        }
        
        private static async Task WriteDataToStream(int data, byte[] buffer, Stream stream)
        {
            buffer[0] = (byte)data;
            buffer[1] = (byte)(((uint)data >> 8) & 0xFF);
            buffer[2] = (byte)(((uint)data >> 16) & 0xFF);
            buffer[3] = (byte)(((uint)data >> 24) & 0xFF);

            await stream.WriteAsync(buffer);
        }
        
        private static async Task<bool> ReadDataFromStream(int bytesCount, byte[] buffer, Stream stream)
        {
            var bytesRead = 0;
            while (bytesRead < bytesCount)
            {
                var count = await stream.ReadAsync(buffer.AsMemory(bytesRead, bytesCount - bytesRead));
                bytesRead += count;
                if (count != 0) 
                    continue;
                
                if (bytesRead != 0 && bytesRead < bytesCount)
                    throw new InvalidOperationException("Недостаточно данных в стриме");

                return true;
            }

            return false;
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