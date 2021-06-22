using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPOOxBDD.VaccinationContext
{
    public partial class Appointment
    {
        public Appointment()
        {
            VaccineReactions = new HashSet<VaccineReaction>();
        }

        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int IdVaccinationPlace { get; set; }
        public DateTime? ArrivalDateTime { get; set; }
        public DateTime? VaccinationDateTime { get; set; }
        public int IdAppointmentType { get; set; }
        public int IdManager { get; set; }
        public int IdCitizen { get; set; }

        public virtual AppointmentType IdAppointmentTypeNavigation { get; set; }
        public virtual Citizen IdCitizenNavigation { get; set; }
        public virtual Manager IdManagerNavigation { get; set; }
        public virtual VaccinationPlace IdVaccinationPlaceNavigation { get; set; }
        public virtual ICollection<VaccineReaction> VaccineReactions { get; set; }
    }
}
