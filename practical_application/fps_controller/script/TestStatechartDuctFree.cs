using Godot;
using System;
using LGWCP.Godot.StatechartSharp;

public partial class TestStatechartDuctFree : Node
{
    protected Statechart Statechart;
    protected StatechartDuct Duct;
    public override void _Ready()
    {
        Statechart = new()
        {
            Name = "StatechartTest",
            IsWaitParentReady = true,
            EventFlag = EventFlagEnum.Process
        };

        AddChild(Statechart);
        Duct = Statechart.Duct;
    }

    public override void _Process(double delta)
    {
        if (Statechart is not null)
        {
            RemoveChild(Statechart);
            Statechart.Free();
            Statechart = null;
        }

        if (Duct is not null)
        {
            GD.Print(Duct, Duct.GetReferenceCount());
        }
    }
}
