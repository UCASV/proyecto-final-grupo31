using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPOOxBDD.VaccinationContext
{
    public partial class Citizen
    {
        public Citizen()
        {
            Appointments = new HashSet<Appointment>();
            Diseases = new HashSet<Disease>();
        }

        public int Id { get; set; }
        public string Dui { get; set; }
        public string FullName { get; set; }
        public string HomeAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int? IdInstitution { get; set; }
        public string InstitutionIdentification { get; set; }
        public int IdPriorityGroup { get; set; }

        public virtual Institution IdInstitutionNavigation { get; set; }
        public virtual PriorityGroup IdPriorityGroupNavigation { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Disease> Diseases { get; set; }
    }
}
