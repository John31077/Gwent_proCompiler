using System;
using System.Collections.Generic;

public class Concat2 : BinaryExpression // @@
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        if (Right.Type!=ExpressionType.String || Left.Type!=ExpressionType.String)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "@ must be string or identifier in both sides"));
            Type = ExpressionType.ErrorType;
            return false;
        }

        Type = ExpressionType.String;
        return right && left;
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