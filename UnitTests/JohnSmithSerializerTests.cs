using SerializerTests.Implementations;
using SerializerTests.Interfaces;
using SerializerTests.Nodes;

namespace UnitTests;

public class JohnSmithSerializerTests
{
    private readonly IListSerializer _serializer = new JohnSmithSerializer();

    [Fact]
    public async Task DeepCopy_One_Ok()
    {
        var node1 = new ListNode
        {
            Data = "node1"
        };

        var node = await _serializer.DeepCopy(node1);
        
        Assert.NotNull(node);
        Assert.Null(node.Next);
        Assert.Null(node.Previous);
        
        Assert.Equal(node1.Data, node.Data);
    }
    
    [Fact]
    public async Task DeepCopy_Ok()
    {
        var node1 = new ListNode
        {
            Data = "node1"
        };
    
        var node2 = new ListNode
        {
            Previous = node1,
            Data = "node2"
        };
        
        var node3 = new ListNode
        {
            Previous = node2,
            Data = "node3",
            Random = node1
        };
        
        var node4 = new ListNode
        {
            Previous = node3,
            Data = "node4",
        };

        node1.Next = node2;
        node1.Random = node4;
        node2.Next = node3;
        node3.Next = node4;

        var node = await _serializer.DeepCopy(node1);
        
        Assert.NotNull(node);
        Assert.Null(node.Previous);
        Assert.NotNull(node.Next);
        Assert.NotNull(node.Next.Next);
        Assert.NotNull(node.Next.Next.Next);
        
        Assert.Same(node.Next.Previous, node);
        Assert.Same(node.Next.Next.Previous, node.Next);
        Assert.Same(node.Next.Next.Next.Previous, node.Next.Next);
        
        Assert.Equal(node1.Data, node.Data);
        Assert.Equal(node2.Data, node.Next.Data);
        Assert.Equal(node3.Data, node.Next.Next.Data);
        Assert.Equal(node4.Data, node.Next.Next.Next.Data);
        
        Assert.Same(node, node.Next.Next.Random);
        Assert.Same(node.Random, node.Next.Next.Next);
        Assert.Null(node.Next.Random);
        Assert.Null(node.Next.Next.Next.Random);
    }
    
    [Fact]
    public async Task Serialize_Deserialize_One_Ok()
    {
        var node1 = new ListNode
        {
            Data = "node1"
        };

        await using var stream = new MemoryStream();
        await _serializer.Serialize(node1, stream);
        stream.Seek(0, SeekOrigin.Begin);
        var node = await _serializer.Deserialize(stream);
        
        Assert.NotNull(node);
        Assert.Null(node.Next);
        Assert.Null(node.Previous);
        
        Assert.Equal(node1.Data, node.Data);
    }
    
    [Fact]
    public async Task Serialize_Deserialize_Ok()
    {
        var node1 = new ListNode
        {
            Data = "node1"
        };
    
        var node2 = new ListNode
        {
            Previous = node1,
            Data = "node2"
        };
        
        var node3 = new ListNode
        {
            Previous = node2,
            Data = "node3",
            Random = node1
        };
        
        var node4 = new ListNode
        {
            Previous = node3,
            Data = "node4",
        };

        node1.Next = node2;
        node1.Random = node4;
        node2.Next = node3;
        node3.Next = node4;

        await using var stream = new MemoryStream();
        await _serializer.Serialize(node1, stream);
        stream.Seek(0, SeekOrigin.Begin);
        var node = await _serializer.Deserialize(stream);
        
        Assert.NotNull(node);
        Assert.Null(node.Previous);
        Assert.NotNull(node.Next);
        Assert.NotNull(node.Next.Next);
        Assert.NotNull(node.Next.Next.Next);
        
        Assert.Same(node.Next.Previous, node);
        Assert.Same(node.Next.Next.Previous, node.Next);
        Assert.Same(node.Next.Next.Next.Previous, node.Next.Next);
        
        Assert.Equal(node1.Data, node.Data);
        Assert.Equal(node2.Data, node.Next.Data);
        Assert.Equal(node3.Data, node.Next.Next.Data);
        Assert.Equal(node4.Data, node.Next.Next.Next.Data);
        
        Assert.Same(node, node.Next.Next.Random);
        Assert.Same(node.Random, node.Next.Next.Next);
        Assert.Null(node.Next.Random);
        Assert.Null(node.Next.Next.Next.Random);
    }
}