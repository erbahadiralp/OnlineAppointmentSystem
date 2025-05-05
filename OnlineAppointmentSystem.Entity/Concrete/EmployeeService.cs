using System.ComponentModel.DataAnnotations.Schema;
using OnlineAppointmentSystem.Entity.Concrete;

namespace OnlineAppointmentSystem.Entity.Concrete
{
    public class EmployeeService
    {
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        [ForeignKey("Service")]
        public int ServiceId { get; set; }

        // Navigation properties
        public virtual Employee Employee { get; set; }
        public virtual Service Service { get; set; }
    }
}