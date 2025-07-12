using Microsoft.EntityFrameworkCore;
using HospitalManagementAPI.Models;

namespace HospitalManagementAPI.Data;

public class HospitalContext : DbContext
{
    public HospitalContext(DbContextOptions<HospitalContext> options) : base(options) { }

    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Patient> Patients { get; set; }

    public DbSet<User> Users { get; set; }
}
