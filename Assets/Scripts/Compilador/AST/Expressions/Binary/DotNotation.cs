using System;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
public class DotNotation : BinaryExpression
{
    public override ExpressionType Type { get; set; }

    public override object? Value { get; set; }

    public DotNotation(CodeLocation location) : base(location)
    {
        Type = ExpressionType.Anytype;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        
        
        if (Left is Identifier && !ListOfIdentifiers.IdentifiersList.Contains(Left.Value.ToString()))
        {
            Tuple<bool, Scope> tuple = scope.IsAssignedIdentifier(Left.Value.ToString(), scope);
            if (tuple.Item1)
            {
                Expression expression = tuple.Item2.varYValores[Left.Value.ToString()];
                Left.Type = expression.Type;
            }
        }
    

        if ((Right is Identifier) && (Right.Value.ToString()==IdentifierType.Power.ToString()||Right.Value.ToString()==IdentifierType.Name.ToString()||
            Right.Value.ToString()==IdentifierType.Type.ToString()||Right.Value==IdentifierType.Faction.ToString()||
            Right.Value.ToString()==IdentifierType.Range.ToString()))
        {
            if (Left.Type != ExpressionType.Card)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a card"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            if ((Right is Identifier) && (Right.Value.ToString() == IdentifierType.Power.ToString()))
            {
                Type = ExpressionType.Number;
            }
            else Type = ExpressionType.Property;

            return left && right;
        }
        else if ((Right is Identifier) && (Right.Value.ToString()==IdentifierType.TriggerPlayer.ToString()))
        {
            if (Left.Type != ExpressionType.Context)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Player;
            return left && right;
        }
        else if ((Right is Identifier) && (Right.Value.ToString() == IdentifierType.Board.ToString()))
        {
            if (Left.Type != ExpressionType.Context)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.List;
            return left && right;
        }
        else if ((Right is Identifier) && (Right.Value.ToString()==IdentifierType.Hand.ToString()||Right.Value.ToString()==IdentifierType.Deck.ToString()||
                 Right.Value.ToString()==IdentifierType.Field.ToString()||Right.Value.ToString()==IdentifierType.Graveyard.ToString()))
        {
            if (Left.Type != ExpressionType.Context)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.List;
            return left && right;
        }
        else if ((Right is Identifier) && (Right.Value.ToString() == IdentifierType.Owner.ToString()))
        {
            if (Left.Type != ExpressionType.Card)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a card"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Player;
            return left && right;
        }
        else if (Right.Type == ExpressionType.Method)
        {
            if (Left.Type != ExpressionType.List && Left.Type != ExpressionType.ContextList)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a list"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Method;
            return right && left;
        }
        else if (Right.Type == ExpressionType.List)
        {
            if (Right is Bracket)
            {
                Bracket bracket = (Bracket)Right;
                if (bracket.Right is Predicate)
                {
                    if (Left.Type != ExpressionType.List)
                    {
                        errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a list"));
                        Type = ExpressionType.ErrorType;
                        return false;
                    }
                    Type = ExpressionType.List;
                    return right && left;
                }

                if (Left.Type != ExpressionType.Context)
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a context"));
                    Type = ExpressionType.ErrorType;
                    return false;
                }

                Type = ExpressionType.ContextList;
                return right && left;
            }
        }
        return false;
    }
    
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("<{0}.{1}>", Left, Right);
        }
        return Value.ToString();
    }

    public override void Evaluate()
    {

    }
}