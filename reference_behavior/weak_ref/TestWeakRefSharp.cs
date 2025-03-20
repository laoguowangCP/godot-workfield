using Godot;
using System;
using System.Collections.Generic;

namespace LGWCP.WorkField.WeakRef;

public partial class TestWeakRefSharp : Node
{
    public override void _Ready()
    {
        GD.Print("---- Test Reference Count ----");
        var n = new RefCounted();
        GD.Print(n.GetReferenceCount());
        var n_wr = WeakRef(n);
        GD.Print(n.GetReferenceCount());
        var n1 = n_wr.GetRef();
        GD.Print(n.GetReferenceCount());
        var n2 = n;
        GD.Print(n.GetReferenceCount());

        GD.Print("---- Test Loop Reference ----");
        var fooObj = new FooObj();
        {
            var fooRes0 = new FooRes();
            var fooRes1 = new FooRes();
            fooRes0.Foo = fooRes1;
            fooRes1.Foo = fooRes0;
            fooObj.Foos.Add(fooRes0);
            fooObj.Foos.Add(fooRes1);
        }

        fooObj.Free();
    }

    public override void _Process(double delta)
    {
        var fooObj = new FooObj();
        var fooRes0 = new FooRes();
        var fooRes1 = new FooRes();
        fooRes0.Foo = fooRes1;
        fooRes1.Foo = fooRes0;
        // fooObj.Foos.Add(fooRes0);
        // fooObj.Foos.Add(fooRes1);

        fooObj.Free();
    }
}

public partial class FooObj : GodotObject
{
    public List<FooRes> Foos = [];
}

public partial class FooRes : Resource
{
    public FooRes Foo;
}
