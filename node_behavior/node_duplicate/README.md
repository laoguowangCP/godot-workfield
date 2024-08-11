# Node Duplicate

Node duplicate copies child nodes. To get shallow duplication, copy paste this little function:

```csharp
public static Node DuplicateShallow(Node node, int flags = 15)
{
    Node node_d = node.Duplicate(flags);
    foreach (var child in node_d.GetChildren())
    {
        node_d.RemoveChild(child);
    }
    return node_d;
}
```
