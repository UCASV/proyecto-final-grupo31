using System;
using System.Collections.Generic;

#nullable disable

namespace ProyectoPOOxBDD.VaccinationContext
{
    public partial class LogInHistory
    {
        public int Id { get; set; }
        public DateTime LogInDateTime { get; set; }
        public int IdManager { get; set; }

        public virtual Manager IdManagerNavigation { get; set; }
    }
}
