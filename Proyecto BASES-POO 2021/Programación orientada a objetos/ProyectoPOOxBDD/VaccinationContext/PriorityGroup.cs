using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPOOxBDD.VaccinationContext
{
    public partial class PriorityGroup
    {
        public PriorityGroup()
        {
            Citizens = new HashSet<Citizen>();
        }

        public int Id { get; set; }
        public string PriorityGroupName { get; set; }

        public virtual ICollection<Citizen> Citizens { get; set; }
    }
}
