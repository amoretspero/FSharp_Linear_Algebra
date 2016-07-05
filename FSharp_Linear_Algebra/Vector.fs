namespace FSharp_Linear_Algebra

open System
open System.Collections
open System.Collections.Generic
open System.Numerics

type vector<'T>(element : 'T []) =
    
    // Private data -------------------------------------------------

    let _size = element.Length

    let _elem = element

    // Instance properties ------------------------------------------

    member v.size = _size

    member v.element = _elem

    // Instance methods ---------------------------------------------

    member v.Format() =
        let sb = System.Text.StringBuilder()
        sb.Append("[") |> ignore
        for i=0 to _size - 1 do
            sb.AppendFormat("{0} ", _elem.[i]) |> ignore
            if (i < _size - 1) then sb.Append(", ") |> ignore
        sb.Append("]") |> ignore
        sb.ToString()

    // Static properties --------------------------------------------

    

    // Static methods -----------------------------------------------
                    

    // Explicit Constructors ----------------------------------------

    

module Vector =
    let inline Add (vec1 : 'T vector) (vec2 : 'T vector) : 'T vector =
        vector(Array.map2 (fun x y -> x + y) vec1.element vec2.element)

    let inline Subtract (vec1 : 'T vector) (vec2 : 'T vector) : 'T vector =
        vector(Array.map2 (fun x y -> x - y) vec1.element vec2.element)

    let inline InnerProduct (vec1 : 'T vector) (vec2 : 'T vector) : 'T vector =
        vector(Array.map2 (fun x y -> x * y) vec1.element vec2.element)