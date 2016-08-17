namespace FSharp_Linear_Algebra

open System
open System.Collections
open System.Collections.Generic
open System.Numerics

[<Class>]
type vector<'T> =

    /// <summary>Generates vector with given array of 'T elements.</summary>
    /// <param name="element">Element of vector to be created.</param>
    new : element : 'T [] -> 'T vector
    
    /// <summary>Gets dimension of given vector.</summary>
    member dim : int

    /// <summary>Gets element of given vector, as form of array.</summary>
    member element : 'T []

    /// <summary>Formats given vector to string.</summary>
    member Format : unit -> string


module Vector =
    
    /// <summary>Adds two given vectors. Two vectors should have same dimensions.</summary>
    /// <param name="vec1">Vector to be added. Left side of operator.</param>
    /// <param name="vec2">Vector to be added. Right side of operator.</param>
    /// <returns>Result of two vectors added.</returns>
    val inline Add : vec1 : 'T vector -> vec2 : 'T vector -> 'T vector
        when 'T : (static member (+) : 'T * 'T -> 'T)

    /// <summary>Subtracts one vector from another. Two vectors should have same dimensions.</summary>
    /// <param name="vec1">Vector to be subtracted from. Left side of operator.</param>
    /// <param name="vec2">Vector to subtract. Right side of operator.</param>
    /// <returns>Result of subtraction, vec1 - vec2.</returns>
    val inline Subtract : vec1 : 'T vector -> vec2 : 'T vector -> 'T vector
        when 'T : (static member(-) : 'T * 'T -> 'T)

    /// <summary>Gets inner product of two vectors. Two vectors should have same dimensions.</summary>
    /// <param name="vec1">Vector to get inner product with another. Left side of operator.</param>
    /// <param name="vec2">Vector to get inner product with another. Right side of operator.</param>
    /// <returns>Result of inner production, scalar value of type 'T.</returns>
    val inline InnerProduct : vec1 : 'T vector -> vec2 : 'T vector -> 'T
        when 'T : (static member (*) : 'T * 'T -> 'T) and
             'T : (static member (+) : 'T * 'T -> 'T) and
             'T : (static member Zero : 'T)

    /// <summary>Gets size of int32 vector as int32.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    val inline SizeInt32 : vec1 : int32 vector -> int32

    /// <summary>Gets size of int64 vector as int64.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    val inline SizeInt64 : vec1 : int64 vector -> int64
   
    /// <summary>Gets size of float32 vector as float32.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    val inline SizeFloat32 : vec1 : float32 vector -> float32

    /// <summary>Gets size of double vector as double.</summary>
    /// <param name="vec1">Vector to get the size of.</param>
    val inline SizeDouble : vec1 : double vector -> double

    /// <summary>Creates and returns zero vector of given size.</summary>
    /// <param name="size">Size of zero vector to create.</param>
    val ZeroVector : dim : int -> 'T vector

    /// <summary>Creates and returns unit vector of given dimension, having one of type 'T at location.
    /// <param name="dim">Dimension of unit vector.</param>
    /// <param name="location">Index of one, starting from 1.</param>
    /// <param name="one">One of type 'T.</param>
    val inline UnitVector : dim : int -> location : int -> one : 'T -> 'T vector
        when 'T : (static member One : 'T) and
             'T : comparison

    /// <summary>Check if given vector is unit vector or not.</summary>
    /// <param name="vec">Vector to check if unit vector or not.</param>
    val inline IsUnitVector : vec : vector<'T> -> bool
        when 'T : (static member ( * ) : 'T * 'T -> 'a) and 
             'a : (static member One : 'a) and 
             'a : (static member ( + ) : 'a * 'a -> 'a) and 
             'a : comparison