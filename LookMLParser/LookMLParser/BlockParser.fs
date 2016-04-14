namespace LookMLParser 

module BlockParser = 
    open BasicParser;

    let endOfLineParser = anyOf ['\n' ; '\r' ]
    let single_whitespace : Parser<char> = anyOf [ ' ' ; '\t' ]

   // let parser_block = 
     //  let parse_first_whitespace = many single_whitespace |>> (fun x -> x.Length)
     //  parseNOf single_whitespace (fst blockIndentation)

    
        


