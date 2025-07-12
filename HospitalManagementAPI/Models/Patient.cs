using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HospitalManagementAPI.Models;

public class Patient
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }


    [Required]
    public string Disease { get; set; } = string.Empty;

    // Foreign key
    public int DoctorId { get; set; }

    // Navigation property
    [JsonIgnore]
    public Doctor? Doctor { get; set; }

    // ✅ Soft delete flag
    public bool IsDeleted { get; set; } = false;

    // Audit Fields
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = null;

}
