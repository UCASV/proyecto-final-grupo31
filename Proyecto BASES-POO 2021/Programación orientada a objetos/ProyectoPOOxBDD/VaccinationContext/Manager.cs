using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPOOxBDD.VaccinationContext
{
    public partial class Manager
    {
        public Manager()
        {
            Appointments = new HashSet<Appointment>();
            Employees = new HashSet<Employee>();
            LogInHistories = new HashSet<LogInHistory>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string KeyCode { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<LogInHistory> LogInHistories { get; set; }
    }
}
