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
        return true;
    }
    
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("{0}.{1}", Left, Right);
        }
        return Value.ToString();
    }

    public override void Evaluate()
    {

    }
}