using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPOOxBDD.VaccinationContext
{
    public partial class VaccinationPlace
    {
        public VaccinationPlace()
        {
            Appointments = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public string Place { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
