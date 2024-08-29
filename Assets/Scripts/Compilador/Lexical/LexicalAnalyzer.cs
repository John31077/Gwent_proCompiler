using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class LexicalAnalyzer
    {
        Dictionary<string, string> operators = new Dictionary<string, string>();
        Dictionary<string, string> keywords = new Dictionary<string, string>();
        Dictionary<string, string> texts = new Dictionary<string, string>();

        public IEnumerable<string> Keywords { get { return keywords.Keys; } }

        /* Associates an operator symbol with the correspondent token value
            Asocia un simbolo operador con el correspondiente valor del token*/
        public void RegisterOperator(string op, string tokenValue)
        {
            this.operators[op] = tokenValue;
        }

        /* Associates a keyword with the correspondent token value */
        public void RegisterKeyword(string keyword, string tokenValue)
        {
            this.keywords[keyword] = tokenValue;
        }

        /* Associates a Text literal starting delimiter with their correspondent ending delimiter */
        public void RegisterText(string start, string end)
        {
            this.texts[start] = end;
        }

        
        /* Matches a new symbol in the code and read it from the string. The new symbol is added to the token list as an operator. */
        private bool MatchSymbol(TokenReader stream, List<Token> tokens)
        {
            foreach (var op in operators.Keys.OrderByDescending(k => k.Length))
                if (stream.Match(op))
                {
                    tokens.Add(new Token(TokenType.Symbol, operators[op], stream.Location));
                    return true;
                }
            return false;
        }

        /* Matches a Text part in the code and read the literal from the stream.
        The tokens list is updated with the new string token and errors is updated with new errors if detected. */
        private bool MatchText (TokenReader stream, List<Token> tokens, List<CompilingError> errors)
        {
            foreach (var start in texts.Keys.OrderByDescending(k=>k.Length))
            {
                string text;
                if (stream.Match(start))
                {
                    if (!stream.ReadUntil(texts[start], out text))
                        errors.Add(new CompilingError(stream.Location, ErrorCode.Expected, texts[start]));
                    tokens.Add(new Token(TokenType.Text, text, stream.Location));
                    return true;
                }
            }
            return false;
        }

        /* Returns all tokens read from the code and populate the errors list with all lexical errors detected. */
        public IEnumerable<Token> GetTokens(string fileName, string code, List<CompilingError> errors)
        {
            List<Token> tokens = new List<Token>();

            TokenReader stream = new TokenReader(fileName, code);

            while (!stream.EOF) //Funciona mientras "pos" no se salga del tamaño de code
            {
            
                string value; //Se define value, en un principio es el elemento posible token



                if (stream.ReadWhiteSpace()) continue; //si hay un espacio en blanco pasa a la siguiente iteracion
            
                /////////////////////////////////////////////////////////////////////////////////
                if (!stream.EOF && stream.Match("."))
                {
                    tokens.Add(new Token(TokenType.Symbol, operators["."], stream.Location));
                }
                /////////////////////////////////////////////////////////////////////////////////


                if (stream.ReadID(out value)) // entra si ReadID fabrico un value con tamaño mayor a 0 (ReadID fabrica un posible identificador o un keyword)
                {
                    //si en el diccionario de keywords está value como keyword, añade a la lista de tokens un token de tipo keyword
                    if (keywords.ContainsKey(value))  tokens.Add(new Token(TokenType.Keyword, keywords[value], stream.Location));

                    //  en cualquier otro el value actual seria un identificador, añade a la lista de tokens un token de tipo identificador
                    else tokens.Add(new Token(TokenType.Identifier, value, stream.Location));
                        
                    continue; //Siga a la siguiente iteracion (no tiene sentido seguir evaluando con este value)
                }
                
                    
                if(stream.ReadNumber(out value))
                {
                    double d;
                    if (!double.TryParse(value, out d))
                    {
                        errors.Add(new CompilingError(stream.Location, ErrorCode.Invalid, "Number format"));
                    }

                    tokens.Add(new Token(TokenType.Number, value, stream.Location));
                    continue;
                }
                /*else if (value[value.Length-1] == '.')
                {
                    errors.Add(new CompilingError(stream.Location, ErrorCode.Invalid, "Number format"));
                    continue;
                }*/
                
                


                if (MatchText(stream, tokens, errors))
                    continue;

                if (MatchSymbol(stream, tokens))
                    continue;

                var unkOp = stream.ReadAny();
                errors.Add(new CompilingError(stream.Location, ErrorCode.Unknown, unkOp.ToString()));
                
            }

            return tokens;
        }

        /* Allows to read from a string numbers, identifiers and matching some prefix. 
        It has some useful methods to do that */
        class TokenReader
        {
            string FileName;
            string code;
            int pos;
            int line;
            int lastLB;

            public TokenReader(string fileName, string code)
            {
                this.FileName = fileName;
                this.code = code;
                this.pos = 0;
                this.line = 1;
                this.lastLB = -1;
            }

            public CodeLocation Location
            {
                get
                {
                    return new CodeLocation
                    {
                        File = FileName,
                        Line = line,
                        Column = pos - lastLB
                    };
                }
            }

            /* Peek the next character */
            public char Peek() 
            {
                if (pos < 0 || pos >= code.Length) //Entra si la posicion "pos" esta por debajo del 0 o "pos" se salió del tamaño de code
                    throw new InvalidOperationException();

                return code[pos]; //En cualquier otro caso retorna el char que esté en la posicion code[pos], osea toma el char
            }

            public bool EOF //Basicamente devuelve true si la posicion del TokenReader es igual o mayor que el tamaño de code (el texto a compilar)
            {
                get { return pos >= code.Length; }
            }

            public bool EOL // Retorna true si el EOF dió true o si en la posicion "pos" de code hay un salto de linea
            {
                get { return EOF || code[pos] == '\n'; }
            }

            public bool ContinuesWith(string prefix)
            {
                if (pos + prefix.Length > code.Length) return false; //retorna false si "pos" mas la longitud del prefijo es mayor que code.Length
                    
                for (int i = 0; i < prefix.Length; i++)
                {
                    if (code[pos + i] != prefix[i]) return false;
                }       
                return true;
            }

            public bool Match(string prefix) //"."
            {
                if (ContinuesWith(prefix))
                {
                    pos += prefix.Length;
                    return true;
                }

                return false;
            }

            public bool ValidIdCharacter(char c, bool begining) //retorna true si el caracter que se evalua es "_" o si el caracter es una letra. De lo contrario retorna false
            {
                return c == '_' || (begining ? char.IsLetter(c) : char.IsLetterOrDigit(c));
            }

            public bool ReadID(out string id) //entra con Value como parametro (en un principio null)
            {
                id = ""; //value deja de ser null o se actualiza a ""

                while (!EOL && ValidIdCharacter(Peek(), id.Length == 0)) //se ejecuta mientras "pos" no tenga un salto de linea ni cumpla EOF y el caracter que se evalua es "_" o una letra
                {
                    id += ReadAny();  //se le añade a id otro caracter (por el out seria value)
                }
                return id.Length > 0; //devuelve true si id es mayor que 0
            }

            public bool ReadNumber(out string number)
            {
                number = ""; //en este caso value ahora seria number


                while (!EOL && char.IsDigit(Peek())) //trabaja mientras no halla salto de linea, "pos" no se pase de code.Length y el elemento actual sea un numero
                {
                    number += ReadAny(); //va acoplando cada digito
                }

                if (!EOL && Match("."))
                {
                    // read decimal part
                    number += '.';
                    //////////////////////////////////////////////////////////////////////////
                    if(EOL || !char.IsDigit(Peek()))
                    {
                        return false;
                    }
                    ///////////////////////////////////////////////////////////////////////////
                    while (!EOL && char.IsDigit(Peek()))
                        number += ReadAny();
                }

                if (number.Length == 0) return false;
                    
                while (!EOL && char.IsDigit(Peek()) && number.Contains('.')) //aqui puede estar el error
                {
                    number += ReadAny();
                }

                return number.Length > 0;
            }

            public bool ReadUntil(string end, out string text)
            {
                text = "";
                while (!Match(end))
                {
                    if (EOL || EOF)
                        return false;
                    text += ReadAny();
                }
                return true;
            }

            public bool ReadWhiteSpace() //retorna true si hay un espacio en blanco y mueve el pos a la siguiente posicion, de lo contrario solamente devuelve false
            {
                if (char.IsWhiteSpace(Peek())) //entra si el elemento "Peek()" es un espacio en blanco
                {
                    ReadAny(); //Cambia a la siguiente posicion suponiendo que "pos" no sea mayor o igual que code.Length
                    return true;
                }
                return false; // al no ser un espacio en blanco el Peek() retorna false
            }

            //////////////////////////////////////////////////////////////////////
            public bool ReadPointSpace()
            {
                if (Peek() == '.')
                {
                    ReadAny();
                    return true;
                }
                return false;
            }
            ////////////////////////////////////////////////////////////////////////
            public char ReadAny() //Sencillamente devuelve el siguiente elemento del code, salta de linea si es necesario (cambiando el valor de linea)
            {
                if (EOF) //Entra si EOF es true (si la "pos" a evaluar esta por encima del tamaño del code)
                    throw new InvalidOperationException();

                if (EOL) //Entra unicamente si en la posicion "pos" hay un salto de linea
                {
                    line++; //Se salta de linea
                    lastLB = pos; //la ultima posicion fue "pos"
                }
                return code[pos++]; //Retorna el valor de code en la siguiente posicion ("pos" se le adiciono 1)
            }
        }

    }

