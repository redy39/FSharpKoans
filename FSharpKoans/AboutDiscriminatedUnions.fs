﻿namespace FSharpKoans
open NUnit.Framework

(*
    A discriminated union is a disjoint set of named cases, where each case
    may have some linked/associated data.  If a discriminated union case has
    associated data, the case name is a function which takes the associated
    data as input and gives a value of the discriminated union type as output.
*)


module ``08: The Good Kind of Discrimination`` = 
    type Subject = // <-- feel free to add your own subjects!
    | Philosophy
    | Linguistics
    | ComputerScience
    | Mathematics
    | Economics
    | Management

    type UndergraduateDegree = 
    | BSc of Subject * Subject
    | BCom of Subject * Subject
    | BPharm
    | BA of Subject * Subject

    type PostgraduateDegree =
    | Honours of Subject
    | Masters of Subject

    [<Test>]
    let ``01 A case isn't the same as a type`` () = 
        let aDegree = BSc (Linguistics, ComputerScience)
        let anotherDegree = BPharm
        let philosopherKing = Masters Philosophy
        aDegree |> should be ofType<UndergraduateDegree> 
        anotherDegree |> should be ofType<UndergraduateDegree> 
        philosopherKing |> should be ofType<PostgraduateDegree> 
   
    [<Test>]
    let ``02 Creating & pattern-matching a discriminated union`` () = 
        let randomOpinion degree =
            match degree with
            | BSc (_, ComputerScience) | BSc (ComputerScience, _) -> "Good choice!"
            | BSc _ -> "!!SCIENCE!!"
            | BPharm -> "Meh, it's OK."
            | BCom _ -> "Money, money, money."
            | BA _ -> "A thinker, eh?"
        randomOpinion (BSc (Mathematics,ComputerScience)) |> should equal "Good choice!"
        randomOpinion (BSc (Mathematics,Economics)) |> should equal "!!SCIENCE!!"
        randomOpinion (BCom (Management, Economics)) |> should equal "Money, money, money."
        randomOpinion (BCom (Linguistics, Management)) |> should equal "Money, money, money."
        randomOpinion (BA (Linguistics, Philosophy)) |> should equal "A thinker, eh?"
        randomOpinion BPharm |> should equal "Meh, it's OK."

    type EquipmentStatus =
    | Available
    | Broken of int // takes an int, gives back en EquipmentStatus
    | Rented of string

    [<Test>]
    let ``03 A discriminated union case with associated data is a function`` () =
        Broken |> should be ofType<int->EquipmentStatus>
        Rented |> should be ofType<string->EquipmentStatus>

    type BinaryTree =
    | Empty
    | Node of string * BinaryTree * BinaryTree

    [<Test>]
    let ``04 A discriminated union can refer to itself (i.e., it can be recursive).`` () =
        let rec depth x =
            match x with
            | Empty -> 0
            | Node (_, a, b) -> 1 + max (depth a) (depth b)
        let d = Node ("last",Empty,Empty)
        let c = Node ("third",Empty,d) 
        let b = Node ("second",Empty,c)
        let a = Node ("first",Empty,b)// <-- you may want to spread this over multiple lines and/or let-bindings ...!
        depth a |> should equal 4
