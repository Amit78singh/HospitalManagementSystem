using HospitalManagementAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalManagementAPI.Controllers
{

    [Authorize(Roles = "Admin")]

    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly HospitalContext _context;

        public AdminController(HospitalContext context)
        {
            _context = context;

        }
        [HttpGet("stats")]
    public async Task<IActionResult> GetDashboardStats()
        {
            var totalDoctors = await _context.Doctors.CountAsync();
            var totalPatients = await _context.Patients.CountAsync(p => !p.IsDeleted);
            var patientsPerDoctor = await _context.Doctors
                .Select( d=> new
                { d.Name,
                patientCount=d.Patients.Count(p=> !p.IsDeleted)
                }).ToListAsync();
            return Ok(new
            {
                totalDoctors,
                totalPatients,
                patientsPerDoctor

            });
        }



    }
}
