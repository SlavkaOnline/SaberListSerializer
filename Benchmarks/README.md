BenchmarkDotNet=v0.13.5, OS=macOS Ventura 13.3.1 (22E261) [Darwin 22.4.0]
Apple M1 Pro, 1 CPU, 8 logical and 8 physical cores
.NET SDK=7.0.100
[Host]     : .NET 7.0.0 (7.0.22.51805), Arm64 RyuJIT AdvSIMD
DefaultJob : .NET 7.0.0 (7.0.22.51805), Arm64 RyuJIT AdvSIMD

* Binary reader/writer

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

* Async stream

|           Method |         Mean |       Error |      StdDev |    Gen0 |   Gen1 | Allocated |
|----------------- |-------------:|------------:|------------:|--------:|-------:|----------:|
|      DeepCopy_10 |     721.4 ns |    13.72 ns |    14.68 ns |  0.2546 |      - |   1.56 KB |
|     DeepCopy_100 |   7,891.6 ns |    69.39 ns |    61.51 ns |  2.3956 | 0.0610 |  14.77 KB |
|    DeepCopy_1000 |  82,578.5 ns |   743.99 ns |   580.86 ns | 23.8037 | 4.7607 | 146.82 KB |
|     Serialize_10 |   1,160.9 ns |    16.72 ns |    13.97 ns |  0.2174 |      - |   1.34 KB |
|    Serialize_100 |  11,146.3 ns |    71.09 ns |    55.50 ns |  2.2583 | 0.0305 |  13.89 KB |
|   Serialize_1000 | 119,812.8 ns | 1,838.78 ns | 1,720.00 ns | 21.3623 | 3.5400 | 131.83 KB |
|   Deserialize_10 |   1,463.1 ns |    23.27 ns |    20.63 ns |  0.3567 | 0.0019 |   2.19 KB |
|  Deserialize_100 |  11,769.1 ns |   137.28 ns |   114.64 ns |  3.1128 | 0.1526 |  19.15 KB |
| Deserialize_1000 | 115,900.4 ns | 1,287.44 ns | 1,141.28 ns | 30.2734 | 7.5684 |  186.4 KB |
