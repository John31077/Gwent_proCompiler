using System.Collections.Generic;

public class CardG : ASTNode
{
    public string Type {get; set;}
    public string Id {get; set;}
    public string faction {get; set;}
    public Expression Power {get; set;}
    public string range {get; set;}
    public List<ASTNode> OnActivation;

    public CardG(string cardType, string id, string faction, Expression power, string range, CodeLocation location) : base (location)
    {
        this.Type = cardType;
        this.Id = id;
        this.faction = faction;
        this.Power = power;
        this.range = range;
        OnActivation = new List<ASTNode>();
    }

    public override bool CheckSemantic(Context context, Scope scope, List<CompilingError> errors)
    {
        bool checkType = false;
        if (Type==CardType.Oro.ToString()||Type==CardType.Plata.ToString()||Type==CardType.Clima.ToString()||Type==CardType.Aumento.ToString()||Type==CardType.Lider.ToString())
        {
            checkType = true;
        }
        else errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The Type must be Oro, Plata, Clima, Aumento or Lider"));

        bool checkFaction = false;
        if (faction==Faction.Empire.ToString()||faction==Faction.Oblivion.ToString())
        {
            checkFaction = true;
        }
        else errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The Faction must be Empire or Oblivion"));


        //Verificar el ataque, en caso de ser clima, aumento o lider
        bool checkPower = Power.CheckSemantic(context, scope, errors);
        if (Power.Type == ExpressionType.Number)
        {
            checkPower = true;
        }
        else errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The Power must be numerical"));

        bool checkRange = false;
        if (range==Range.Melee.ToString()||range==Range.Ranged.ToString()||range==Range.Siege.ToString())
        {
            checkRange = true;
        }
        else errors.Add(new CompilingError(Location, ErrorCode.Invalid, "The Ranged must be Melee, Ranged or Siege"));

        
        bool checkInstruction = false;
        bool checkInstructions = true;

        foreach (ASTNode instruction in OnActivation)
        {
            checkInstruction = instruction.CheckSemantic(context, scope.CreateChild(), errors);
            if (checkInstruction == false)
            {
                checkInstructions = false;
            }
        }


        return checkType && checkFaction && checkPower && checkRange && checkInstructions;
    }
}

public enum CardType
{
    Oro,
    Plata,
    Clima,
    Aumento,
    Lider,
    Unknown
}
public enum Faction
{
    Empire,
    Oblivion,
    Unknown
}
public enum Range
{
    Melee,
    Ranged,
    Siege,
    Unknown
}