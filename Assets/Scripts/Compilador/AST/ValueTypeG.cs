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

public class ParametroValor
{
    public string Id { get; private set; }

    public Expression Expression { get; private set; }


    public ParametroValor(string id, Expression expression)
    {
        this.Id = id;
        this.Expression = expression;
    }
}

public enum TypeOfValue
{
    Bool,
    String,
    Number,
}