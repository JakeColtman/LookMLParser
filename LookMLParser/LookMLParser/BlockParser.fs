namespace LookMLParser 

module BlockParser = 
    open BasicParser;
    open LookMLParser;

    let endOfLineParser = string_parser "\r\n"
    let single_whitespace : Parser<char> = anyOf [ ' ' ; '\t' ]

    let parser_block (indentationLevel : int)= 
        let line_start_parser = manyN single_whitespace indentationLevel
        let line_parser = line_start_parser >>. extendedString .>> colon .>> whitespace .>>. extendedString.>> endOfLineParser
        many line_parser
     //  let parse_first_whitespace = many single_whitespace |>> (fun x -> x.Length)
     //  parseNOf single_whitespace (fst blockIndentation)

    
        


