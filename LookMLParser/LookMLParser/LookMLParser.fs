namespace LookMLParser

module LookMLParser = 

    let single_whitespace = BasicParser.anyOf [ ' '  ; '\n' ; '\r' ; '\t' ]
    let whitespace = BasicParser.many single_whitespace

