using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProyectoPOOxBDD.VaccinationContext;

namespace ProyectoPOOxBDD.ViewModels
{
    public static class VaccinationMapper
    {
        public static BoothVm MapBoothToBoothVm(Booth e)
        {
            return new BoothVm()
            {
                Id = e.Id,
                BoothAddress = e.BoothAddress,
                PhoneNumber = e.PhoneNumber,
                EmailAddress = e.EmailAddress
            };
        }

        public static AppointmentVm MapAppointmentToAppointmentVm(Appointment e)
        {
            AppointmentVm appointmentVm = new AppointmentVm()
            {
                Id = e.Id,
                AppointmentType = e.IdAppointmentTypeNavigation.TypeName,
                AppointmentDateTime = e.AppointmentDateTime,
                VaccinationPlace = e.IdVaccinationPlaceNavigation.Place
            };

            if (e.ArrivalDateTime != null)
                appointmentVm.Attendance = true;
            else
                appointmentVm.Attendance = false; 

            return appointmentVm;
        }
    }
}