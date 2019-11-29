﻿namespace ES.Sacara.Ir.Parser

open System
open ES.Sacara.Ir.Parser.IrAst
open System.Text

module internal IrParserUtility =    

    let emptyStatement() = 
        Empty

    let number(num: Int32) =
        Number num

    let identifier(name: String) =
        Identifier name

    let push(expr: Expression) =
        Push {Operand=expr}

    let pop(identifier: String) =
        Pop {Identifier=identifier}

    let inc() =
        Inc

    let procedureDefinition(name: String, statements: Statement list) =
        Procedure {Name=name; Body=statements}

    let procedureDefinitionWithArguments(name: String, parameters: String list, statements: Statement list) =
        // main function must be considered in a different way since 
        // it is supposed that the program argument are passed as local vars and not 
        // pushed in the stack (which is a forbidden place to user). By doing a push and pop 
        // of local vars I'm sure that the code allocate the right space. If this is not done
        // and the user specifies few parameters a problem may occour.
        let parameterStatements = [
            for arg in parameters do                
                if name.Equals("main", StringComparison.Ordinal) then 
                    yield push(identifier(arg))
                yield pop(arg)
        ]            
        Procedure {Name=name; Body=parameterStatements@statements}

    let label(name: String, statement: Statement) =
        Label {Name=name.Substring(0, name.Length-1); Statement=statement}

    let call() =
        Call {Native=false}

    let callNative() =
        Call {Native=true}

    let read() =
        Read {Native=false}

    let readNative() =
        Read {Native=true}

    let write() =
        Write {Native=false}

    let writeNative() =
        Write {Native=true}

    let jumpIf(equals: Boolean, less: Boolean) =
        JumpIf {JumpIfEquals=equals; JumpIfLess=less}

    let jump() =
        Jump

    let alloca() =
        Alloca

    let memoryByte(values: Int32 list list) =
        Byte (values |> List.concat)

    let memoryWord(values: Int32 list) =
        let canonicalizedValues = 
            values 
            |> List.map(fun value -> if value > 0XFFFF then 0xFFFF else value)

        Word canonicalizedValues

    let memoryDword(values: Int32 list) =
        DoubleWord values

    let getStringBytes(str: String) =
        Encoding.UTF8.GetBytes(str)
        |> Seq.map(fun b -> int32 b)
        |> Seq.toList

    let nop() =
        Nop

    let getIp() =
        GetIp

    let add() =
        Add

    let ret() =
        Ret

    let halt() =
        Halt

    let compare() =
        Cmp

    let getSp() =
        GetSp

    let stackWrite() =
        StackWrite

    let stackRead() =
        StackRead

    let sub() =
        Sub

    let mul() =
        Mul

    let div() =
        Div

    let bitAnd() =
        And

    let shiftRight() =
        ShiftRight

    let shiftLeft() =
        ShiftLeft

    let bitOr() =
        Or

    let bitNot() =
        Not

    let xor() =
        Xor

    let nor() =
        Nor

    let setIp() =
        SetIp

    let setSp() =
        SetSp

    // list of directives or complex expression, 
    // these are not real instruction and must be translated to statement block or expression block
    let jumpDirective(expr: Expression) =
        [
            push(expr)
            jump()
        ] |> Block

    let invoke(procName: String, args: Expression list) =
        [
            for arg in args |> List.rev -> push(arg)
            yield push(number(args.Length))
            yield push(identifier(procName.TrimStart('.')))
            yield call()
        ] |> Block

    let setLocalVariable(variable: String, value: Expression) =
        [
            match value with
            | StatementExpression statement -> 
                match statement with
                | Block statementList -> yield! statementList
                | _ -> failwith "Invalid operation on local var set"
            | Number num -> 
                yield push(number(num))
            | Identifier ident -> 
                yield push(identifier(ident))            
            yield pop(variable)
        ] |> Block 

    let addDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            add()
        ] 
        |> Block

    let addDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| addDirective(expression1, expression2)

    let subDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            sub()
        ] 
        |> Block

    let subDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| subDirective(expression1, expression2)

    let mulDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            mul()
        ] 
        |> Block

    let mulDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| mulDirective(expression1, expression2)

    let divDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            div()
        ] 
        |> Block

    let divDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| divDirective(expression1, expression2)

    let cmpDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            compare()
        ] 
        |> Block

    let cmpDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| cmpDirective(expression1, expression2)

    let andDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            bitAnd()
        ] 
        |> Block

    let andDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| andDirective(expression1, expression2)

    let orDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            bitOr()
        ] 
        |> Block

    let orDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| orDirective(expression1, expression2)

    let shiftrDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            shiftRight()
        ] 
        |> Block

    let shiftrDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| shiftrDirective(expression1, expression2)

    let shiftlDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            shiftLeft()
        ] 
        |> Block

    let shiftlDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| shiftlDirective(expression1, expression2)

    let xorDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            xor()
        ] 
        |> Block

    let xorDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| xorDirective(expression1, expression2)

    let norDirective(expression1: Expression, expression2: Expression) =
        [
            push(expression2)
            push(expression1)
            nor()
        ] 
        |> Block

    let norDirectiveExpression(expression1: Expression, expression2: Expression) =
        StatementExpression <| norDirective(expression1, expression2)

    let incDirective(variable: String) =
        [
            push(identifier(variable))
            inc()
            pop(variable)
        ] 
        |> Block

    let includeFile(filePath: String) =
        IncludeFile filePath

    let readByteDirective(address: Expression) =
        [
            push(number(1))
            push(address)
            read()
        ] 
        |> Block

    let readWordDirective(address: Expression) =
        [
            push(number(2))
            push(address)
            read()
        ] 
        |> Block

    let readDoubleWordDirective(address: Expression) =
        [
            push(number(3))
            push(address)
            read()
        ] 
        |> Block