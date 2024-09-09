using System;
using System.Collections.Generic;

public class Bool : AtomExpression
{
    public override ExpressionType Type
    {
        get
        {
            return ExpressionType.Bool;
        }
        set { }
    }
    public bool IsBool
    {
        get
        {
            bool a;
            return bool.TryParse(Value.ToString(), out a);
        }
    }

    public Bool(string value, CodeLocation location) : base(location)
    {
        Value = value;
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    }



    public override void Evaluate()
    {
        
    }

    public override string ToString()
    {
        return String.Format("{0}",Value);
    }

    public override object? Value { get; set; }
}