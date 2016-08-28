namespace FSharp_Linear_Algebra.Matrix

open System
open System.Collections
open System.Collections.Generic
open System.Linq
open System.Numerics

///<summary>Class for random-generated matrices.</summary>
[<Class>]
type RandomMatrix () =
    
    /// <summary>Generates Byte random matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member rm.RandomMatrixByte (row : int) (col : int) : Byte matrix =
        let rnd = new System.Random()
        let elem = Array2D.init row col (fun _ _ -> 
                                                    let buf = [| 0uy |]
                                                    rnd.NextBytes(buf)
                                                    buf.First())
        matrix<byte>(row, col, elem)

    
    /// <summary>Generates int32 random matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member rm.RandomMatrixInt32 (row : int) (col : int) : Int32 matrix =
        let rnd = new System.Random()
        let elem = Array2D.init row col (fun _ _ -> rnd.Next())
        matrix<int32>(row, col, elem)

    /// <summary>Generates int64 random matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member rm.RandomMatrixInt64 (row : int) (col : int) : Int64 matrix =
        let rnd = new System.Random()
        let elem = Array2D.init row col (fun _ _ -> ((int64)(rnd.Next()) <<< 32) + (int64)(rnd.Next()))
        matrix<int64>(row, col, elem)

    /// <summary>Generates single(float32) matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member rm.RandomMatrixSingle (row : int) (col : int) : single matrix =
        let rnd = new System.Random()
        let elem = Array2D.init row col (fun _ _ -> (float32)(rnd.NextDouble()))
        matrix<single>(row, col, elem)

    /// <summary>Generates double matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member rm.RandomMatrixDouble (row : int) (col : int) : double matrix =
        let rnd = new System.Random()
        let elem = Array2D.init row col (fun _ _ -> rnd.NextDouble())
        matrix<double>(row, col, elem)

    /// <summary>Generates decimal matrix of given size.</summary>
    /// <param name="row">Number of rows.</param>
    /// <param name="col">Number of columns.</param>
    /// <returns>Returns generated row*col size matrix.</returns>
    member rm.RandomMatrixDecimal (row : int) (col : int) : decimal matrix =
        let rnd = new System.Random((int)(System.DateTime.Now.ToBinary()))
        let scale = (byte)(rnd.Next(29))
        let elem = Array2D.init row col (fun _ _ -> new Decimal(rnd.Next(), rnd.Next(), rnd.Next(), (rnd.Next(0, 1) = 1), (byte)(rnd.Next(29))))
        matrix<decimal>(row, col, elem)
