#include <iostream>
#include <iterator>
#include <omp.h>

void saxpy_openmp(int n, float a, float x[], float y[]) {
#pragma omp parallel for
	for (int i = 0; i < n; i++)
	{
		y[i] = a * x[i] + y[i];
	}
}
int main()
{
	std::cout << "Hello World!\n";
	float a[] = { 1, 2, 3, 4, 5, 5, 6, 7, 8, 9 };
	float b[10] = { 1, 2, 3, 4, 5, 5, 6, 7, 8, 9 };

	saxpy_openmp(10, 2, a, b);
	std::copy(b, b + 10, std::ostream_iterator<int>(std::cout, ","));

}

