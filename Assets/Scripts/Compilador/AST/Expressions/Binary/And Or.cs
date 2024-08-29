using System;

public class And : BinaryExpression
{
    public override ExpressionType Type { get; set; }

    public override object? Value { get; set; }

    public And(CodeLocation location) : base(location)
    {

    }

    
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} && {1})", Left, Right);
        }
        return Value.ToString();
    }

    public override void Evaluate()
    {

    }
}

public class Or : BinaryExpression
{
    public override ExpressionType Type { get; set; }

    public override object? Value { get; set; }

    public Or(CodeLocation location) : base(location)
    {

    }

    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} || {1})", Left, Right);
        }
        return Value.ToString();
    }    


    public override void Evaluate()
    {

    }
}