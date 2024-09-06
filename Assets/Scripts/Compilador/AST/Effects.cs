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
        if (context.elements.Contains(Id))
        {
            errors.Add(new CompilingError(Location, ErrorCode.Invalid, "Effect already defined"));
            return false;
        }
        else
        {
            context.elements.Add(Id);
        }
        return true;
    }

    
    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        return true; //borrar
        
       /* bool checkWeak = true;
        foreach (string element in Weak)
        {
            if (!context.elements.Contains(element))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, String.Format("{0} element does not exists", element)));
                checkWeak = false;
            }
            if (scope.elements.Contains(element))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, String.Format("{0} element already in use", element)));
                checkWeak = false;
            }
            else
            {
                scope.elements.Add(element);
            }
        }


        bool checkStrong = true;
        foreach (string element in Strong)
        {
            if (!context.elements.Contains(element))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, String.Format("{0} element does not exists", element)));
                checkStrong = false;
            }
            if (scope.elements.Contains(element))
            {
                errors.Add(new CompilingError(Location, ErrorCode.Invalid, String.Format("{0} element already in use", element)));
                checkStrong = false;
            }
            else
            {
                scope.elements.Add(element);
            }
        }
        return checkWeak && checkStrong;*/
    }
}