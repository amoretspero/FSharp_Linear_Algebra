namespace FSharp_Linear_Algebra

open System
open System.Collections
open System.Collections.Generic
open System.Numerics

[<Class>]
type vector<'T> =

    new : 'T [] -> 'T vector
    
    member size : int

    member element : 'T []

    member Format : unit -> string


module Vector =
    
    val inline Add : vec1 : 'T vector -> vec2 : 'T vector -> 'T vector
        when 'T : (static member (+) : 'T * 'T -> 'T)

    val inline Sub : vec1 : 'T vector -> vec2 : 'T vector -> 'T vector
        when 'T : (static member(-) : 'T * 'T -> 'T)

    val inline InnerProduct : vec1 : 'T vector -> vec2 : 'T vector -> 'T vector
        when 'T : (static member (*) : 'T * 'T -> 'T)