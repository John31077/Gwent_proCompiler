using System.Collections.Generic;
using UnityEngine.UIElements;

public class PostAction : ASTNode
{
    public List<ASTNode> PostActionList;
    


    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true;
    }

    public PostAction(CodeLocation location) : base (location)
    {
        PostActionList = new List<ASTNode>();
    }
}