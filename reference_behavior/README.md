# RefCounted 和 WeakRef

类似 cpp 的智能指针。

很多对象其实都是继承自 RefCounted 的，一般是资源啥的，依附于某个节点或是其他资源。

然而 RefCounted 对象有点危险，它们只有在引用归零的时候才会被真确销毁回收。如果两个 RefCounted 对象互相引用的话，就会构成**循环引用**，这样两个 RefCounted 就永远保持在内存里不会被回收。

涉及循环引用时，请使用 [WeakRef](https://docs.godotengine.org/en/stable/classes/class_weakref.html) ：

```gdscript
var x = weakref(RefCounted.new())
```

比较反直觉的是， RefCounted 有很多扩展，但 WeakRef 并不应该扩展。

如果你不想依赖于 Node ，就来试试 RefCounted 吧。



