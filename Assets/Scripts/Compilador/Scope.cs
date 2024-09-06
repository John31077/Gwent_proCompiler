/* The scope is used to check if the effect are alredy in use inside
every Card or Effect declaration. We must create a Scope Child in every case */
using System.Collections.Generic;

public class Scope
    {
        public Scope? Parent;

        public List<string> effects;

        public Scope()
        {
            effects = new List<string>();   
        }

        public Scope CreateChild()
        {
            Scope child = new Scope();
            child.Parent = this;
               
            return child;
        }

    }