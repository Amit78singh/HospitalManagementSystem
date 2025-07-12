using System.ComponentModel.DataAnnotations;

namespace HospitalManagementAPI.Models;

public class Doctor
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Specialty { get; set; } = string.Empty;

    // ✅ Soft delete flag
    public bool IsDeleted { get; set; } = false;


    // audit fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;


    // ✅ Make this optional by initializing or making it nullable
    public List<Patient> Patients { get; set; } = new();

}
