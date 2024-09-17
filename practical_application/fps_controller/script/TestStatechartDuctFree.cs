using Godot;
using System;
using LGWCP.StatechartSharp;

public partial class TestStatechartDuctFree : Node
{
    protected Statechart Statechart;
    protected StatechartDuct duct;
    public override void _Ready()
    {
        Statechart = new()
        {
            Name = "StatechartTest",
            IsWaitParentReady = true,
            EventFlag = EventFlagEnum.Process
        };

        AddChild(Statechart);
        duct = Statechart.Duct;
    }

    public override void _Process(double delta)
    {
        if (Statechart is not null)
        {
            RemoveChild(Statechart);
            Statechart.Free();
            Statechart = null;
        }

        if (duct is not null)
        {
            GD.Print(duct);
        }
    }
}
