using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SearchService;
using UnityEngine;

public class Bracket : BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}

    public Bracket(CodeLocation location) : base(location){}

    

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right;
        bool left;
        if (Right == null)
        {
            right = true;


            if (Left.Value.ToString() == IdentifierType.Pop.ToString() || Left.Value.ToString() == IdentifierType.Shuffle.ToString())
            {
                left = true;
            }
            else
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Bad method declaration, Pop() or Suffle()"));
                left = false;
            }

            Type = ExpressionType.Method;
            return right && left;
        }
        else if (Right is Predicate)
        {
            right = Right.CheckSemantic(context, scope, errors);

            if (Right is Identifier && !ListOfIdentifiers.IdentifiersList.Contains(Right.Value.ToString()))
            {
                Tuple<bool, Scope> tuple = scope.IsAssignedIdentifier(Right.Value.ToString(), scope);
                if (tuple.Item1)
                {
                    Expression expression = tuple.Item2.varYValores[Right.Value.ToString()];
                    Left.Type = expression.Type;
                }
            }

            if (Right.Type != ExpressionType.Bool || ((Left is Identifier) && Left.Value.ToString() != IdentifierType.Find.ToString()))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Bad method declaration, Find(Predicate)"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.List;

            left = true;
            return right && left;
        }
        else if ((Left is Identifier) && (Left.Value.ToString()==IdentifierType.DeckOfPlayer.ToString()||Left.Value.ToString()==IdentifierType.FieldOfPlayer.ToString()||
                 Left.Value.ToString()==IdentifierType.HandOfPlayer.ToString()||Left.Value.ToString()==IdentifierType.GraveyardOfPlayer.ToString()))
        {
            right = Right.CheckSemantic(context, scope, errors);   
            left = true;     

            if (Right is Identifier && !ListOfIdentifiers.IdentifiersList.Contains(Right.Value.ToString()))
            {
                Tuple<bool, Scope> tuple = scope.IsAssignedIdentifier(Right.Value.ToString(), scope);
                if (tuple.Item1)
                {
                    Expression expression = tuple.Item2.varYValores[Right.Value.ToString()];
                    Right.Type = expression.Type;
                }
            }

            if (Right.Type != ExpressionType.Player)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Bad method declaration, param must be a player"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.List;

            return right && left;
        }
        else if ((Left is Identifier) && (Left.Value.ToString()==IdentifierType.Push.ToString()||Left.Value.ToString()==IdentifierType.Remove.ToString()||
                 Left.Value.ToString()==IdentifierType.SendBottom.ToString()))
        {
            right = Right.CheckSemantic(context, scope, errors);
            left = true;

            if (Right is Identifier && !ListOfIdentifiers.IdentifiersList.Contains(Right.Value.ToString()))
            {
                Tuple<bool, Scope> tuple = scope.IsAssignedIdentifier(Right.Value.ToString(), scope);
                if (tuple.Item1)
                {
                    Expression expression = tuple.Item2.varYValores[Right.Value.ToString()];
                    Left.Type = expression.Type;
                }
            }

            if (Right.Type != ExpressionType.Card)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Bad method declaration, param must be a card"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Method;
            return right && left;
        }
        else
        {
            Type = ExpressionType.ErrorType;
            return false;
        }
    }

    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("<{0}({1})>", Left, Right);
        }
        return Value.ToString();
    }

    public override void Evaluate()
    {
        
    }
}

