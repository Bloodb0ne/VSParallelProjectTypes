# VSParallelProjectTypes
A VSIX package for VS2019 adding project templates for learning parallel programming


Simple VSIX extension that adds **OpenCL**,**OpenMP**,**TBB** project templates with an additional Options Page for setting the paths for Thread Building Blocks.
All of the boilerplate projects implement simplified Vector Addition.

The **OpenCL** project is using the **NVIDIA SDK**, because i found it easyer to use and didn't really care about what version is supported. Maybe i should change it using the C++ Wrapper, but havent decided if i would write my own atm.