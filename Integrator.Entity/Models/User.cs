using System;

namespace Integrator.Models
{
    [Serializable]
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Division { get; set; }
        public string NameSurname { get {
                return this.Name + " " + this.Surname;    
            }
        }
    }
}
