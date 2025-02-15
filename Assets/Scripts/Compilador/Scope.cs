using System;
using System.Collections.Generic;
using Unity.VisualScripting;

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
        public Tuple<bool,Scope> IsAssignedIdentifier(string identifier, Scope scope)
        {
            if (scope.varYValores.ContainsKey(identifier))
            {
                Tuple<bool, Scope> tuple = new Tuple<bool, Scope>(true, scope);
                return tuple;
            }
            else
            {
                if (scope.Parent == null && !scope.varYValores.ContainsKey(identifier))
                {
                    Tuple<bool, Scope> tuple = new Tuple<bool, Scope>(false, this);
                    return tuple;
                }
                return IsAssignedIdentifier(identifier, scope.Parent);
            }
        }

    }