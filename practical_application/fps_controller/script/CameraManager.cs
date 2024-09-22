using Godot;
using System;
using LGWCP.Godot.Util.Mathy;

namespace LGWCP.WorkField.FpsController;

public partial class CameraManager : Node
{
    [Export] public FpsController FpsController;
    protected PhysicsBody3D BobCamRigid;
    protected Generic6DofJoint3D BobCamJoint;

    protected PhysicsBody3D BobCamJointTarget;
    protected Vector3 CamDampPosVel = new();
    protected Vector3 CamDampRotVel = new();
    protected Camera3D BobCam;
    protected SpringDamperV3 SD3Pos;

    public override void _Ready()
    {
        // ConfigBobCamJoint();
        ConfigBobCamCustomDamp();
        SD3Pos = new SpringDamperV3(
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(16f, 16f, 16f));
    }

    public override void _PhysicsProcess(double delta)
    {
        float deltaTime = (float)delta;
        // GD.Print(BobCamJoint.GlobalPosition);
        
        var currentPos = new Vector3(
            BobCam.GlobalPosition.X,
            BobCam.GlobalPosition.Y,
            BobCam.GlobalPosition.Z);

        /*
        Mathy.SpringDamp(
            ref currentPos.Y,
            ref CamDampVel.Y,
            BobCamJointTarget.GlobalPosition.Y,
            0f,
            0.5f,
            4f, 0.25f,
            deltaTime);
        */
        
        SD3Pos.Step(
            ref currentPos,
            ref CamDampPosVel,
            BobCamJointTarget.GlobalPosition,
            FpsController.Velocity, // 0f,
            deltaTime);

        BobCam.GlobalPosition = currentPos;
        GD.Print(FpsController.Quaternion);
    }

    protected void ConfigBobCamJoint()
    {
        BobCamRigid = GetNodeOrNull<PhysicsBody3D>("BobCamRigid");
        BobCamJoint = GetNodeOrNull<Generic6DofJoint3D>("BobCamJoint");

        BobCamJointTarget = FpsController.CamJointTarget;
        BobCamRigid.GlobalPosition = BobCamJointTarget.GlobalPosition;
        BobCamJoint.GlobalPosition = BobCamJointTarget.GlobalPosition;
        BobCamJoint.NodeA = BobCamJoint.GetPathTo(BobCamRigid);
        BobCamJoint.NodeB = BobCamJoint.GetPathTo(BobCamJointTarget);
    }

    protected void ConfigBobCamCustomDamp()
    {
        BobCam = GetNodeOrNull<Camera3D>("BobCam");
        BobCamJointTarget = FpsController.CamJointTarget;
        CamDampPosVel = new Vector3();
        BobCam.GlobalPosition = BobCamJointTarget.GlobalPosition;
    }
}
