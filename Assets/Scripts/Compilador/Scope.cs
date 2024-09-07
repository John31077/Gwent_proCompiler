using System.Collections.Generic;

public class Scope
    {
        public Scope? Parent;

        public List<string> identifiers;

        public Scope()
        {
            identifiers = new List<string>();   
        }

        public Scope CreateChild()
        {
            Scope child = new Scope();
            child.Parent = this;
               
            return child;
        }

        public void AssignIdentifier()
        {

        }

        //Metodo para verificar si un identificador ya está definido en el scope o su padre o ancestros.
        public bool IsAssignedIdentifier(string identifier, Scope scope)
        {
            if (scope.identifiers.Contains(identifier))
            {
                return true;
            }
            else
            {
                if (scope.Parent == null && !scope.identifiers.Contains(identifier))
                {
                    return false;
                }
                return IsAssignedIdentifier(identifier, scope.Parent);
            }
        }

    }