
#include <iostream>

#include "tbb/parallel_for.h"
#include "tbb/blocked_range.h"

void saxpy_tbb(int n, float a, float x[], float y[]) {
	
	tbb::parallel_for(
		tbb::blocked_range<int>(0, n),
		[&](tbb::blocked_range<int> r) {
			for (size_t i = r.begin(); i != r.end(); ++i) {
				y[i] = a * x[i] + y[i];
			}
		}
	);
}
int main()
{
    std::cout << "Hello World!\n";
	float a[] = { 1, 2, 3, 4, 5, 5, 6, 7, 8, 9 };
	float b[10] = { 1, 2, 3, 4, 5, 5, 6, 7, 8, 9 };

	saxpy_tbb(10, 2, a, b);
	std::copy(b, b + 10, std::ostream_iterator<int>(std::cout, ","));
	
}
