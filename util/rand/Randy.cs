using System;


namespace LGWCP.Util.Randy;


#region randy

public class Randy
{
    const double UIntToDouble = 1.0 / 42_9496_7296.0;

    public static int NextInt(IRNG32 rng)
    {
        return (int)rng.Next();
    }
    
    public static int NextInt(IRNG32 rng, int floor, int ceil)
    {
        int x = (int)rng.Next() % (ceil - floor);
        return (x > 0) ? (floor + x) : (ceil - x);
    }

    public static float NextSingle01(IRNG32 rng)
    {
        return (float)(rng.Next() * UIntToDouble);
    }

    public static float NextSingle(IRNG32 rng, float floor, float ceil)
    {
        // floor >= ceil
        var ratio = (float)(rng.Next() * UIntToDouble);
        return (1-ratio) * floor + ratio * ceil;
    }

    public static double NextDouble01(IRNG32 rng)
    {
        return rng.Next() * UIntToDouble;
    }

    public static double NextDouble(IRNG32 rng, double floor, double ceil)
    {
        // floor >= ceil
        var ratio = rng.Next() * UIntToDouble;
        return (1-ratio) * floor + ratio * ceil;
    }

    public static bool NextBool(IRNG32 rng)
    {
        return rng.Next() % 2 == 1;
    }
}

#endregion


#region rng interface

public interface IRNG32
{
    public UInt32 Next();
}

public interface IRNGStateful
{
    public UInt64 GetRNGState();
    public void SetRNGState(UInt64 state);
}

#endregion


#region pcg32fast

public class PCG32Fast : IRNG32, IRNGStateful
{
    protected const UInt64 mul = 6364136223846793005UL;
    protected const UInt64 inc = 1442695040888963407UL;

    protected UInt64 _state;

    public PCG32Fast(UInt64 state)
    {
        // seed is modified to ord
        bool isStateOrd = state << 63 >> 63 == 1UL;
        if (isStateOrd)
        {
            _state = state;
        }
    }

    public PCG32Fast(int seed = 0)
    {
        // seed is modified to odd
        _state = ((UInt64)seed << 1) + inc;
    }

    public UInt32 Next()
    {
        unchecked
        {
            UInt64 x = _state;
            int count = (int)(x >> 61);	// 61 = 64 - 3
            _state = x * mul;
            x ^= x >> 22;
            return (UInt32)(x >> (22 + count)); // 22 = 32 - 3 - 7
        }
    }

    public UInt64 GetRNGState()
    {
        return _state;
    }

    public void SetRNGState(UInt64 state)
    {
        _state = state;
    }
}

#endregion


#region pcg32

public class PCG32 : IRNG32, IRNGStateful
{
    // PCG-XSH-RR with 64-bit state and 32-bit output
    protected const UInt64 mul = 6364136223846793005UL;
    protected const UInt64 inc = 1442695040888963407UL;

    protected UInt64 _state;

    public PCG32(UInt64 state)
    {
        // seed is modified to ord
        _state = state;
    }

    public PCG32(int seed = 0)
    {
        // seed is modified to ord
        _state = (ulong)seed + inc;
    }

    public UInt32 Next()
    {
        unchecked
        {
            UInt64 x = _state;
            _state = _state * mul + inc;
            int count = (int)(x >> 59);	// 59 = 64 - 5
            UInt32 y = (UInt32)((x ^ (x >> 18)) >> 27);
            return y >> count | y << (-count & 31); // 22 = 32 - 3 - 7
        }
    }

    public UInt64 GetRNGState()
    {
        return _state;
    }

    public void SetRNGState(UInt64 state)
    {
        _state = state;
    }
}

#endregion

