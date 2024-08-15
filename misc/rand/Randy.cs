using System;


namespace LGWCP.Randy;

public class Randy
{
    const double UIntToDouble = 1.0 / 42_9496_7296.0;

    public static float NextSingle(IRNG32 rng)
    {
        return (float)(rng.Next() * UIntToDouble);
    }

    public static float NextSingle(IRNG32 rng, float floor, float ceil)
    {
        var ratio = (float)(rng.Next() * UIntToDouble);
        return (1-ratio) * floor + ratio * ceil;
    }

    public static double NextDouble(IRNG32 rng)
    {
        return rng.Next() * UIntToDouble;
    }

    public static double NextDouble(IRNG32 rng, double floor, double ceil)
    {
        var ratio = rng.Next() * UIntToDouble;
        return (1-ratio) * floor + ratio * ceil;
    }
}


#region rng interface

public interface IRNG32
{
    public UInt32 Next();
}

public interface IRNG64
{
    public UInt64 Next();
}

#endregion


#region pcg32fast
public class PCG32Fast : IRNG32
{
    const UInt64 mul = 636_4136_2238_4679_3005UL;

    public UInt64 State
    {
        get => _state;
        set
        {
            var isOrder = value << 63 >> 63 == 1UL;
            if (isOrder)
            {
                _state = value;
            }
        }
    }
    private ulong _state;

    public PCG32Fast(UInt64 seed = 0)
    {
        // seed is modified to ord
        State = seed << 1 | 1;
    }

    public UInt32 Next()
    {
        {
            UInt64 x = _state;
            int count = (int)(x >> 61);	// 61 = 64 - 3
            _state = x * mul;
            x ^= x >> 22;
            return (UInt32)(x >> (22 + count)); // 22 = 32 - 3 - 7
        }
    }
}
#endregion


#region pcg32
#endregion

