using System;
using System.Collections.Generic;
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
        //Falta despues del dotnotation que venga un metodo, que seria un bracket, en la izquierda seria una lista o una carta en dependencia
        bool right = CheckSemantic(context, scope, errors);
        bool left = CheckSemantic(context, scope, errors);

        if (Right.Value == IdentifierType.Power.ToString()||Right.Value == IdentifierType.Name.ToString()||
            Right.Value == IdentifierType.Type.ToString()||Right.Value == IdentifierType.Faction.ToString()||
            Right.Value == IdentifierType.Range.ToString())
        {
            if (Left.Type != ExpressionType.Card)
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Left expression must be a card"));
                Type = ExpressionType.ErrorType;
                return false;
            }

            Type = ExpressionType.Property;
            return left && right;
        }
        else if (Right.Value==IdentifierType.TriggerPlayer.ToString())
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
        else if (Right.Value == IdentifierType.Board.ToString())
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
        else if (Right.Value==IdentifierType.Hand.ToString()||Right.Value==IdentifierType.Deck.ToString()||
                 Right.Value==IdentifierType.Field.ToString()||Right.Value==IdentifierType.Graveyard.ToString())
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
        else if (Right.Value == IdentifierType.Owner.ToString())
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