using System;
using System.Collections.Generic;

public class Igual : BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}
    public override void Evaluate()
    {

    }
    public Igual(CodeLocation location) : base(location){}

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        if (Right.Type != Left.Type)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "diferents expresion types in both sides of =="));
            Type = ExpressionType.ErrorType;
            return false;
        }
        
        Type = ExpressionType.Bool;
        return right && left;
    }
    
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} == {1})", Left, Right);
        }
        return Value.ToString();
    }
}