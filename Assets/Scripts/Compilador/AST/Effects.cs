using System.Collections.Generic;

public class Effect : ASTNode
{
    public string Id {get; set;}
    
    public List<Parametro> ParamsExpresions;

    public List<ASTNode> ActionList;

    public Effect(string id, CodeLocation location) : base (location)
    {
        this.Id = id;
        ParamsExpresions = new List<Parametro>();
        ActionList = new List<ASTNode>();
    }

    public bool CollectElements(Context context, Scope scope, List<CompilingError> errors)
    {
        if (context.effects.Contains(Id))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Effect already defined"));
            return false;
        }
        else
        {
            context.effects.Add(Id);
        }
        return true;
    }

    
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        foreach (Parametro parametro in ParamsExpresions)
        {
            scope.varYValores.Add(parametro.Id, null);
        }


        bool checkInstruction = false;
        bool checkInstructions = true;

        foreach (ASTNode instruction in ActionList)
        {
            if (!(instruction is Assign)||!(instruction is AddIgual)||!(instruction is SubIgual)||!(instruction is PorIgual||!(instruction is DivIgual)))
            {
                if (!(instruction is While)||!(instruction is For))
                {
                    errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid instruction"));
                    checkInstructions = false;
                    continue;
                }
            }

            checkInstruction = instruction.CheckSemantic(context, scope.CreateChild(), errors);
            if (checkInstruction == false)
            {
                checkInstructions = false;
            }
        }
        return checkInstructions;
    }
}