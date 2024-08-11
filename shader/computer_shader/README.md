# 计算着色器（群友也能看懂）

## 编写着色器

> 使用 glsl 编写，godot 要用的话需要开头加个 `#[compute]` 预处理。实际上 godot 用的还有些 vulkan 的特性。

### layout

编写计算着色器，开头需要定义数据布局 layout ，可以理解为“规定管线如何获取数据”。有以下不同类型的数据：

- Invocation
- 一般的贴图
- 自定义数据类型
- Push Constant

#### Invocation

首先是 Invocation 值，相当于用来告诉gpu每个“核”需要处理多少个并行任务：

```glsl
layout(local_size_x = 8, local_size_y = 8, local_size_z = 1) in;
```

这里叫做 local_size 的 qualifier （layout 里每一项都叫一个 qualifier） 可以帮助你索引 gl_GlobalInvocationID ，设定一般和你使用情景的维度对齐：

- 处理数组，就设为 (k, 1, 1) ， gl_GlobalInvocationID.x 就是数组的索引。
- 处理图像，就设为 (m, n, 1) ， gl_GlobalInvocationID.xy 就是图像上对应的点。

注意 Invocation 的 layout 是特殊的，不需要命名，要加一个 in 表示数据只传入不传出。

#### 贴图

贴图一般类似这样：

```glsl
layout(r32f, set = 0, binding = 0) uniform restrict readonly image2D current_image;
```

首先是解释图片类型的 qualifier ，一般是rgb字母+位数+类型， r32f 就是一个像素是 32 位 float 只表示一个 r 值。

然后就是 set 和 binding ，简单来说它们都是数据的索引，可以参考 [vkguide-descriptors](https://vkguide.dev/docs/chapter-4/descriptors/) ：

- set 的目的是用来区分绑定频率的（管线都是用一组数据组成的 set 进行绑定），一般 set0是每帧绑定的公共数据、set1每pass绑定、set2是材质的数据、set3是对象的数据。有些 gpu 对 set 数量支持不多的所以慎用（不知道自己在干嘛的话就 set=0 ）。
- binding 相当于一组数据里的绑定索引，注意 binding 的数据是连续的，有 binding3 就必须有 2、1、0 。

这里我们要给贴图布局。贴图一般都是全局资源（uniform）

#### 自定义数据类型

自定义数据类型需要额外定义一个结构体，本质都是往一个 buffer 中填东西，主要两类：

- 定长数据
- 不定长数据

不定长数据不能使用 uniform ，而是使用 buffer 修饰符。结构体中不定长的成员必须放在最后：

```glsl
layout(std430, binding = 2) buffer MyBuffer
{
    mat4 matrix;
    float lotsOfFloats[];
};
```

这里 std430 是一种字节对齐规范，默认可以帮助你在结构体很复杂的情况下，仍然能和 C++ 中结构体对齐情况保持一致。

#### Push Constant


## 编写渲染相关代码

有了着色器，我们还需要一些 cpu 上的代码去指挥 gpu 干活，这个过程简单来说包括：

- 加载 glsl 文件，变成 gpu 可读的、中间语言形式的计算着色器
- 将着色器实例化为 pipeline
- 准备要传递给 gpu 的数据，对应着色器中的数据布局进行绑定，并打包
- 创建指令列表，设定 pipeline 、数据包，分配核心数
- 提交，同步，再读取返回的数据


数据绑定的单位是 `RDUniform` ，我们需要将多个数据打包成 set 才是着色器绑定的单位：

```csharp
RID UniformSetCreate(Array<RDUniform> uniforms, RID shader, int shader_set )
```

这里除了要把打包好的 uniform 传进去之外，还需要着色器和 set 的信息，这样图形 api 才知道数据包具体要安排多少空间、其中各个 uniform 要如何放置对齐（注意这里只是利用 set 的信息，而非绑定到 set 上）。



