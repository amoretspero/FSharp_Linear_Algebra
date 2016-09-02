## Update History  
  
### 0.3.2.0 -> 0.4.0.0  
  
- LDU decomposition is supported for generic types.  
- Testing scripts for matrix is available. Tests addition, subtraction, multiplication and LDU decomposition.  
- Testing scripts uses MathNet linear algebra library for testing.    
  
### 0.3.1.0 -> 0.3.2.0  
  
- For matrices, read from file(<code>int32</code>, <code>double</code> types only.) and write to file(all types) are supported.  
- Testing scripts are in progress.  
  
### 0.3.0.0 -> 0.3.1.0  
  
- Gauss elimination supports PA=LU style elimination. It will return (P * L * U), size-3 tuple containing elimination result when elimination is possible.
  
### 0.2.1.1 -> 0.3.0.0  
  
- For <code>int32</code>, <code>int64</code>, <code>float32</code>, <code>double</code> typed vectors, size can be calculated.  
- Added zero vector, unit vector creator.  
- Added method to check if given vector is unit vector.  