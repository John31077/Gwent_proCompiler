using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class For : ASTNode
{
    public List<ASTNode> ActionList {get; set;}

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkInstruction = false;
        bool checkInstructions = true;

        foreach (ASTNode instruction in ActionList)
        {
            if (!(instruction is Assign)&&!(instruction is AddIgual)&&!(instruction is SubIgual)&&!(instruction is PorIgual&&!(instruction is DivIgual)))
            {
                if (!(instruction is While)&&!(instruction is For) && !(instruction is DotNotation))
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid instruction"));
                    checkInstructions = false;
                    continue;
                }
            }


            if (instruction is While || instruction is For)
            {
                checkInstruction = instruction.CheckSemantic(context, scope.CreateChild(), errors);    
            }
            else
            {
                checkInstruction = instruction.CheckSemantic(context, scope, errors);
            }


            if (checkInstruction == false)
            {
                checkInstructions = false;
            }
        }

        return checkInstructions;
    }
    
    public For(CodeLocation location) : base (location)
    {
        ActionList = new List<ASTNode>();
    }
}