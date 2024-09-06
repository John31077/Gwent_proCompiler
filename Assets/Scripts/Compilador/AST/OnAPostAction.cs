using System.Collections.Generic;
using UnityEngine.UIElements;

public class PostAction : ASTNode
{
    public List<ASTNode> PostActionList;
    




    public PostAction(CodeLocation location) : base (location)
    {
        PostActionList = new List<ASTNode>();
    }
}