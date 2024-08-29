using System.Collections.Generic;

public class For : ASTNode
{
    public List<ASTNode> ActionList {get; set;}

    public For(CodeLocation location) : base (location)
    {
        ActionList = new List<ASTNode>();
    }
}