using Godot;
using System;
using LGWCP.Godot.StatechartSharp;
using LGWCP.Godot.Util.Mathy;

namespace LGWCP.WorkField.FpsController;

public partial class FpsController : CharacterBody3D
{
    [Export] protected float MoveMaxSpeed = 5.0f;
    [Export] protected float MoveAccel = 25.0f;
    [Export] protected float MoveDecel = 50.0f;
    [Export] protected float CamMaxSpeed = 0.05f;
    [Export] protected float CamMaxPitch = 87.0f;
    [Export] protected float ForwardRatio = 1.25f;
    [Export] protected float BackwardRatio = 0.75f;
    [Export] protected float JumpVelocity = 5.0f;
    // [Export] public PhysicsBody3D CamJointTarget { get; protected set; }
    
    protected Node3D Neck;
    public Node3D Head { get; protected set; }
    protected Vector3 Vel;
    protected float Gravity;
    protected RigidBody3D BobCamBody;

    public override void _Ready()
    {
        Neck = GetNodeOrNull<Node3D>("Neck");
        Head = GetNode<Node3D>("Neck/Head");
        BobCamBody = GetNodeOrNull<RigidBody3D>("BobCamBody");

        Input.MouseMode = Input.MouseModeEnum.Captured;
        Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle() * 1.5f;
        CamMaxSpeed = Mathf.DegToRad(CamMaxSpeed);
        CamMaxPitch = Mathf.DegToRad(CamMaxPitch);
    }

    public void RI_StandWalk(StatechartDuct duct)
	{
		var delta = (float)(duct.PhysicsDelta);
		Vel = Velocity;

		// Add the gravity.
		Vel.Y = IsOnFloor() ? 0.0f : Vel.Y - Gravity * delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("Space") && IsOnFloor())
		{
			Vel.Y = JumpVelocity;
		}

		/*
		Get the input direction
			1. Get vector (local) from input
			2. Local to world
			3. Modify
		*/
		Vector2 inputDir = Input.GetVector("Leftward", "Rightward", "Forward", "Backward");
		inputDir /= Mathf.Max(1.0f, inputDir.Length());
		inputDir *= MaxSpeedMultipler(inputDir, ForwardRatio,BackwardRatio);
		Vector3 direction = Basis * new Vector3(inputDir.X, 0, inputDir.Y);

		// Target vel is clamped under max speed
		Vector3 targetVel = direction * MoveMaxSpeed;
		Vel.X = Mathf.MoveToward(Vel.X, targetVel.X,
			delta * GetAccelOrDecel(Vel.X, targetVel.X, MoveAccel, MoveDecel));
		Vel.Z = Mathf.MoveToward(Vel.Z, targetVel.Z,
			delta * GetAccelOrDecel(Vel.Z, targetVel.Z, MoveAccel, MoveDecel));
		
		Velocity = Vel;
		MoveAndSlide();
	}

    public void RI_ViewRotate(StatechartDuct duct)
    {
		var @event = duct.Input;
		if (@event is InputEventMouseMotion mouseMotion)
		{
			RotateY(-mouseMotion.Relative.X * CamMaxSpeed);
			Vector3 headRotation = Head.Rotation;
			headRotation.X = Mathf.Clamp(
				headRotation.X - mouseMotion.Relative.Y * CamMaxSpeed,
				-CamMaxPitch, CamMaxPitch);
			Head.Rotation = headRotation;
		}
    }

    protected static float GetAccelOrDecel(float from, float to, float accel, float decel)
	{
		bool isAccel;

		if (from == 0.0f)
		{
			// from stillness (from == 0.0f), use accel
			isAccel = true;
		}
		else
		{
			// to stillness (to == 0.0f), use decel
			isAccel = from * to > 0.0f
				&& MathF.Abs(from) < MathF.Abs(to);
		}
		
		return isAccel ? accel : decel;
	}

    protected static float MaxSpeedMultipler(Vector2 dir, float forwardRatio, float backwardRatio)
	{
		// A function like oval to normalize magnitude of direction

		// dir.Y -> origin Z direction, <0 is forward
		float r = dir.Y < 0.0f ? forwardRatio : backwardRatio;

		float dirXSqr = dir.X * dir.X;
		float dirMagSqr = dirXSqr + dir.Y * dir.Y;

		if (dirMagSqr == 0.0f)
		{
			// X == Y == 0 , multipler is 1.0f
			return 1.0f;
		}

		float cosSqr = dirXSqr / dirMagSqr;
		float mul = r + (1 - r) * cosSqr;
		return mul;
	}
}
