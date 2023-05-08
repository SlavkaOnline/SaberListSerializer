BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.3.1 (22E261) [Darwin 22.4.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK=7.0.100
[Host]     : .NET 7.0.0 (7.0.22.51805), Arm64 RyuJIT AdvSIMD
DefaultJob : .NET 7.0.0 (7.0.22.51805), Arm64 RyuJIT AdvSIMD


|           Method |         Mean |     Error |    StdDev |    Gen0 |   Gen1 | Allocated |
|----------------- |-------------:|----------:|----------:|--------:|-------:|----------:|
|      DeepCopy_10 |     694.8 ns |   8.62 ns |   8.06 ns |  0.2546 |      - |   1.56 KB |
|     DeepCopy_100 |   7,808.3 ns |  92.61 ns |  77.33 ns |  2.3956 | 0.0610 |  14.77 KB |
|    DeepCopy_1000 | 122,037.8 ns | 799.98 ns | 709.16 ns | 23.6816 | 4.6387 | 146.82 KB |
|     Serialize_10 |     474.4 ns |   3.89 ns |   3.45 ns |  0.2193 |      - |   1.34 KB |
|    Serialize_100 |   4,035.1 ns |  11.41 ns |  10.12 ns |  1.9379 | 0.0229 |  11.88 KB |
|   Serialize_1000 |  43,543.0 ns | 314.66 ns | 262.76 ns | 21.4233 | 3.5400 | 131.84 KB |
|   Deserialize_10 |     760.8 ns |   3.86 ns |   3.22 ns |  0.4473 |      - |   2.74 KB |
|  Deserialize_100 |   6,142.1 ns |  55.20 ns |  43.10 ns |  3.2120 | 0.1450 |   19.7 KB |
| Deserialize_1000 |  61,236.1 ns | 826.19 ns | 772.82 ns | 30.2734 | 7.5684 | 186.95 KB |
