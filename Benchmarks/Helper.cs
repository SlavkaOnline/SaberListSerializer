using SerializerTests.Nodes;

namespace Benchmarks;

public static class Helper
{

    public static ListNode CreateWithSameData(int count, string data)
    {
        var head = new ListNode()
        {
            Data = data
        };
        
        var current = head;
        while (--count != 0)
        {
            var node = new ListNode()
            {
                Data = data,
                Previous = current,
            };
            current.Next = node;
            current = current.Next;
        }

        return head;
    }

}