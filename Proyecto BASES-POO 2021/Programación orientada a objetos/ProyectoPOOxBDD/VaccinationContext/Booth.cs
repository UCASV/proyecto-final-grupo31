using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPOOxBDD.VaccinationContext
{
    public partial class Booth
    {
        public Booth()
        {
            EmployeeIdManagedBoothNavigations = new HashSet<Employee>();
            EmployeeIdWorkedBoothNavigations = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string BoothAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public virtual ICollection<Employee> EmployeeIdManagedBoothNavigations { get; set; }
        public virtual ICollection<Employee> EmployeeIdWorkedBoothNavigations { get; set; }
    }
}
