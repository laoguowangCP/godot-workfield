# Randy

## 介绍

Randy 是一个迷你随机数生成库，主要关注几点：

- 可以获取随机数状态，以供保存加载（其他库做得到吗）
- 使用 PCG 代替 LCG ，理论上随机效果更好，还会更快一点？
- 很短，我自己能看懂

## 使用

1. 创建 `PCG32` 、 `PCG32Fast` 这种 RNG 对象（或是你自己定义你喜欢的 RNG），传入 `int` 种子或是 `UInt64` 的状态值。
2. 用 `Randy` 的静态方法去获取你需要类型的随机数（ RNG 原生只能输出 `UInt32` 的随机数）
3. 对你的 RNG 使用 `GetRNGState` / `SetRNGState` 来获取/重建随机状态。

## 注意

- 只实现了两个 PCG ，还没有支持 stream 啥的（我没看懂），需要额外的方法存储其他状态。
- 不保证和其他随机数生成器输出一样，没测过