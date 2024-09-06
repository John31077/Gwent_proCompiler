using System.Collections.Generic;

public class For : ASTNode
{
    public List<ASTNode> ActionList {get; set;}

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    }
    
    public For(CodeLocation location) : base (location)
    {
        ActionList = new List<ASTNode>();
    }
}