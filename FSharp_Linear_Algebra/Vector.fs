namespace FSharp_Linear_Algebra.Vector

open System
open System.Collections
open System.Collections.Generic
open System.Numerics

/// <summary>Class for generic vectors.</summary>
type vector<'T>(element : 'T []) =
    
    // Private data -------------------------------------------------

    /// <summary>Dimension of given vector.</summary>
    let _dim = element.Length

    /// <summary>Element of given vector.</summary>
    let _elem = element

    // Instance properties ------------------------------------------

    /// <summary>Gets dimension of given vector.</summary>
    member v.dim = _dim

    /// <summary>Gets element of given vector, as form of array.</summary>
    member v.element = _elem

    // Instance methods ---------------------------------------------

    /// <summary>Formats given vector to string.</summary>
    member v.Format() =
        let sb = System.Text.StringBuilder()
        sb.Append("[") |> ignore
        for i=0 to _dim - 1 do
            sb.AppendFormat("{0} ", _elem.[i]) |> ignore
            if (i < _dim - 1) then sb.Append(", ") |> ignore
        sb.Append("]") |> ignore
        sb.ToString()

    // Static properties --------------------------------------------

    

    // Static methods -----------------------------------------------
                    

    // Explicit Constructors ----------------------------------------

    
/// <summary>Module for operations on vectors.</summary>
module Vector =
    
    /// <summary>Adds two given vectors. Two vectors should have same dimensions.</summary>
    /// <param name="vec1">Vector to be added. Left side of operator.</param>
    /// <param name="vec2">Vector to be added. Right side of operator.</param>
    /// <returns>Result of two vectors added.</returns>
    let inline Add (vec1 : 'T vector) (vec2 : 'T vector) : 'T vector =
        vector(Array.map2 (fun x y -> x + y) vec1.element vec2.element)

    /// <summary>Subtracts one vector from another. Two vectors should have same dimensions.</summary>
    /// <param name="vec1">Vector to be subtracted from. Left side of operator.</param>
    /// <param name="vec2">Vector to subtract. Right side of operator.</param>
    /// <returns>Result of subtraction, vec1 - vec2.</returns>
    let inline Subtract (vec1 : 'T vector) (vec2 : 'T vector) : 'T vector =
        vector(Array.map2 (fun x y -> x - y) vec1.element vec2.element)

    /// <summary>Gets inner product of two vectors. Two vectors should have same dimensions.</summary>
    /// <param name="vec1">Vector to get inner product with another. Left side of operator.</param>
    /// <param name="vec2">Vector to get inner product with another. Right side of operator.</param>
    /// <returns>Result of inner production, scalar value of type 'T.</returns>
    let inline InnerProduct (vec1 : 'T vector) (vec2 : 'T vector) : 'T =
        Array.sum(Array.map2 (fun x y -> x * y) vec1.element vec2.element)
    
    /// <summary>Gets size of vector int32 as int32.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    let inline SizeInt32 (vec1 : int32 vector) : int32 =
        int(Math.Sqrt((float)(Array.sum(Array.map (fun x -> x * x) vec1.element))))

    /// <summary>Gets size of vector<int64> as int64.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    let inline SizeInt64 (vec1 : int64 vector) : int64 =
        int64(Math.Sqrt((float)(Array.sum(Array.map (fun x -> x * x) vec1.element))))

    /// <summary>Gets size of vector<float32> as float32.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    let inline SizeFloat32 (vec1 : float32 vector) : float32 =
        (float32)(Math.Sqrt((float)(Array.sum(Array.map (fun x -> x * x ) vec1.element))))

    /// <summary>Gets size of vector<double> as double.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    let inline SizeDouble (vec1 : double vector) : double =
        Math.Sqrt(Array.sum(Array.map (fun x -> x * x) vec1.element))

    /// <summary>Creates and returns zero vector of given dimension.</summary>
    /// <param name="size">Size of zero vector to create.</param>
    let ZeroVector (dim : int) : 'T vector =
        vector(Array.zeroCreate dim)

    /// <summary>Creates and returns unit vector of given dimension, having one of type 'T at location.
    /// <param name="dim">Dimension of unit vector.</param>
    /// <param name="location">Index of one, starting from 1.</param>
    /// <param name="one">One of type 'T.</param>
    let inline UnitVector (dim : int) (location : int) (one : 'T) : 'T vector =
        do if LanguagePrimitives.GenericComparison one LanguagePrimitives.GenericOne <> 0 then failwith "One is not one!"
        let elem = Array.zeroCreate dim
        elem.SetValue(one, location-1)
        vector(elem)

    /// <summary>Check if given vector is unit vector or not.</summary>
    /// <param name="vec">Vector to check if unit vector or not.</param>
    let inline IsUnitVector (vec : 'T vector) : bool =
        let mutable sum = vec.element.[0] * vec.element.[0]
        for i=1 to vec.dim - 1 do sum <- sum + (vec.element.[i] * vec.element.[i])
        if LanguagePrimitives.GenericComparison sum LanguagePrimitives.GenericOne <> 0 then
            false
        else
            true