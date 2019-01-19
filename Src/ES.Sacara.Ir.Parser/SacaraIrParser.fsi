// Signature file for parser generated by fsyacc
module ES.Sacara.Ir.Parser.SacaraIrParser
type token = 
  | INTEGER of (int32)
  | LABEL of (string)
  | IDENTIFIER of (string)
  | STRING of (string)
  | DIV
  | AND
  | SHIFTR
  | SHIFTL
  | OR
  | NOT
  | XOR
  | NOR
  | SETIP
  | SETSP
  | INC
  | EOF
  | CMP
  | GETSP
  | SWRITE
  | SREAD
  | BYTE
  | WORD
  | DWORD
  | NEWLINE
  | COMMA
  | SUB
  | MUL
  | GETIP
  | RET
  | JUMP
  | JUMPIFL
  | JUMPIFLE
  | JUMPIFG
  | JUMPIFGE
  | ALLOCA
  | HALT
  | PROC
  | ENDP
  | PUSH
  | POP
  | ADD
  | NOP
  | CALL
  | NCALL
  | WRITE
  | NWRITE
  | READ
  | NREAD
type tokenId = 
    | TOKEN_INTEGER
    | TOKEN_LABEL
    | TOKEN_IDENTIFIER
    | TOKEN_STRING
    | TOKEN_DIV
    | TOKEN_AND
    | TOKEN_SHIFTR
    | TOKEN_SHIFTL
    | TOKEN_OR
    | TOKEN_NOT
    | TOKEN_XOR
    | TOKEN_NOR
    | TOKEN_SETIP
    | TOKEN_SETSP
    | TOKEN_INC
    | TOKEN_EOF
    | TOKEN_CMP
    | TOKEN_GETSP
    | TOKEN_SWRITE
    | TOKEN_SREAD
    | TOKEN_BYTE
    | TOKEN_WORD
    | TOKEN_DWORD
    | TOKEN_NEWLINE
    | TOKEN_COMMA
    | TOKEN_SUB
    | TOKEN_MUL
    | TOKEN_GETIP
    | TOKEN_RET
    | TOKEN_JUMP
    | TOKEN_JUMPIFL
    | TOKEN_JUMPIFLE
    | TOKEN_JUMPIFG
    | TOKEN_JUMPIFGE
    | TOKEN_ALLOCA
    | TOKEN_HALT
    | TOKEN_PROC
    | TOKEN_ENDP
    | TOKEN_PUSH
    | TOKEN_POP
    | TOKEN_ADD
    | TOKEN_NOP
    | TOKEN_CALL
    | TOKEN_NCALL
    | TOKEN_WRITE
    | TOKEN_NWRITE
    | TOKEN_READ
    | TOKEN_NREAD
    | TOKEN_end_of_input
    | TOKEN_error
type nonTerminalId = 
    | NONTERM__startprogram
    | NONTERM_program
    | NONTERM_sourceElementList
    | NONTERM_procDefinition
    | NONTERM_statementList
    | NONTERM_statement
    | NONTERM_statementNoLabel
    | NONTERM_expression
    | NONTERM_integerList
    | NONTERM_dataList
    | NONTERM_data
/// This function maps tokens to integer indexes
val tagOfToken: token -> int

/// This function maps integer indexes to symbolic token ids
val tokenTagToTokenId: int -> tokenId

/// This function maps production indexes returned in syntax errors to strings representing the non terminal that would be produced by that production
val prodIdxToNonTerminal: int -> nonTerminalId

/// This function gets the name of a token as a string
val token_to_string: token -> string
val program : (Microsoft.FSharp.Text.Lexing.LexBuffer<'cty> -> token) -> Microsoft.FSharp.Text.Lexing.LexBuffer<'cty> -> ( Program ) 
