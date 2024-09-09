using System;
using System.Collections.Generic;

public class Indexador : BinaryExpression
{
    public override ExpressionType Type { get; set; }

    public override object? Value { get; set; }

    public Indexador(CodeLocation location) : base(location)
    {
        Type = ExpressionType.Anytype;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool right = Right.CheckSemantic(context, scope, errors);
        bool left = Left.CheckSemantic(context, scope, errors);
        if (Right.Type != ExpressionType.Number || Left.Type != ExpressionType.List)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "invalid indexer"));
            Type = ExpressionType.ErrorType;
            return false;
        }

        Type = ExpressionType.Card;
        return right && left;
    }
    
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("<{0}[{1}]>", Left, Right);
        }
        return Value.ToString();
    }

    public override void Evaluate()
    {

    }
}