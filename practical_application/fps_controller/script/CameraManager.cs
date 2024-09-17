using Godot;
using System;

namespace LGWCP.WorkField.FpsController;

public partial class CameraManager : Node
{
    [Export] public FpsController FpsController;
    protected PhysicsBody3D BobCamRigid;
    protected Generic6DofJoint3D BobCamJoint;

    public override void _Ready()
    {
        BobCamRigid = GetNodeOrNull<PhysicsBody3D>("BobCamRigid");
        BobCamJoint = GetNodeOrNull<Generic6DofJoint3D>("BobCamJoint");
        var BobCamJointTarget = FpsController.CamJointTarget;
        BobCamRigid.GlobalPosition = BobCamJointTarget.GlobalPosition;
        BobCamJoint.GlobalPosition = BobCamJointTarget.GlobalPosition;
        BobCamJoint.NodeA = BobCamJoint.GetPathTo(BobCamRigid);
        BobCamJoint.NodeB = BobCamJoint.GetPathTo(BobCamJointTarget);
    }

    public override void _PhysicsProcess(double delta)
    {
        // GD.Print(BobCamJoint.GlobalPosition);
    }
}
