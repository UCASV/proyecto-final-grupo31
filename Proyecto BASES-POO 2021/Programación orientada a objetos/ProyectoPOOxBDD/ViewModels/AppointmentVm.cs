using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPOOxBDD.ViewModels
{
    public class AppointmentVm
    {
        public int Id { get; set; }
        public string AppointmentType { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string VaccinationPlace { get; set; }
        public bool Attendance { get; set; }
    }
}
