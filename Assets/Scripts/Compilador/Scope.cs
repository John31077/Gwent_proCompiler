using System.Collections.Generic;

public class Scope
    {
        public Scope? Parent;

        public Dictionary<string, Expression> varYValores;

        public Scope()
        {
            varYValores = new Dictionary<string, Expression>();
        }


        public Scope CreateChild()
        {
            Scope child = new Scope();
            child.Parent = this;
               
            return child;
        }


        //Metodo para verificar si un identificador ya está definido en el scope o en su padre o ancestros.
        public bool IsAssignedIdentifier(string identifier, Scope scope)
        {
            if (scope.varYValores.ContainsKey(identifier))
            {
                return true;
            }
            else
            {
                if (scope.Parent == null && !scope.varYValores.ContainsKey(identifier))
                {
                    return false;
                }
                return IsAssignedIdentifier(identifier, scope.Parent);
            }
        }

    }