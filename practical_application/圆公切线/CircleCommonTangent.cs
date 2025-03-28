using Godot;
using System;

using LGWCP.Util.Randy;
using LGWCP.Godot.Util.Mathy;

public partial class CircleCommonTangent : Node2D
{
    public CharacterBody2D Body1;
    public CharacterBody2D Body2;
    public CollisionShape2D P1;
    public CollisionShape2D P2;
    public CircleShape2D C1;
    public CircleShape2D C2;
    public Line2D L1;
    public Line2D L2;

    [Export]
    public float Elapse = 1f;
    public float Timer = 0f;
    public PCG32Fast Rand = new();

    public override void _Ready()
    {
        Body1 = GetNode<CharacterBody2D>("P1");
        P1 = GetNode<CollisionShape2D>("P1/CollisionShape2D");
        C1 = (CircleShape2D)P1.Shape;
        Body2 = GetNode<CharacterBody2D>("P2");
        P2 = GetNode<CollisionShape2D>("P2/CollisionShape2D");
        C2 = (CircleShape2D)P2.Shape;
        
        L1 = GetNode<Line2D>("Line1");
        L2 = GetNode<Line2D>("Line2");
        Timer = Elapse;
    }

    public override void _Process(double delta)
    {
        Timer += (float)delta;
        if (Timer < Elapse)
        {
            return;
        }
        else
        {
            Timer -= Elapse;
        }

        // Rand
        Body1.Position = new (Randy.NextSingle(Rand, -500f, 500f), Randy.NextSingle(Rand, -500f, 500f));
        Body2.Position = new (Randy.NextSingle(Rand, -500f, 500f), Randy.NextSingle(Rand, -500f, 500f));
        C1.Radius = Randy.NextSingle(Rand, 10f, 75f);
        C2.Radius = Randy.NextSingle(Rand, 10f, 75f);

        // Do math
        var posP1 = P1.GlobalPosition;
        var posP2 = P2.GlobalPosition;
        var p1p2Vec = posP1 - posP2;
        var p1p2 = p1p2Vec.Length();
        var r1 = C1.Radius;
        var r2 = C2.Radius;

        if (MathF.Abs(r1 - r2) <= Mathy.EPSILON)
        {
            var dx = p1p2Vec.X;
            var dy = p1p2Vec.Y;
            var sinTheta1 = dy / p1p2;
            var cosTheta1 = -dx / p1p2;
            var sinTheta2 = -dy / p1p2;
            var cosTheta2 = dx / p1p2;

            L1.Points = [ ToLocal(new(posP1.X + r1 * cosTheta1, posP1.Y + r1 * sinTheta1)), ToLocal(new(posP2.X + r2 * cosTheta1, posP2.Y + r2 * sinTheta1)) ];
            L2.Points = [ ToLocal(new(posP1.X + r1 * cosTheta2, posP1.Y + r1 * sinTheta2)), ToLocal(new(posP2.X + r2 * cosTheta2, posP2.Y + r2 * sinTheta2)) ];
        }
        else
        {
            var a = r1 - r2 - (posP1.X - posP2.X);
            var b = 2f * (posP1.Y - posP2.Y);
            var c = r1 - r2 + (posP1.X - posP2.X);
            var d = b * b - 4f * a * c;
            if (d < 0)
            {
                L1.Points = [];
                L2.Points = [];
            }
            else
            {
                var t1 = (-b + MathF.Sqrt(d)) / (2f * a);
                var sinTheta1 = 2f * t1 / (1 + t1 * t1);
                var cosTheta1 = (1f - t1 * t1) / (1 + t1 * t1);
                var t2 = (-b - MathF.Sqrt(d)) / (2f * a);
                var sinTheta2 = 2f * t2 / (1 + t2 * t2);
                var cosTheta2 = (1f - t2 * t2) / (1 + t2 * t2);

                GD.Print(r1, " ", r2, " ", cosTheta1, " ", sinTheta1);

                L1.Points = [ new(posP1.X + r1 * cosTheta1, posP1.Y + r1 * sinTheta1), new(posP2.X + r2 * cosTheta1, posP2.Y + r2 * sinTheta1) ];
                L2.Points = [ new(posP1.X + r1 * cosTheta2, posP1.Y + r1 * sinTheta2), new(posP2.X + r2 * cosTheta2, posP2.Y + r2 * sinTheta2) ];
            }
        }
    }
}
