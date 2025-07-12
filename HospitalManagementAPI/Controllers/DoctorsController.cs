using ClosedXML.Excel;
using CsvHelper;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;

namespace HospitalManagementAPI.Controllers;


[Authorize]

[ApiController]
[Route("api/[controller]")]

public class DoctorsController : ControllerBase
{
    private readonly HospitalContext _context;

    public DoctorsController(HospitalContext context)
    {
        _context = context;
    }

    // ✅ GET: api/Doctors?search=John&specialty=Cardiology&sortBy=name&sortOrder=asc
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors(
        string? search = null,
        string? specialty = null,
        string? sortBy = null,
        string? sortOrder = "asc",
        int pageNumber = 1,
        int pageSize = 10)

    {
        var query = _context.Doctors.Where(d => !d.IsDeleted).AsQueryable();

        // 🔍 Search by name
        if (!string.IsNullOrEmpty(search))
            query = query.Where(d => d.Name.Contains(search));

        // 🎯 Filter by Specialty (corrected)
        if (!string.IsNullOrEmpty(specialty))
            query = query.Where(d => d.Specialty == specialty);

        // 🔃 Sorting
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "name" => sortOrder == "desc" ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name),
                "specialty" => sortOrder == "desc" ? query.OrderByDescending(d => d.Specialty) : query.OrderBy(d => d.Specialty),
                _ => query
            };
        }
        // 📄 Pagination

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var doctors = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
        return Ok(new
        {
            data = doctors,
            totalPages,
            currentPage = pageNumber
        });
    }

[HttpGet("{id}")]
    public async Task<ActionResult<Doctor>> GetDoctor(int id)
    {
        var doctor = await _context.Doctors.Include(d => d.Patients).FirstOrDefaultAsync(d => d.Id == id);
        if (doctor == null) return NotFound();
        return doctor;
    }

    [HttpGet("export")]
    public async Task<IActionResult> ExportDoctorsToCsv()
    {
        var doctors = await _context.Doctors.ToListAsync();

        using var writer = new StringWriter();
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(doctors);

        var csvContent = writer.ToString();
        return File(System.Text.Encoding.UTF8.GetBytes(csvContent), "text/csv", "doctors.csv");
    }

    [HttpGet("export-excel")]
    public async Task<IActionResult> ExportDoctorsToExcel()
    {
        var doctors = await _context.Doctors.Include(d => d.Patients).ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Doctors");
        worksheet.Cell(1, 1).Value = "Id";
        worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Specialty";
        worksheet.Cell(1, 4).Value = "Patients Count";

        for (int i = 0; i < doctors.Count; i++)
        {
            var d = doctors[i];
            worksheet.Cell(i + 2, 1).Value = d.Id;
            worksheet.Cell(i + 2, 2).Value = d.Name;
            worksheet.Cell(i + 2, 3).Value = d.Specialty;
            worksheet.Cell(i + 2, 4).Value = d.Patients?.Count ?? 0; // Handle null case

        }
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();
        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "doctors.xlsx");



    }


    [HttpPost]
    public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
    {

        doctor.CreatedAt = DateTime.UtcNow; // Set creation time
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDoctor), new { id = doctor.Id }, doctor);
    }



   [Authorize(Roles = "Admin")]
   [HttpPut("{id}")]
        
        public async Task<IActionResult> PutDoctor(int id, Doctor doctor)
        {
            if (id != doctor.Id) return BadRequest();

            doctor.UpdatedAt = DateTime.UtcNow; // Set update time
        _context.Entry(doctor).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();
         doctor.IsDeleted = true; // Soft delete
        await _context.SaveChangesAsync();
            return NoContent();
        }
    
}