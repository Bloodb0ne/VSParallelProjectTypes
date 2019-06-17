__kernel void
saxpy_opencl(
	__constant float a,
	__global float* x,
	__global float* y
) {
	int i = get_global_id(0);
	y[i] = a * x[i] + y[i];
}