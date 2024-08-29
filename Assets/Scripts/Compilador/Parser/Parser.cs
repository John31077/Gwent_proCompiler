using System.Collections.Generic;
using System.IO.Pipes;
//using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using UnityEngine;

public class Parser
{
    public Parser(TokenStream stream)
    {
        Stream = stream;
    }
    public TokenStream Stream { get; private set; }

    public ElementalProgram ParseProgram(List<CompilingError> errors)
    {
        ElementalProgram program = new ElementalProgram(new CodeLocation());

        if (!Stream.CanLookAhead(0)) return program;

        //Here we parse all the declared effects
        while (Stream.LookAhead().Value == TokenValues.effect)
        {
            Effect effect = ParseEffect(errors);
            program.Effects[effect.Id] = effect;

            //After every Element declaration must be a ;
            /*if (!Stream.Next(TokenValues.StatementSeparator))
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "; expected"));
                System.Console.WriteLine("Hay errores " + errors.Count);
                return program;
            }*/

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

        if (!Stream.Next(TokenValues.Params)) // Params
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "Params Expected"));
        }

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
            Stream.MoveNext(1); //Retrocede una posicion
            ParseParams(errors, effect);
        } // }
                    
        if (!Stream.Next(TokenValues.ValueSeparator)) // ,
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "ValueSeparator Expected"));
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
        
        //ParseAction(errors, effect.ActionList);


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
        
        //Falta terminar el metodo ParseAction



        Expression? exp = ParseExpression();   //Esto es para las expresiones
        if (exp != null)
        {
            Debug.Log(exp.ToString());
        }
        else Debug.Log("Es nulo");
        if(exp == null)
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Invalid, "Bad expression"));
        }
        effect.ActionList.Add(exp);

        return effect;






        //Me quede en el Action
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




        //Se llego a la parte mala, el PostAction, ver como desarrollar esto






        //Ver bien esta parte
    }

*/
    private void ParseAction(List<CompilingError> errors, List<ASTNode> actionList) //Parsea el Action de Effect, parece ser que hay que darle de parametro un objeto especifico para que almacene expresiones de AST
    {
        //Estas cosas por lo visto deben estar en un while
        if (Stream.Next(TokenValues.id)) //dentro de Action un id solo puede estar antes de un "=" o de un ".", otra forma es error
        {
            if (Stream.Next(TokenValues.Assign))
            {
                DeclaracionVariable declaracionVariable = new DeclaracionVariable(Stream.LookAhead(-1).Value, null, Stream.LookAhead().Location);

            }
            else if (Stream.Next(TokenValues.Point))
            {

            }
            else 
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "= or . expected"));
            }
        }


        if (Stream.Next(TokenValues.While))
        {
            ParseWhile(errors, actionList);
        }

        if (Stream.Next(TokenValues.For))
        {
            ParseFor(errors, actionList);
        }
    }



    private void ParseDeclaracionVariableInAction(List<CompilingError> errors, DeclaracionVariable declaracion)
    {
        //Despues del "=", puede venir una 
    }












    private void ParseWhile(List<CompilingError> errors, List<ASTNode> actionList) //NO SE HA PUESTO LA DECLARACION DE UN SOLO ARGUMENTO
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

            if (!Stream.Next(TokenValues.ClosedCurlyBraces))
            {
                errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
            }

            actionList.Add(While);
            return;
        }
       // else if () //Si es distinto, parsea una instruccion










        if (!Stream.Next(TokenValues.OpenCurlyBraces))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "{ expected"));
        }

        ParseAction(errors, While.ActionList);

        if (!Stream.Next(TokenValues.ClosedCurlyBraces))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
        }

        actionList.Add(While);
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

        if (!Stream.Next(TokenValues.OpenCurlyBraces))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "{ expected"));
        }

        ParseAction(errors, @for.ActionList);

        if (!Stream.Next(TokenValues.ClosedCurlyBraces))
        {
            errors.Add(new CompilingError(Stream.LookAhead().Location, ErrorCode.Expected, "} expected"));
        }

        actionList.Add(@for);
    }

    private bool ParseVariable(List<CompilingError> errors, Effect effect) //Parsea una variable, normalmente se llamada desde Params
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
        Stream.MoveBack(1);
        while (ParseVariable(errors, effect))
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
        Expression? newLeft = ParseExpressionLv2(left);
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
        return left;
    }

    private Expression? ParseExpressionLv2(Expression? left)
    {
        Expression? newLeft = ParseExpressionLv3(left);
        return ParseExpressionLv2_(newLeft);
    }
    private Expression? ParseExpressionLv2_(Expression? left)
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
            return exp;
        }
        return null;
    }
    private Expression? ParseAdd(Expression? left)
    {
        Add sum = new Add(Stream.LookAhead().Location);

        if (left == null || !Stream.Next(TokenValues.Add))
            return null;
        
        sum.Left = left; //1

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
}