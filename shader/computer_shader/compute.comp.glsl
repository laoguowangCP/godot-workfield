#[compute]
#version 450

// Invocations in the (x, y, z) dimension
layout(local_size_x = 2, local_size_y = 1, local_size_z = 1) in;

// A binding to the buffer we create in our script
layout(set = 0, binding = 0, std430) restrict buffer MyDataBuffer0 {
    float data[];
} my_data_buffer_0;

layout(set = 0, binding = 1, std430) restrict buffer MyDataBuffer0Another {
    float data[];
} my_data_buffer_0_another;

layout(set = 2, binding = 0, std430) restrict buffer MyDataBuffer {
    float data[];
} my_data_buffer;

layout(set = 2, binding = 1, std430) restrict buffer MyDataBufferAnother {
    float data[];
}
my_data_buffer_another;

void main() {
    my_data_buffer_0.data[gl_GlobalInvocationID.x] *= 2.0;
    my_data_buffer_0_another.data[gl_GlobalInvocationID.x] *= -2.0;
    // gl_GlobalInvocationID.x uniquely identifies this invocation across all work groups
    my_data_buffer.data[gl_GlobalInvocationID.x] *= -2.0;
    my_data_buffer_another.data[gl_GlobalInvocationID.x] *= 2.0;
}
