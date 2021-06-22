using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPOOxBDD.VaccinationContext
{
    public partial class VaccineReaction
    {
        public int IdAppointment { get; set; }
        public int IdSideEffect { get; set; }
        public int AppearenceTime { get; set; }

        public virtual Appointment IdAppointmentNavigation { get; set; }
        public virtual SideEffect IdSideEffectNavigation { get; set; }
    }
}
