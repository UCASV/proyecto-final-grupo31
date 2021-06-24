using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPOOxBDD.VaccinationContext
{
    public partial class SideEffect
    {
        public SideEffect()
        {
            VaccineReactions = new HashSet<VaccineReaction>();
        }

        public int Id { get; set; }
        public string SideEffectName { get; set; }

        public virtual ICollection<VaccineReaction> VaccineReactions { get; set; }
    }
}
