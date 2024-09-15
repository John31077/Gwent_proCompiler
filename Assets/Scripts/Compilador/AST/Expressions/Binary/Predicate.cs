using System;
using System.Collections.Generic;

public class Predicate : BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}

    public Predicate(CodeLocation location) : base(location){}

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        if (Left.Type != ExpressionType.Card || Right.Type != ExpressionType.Bool)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid predicate"));
            Type = ExpressionType.ErrorType;
            return false;
        }

        Type = ExpressionType.Bool;
        return right && left;
    }
    public override void Evaluate()
    {
        /*Right.Evaluate();
        Left.Evaluate();
        
        bool value = (double)Left.Value >= (double)Right.Value;
        Value = value;*/
    }

    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0}) => {1}", Left, Right);
        }
        return Value.ToString();
    }
}

