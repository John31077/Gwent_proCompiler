using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using Debug = UnityEngine.Debug;

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

            if (parametro.typeOfValue == TypeOfValue.Number)
            scope.varYValores.Add(parametro.Id, new Number(0, new CodeLocation()));
            else if (parametro.typeOfValue == TypeOfValue.String)
            scope.varYValores.Add(parametro.Id, new StringC("", new CodeLocation()));
            else if (parametro.typeOfValue == TypeOfValue.Bool)
            scope.varYValores.Add(parametro.Id, new Bool("false", new CodeLocation()));
            else errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Invalid parameter value"));
        }


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
}