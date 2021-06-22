using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPOOxBDD.VaccinationContext
{
    public partial class Employee
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string HomeAddress { get; set; }
        public int IdEmployeeType { get; set; }
        public int IdWorkedBooth { get; set; }
        public int? IdManagedBooth { get; set; }
        public int? IdManager { get; set; }

        public virtual EmployeeType IdEmployeeTypeNavigation { get; set; }
        public virtual Booth IdManagedBoothNavigation { get; set; }
        public virtual Manager IdManagerNavigation { get; set; }
        public virtual Booth IdWorkedBoothNavigation { get; set; }
    }
}
