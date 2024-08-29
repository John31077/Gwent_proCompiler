using System.Data;

public class Token
{
    public string Value { get; private set; }
    public TokenType Type { get; private set; }
    public CodeLocation Location { get; private set; }
    public Token(TokenType type, string value, CodeLocation location)
    {
        this.Type = type;
        this.Value = value;
        this.Location = location;
    }

    public override string ToString()
    {
        return string.Format("{0} [{1}]", Type, Value);
    }
}

public struct CodeLocation
{
    public string File;
    public int Line;
    public int Column;
}


public enum TokenType
{
    Unknwon,
    Number,
    Text,
    Keyword,
    Identifier,
    Symbol
}

public class TokenValues //Clase donde se encuentran los valores de los tokens (se usarán para crear los tokens despues)
{
    protected TokenValues() { }

    //Zona de los operadores
    public const string Add = "Addition"; // +
    public const string Sub = "Subtract"; // -
    public const string Mul = "Multiplication"; // *
    public const string Div = "Division"; // /
    public const string Pow = "Pow"; // ^

    public const string Assign = "Assign"; // =
    public const string Implica = "Implica"; // =>
    public const string MayorQue = "MayorQue"; // >
    public const string MenorQue = "MenorQue"; // <
    public const string MayorIgual = "MayorIgual"; // >=
    public const string MenorIgual = "MenorIgual"; // <=
    public const string Igual = "Igual"; // ==
    public const string Concat = "Concat"; // @
    public const string ConcatEspacio = "ConcatEspacio"; // @@
    public const string Conjuncion = "Conjuncion"; // &&
    public const string Disyuncion = "Disyuncion"; // ||

    





    public const string ValueSeparator = "ValueSeparator"; // ,
    public const string StatementSeparator = "StatementSeparator"; // ;
    public const string TwoPoints = "TwoPoints"; // :
    public const string Point = "Point"; // .


    public const string OpenBracket = "OpenBracket"; // (
    public const string ClosedBracket = "ClosedBracket"; // )
    public const string OpenCurlyBraces = "OpenCurlyBraces"; // {
    public const string ClosedCurlyBraces = "ClosedCurlyBraces"; // }
    public const string OpenCorchetes = "OpenCorchete"; // [
    public const string ClosedCorchetes = "ClosedCorchetes"; // ]

    //Zona de palabras claves

    public const string effect = "effect"; // effect
    public const string Name = "Name"; // Name
    public const string Params = "Params"; // Params
    public const string Number = "Number"; // Number
    public const string String = "String"; // String
    public const string Bool = "Bool"; // Bool
    public const string Action = "Action"; // Action

    public const string targets = "targets"; // targets
    public const string target = "target"; // target
    public const string HandOfPlayer = "HandOfPlayer"; // HandOfPlayer
    public const string FieldOfPlayer = "FieldOfPlayer"; // FieldOfPlayer
    public const string GraveyardOfPlayer = "GraveyardOfPlayer"; // GraveyardOfPlayer
    public const string DeckOfPlayer = "DeckOfPlayer"; // DeckOfPlayer
    public const string Hand = "Hand"; // Hand
    public const string Field = "Field"; // Field
    public const string Graveyard = "Graveyard"; // Graveyard
    public const string Deck = "Deck"; // Deck
    public const string Owner = "Owner"; // Owner
    public const string TriggerPlayer = "TriggerPlayer"; // TriggerPlayer
    public const string Board = "Board"; // Board

    public const string context = "context"; // context
    public const string Find = "Find"; // Find
    public const string Push = "Push"; // Push
    public const string SendBotttom = "SendBottom"; // SendBottom
    public const string Pop = "Pop"; // Pop
    public const string Remove = "Remove"; // Remove
    public const string Shuffle = "Shuffle"; // Shuffle
    

    public const string card = "card"; // card
    public const string Type = "Type"; // Type
    public const string Faction = "Faction"; // Faction
    public const string Power = "Power"; //Power
    public const string Range = "Range"; // Range
    public const string OnActivation = "OnActivation"; // OnActivation
    public const string Effect = "Effect"; // Effect
    public const string Selector = "Selector"; // Selector
    public const string Source = "Source"; // Source
    public const string Single = "Single"; // Single
    public const string Predicate = "Predicate"; // Type
    public const string PostAction = "PostAction"; // PostAction


    public const string hand = "hand"; // hand
    public const string otherHand = "otherHand"; // otherHand
    public const string deck = "deck"; // deck
    public const string otherDeck = "otherDeck"; // otherDeck
    public const string field = "field"; // field
    public const string otherField = "otherField"; // otherField
    public const string parent = "parent"; // parent



    public const string TrueExpresion = "trueExpresion"; // true
    public const string FalseExpresion = "falseExpresion"; //false





    public const string For = "for"; // for
    public const string In = "in"; // in
    public const string While = "while"; // while
    public const string id = "id"; // id
}



