#[compute]
#version 450

// Invocations in the (x, y, z) dimension
layout(local_size_x = 2, local_size_y = 1, local_size_z = 1) in;

layout(set = 2, binding = 0, std430) restrict buffer MyDataBufferAnother {
    float data[];
}
my_data_buffer_another;

// A binding to the buffer we create in our script
layout(set = 2, binding = 0, std430) restrict buffer MyDataBuffer {
    float data[];
}
my_data_buffer;

void main() {
    // gl_GlobalInvocationID.x uniquely identifies this invocation across all work groups
    my_data_buffer.data[gl_GlobalInvocationID.x] *= 2.0;
}
