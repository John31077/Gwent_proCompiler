using System;

public class MenorQ : BinaryExpression
{
    public override ExpressionType Type {get; set;}
    public override object? Value {get; set;}
    public override void Evaluate()
    {

    }
    public MenorQ(CodeLocation location) : base(location){}
    public override string ToString()
    {
        if (Value == null)
        {
            return String.Format("({0} < {1})", Left, Right);
        }
        return Value.ToString();
    }
}