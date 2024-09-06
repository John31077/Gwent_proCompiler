using System;
using System.Collections.Generic;

public class Concat2 : BinaryExpression // @@
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    }
    public override void Evaluate()
    {

    }
    public Concat2(CodeLocation location) : base(location){}
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} @@ {1})", Left, Right);
        }
        return Value.ToString();
    }
}