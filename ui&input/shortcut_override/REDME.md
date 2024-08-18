# Shortcut 覆盖

用 Shortcut 或其他组合按键去覆盖单独的按键，需要通过 Control 节点管理：

```csharp
// public partial class ShiftScrollUp : Control

public override void _Input(InputEvent @event)
{
    if (Input.IsActionJustPressed("ShiftScrollUp"))
    {
        GD.Print("ShiftScrollUp");
        AcceptEvent();
    }
}

```

没错，使用 `Control.AcceptEvent()` ，就会“消耗”当前函数被传入的这个 `InputEvent` ，阻止继续传递给其他的节点以及后续的输入阶段。

注意仍然需要关心顺序，首先是输入读取阶段，其次是每个阶段场景树中节点执行的顺序。 `AcceptEvent()` 未执行前单独按键仍然可以被读取触发。


