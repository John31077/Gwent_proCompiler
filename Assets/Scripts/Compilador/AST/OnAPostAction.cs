using System.Collections.Generic;
using UnityEngine.UIElements;

public class PostAction : ASTNode
{
    public List<ASTNode> PostActionList;
    


    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkInstruction = false;
        bool checkInstructions = true;

        foreach (ASTNode instruction in PostActionList)
        {
            checkInstruction = instruction.CheckSemantic(context, scope.CreateChild(), errors);
            if (checkInstruction == false)
            {
                checkInstructions = false;
            }
        }

        return checkInstructions;
    }

    public PostAction(CodeLocation location) : base (location)
    {
        PostActionList = new List<ASTNode>();
    }
}