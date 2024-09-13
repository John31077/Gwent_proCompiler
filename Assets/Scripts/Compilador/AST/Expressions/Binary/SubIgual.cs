using System;
using System.Collections.Generic;
using System.Diagnostics;

public class SubIgual : BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}

    public SubIgual(CodeLocation location) : base(location){}

    public override void Evaluate()
    {
        Type = ExpressionType.Anytype;
    }

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
        
        if (Right.Type!=ExpressionType.Number || Left.Type!=ExpressionType.Number)
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "-= must be number or identifier in both sides"));
            Type = ExpressionType.ErrorType;
            return false;
        }

        Type = ExpressionType.Number;
        return right && left;
    }

    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("{0} -= {1}", Left, Right);
        }
        return Value.ToString();
    }
}

