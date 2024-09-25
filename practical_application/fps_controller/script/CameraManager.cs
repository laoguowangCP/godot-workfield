using Godot;
using System;
using LGWCP.Godot.Util.Mathy;

namespace LGWCP.WorkField.FpsController;

public partial class CameraManager : Node
{
    [Export] public FpsController Player;
    protected Node3D Head;
    protected Vector3 CamDampPosVel = new();
    protected Quaternion CamDampRotVel = new();
    protected Camera3D BobCam;
    protected SpringDamperV3 SD3Pos;
    protected SpringDamperF SDRotX;
    protected SpringDamperF SDRotY;
    protected float RotXVel;
    protected float RotYVel;


    public override void _Ready()
    {
        // ConfigBobCamJoint();
        ConfigBobCamCustomDamp();
        SD3Pos = new SpringDamperV3(
            new Vector3(0.4f, 0.4f, 0.4f),
            new Vector3(16f, 16f, 16f));
        SDRotX = new SpringDamperF(0.6f, 16f);
        SDRotY = new SpringDamperF(0.6f, 16f);
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaTime = (float)delta;
        
        var currentPos = BobCam.Position;
        var targetPos = Head.GlobalPosition;
        
        var currentRot = BobCam.Quaternion;
        var targetRot = Player.Quaternion * Head.Quaternion;
        // GD.Print(targetRot);
        
        SD3Pos.Step(
            ref currentPos,
            ref CamDampPosVel,
            targetPos,
            Player.Velocity, // 0f,
            deltaTime);

        BobCam.Position = currentPos;
        // BobCam.Quaternion = currentRot;


        float rotX = BobCam.Rotation.X;
        float rotY = BobCam.Rotation.Y;
        Mathy.RotModifierRad(ref rotY);
        SDRotX.StepRad(
            ref rotX,
            ref RotXVel,
            Head.Rotation.X,
            0f,
            deltaTime);
        SDRotY.StepRad(
            ref rotY,
            ref RotYVel,
            Player.Rotation.Y,
            0f,
            deltaTime);
        BobCam.Rotation = new Vector3(rotX, rotY, BobCam.Rotation.Z);
    }

    protected void ConfigBobCamCustomDamp()
    {
        BobCam = GetNodeOrNull<Camera3D>("BobCam");
        Head = Player.Head;
        CamDampPosVel = new Vector3();
        BobCam.Position = Head.GlobalPosition;
    }
}
