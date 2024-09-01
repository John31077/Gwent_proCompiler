using System.Collections.Generic;
using System.IO.Pipes;
using System.Runtime.InteropServices.WindowsRuntime;

//using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

public class Parser
{
    public Parser(TokenStream stream)
    {
        Stream = stream;
    }
    public TokenStream Stream { get; private set; }
    public static List<CompilingError> compilingErrors = new List<CompilingError>(); //Necesario para el metodo de brackets

    public ElementalProgram ParseProgram(List<CompilingError> errors)
    {
        ElementalProgram program = new ElementalProgram(new CodeLocation());

        if (!Stream.CanLookAhead(0)) return program;

        
        
        //Here we parse all the declared effects
        while (Stream.LookAhead().Value == TokenValues.effect)
        {
 
            Effect effect = ParseEffect(errors);
            program.Effects[effect.Id] = effect;
 
            if (!Stream.Next())
            {
                break;
            }
        }

        //Here we parse all the declared cards
       /* while (Stream.LookAhead().Value == TokenValues.card)
        {
            CardG card = ParseCard(errors);
            program.Cards[card.Id] = card;

            if (!Stream.Next())
            {
                break;
            }
        }
        */
       return program;
    }

    private Effect ParseEffect(List<CompilingError> errors)
    {
        
        
        Effect effect = new Effect("null", Stream.LookAhead().Location);
        

        if (!Stream.Next(TokenValues.OpenCurlyBraces)) // {
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OpenCurlyBraces Expected"));
        }
        
        if (!Stream.Next(TokenValues.Name)) // Name
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Name Expected"));
        }

        if (!Stream.Next(TokenValues.TwoPoints)) //:
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
        }
        
        if (!Stream.Next(TokenType.Text)) // Text
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Text Expected"));
        }

        effect.Id = Stream.LookAhead().Value; //Se añade el nombre al effecto
        
        if (!Stream.Next(TokenValues.ValueSeparator)) // ,
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "ValueSeparator Expected"));
        }

        if (Stream.LookAhead(1).Value == TokenValues.Params) // Params
        {
            Stream.MoveNext(1);

            if (!Stream.Next(TokenValues.TwoPoints)) // :
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
            }

            if (!Stream.Next(TokenValues.OpenCurlyBraces)) // {
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OpenCurlyBraces Expected"));
            }

            if (!Stream.Next(TokenValues.ClosedCurlyBraces)) //Si no hay una llave cerrada entra
            {
                ParseParams(errors, effect);
            } // }

            if (!Stream.Next(TokenValues.ValueSeparator)) // ,
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "ValueSeparator Expected"));
            }
        }
        
        if (!Stream.Next(TokenValues.Action)) // Action
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Action Expected"));
        }

        if (!Stream.Next(TokenValues.TwoPoints)) //:
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
        }
        if (!Stream.Next(TokenValues.OpenBracket)) //(
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OpenBracket Expected"));
        }

        if (!Stream.Next(TokenValues.targets)) //targets
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "targets Expected"));
        }

        if (!Stream.Next(TokenValues.ValueSeparator)) // ,
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "targets Expected"));
        }

        if (!Stream.Next(TokenValues.context)) // context
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "context Expected"));
        }

        if (!Stream.Next(TokenValues.ClosedBracket)) // )
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "context Expected"));
        }

        if (!Stream.Next(TokenValues.Implica)) // =>
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "=> Expected"));
        }

        if (!Stream.Next(TokenValues.OpenCurlyBraces)) // {
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OpenCurlyBraces Expected"));
        }
        
        if (Stream.LookAhead(1).Value != TokenValues.ClosedCurlyBraces) // Si no hay } parsea Action
        {
            ParseAction(errors, effect.ActionList);
            //el cierre restante despues de parsear Action
            if (!Stream.Next(TokenValues.ClosedCurlyBraces))
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} Expected"));
            }
            return effect;
        }
        else
        {
            Stream.MoveNext(1);
            if (!Stream.Next(TokenValues.ClosedCurlyBraces))
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} Expected"));
            }
        }

        return effect;                           
    }

    /*public CardG ParseCard(List<CompilingError> errors)
    {
        CardG card = new CardG(null, "null", null, null, null, Stream.LookAhead().Location);

        if (!Stream.Next(TokenValues.OpenCurlyBraces)) // {
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OpenCurlyBraces Expected"));
        }

        if (!Stream.Next(TokenValues.Type)) // Type
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Type Expected"));
        }

        if (!Stream.Next(TokenValues.TwoPoints)) // :
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
        }

        if (!Stream.Next(TokenType.Text)) // Text
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Text Expected"));
        }
        card.Type = Stream.LookAhead().Value;

        if (!Stream.Next(TokenValues.ValueSeparator)) // ,
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ", Expected"));
        }

        if (!Stream.Next(TokenValues.Name)) // Name
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Name Expected"));
        }

        if (!Stream.Next(TokenValues.TwoPoints)) // :
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
        }

        if (!Stream.Next(TokenType.Text)) // Text
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Text Expected"));
        }
        card.Id = Stream.LookAhead().Value;

        if (!Stream.Next(TokenValues.ValueSeparator)) // ,
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ", Expected"));
        }

        if (!Stream.Next(TokenValues.Power)) // Power
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Power Expected"));
        }

        if (!Stream.Next(TokenValues.TwoPoints)) // :
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
        }

        if (Stream.LookAhead(1).Value != TokenValues.ValueSeparator) // ,
        {
            Expression? exp = ParseExpression(); 
            if(exp == null)
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
            }
            card.Power = exp;
            
            if (!Stream.Next(TokenValues.ValueSeparator)) // ,
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ", Expected"));
            }
        }

        if (!Stream.Next(TokenValues.Range)) // Range
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Range Expected"));
        }

        if (!Stream.Next(TokenValues.TwoPoints)) // :
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
        }

        if (!Stream.Next(TokenValues.OpenCorchetes)) // [
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
        }

        if (!Stream.Next(TokenType.Text)) // Text
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Text Expected"));
        }
        card.range = Stream.LookAhead().Value;

        if (!Stream.Next(TokenValues.OpenCorchetes)) // ]
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints Expected"));
        }

        if (!Stream.Next(TokenValues.OnActivation)) // OnActivation
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "OnActivation Expected"));
        }

        if (!Stream.Next(TokenValues.TwoPoints)) // :
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ": Expected"));
        }

        if (!Stream.Next(TokenValues.OpenCorchetes)) // [
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "[ Expected"));
        }

        if (Stream.LookAhead(1).Value == TokenValues.ClosedCorchetes)
        {
            return card;
        }



        






        //Ver bien esta parte
    }



    private Expression ParseOnActivation(List<CompilingError> errors)
    {
        while (Stream.Position < Stream.Count)
        {

        }
    }*/







    private Expression ParsePredicate(List<CompilingError> errors)
    {
        if (!Stream.Next(TokenValues.OpenBracket))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "( expected"));
        }

        Expression? exp = ParseExpression(); 
        if(exp == null)
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
        }

        if (!Stream.Next(TokenValues.ClosedBracket))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ") expected"));
        }

        if (!Stream.Next(TokenValues.Implica))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "=> expected"));
        }

        Expression? exp1 = ParseExpression(); 
        if(exp == null)
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
        }

        Predicate predicate = new Predicate(Stream.LookAhead().Location);
        predicate.Left = exp;
        predicate.Right = exp1;
        if (predicate.Left == null || predicate.Right == null)
        {
            return null;
        }
        return predicate;
    }
    private Expression ParseBracket(List<CompilingError> errors, Expression expression)
    {
        if (Stream.LookAhead(1).Value == TokenValues.OpenBracket)
        {
            Stream.MoveNext(1);

           // List<CompilingError> errorsPredicate = new List<CompilingError>();
            Bracket bracket = new Bracket(Stream.LookAhead().Location);
            Expression exp = ParseExpression();
            if (exp == null)
            {
                if (Stream.LookAhead(1).Value == TokenValues.ClosedBracket)
                {
                    Stream.MoveNext(1);
                    bracket.Left = expression;
                    bracket.Right = null;
                    return bracket;
                }

                exp = ParsePredicate(compilingErrors);
                if (exp == null)
                {
                    return null;
                }
            }
            
            if (!Stream.Next(TokenValues.ClosedBracket))
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ") expected"));
            }

            bracket.Left = expression;
            bracket.Right = exp;
            return bracket;

        }
        return expression;
    }
    private void ParseAction(List<CompilingError> errors, List<ASTNode> actionList) //Parsea el Action de Effect, parece ser que hay que darle de parametro un objeto especifico para que almacene expresiones de AST
    {
        while (Stream.Position < Stream.Count) //Recorre mientras hallan instrucciones
        {
            if (Stream.Next(TokenValues.While)) //Parsea instrucciones while
            {
                ParseWhile(errors, actionList);
                if (!Stream.Next(TokenValues.StatementSeparator)) //Verificar ; despues de cada instruccion
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
            }
            else if (Stream.Next(TokenValues.For)) //Parsea instrucciones for
            {
                ParseFor(errors, actionList);
                if (!Stream.Next(TokenValues.StatementSeparator)) //Verificar ; despues de cada instruccion
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
            }
            else if (Stream.Next(TokenValues.ClosedCurlyBraces))   //Si se encuentra una } entonces se acaba el ciclo
            {
                break;
            }
            else // si no es un while o un for , entonces es una expresiom
            {
                Expression? exp = ParseExpression();
                
                if(exp == null)
                {
                    Stream.MoveNext(1);
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
                }
                else actionList.Add(exp);
               
                
                
                if (!Stream.Next(TokenValues.StatementSeparator)) //Verificar ; despues de cada instruccion
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                }
            }
        }  
    }

    private Expression ParseIndexer(List<CompilingError> errors, Expression expression)
    {
        if (expression == null) return null;

        if (Stream.LookAhead(1).Value == TokenValues.OpenCorchetes)
        {
            Stream.MoveNext(1);

            Expression? exp = ParseExpression(); 
            if(exp == null)
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression in indexer"));
            }
            if (!Stream.Next(TokenValues.ClosedCorchetes))
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "] expected"));
            }
            Indexador indexador = new Indexador(Stream.LookAhead().Location);
            indexador.Left = expression;
            indexador.Right = exp;
            return indexador;
        }
        return expression;
    }
    private void ParseWhile(List<CompilingError> errors, List<ASTNode> actionList)
    {
        While While = new While(null, Stream.LookAhead().Location);

        if (!Stream.Next(TokenValues.OpenBracket))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "( expected"));
        }

        Expression? exp = ParseExpression(); 
        if(exp == null)
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
        }
        else While.Condition = exp;

        if (!Stream.Next(TokenValues.ClosedBracket))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, ") expected"));
        }

        if (Stream.LookAhead(1).Value == TokenValues.OpenCurlyBraces)
        {
            Stream.MoveNext(1);

            ParseAction(errors, While.ActionList);
            Stream.MoveBack(1);

            if (!Stream.Next(TokenValues.ClosedCurlyBraces))
            {   
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
            }

            actionList.Add(While);
            return;
        }
        else //Si es distinto, parsea una instruccion
        {
            Expression? exp1 = ParseExpression(); 
            if(exp1 == null)
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
            }
            While.ActionList.Add(exp1);
            actionList.Add(While);    
        }
    }

    private void ParseFor(List<CompilingError> errors, List<ASTNode> actionList) //NO SE HA PUESTO LA DECLARACION DE UN SOLO ARGUMENTO
    {
        For @for = new For(Stream.LookAhead().Location);

        if (!Stream.Next(TokenValues.target))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "target expected"));
        }

        if (!Stream.Next(TokenValues.In))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "in expected"));
        }

        if (!Stream.Next(TokenValues.targets))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "targets expected"));
        }

        if (Stream.LookAhead(1).Value == TokenValues.OpenCurlyBraces)
        {
            Stream.MoveNext(1);

            ParseAction(errors, @for.ActionList);
            Stream.MoveBack(1);

            if (!Stream.Next(TokenValues.ClosedCurlyBraces))
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
            }

            actionList.Add(@for);
            return;
        }
        else //Si es distinto, parsea una instruccion
        {
        Debug.Log(Stream.LookAhead().Value + "sadasdasdasd");
            Expression? exp1 = ParseExpression(); 
            if(exp1 == null)
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
            }
            @for.ActionList.Add(exp1);
            actionList.Add(@for);    
        }
    }

    private bool ParseParametro(List<CompilingError> errors, Effect effect) //Parsea una variable, normalmente se llamada desde Params
    {
        string id;
        TypeOfValue typeOfValue;
        
        if (!Stream.Next(TokenType.Identifier))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Id expected"));
        }
        id = Stream.LookAhead().Value; 

        if (!Stream.Next(TokenValues.TwoPoints))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "TwoPoints expected"));
        }

        
        Stream.MoveNext(1);

        if (Stream.LookAhead().Value == TokenValues.Bool) typeOfValue = TypeOfValue.Bool;
        else if (Stream.LookAhead().Value == TokenValues.String)  typeOfValue = TypeOfValue.String;
        else if (Stream.LookAhead().Value == TokenValues.Number) typeOfValue = TypeOfValue.Number;
        else
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Bool, Number or String expected"));
            Stream.MoveNext(1);
            return false;
        }

        Parametro parametro = new Parametro(id, typeOfValue);
        effect.ParamsExpresions.Add(parametro);

        return true;
    }

    private void ParseParams(List<CompilingError> errors, Effect effect) //Parsea el interior de Params
    {
        while (ParseParametro(errors, effect))
        {
            if (!Stream.Next(TokenValues.ValueSeparator))
            {
                //Stream.MoveBack(0);
                if (!Stream.Next(TokenValues.ClosedCurlyBraces)) //aumentar
                {
                    errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "ClosedCurlyBraces expected"));
                    Stream.MoveNext(1);
                }

                break;
            }
        }
    }

    private Expression? ParseExpression() //prioridad igual
    {
        return ParseExpressionLv0(null);
    }

    private Expression? ParseExpressionLv05(Expression? left)
    {
        Expression? newLeft = ParseExpressionLv1(left);
        return ParseExpressionLv05_(newLeft);
        //return exp;
    }
    private Expression? ParseExpressionLv05_(Expression? left)
    {
        Expression? exp = MenorQ(left);
        if(exp != null)
        {
            return exp;
        }
        exp = MenorIgual(left);
        if(exp != null)
        {
            return exp;
        }
        exp = MayorQ(left);
        if(exp != null)
        {
            return exp;
        }
        exp = MayorIgual(left);
        if(exp != null)
        {
            return exp;
        }
        exp = Igual(left);
        if(exp != null)
        {
            return exp;
        }
        return left;
    }
    private Expression? ParseExpressionLv1(Expression? left)
    {
        Expression? newLeft = ParseExpressionLv2(left); //aqui de forma primera entro con target
        return ParseExpressionLv1_(newLeft);
        //return exp;
    }

    private Expression? ParseExpressionLv1_(Expression? left)
    {
        Expression? exp = ParseAdd(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParseSub(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParseConcat1(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParseConcat2(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParseDotNotation(left);
        if(exp != null)
        {
            return exp;
        }
        return left;
    }

    private Expression? ParseExpressionLv2(Expression? left)
    {
        Expression? newLeft = ParseExpressionLv3(left);
        return ParseExpressionLv2_(newLeft);
    }
    private Expression? ParseExpressionLv2_(Expression? left) //entra una primera vez con target
    {
        Expression? exp = ParseMul(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParseDiv(left);
        if(exp != null)
        {
            return exp;
        }
        return left;
    }

    private Expression? ParseExpressionLv0(Expression? left)
    {
        Expression? newLeft = ParseExpressionLv05(left);
        return ParseExpressionLv0_(newLeft);
    }
    private Expression? ParseExpressionLv0_(Expression? left)
    {
        Expression? exp = Or(left);
        if(exp != null)
        {
            return exp;
        }
        exp = And(left);
        if(exp != null)
        {
            return exp;
        }
        exp = Assign(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParseAddIgual(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParseSubIgual(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParsePorIgual(left);
        if(exp != null)
        {
            return exp;
        }
        exp = ParseDivIgual(left);
        if(exp != null)
        {
            return exp;
        }
        return left;
    }
    private Expression? ParseExpressionLv3(Expression? left)
    {
        Expression? exp = ParseNumber();
        
        if(exp != null)
        {
            return exp;
        }
        exp = ParseText();
        if(exp != null)
        {
            return exp;
        }
        exp = ParseIdentifier();
        if(exp != null)
        {
            exp = ParseBracket(compilingErrors, exp);
            exp = ParseIndexer(compilingErrors, exp);
            return exp;
        }
            
        return left;
    }

    private Expression? ParseConcat1(Expression? left)
    {
        Concat1 concat1 = new Concat1(Stream.LookAhead().Location);

        if (left == null || !Stream.Next(TokenValues.Concat))
            return null;
        
        concat1.Left = left;

        Expression? right = ParseExpressionLv2(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        concat1.Right = right;

        return ParseExpressionLv1_(concat1);
    }
    private Expression? ParseConcat2(Expression? left)
    {
        Concat2 concat2 = new Concat2(Stream.LookAhead().Location);

        if (left == null || !Stream.Next(TokenValues.ConcatEspacio))
            return null;
        
        concat2.Left = left;

        Expression? right = ParseExpressionLv2(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        concat2.Right = right;

        return ParseExpressionLv1_(concat2);
    }
    private Expression? ParseAdd(Expression? left)
    {
        Add sum = new Add(Stream.LookAhead().Location);

        if (left == null || !Stream.Next(TokenValues.Add))
            return null;
        
        sum.Left = left;

        Expression? right = ParseExpressionLv2(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        sum.Right = right;
        
        return ParseExpressionLv1_(sum);
    }

    private Expression? ParseSub(Expression? left)
    {
        Sub sub = new Sub(Stream.LookAhead().Location);

        if (left == null || !Stream.Next(TokenValues.Sub))
            return null;
        
        sub.Left = left;

        Expression? right = ParseExpressionLv2(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        sub.Right = right;

        return ParseExpressionLv1_(sub);
    }

    private Expression? ParseMul(Expression? left)
    {
        Mul mul = new Mul(Stream.LookAhead().Location);

        if (left == null || !Stream.Next(TokenValues.Mul))
            return null;
        
        mul.Left = left; 

        Expression? right = ParseExpressionLv3(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        mul.Right = right;

        return ParseExpressionLv2_(mul);
    }

    private Expression? ParseDiv(Expression? left)
    {
        Div div = new Div(Stream.LookAhead().Location);

        if (left == null || !Stream.Next(TokenValues.Div)) return null;
        
        div.Left = left;

        Expression? right = ParseExpressionLv3(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        div.Right = right;

        return ParseExpressionLv2_(div);
    }


    private Expression? ParseDotNotation(Expression? left)
    {
        DotNotation dotNotation = new DotNotation(Stream.LookAhead().Location);

        if (left == null || !Stream.Next(TokenValues.Point))
            return null;
        
        
        dotNotation.Left = left;

        Expression? right = ParseExpressionLv2(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        dotNotation.Right = right;

        return ParseExpressionLv1_(dotNotation);
    }

    

    private Expression? MenorQ(Expression? left)
    {
        MenorQ menorQ = new MenorQ(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.MenorQue))
        {
            return null;
        }
        menorQ.Left = left;
        Expression? right = ParseExpressionLv1(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        menorQ.Right = right;

        return ParseExpressionLv05_(menorQ);
    }
    private Expression? MenorIgual(Expression? left)
    {
        MenorIgual menorIgual = new MenorIgual(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.MenorIgual))
        {
            return null;
        }
        menorIgual.Left = left;
        Expression? right = ParseExpressionLv1(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        menorIgual.Right = right;

        return ParseExpressionLv05_(menorIgual);
    }
    private Expression? MayorQ(Expression? left)
    {
        MayorQ mayorQ = new MayorQ(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.MayorQue))
        {
            return null;
        }
        mayorQ.Left = left;
        Expression? right = ParseExpressionLv1(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        mayorQ.Right = right;

        return ParseExpressionLv05_(mayorQ);
    }
    private Expression? MayorIgual(Expression? left)
    {
        MayorIgual mayorIgual = new MayorIgual(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.MayorIgual))
        {
            return null;
        }
        mayorIgual.Left = left;
        Expression? right = ParseExpressionLv1(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        mayorIgual.Right = right;

        return ParseExpressionLv05_(mayorIgual);
    }
    private Expression? Igual(Expression? left)
    {
        Igual igual = new Igual(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.Igual))
        {
            return null;
        }
        igual.Left = left;
        Expression? right = ParseExpressionLv1(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        igual.Right = right;

        return ParseExpressionLv05_(igual);
    }
    private Expression? Or(Expression? left)
    {
        Or or = new Or(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.Disyuncion))
        {
            return null;
        }
        or.Left = left;
        Expression? right = ParseExpressionLv05(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        or.Right = right;

        return ParseExpressionLv0_(or);
    }
    private Expression? And(Expression? left)
    {
        And and = new And(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.Conjuncion))
        {
            return null;
        }
        and.Left = left;
        Expression? right = ParseExpressionLv05(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        and.Right = right;

        return ParseExpressionLv0_(and);
    }

    private Expression? Assign(Expression? left)
    {
        Assign assign = new Assign(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.Assign))
        {
            return null;
        }
        assign.Left = left;
        Expression? right = ParseExpressionLv05(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        assign.Right = right;

        return ParseExpressionLv0_(assign);
    }
    private Expression? ParseAddIgual(Expression? left)
    {
        AddIgual addIgual = new AddIgual(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.MasIgual))
        {
            return null;
        }
        addIgual.Left = left;
        Expression? right = ParseExpressionLv05(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        addIgual.Right = right;

        return ParseExpressionLv0_(addIgual);
    }
    private Expression? ParseSubIgual(Expression? left)
    {
        SubIgual subIgual = new SubIgual(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.MenosIgual))
        {
            return null;
        }
        subIgual.Left = left;
        Expression? right = ParseExpressionLv05(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        subIgual.Right = right;

        return ParseExpressionLv0_(subIgual);
    }
    private Expression? ParsePorIgual(Expression? left)
    {
        PorIgual porIgual = new PorIgual(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.PorIgual))
        {
            return null;
        }
        porIgual.Left = left;
        Expression? right = ParseExpressionLv05(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        porIgual.Right = right;

        return ParseExpressionLv0_(porIgual);
    }
    private Expression? ParseDivIgual(Expression? left)
    {
        DivIgual divIgual = new DivIgual(Stream.LookAhead().Location);
        if (left == null || !Stream.Next(TokenValues.DivIgual))
        {
            return null;
        }
        divIgual.Left = left;
        Expression? right = ParseExpressionLv05(null);
        if(right == null)
        {
            Stream.MoveBack(2);
            return null;
        }
        divIgual.Right = right;

        return ParseExpressionLv0_(divIgual);
    }
    private Expression? ParseNumber()
    {
        if (!Stream.Next(TokenType.Number)) return null; 
        return new Number(double.Parse(Stream.LookAhead().Value), Stream.LookAhead().Location);
    }

    private Expression? ParseText()
    {
        if (!Stream.Next(TokenType.Text)) return null;
        return new StringC(Stream.LookAhead().Value, Stream.LookAhead().Location);
    }
    private Expression? ParseIdentifier()
    {
        if (!Stream.Next(TokenType.Identifier)) return null;
        return new Identifier(Stream.LookAhead().Value, Stream.LookAhead().Location);
    }
    private Expression? ParseBool()
    {
        if (!Stream.Next(TokenType.Identifier)) return null;
        return new Identifier(Stream.LookAhead().Value, Stream.LookAhead().Location);
    }
}