using System.Collections.Generic;

public class CardG : ASTNode
{
    public string Type {get; set;}
    public string Id {get; set;}
    public string faction {get; set;}
    public Expression Power {get; set;}
    public string range {get; set;}
    public List<Expression> OnActivation;

    public CardG(string cardType, string id, string faction, Expression power, string range, CodeLocation location) : base (location)
    {
        this.Type = cardType;
        this.Id = id;
        this.faction = faction;
        this.Power = power;
        this.range = range;
        OnActivation = new List<Expression>();
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