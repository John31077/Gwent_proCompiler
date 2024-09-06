using System;
using System.Collections.Generic;

public class Mul : BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}

    public Mul(CodeLocation location) : base(location){}

    public override void Evaluate()
    {
        Right.Evaluate();
        Left.Evaluate();
        
        Value = (double)Right.Value * (double)Left.Value;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    }

    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} * {1})", Left, Right);
        }
        return Value.ToString();
    }
}