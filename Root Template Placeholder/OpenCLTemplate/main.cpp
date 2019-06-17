#include <iostream>
#include <fstream>
#include <string>
#include <iterator>
#include <CL/cl.h>

constexpr int LIST_SIZE = 10;

int main()
{
	std::cout << "$custommessage$\n";

	float a = 2.0;
	float x[] = { 1, 2, 3, 4, 5, 5, 6, 7, 8, 9 };
	float y[LIST_SIZE] = { 1, 2, 3, 4, 5, 5, 6, 7, 8, 9 };

	std::ifstream kernelFile;
	kernelFile.open("vector_add_kernel.cl",std::ifstream::in);
	std::string kernelSource((std::istreambuf_iterator<char>(kernelFile)), std::istreambuf_iterator<char>());


	if (kernelFile.is_open()) {
		cl_int ret = -1;
		cl_device_id devID = NULL;
		cl_uint numDevices;
		cl_uint numPlatforms;

		ret = clGetPlatformIDs(0, NULL, &numPlatforms);
		if (clGetPlatformIDs(0, NULL, &numPlatforms) == CL_SUCCESS) {
			//Platforms available
			cl_platform_id *availPlatforms = new cl_platform_id[numPlatforms];

			//Fetch all platforms
			clGetPlatformIDs(numPlatforms, availPlatforms, NULL);
			
			//Fetch one device
			ret = clGetDeviceIDs(availPlatforms[0], CL_DEVICE_TYPE_ALL, 1, &devID, &numDevices);
			if (ret == CL_SUCCESS) {

				cl_context clCTX = clCreateContext(NULL, 1, &devID, NULL, NULL, &ret);
				if (ret == CL_SUCCESS) {

					//Setup Code for Kernel Execution
					cl_command_queue cmdQueue = clCreateCommandQueue(clCTX, devID, 0, &ret);

					
					cl_mem xObjBuff = clCreateBuffer(clCTX, CL_MEM_READ_ONLY, LIST_SIZE * sizeof(float), NULL, &ret);
					cl_mem yObjBuff = clCreateBuffer(clCTX, CL_MEM_READ_WRITE, LIST_SIZE * sizeof(float), NULL, &ret);
					
					//Push the vectors into device memory 
					// ( blocking calls so we dont have to check write status completion )
					clEnqueueWriteBuffer(cmdQueue, xObjBuff, CL_TRUE, 0, LIST_SIZE * sizeof(float), &x, 0, NULL, NULL);
					clEnqueueWriteBuffer(cmdQueue, yObjBuff, CL_TRUE, 0, LIST_SIZE * sizeof(float), &y, 0, NULL, NULL);

					
					const char* kernelSourceArr = kernelSource.c_str();

					cl_program vectorProg = clCreateProgramWithSource(clCTX, 1, &kernelSourceArr, 0, &ret);

					ret = clBuildProgram(vectorProg, 1, &devID, NULL, NULL, NULL);
					std::cout << "Return value build kernel" << ret;

					size_t log;

					clGetProgramBuildInfo(vectorProg, devID, CL_PROGRAM_BUILD_LOG, 0, NULL, &log);
					std::cout << "Build log size" << log;
					char* buildlog = new char[log];
					clGetProgramBuildInfo(vectorProg, devID, CL_PROGRAM_BUILD_LOG, log, buildlog, NULL);
					std::cout << buildlog;
					delete[] buildlog;

					cl_kernel vectorKernel = clCreateKernel(vectorProg, "saxpy_opencl", &ret);
					std::cout << "Return value create kernel" << ret;
					
					//No need to allocate buffer for a scalar value 
					clSetKernelArg(vectorKernel, 0, sizeof(float), (void*)& a);
					clSetKernelArg(vectorKernel, 1, sizeof(cl_mem), (void*)& xObjBuff);
					clSetKernelArg(vectorKernel, 2, sizeof(cl_mem), (void*)& yObjBuff);

					std::cout << "Execution...";
					size_t work_size = LIST_SIZE;
					size_t local_size = 2; // Split it into blocks of 4
					ret = clEnqueueNDRangeKernel(cmdQueue, vectorKernel, 1, NULL, &work_size, &local_size, 0, NULL, NULL);
					if (ret == CL_SUCCESS) {
						
					}
					else {
						std::cout << "Failed Execution " << ret;
					}

					//Read back the value to host memory
					ret = clEnqueueReadBuffer(cmdQueue, yObjBuff, CL_TRUE, 0, LIST_SIZE * sizeof(float), y, 0, NULL, NULL);

					if (ret == CL_SUCCESS) {
						//Print Values
						std::copy(y, y + 10, std::ostream_iterator<int>(std::cout, ","));
					}
					


					// Cleanup
					clFlush(cmdQueue);
					clFinish(cmdQueue);

					clReleaseKernel(vectorKernel);
					clReleaseProgram(vectorProg);
					
					clReleaseMemObject(xObjBuff);
					clReleaseMemObject(yObjBuff);
					clReleaseCommandQueue(cmdQueue);
					clReleaseContext(clCTX);

					//Vectors dont need mem release because they are not dyn.allocated
				}
				else {
					std::cout << "Unable to create context. Error Code: " << ret;
				}
			}
			else {
				std::cout << "Unable to select device. Error Code: " << ret;
			}
		}else {
			std::cout << "No platforms available, check for OpenCL drivers/hardware. Error Code" << ret;
		}
		
	}
	else {
		std::cout << "Kernel File cannot be loaded";
	}
}