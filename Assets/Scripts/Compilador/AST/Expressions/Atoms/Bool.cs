using System;

public class Bool : AtomExpression
{
    public bool IsBool
    {
        get
        {
            bool a;
            return bool.TryParse(Value.ToString(), out a);
        }
    }

    public Bool(bool value, CodeLocation location) : base(location)
    {
        Value = value;
    }

    public override ExpressionType Type
    {
        get
        {
            return ExpressionType.Bool;
        }
        set { }
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