using System;
using System.Runtime.CompilerServices;
using Godot;

namespace LGWCP.Godot.Util.Mathy;


/*

Math lib dedicated for godot. Helper functions for vector, and many fastapprox algorithm.
"Kinda math, but not so math."

*/

#region Mathy

public class Mathy
{
	#region FastApprox
	public const float SMALL_NUMBER = 1e-8f;
	public const float PI = 3.1415926535897932f;
	public const float PI_INV = 0.3183098861837907f;
	public const float PI_HALF = 1.5707963267948966f;
	public const float PI_TWO = 6.283185307179586f;
	public const float PI_SQ = 9.869604401089358f;
	public const float PI_SQ_INV = 0.10132118364233778f;
	public const float PI_SQRT = 1.7724538509055159f;
	public const float PI_SQRT_INV = 0.5641895835477563f;

    public static void SpringDamp(
        ref float current,
        ref float currentVel,
        float target,
        float targetVel,
        float damping,
        float stiffSqrt,
        float stiffSqrtInv,
        float deltaTime)
    {
        if (deltaTime <= 0.0f)
		{
			return;
		}

		float w = stiffSqrt * PI_TWO;
		float wInv = stiffSqrtInv * PI_INV * 0.5f;
		// Handle special cases
		if (w < SMALL_NUMBER) // no strength which means no damping either
		{
			current += currentVel * deltaTime;
			return;
		}
		else if (damping < SMALL_NUMBER) // No damping at all
		{
			float err = current - target;
			float b = currentVel * wInv;
            SinCos(out float S, out float C, w * deltaTime);
            current = target + err * C + b * S;
			currentVel = currentVel * C - err * (w * S);
			return;
		}

		// Target velocity turns into an offset to the position
		float smoothingTime = 2.0f * wInv;
		float targetAdj = target + targetVel * (damping * smoothingTime);
 		float errAdj = current - targetAdj;

		// Handle the cases separately
		if (damping > 1.0f) // Overdamped
		{
			float wd = w * MathF.Sqrt(Square(damping) - 1.0f);
			float c2 = -(currentVel + (w * damping - wd) * errAdj) / (2.0f * wd);
			float c1 = errAdj - c2;
			float a1 = (wd - damping * w);
			float a2 = -(wd + damping * w);
			// Note that A1 and A2 will always be negative. We will use an approximation for 1/Exp(-A * DeltaTime).
			float a1Dt = a1 * deltaTime;
			float a2Dt = a2 * deltaTime;
			// This approximation in practice will be good for all DampingRatios
			float e1 = ExpApprox3(a1Dt);
			// As DampingRatio gets big, this approximation gets worse, but mere inaccuracy for overdamped motion is
			// not likely to be important, since we end up with 1 / BigNumber
			float e2 = ExpApprox3(a2Dt);
			current = targetAdj + e1 * c1 + e2 * c2;
			currentVel = e1 * c1 * a1 + e2 * c2 * a2;
		}
		else if (damping < 1.0f) // Underdamped
		{
			float wd = w * MathF.Sqrt(1.0f - Square(damping));
			float a = errAdj;
			float b = (currentVel + errAdj * (damping * w)) / wd;
            SinCos(out float s, out float c, wd * deltaTime);
            float e0 = damping * w * deltaTime;
			// Needs E0 < 1 so DeltaTime < SmoothingTime / (2 * DampingRatio * Sqrt(1 - DampingRatio^2))
			float e = ExpApprox3(-e0);
			current = e * (a * c + b * s);
			currentVel = -current * damping * w;
			currentVel += e * (b * (wd * c) - a * (wd * s));
			current += targetAdj;
		}
		else // Critical damping
		{
			float c1 = errAdj;
			float c2 = currentVel + errAdj * w;
			float e0 = w * deltaTime;
			// Needs E0 < 1 so deltaTime < SmoothingTime / 2 
			float e = ExpApprox3(-e0);
			current = targetAdj + (c1 + c2 * deltaTime) * e;
			currentVel = (c2 - c1 * w - c2 * (w * deltaTime)) * e;
		}
    }

    // -0.5~0.5 usable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ExpApprox1(float x)
    { 
        return (6+x*(6+x*(3+x)))*0.16666666f; 
    }

    // -0.8~0.8 usable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ExpApprox2(float x)
    { 
        return (24+x*(24+x*(12+x*(4+x))))*0.041666666f;
    }

    // -1.0~1.0 usable
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ExpApprox3(float x)
    { 
        return (120+x*(120+x*(60+x*(20+x*(5+x)))))*0.0083333333f;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float ExpApprox4(float x)
    { 
        return 720+x*(720+x*(360+x*(120+x*(30+x*(6+x)))))*0.0013888888f;
    }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void SinCos(out float s, out float c, float rad)
	{
		(s, c) = MathF.SinCos(rad);
	}

	#endregion


	#region Helper

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Square(float x)
	{
		return x*x;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float Sqrt(float v)
	{
		return MathF.Sqrt(v);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector2 Sqrt(Vector2 v)
	{
		return new Vector2(MathF.Sqrt(v.X), MathF.Sqrt(v.Y));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Sqrt(Vector3 v)
	{
		return new Vector3(MathF.Sqrt(v.X), MathF.Sqrt(v.Y), MathF.Sqrt(v.Z));
	}

	#endregion
}

#endregion


#region SpringDamper

/*
SpringDamp helper classes, minimize calculation cost
*/

public class SpringDamper
{
	protected class DampStepper
	{
		protected float Damping;
		protected float StiffSqrt;
		protected float StiffSqrtInv;
		protected float W;
		protected float WInv;

		public DampStepper(float damping, float stiffSqrt, float stiffSqrtInv, float w, float wInv)
		{
			Damping = damping;
			StiffSqrt = stiffSqrt;
			StiffSqrtInv = stiffSqrtInv;
			W = w;
			WInv = wInv;
		}

		public virtual void Step(
			ref float current,
			ref float currentVel,
			float target,
			float targetVel,
			float deltaTime) {}
	}

	protected class DampStepperSmallW : DampStepper
	{
		public DampStepperSmallW(float damping, float stiffSqrt, float stiffSqrtInv, float w, float wInv)
			: base(damping, stiffSqrt, stiffSqrtInv, w, wInv) {}
		public override void Step(
			ref float current,
			ref float currentVel,
			float target,
			float targetVel,
			float deltaTime)
		{
			current += currentVel * deltaTime;
			return;
		}
	}

	protected class DampStepperSmallDamping : DampStepper
	{
		public DampStepperSmallDamping(float damping, float stiffSqrt, float stiffSqrtInv, float w, float wInv)
			: base(damping, stiffSqrt, stiffSqrtInv, w, wInv) {}
		public override void Step(
			ref float current,
			ref float currentVel,
			float target,
			float targetVel,
			float deltaTime)
		{
			var err = current - target;
			var b = currentVel * WInv;
            Mathy.SinCos(out float s, out float c, W * deltaTime);
            current = target + err * c + b * s;
			currentVel = currentVel * c - err * (W * s);
			return;
		}
	}

	protected class DampStepperOverDamp : DampStepper
	{
		protected float WD;
		protected float WDInv;
		protected float SmoothingTime;
		protected float A1;
		protected float A2;
		protected float ErrRatio;

		public DampStepperOverDamp(float damping, float stiffSqrt, float stiffSqrtInv, float w, float wInv)
			: base(damping, stiffSqrt, stiffSqrtInv, w, wInv)
		{
			WD = W * MathF.Sqrt(Mathy.Square(Damping) - 1f);
			WDInv = 1f / WD;
			SmoothingTime = 2.0f * WInv;
			A1 = WD - Damping * W;
			A2 = -(WD + Damping * W);
			ErrRatio = W * Damping - WD;
		}

		public override void Step(
			ref float current,
			ref float currentVel,
			float target,
			float targetVel,
			float deltaTime)
		{
			// var SmoothingTime = 2.0f * WInv;
			float targetAdj = target + targetVel * (Damping * SmoothingTime);
			float err = current - targetAdj;

			// float WD = W * MathF.Sqrt(Mathy.Square(Damping) - 1.0f);
			// float c2 = -(currentVel + (W * Damping - WD) * Err) / (2.0f * WD);
			// float c2 = -(currentVel + (W * Damping - WD) * err) * WDInv * 0.5f;
			float c2 = -(currentVel + ErrRatio * err) * WDInv * 0.5f;
			float c1 = err - c2;
			// float A1 = (WD - Damping * W);
			// float A2 = -(WD + Damping * W);
			float a1Dt = A1 * deltaTime;
			float a2Dt = A2 * deltaTime;
			float e1 = Mathy.ExpApprox3(a1Dt);
			float e2 = Mathy.ExpApprox3(a2Dt);
			current = targetAdj + e1 * c1 + e2 * c2;
			currentVel = e1 * c1 * A1 + e2 * c2 * A2;
		}
	}
	
	protected class DampStepperUnderDamp : DampStepper
	{
		protected float WD;
		protected float WDInv;
		protected float SmoothingTime;
		protected float DampingW;

		public DampStepperUnderDamp(float damping, float stiffSqrt, float stiffSqrtInv, float w, float wInv)
			: base(damping, stiffSqrt, stiffSqrtInv, w, wInv)
		{
			WD = W * MathF.Sqrt(1f - Mathy.Square(Damping));
			WDInv = 1f / WD;
			SmoothingTime = 2.0f * WInv;
			DampingW = Damping * W;
		}

		public override void Step(
			ref float current,
			ref float currentVel,
			float target,
			float targetVel,
			float deltaTime)
		{
			// var SmoothingTime = 2.0f * WInv;
			float targetAdj = target + targetVel * (Damping * SmoothingTime);
			float errAdj = current - targetAdj;

			// float WD = W * MathF.Sqrt(1.0f - Mathy.Square(Damping));
			float a = errAdj;
			// float b = (currentVel + errAdj * (Damping * W)) / WD;
			float b = (currentVel + errAdj * Damping * W) / WD;
            Mathy.SinCos(out float s, out float c, WD * deltaTime);
            // float e0 = Damping * W * deltaTime;
			// float e = Mathy.ExpApprox3(-e0);
			float e = Mathy.ExpApprox3(-Damping * W * deltaTime);
			current = e * (a * c + b * s);
			currentVel = -current * DampingW;
			currentVel += e * (b * (WD * c) - a * (WD * s));
			current += targetAdj;
		}
	}

	protected class DampStepperCriticalDamp : DampStepper
	{
		protected float SmoothingTime;
		protected float DST;

		public DampStepperCriticalDamp(float damping, float stiffSqrt, float stiffSqrtInv, float w, float wInv)
			: base(damping, stiffSqrt, stiffSqrtInv, w, wInv)
		{
			SmoothingTime = 2.0f * WInv;
			DST = Damping * SmoothingTime;
		}

		public override void Step(
			ref float current,
			ref float currentVel,
			float target,
			float targetVel,
			float deltaTime)
		{
			// var SmoothingTime = 2.0f * WInv;
			// float targetAdj = target + targetVel * (Damping * SmoothingTime);
			float targetAdj = target + targetVel * DST;
			float err = current - targetAdj;

			float c1 = err;
			float c2 = currentVel + err * W;
			// float e0 = W * deltaTime;
			// float e = Mathy.ExpApprox3(-e0);
			float e = Mathy.ExpApprox3(-W * deltaTime);
			current = targetAdj + (c1 + c2 * deltaTime) * e;
			currentVel = (c2 - c1 * W - c2 * (W * deltaTime)) * e;
		}
	}

	// public virtual void Step(ref float current, ref float currentVel, float target, float targetVel, float deltaTime) {}
}

public class SpringDamperF : SpringDamper
{
	protected DampStepper Stepper;
	public SpringDamperF(float damping, float stiff)
	{
		var stiffSqrt = MathF.Sqrt(stiff);
		var stiffSqrtInv = 1f / stiffSqrt;
		var w = stiffSqrt * Mathy.PI_TWO;
		var wInv = stiffSqrtInv * Mathy.PI_INV * 0.5f;

		if (w < Mathy.SMALL_NUMBER)
		{
			Stepper = new DampStepperSmallW(damping, stiffSqrt, stiffSqrtInv, w, wInv);
		}
		else if (damping < Mathy.SMALL_NUMBER)
		{
			Stepper = new DampStepperSmallDamping(damping, stiffSqrt, stiffSqrtInv, w, wInv);
		}
		else if (damping > 1.0f)
		{
			Stepper = new DampStepperOverDamp(damping, stiffSqrt, stiffSqrtInv, w, wInv);
		}
		else if (damping < 1.0f)
		{
			Stepper = new DampStepperUnderDamp(damping, stiffSqrt, stiffSqrtInv, w, wInv);
		}
		else // damping == 1.0f
		{
			Stepper = new DampStepperCriticalDamp(damping, stiffSqrt, stiffSqrtInv, w, wInv);
		}
	}
	
	public void Step(
        ref float current,
        ref float currentVel,
        float target,
        float targetVel,
        float deltaTime)
    {
        if (deltaTime <= 0.0f)
		{
			return;
		}

		Stepper.Step(ref current, ref currentVel, target, targetVel, deltaTime);
    }
}

public class SpringDamperV2 : SpringDamper
{
	protected DampStepper[] Steppers = new DampStepper[2];
	public SpringDamperV2(Vector2 damping, Vector2 stiff)
	{
		var stiffSqrt = Mathy.Sqrt(stiff);
		var stiffSqrtInv = Vector2.One / stiff;
		var w = stiffSqrt * Mathy.PI_TWO;
		var wInv = stiffSqrtInv * Mathy.PI_INV * 0.5f;

		for (int i = 0; i < Steppers.Length; ++i)
		{
			if (w[i] < Mathy.SMALL_NUMBER)
			{
				Steppers[i] = new DampStepperSmallW(damping[i], stiffSqrt[i], stiffSqrtInv[i], w[i], wInv[i]);
			}
			else if (damping[i] < Mathy.SMALL_NUMBER)
			{
				Steppers[i] = new DampStepperSmallDamping(damping[i], stiffSqrt[i], stiffSqrtInv[i], w[i], wInv[i]);
			}
			else if (damping[i] > 1.0f)
			{
				Steppers[i] = new DampStepperOverDamp(damping[i], stiffSqrt[i], stiffSqrtInv[i], w[i], wInv[i]);
			}
			else if (damping[i] < 1.0f)
			{
				Steppers[i] = new DampStepperUnderDamp(damping[i], stiffSqrt[i], stiffSqrtInv[i], w[i], wInv[i]);
			}
			else // damping == 1.0f
			{
				Steppers[i] = new DampStepperCriticalDamp(damping[i], stiffSqrt[i], stiffSqrtInv[i], w[i], wInv[i]);
			}
		}
	}

	public void Step(
        ref Vector2 current,
        ref Vector2 currentVel,
        Vector2 target,
        Vector2 targetVel,
        float deltaTime)
    {
        if (deltaTime <= 0.0f)
		{
			return;
		}

		Steppers[0].Step(ref current.X, ref currentVel.X, target.X, targetVel.X, deltaTime);
		Steppers[1].Step(ref current.Y, ref currentVel.Y, target.Y, targetVel.Y, deltaTime);
    }
}

public class SpringDamperV3 : SpringDamper
{
	protected DampStepper[] Steppers = new DampStepper[3];
	public SpringDamperV3(Vector3 damping, Vector3 stiff)
	{
		var stiffSqrt = Mathy.Sqrt(stiff);
		var stiffSqrtInv = Vector3.One / stiff;
		var w = stiffSqrt * Mathy.PI_TWO;
		var wInv = stiffSqrtInv * Mathy.PI_INV * 0.5f;

		for (int i = 0; i < Steppers.Length; ++i)
		{
			if (w[i] < Mathy.SMALL_NUMBER)
			{
				Steppers[i] = new DampStepperSmallW(damping[i], stiffSqrt[i], stiffSqrtInv[i], w[i], wInv[i]);
			}
			else if (damping[i] < Mathy.SMALL_NUMBER)
			{
				Steppers[i] = new DampStepperSmallDamping(damping[i], stiffSqrt[i], stiffSqrtInv[i], w[i], wInv[i]);
			}
			else if (damping[i] > 1.0f)
			{
				Steppers[i] = new DampStepperOverDamp(damping[i], stiffSqrt[i], stiffSqrtInv[i], w[i], wInv[i]);
			}
			else if (damping[i] < 1.0f)
			{
				Steppers[i] = new DampStepperUnderDamp(damping[i], stiffSqrt[i], stiffSqrtInv[i], w[i], wInv[i]);
			}
			else // damping == 1.0f
			{
				Steppers[i] = new DampStepperCriticalDamp(damping[i], stiffSqrt[i], stiffSqrtInv[i], w[i], wInv[i]);
			}
		}
	}

	public void Step(
        ref Vector3 current,
        ref Vector3 currentVel,
        Vector3 target,
        Vector3 targetVel,
        float deltaTime)
    {
        if (deltaTime <= 0.0f)
		{
			return;
		}

		Steppers[0].Step(ref current.X, ref currentVel.X, target.X, targetVel.X, deltaTime);
		Steppers[1].Step(ref current.Y, ref currentVel.Y, target.Y, targetVel.Y, deltaTime);
		Steppers[2].Step(ref current.Z, ref currentVel.Z, target.Z, targetVel.Z, deltaTime);
    }
}


#endregion
