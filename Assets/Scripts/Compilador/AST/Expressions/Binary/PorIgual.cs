using System;
using System.Collections.Generic;

public class PorIgual : BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}

    public PorIgual(CodeLocation location) : base(location){}

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    }

    public override void Evaluate()
    {
        Type = ExpressionType.Anytype;
    }

    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("{0} *= {1}", Left, Right);
        }
        return Value.ToString();
    }
}



