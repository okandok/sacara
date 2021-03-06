﻿namespace ES.Sacara.Ir.Core

open System
open System.Reflection
open System.Collections.Generic
open Newtonsoft.Json
open System.IO
open Microsoft.FSharp.Reflection

[<Struct>]
// the operand direction is the same as the one specified in INTEL syntax, that is: <op> <dst>, <src>
type IrInstruction =
    | Ret
    | Nop
    | Add
    | Push
    | Pop
    | Call
    | NativeCall
    | Read
    | NativeRead
    | Write
    | NativeWrite
    | GetIp
    | Jump
    | JumpIfLess
    | JumpIfLessEquals
    | JumpIfGreater
    | JumpIfGreaterEquals
    | Alloca
    | Byte
    | Word
    | DoubleWord
    | Halt
    | Cmp
    | GetSp
    | StackWrite
    | StackRead
    | Sub
    | Mul
    | Div
    | Mod
    | And
    | ShiftRight
    | ShiftLeft
    | Or
    | Not
    | Xor
    | Nor
    | SetIp
    | SetSp
    | Inc

    override this.ToString() =
        match this with
        | Ret -> "ret"
        | Nop -> "nop"
        | Add -> "add"
        | Push -> "push"
        | Pop -> "pop"
        | Call -> "call"
        | NativeCall -> "ncall"
        | Read -> "read"
        | NativeRead -> "nread"
        | Write -> "write"
        | NativeWrite -> "nwrite"
        | GetIp -> "getip"
        | Jump -> "jump"
        | JumpIfLess -> "jumpifl"
        | JumpIfLessEquals -> "jumpifle"
        | JumpIfGreater -> "jumpifg"
        | JumpIfGreaterEquals -> "jumpifge"
        | Alloca -> "alloca"
        | Byte -> "byte"
        | Word -> "word"
        | DoubleWord -> "dword"
        | Halt -> "halt"
        | Cmp -> "cmp"
        | GetSp -> "getsp"
        | StackWrite -> "swrite"
        | StackRead -> "sread"
        | Sub -> "sub"
        | Mul -> "mul"
        | Div -> "div"
        | Mod -> "mod"
        | And -> "and"
        | ShiftRight -> "shiftr"
        | ShiftLeft -> "shiftl"
        | Or -> "or"
        | Not -> "not"
        | Xor -> "xor"
        | Nor -> "nor"
        | SetIp -> "setip"
        | SetSp -> "setsp"
        | Inc -> "inc"

// these are the op codes of the VM
type VmInstruction =    
    | VmRet
    | VmNop
    | VmAdd
    | VmPushImmediate
    | VmPushVariable
    | VmPop
    | VmCall
    | VmJump
    | VmAlloca
    | VmHalt
    | VmCmp
    | VmStackWrite
    | VmStackRead
    | VmJumpIfLess
    | VmJumpIfLessEquals
    | VmJumpIfGreater
    | VmJumpIfGreaterEquals
    | VmRead
    | VmWrite
    | VmGetIp
    | VmGetSp    
    | VmNativeRead    
    | VmNativeWrite
    | VmNativeCall
    | VmByte
    | VmWord
    | VmDoubleWord
    | VmSub
    | VmMul
    | VmDiv
    | VmAnd
    | VmShiftRight
    | VmShiftLeft
    | VmOr
    | VmNot
    | VmXor
    | VmNor
    | VmSetIp
    | VmSetSp
    | VmInc
    | VmMod

module Instructions =
    type VmOpCodeItem() =
        member val Name = String.Empty with get, set
        member val Bytes = new List<Byte array>() with get, set
        member val OpCodes = new List<Int32>() with get, set

    let readVmOpCodeBinding() =
        let currentDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)
        let filename = Path.Combine(currentDir, "vm_opcodes.json")
        if File.Exists(filename) then
            let json = File.ReadAllText(filename)
            JsonConvert.DeserializeObject<List<VmOpCodeItem>>(json)
            |> Seq.map(fun item ->
                let vmOpCode =
                    FSharpType.GetUnionCases typeof<VmInstruction>
                    |> Array.find(fun case -> case.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase))
                    |> fun case -> FSharpValue.MakeUnion(case,[||]) :?> VmInstruction
                let bytes = item.OpCodes |> Seq.toList
                (vmOpCode, bytes)
            )
            |> Map.ofSeq        
        else
            raise(new ApplicationException(String.Format("JSON file with opcodes '{0}' not found.", filename)))

    // each VM opcode can have different format. The file was generate with the GenerateVmOpCodes utility
    let bytes = readVmOpCodeBinding()