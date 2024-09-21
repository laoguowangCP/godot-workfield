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
    protected Vector3 CamDampVel;
    protected Camera3D BobCam;
    protected SpringDamperF SpringDamper;

    public override void _Ready()
    {
        // ConfigBobCamJoint();
        ConfigBobCamCustomDamp();
        SpringDamper = new SpringDamperF(0.5f, 16f);
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
        
        SpringDamper.Step(
            ref currentPos.Y,
            ref CamDampVel.Y,
            BobCamJointTarget.GlobalPosition.Y,
            FpsController.Velocity.Y, // 0f,
            deltaTime);

        BobCam.GlobalPosition = currentPos;
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
        CamDampVel = new Vector3();
        BobCam.GlobalPosition = BobCamJointTarget.GlobalPosition;
    }
}
