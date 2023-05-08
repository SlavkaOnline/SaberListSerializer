using BenchmarkDotNet.Attributes;
using SerializerTests.Implementations;
using SerializerTests.Interfaces;
using SerializerTests.Nodes;

namespace Benchmarks;

[MemoryDiagnoser]
public class JohnSmithSerializerBenchmarks
{

    private readonly IListSerializer _serializer = new JohnSmithSerializer();

    private readonly ListNode _list10 = Helper.CreateWithSameData(10, "data");
    private readonly ListNode _list100 = Helper.CreateWithSameData(100, "data");
    private readonly ListNode _list1000 = Helper.CreateWithSameData(1000, "data");
    private byte[] _ms10;
    private byte[] _ms100;
    private byte[] _ms1000;

    [GlobalSetup(Target = nameof(Deserialize_10))]
    public void GlobalSetup10()
    {
        using var ms = new MemoryStream();
        _serializer.Serialize(_list10, ms).GetAwaiter().GetResult();
        _ms10 = ms.ToArray();
    }
    
    [GlobalSetup(Target = nameof(Deserialize_100))]
    public void GlobalSetup100()
    {
        using var ms = new MemoryStream();
        _serializer.Serialize(_list100, ms).GetAwaiter().GetResult();
        _ms100 = ms.ToArray();
    }
    
    [GlobalSetup(Target = nameof(Deserialize_1000))]
    public void GlobalSetup1000()
    {
        using var ms = new MemoryStream();
        _serializer.Serialize(_list1000, ms).GetAwaiter().GetResult();
        _ms1000 = ms.ToArray();
    }
    
    [Benchmark]
    public async Task DeepCopy_10()
    {
        await _serializer.DeepCopy(_list10);
    }
    
    [Benchmark]
    public async Task DeepCopy_100()
    {
        await _serializer.DeepCopy(_list100);
    }
    
    [Benchmark]
    public async Task DeepCopy_1000()
    {
       await _serializer.DeepCopy(_list1000);
    }
    
    [Benchmark]
    public async Task Serialize_10()
    {
        await using var ms = new MemoryStream();
        await _serializer.Serialize(_list10, ms);
    }
    
    [Benchmark]
    public async Task Serialize_100()
    {
        await using var ms = new MemoryStream();
        await _serializer.Serialize(_list100, ms);
    }
    
    [Benchmark]
    public async Task Serialize_1000()
    {
        await using var ms = new MemoryStream();
        await _serializer.Serialize(_list1000, ms);
    }
    
    [Benchmark]
    public async Task Deserialize_10()
    {
        using var ms = new MemoryStream(_ms10);
        await _serializer.Deserialize(ms);
    }
    
    [Benchmark]
    public async Task Deserialize_100()
    {
        using var ms = new MemoryStream(_ms100);
        await _serializer.Deserialize(ms);
    }
    
    [Benchmark]
    public async Task Deserialize_1000()
    {
        using var ms = new MemoryStream(_ms1000);
        await _serializer.Deserialize(ms);
    }
}