public class Parametro
{
    public string Id { get; private set; }

    public TypeOfValue typeOfValue { get; private set; }


    public Parametro(string id, TypeOfValue typeOfValue)
    {
        this.Id = id;
        this.typeOfValue = typeOfValue;
    }
}

public enum TypeOfValue
{
    Bool,
    String,
    Number,
}