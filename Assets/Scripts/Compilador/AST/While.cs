using System.Collections.Generic;

public class While : ASTNode
{
    public Expression Condition;
    public List<ASTNode> ActionList;

    public While(Expression condition, CodeLocation location) : base (location)
    {
        ActionList = new List<ASTNode>();
    }
}