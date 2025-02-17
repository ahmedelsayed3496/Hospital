using Hospital.DataAccess;
using Hospital.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hospital.Controllers
{
    public class DoctorsController : Controller
    {
        ApplicationDbContext dbContext = new ApplicationDbContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BookAppointment()
        {
            var doctors = dbContext.Doctors;
            return View(doctors.ToList());
        }
        [HttpGet]
        public IActionResult CompleteAppointment(int id)
        {
            var doctor = dbContext.Doctors.FirstOrDefault(e => e.Id == id);
            if (doctor == null)
            {
                return RedirectToAction("NotFoundPage");
            }

            return View(doctor);
        }

        [HttpPost]
        public IActionResult CompleteAppointment(string patientName, DateTime appointmentDate, TimeSpan appointmentTime, int doctorId)
        {
            var appointment = new Appointment
            {
                PatientName = patientName,
                AppointmentDate = appointmentDate,
                AppointmentTime = appointmentTime,
                DoctorId = doctorId
            };

            dbContext.Appointments.Add(appointment);
            dbContext.SaveChanges();

            return RedirectToAction("ReservationsManagement");
        }
        public IActionResult ReservationsManagement()
        {          
            var appointments = dbContext.Appointments.Include(e => e.Doctor);

            return View(appointments.ToList());
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
