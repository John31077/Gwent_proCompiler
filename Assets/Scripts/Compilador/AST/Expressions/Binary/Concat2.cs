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

        if (Left is Identifier)
        {
            Tuple<bool, Scope> tuple = scope.IsAssignedIdentifier(Left.Value.ToString(), scope);
            if (tuple.Item1)
            {
                Expression expression = tuple.Item2.varYValores[Left.Value.ToString()];
                Left.Type = expression.Type;
            }
        }

        if (Right is Identifier)
        {
            Tuple<bool, Scope> tuple = scope.IsAssignedIdentifier(Right.Value.ToString(), scope);
            if (tuple.Item1)
            {
                Expression expression = tuple.Item2.varYValores[Right.Value.ToString()];
                Left.Type = expression.Type;
            }
        }
        
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
        Right.Evaluate();
        Left.Evaluate();

        string right = (string)Right.Value;
        string left = (string)Left.Value;
        
        Value = right + " "  + left;
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