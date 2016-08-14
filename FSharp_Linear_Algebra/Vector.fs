namespace FSharp_Linear_Algebra

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
    
    /// <summary>Gets size of int32 vector as int32.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    let inline SizeInt32 (vec1 : int32 vector) : int32 =
        int(Math.Sqrt((float)(Array.sum(Array.map (fun x -> x * x) vec1.element))))

    /// <summary>Gets size of float32 vector as float32.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    let inline SizeFloat32 (vec1 : float32 vector) : float32 =
        (float32)(Math.Sqrt((float)(Array.sum(Array.map (fun x -> x * x ) vec1.element))))

    /// <summary>Gets size of double vector as double.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    let inline SizeDouble (vec1 : double vector) : double =
        Math.Sqrt(Array.sum(Array.map (fun x -> x * x) vec1.element))