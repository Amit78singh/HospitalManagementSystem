using ClosedXML.Excel;
using CsvHelper;
using HospitalManagementAPI.Data;
using HospitalManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace HospitalManagementAPI.Controllers;



[Authorize]
[ApiController]
[Route("api/[controller]")]

public class PatientsController : ControllerBase
{
    private readonly HospitalContext _context;

    public PatientsController(HospitalContext context)
    {
        _context = context;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetPatients(
    string? search = null,
    string? disease = null,
    int? doctorId = null,
    string? sortBy = null,
    string? sortOrder = "asc",
    int pageNumber = 1,
    int pageSize = 10)
    {
        var query = _context.Patients.Include(p => p.Doctor).Where(p=> !p.IsDeleted).AsQueryable();

        // 🔍 Search by patient name
        if (!string.IsNullOrEmpty(search))
            query = query.Where(p => p.Name.Contains(search));

        // 🎯 Filter by disease
        if (!string.IsNullOrEmpty(disease))
            query = query.Where(p => p.Disease == disease);

        // 🧑‍⚕️ Filter by DoctorId
        if (doctorId.HasValue)
            query = query.Where(p => p.DoctorId == doctorId);

        // 🔃 Sorting
        if (!string.IsNullOrEmpty(sortBy))
        {
            query = sortBy.ToLower() switch
            {
                "name" => sortOrder == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "age" => sortOrder == "desc" ? query.OrderByDescending(p => p.Age) : query.OrderBy(p => p.Age),
                "disease" => sortOrder == "desc" ? query.OrderByDescending(p => p.Disease) : query.OrderBy(p => p.Disease),
                _ => query
            };
        }

        // 👇 Pagination logic
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        var data = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return Ok(new
        {
            data,
            totalPages
        });
    }
        
    


    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatient(int id)
    {
        var patient = await _context.Patients.Include(p => p.Doctor).FirstOrDefaultAsync(p => p.Id == id);

        if (patient == null) return NotFound();

        return patient;
    }


    // CSV Helper for exporting patients
    [HttpGet("export")]
    public async Task<IActionResult> ExportPatientsToCsv()
    {
        var patients = await _context.Patients.Include(p => p.Doctor).ToListAsync();
        using var writer = new StringWriter();
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(patients);
        var csvContent = writer.ToString();
        return File(System.Text.Encoding.UTF8.GetBytes(csvContent), "text/csv", "patients.csv");
    }


    [HttpGet("export- excel")]
    public async Task<IActionResult> ExportPatientsToExcel()
    {
        var patients = await _context.Patients.Include(p => p.Doctor).ToListAsync();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Patients");


        // Add headers
       worksheet.Cell(1, 1).Value = "Id";
       worksheet.Cell(1, 2).Value = "Name";
        worksheet.Cell(1, 3).Value = "Age";
        worksheet.Cell(1, 4).Value = "Disease";
        worksheet.Cell(1, 5).Value = "DoctorId";
      
        // Add data
        for (int i = 0; i < patients.Count; i++)
        {
            var p = patients[i];
            worksheet.Cell(i + 2, 1).Value = p.Id;
            worksheet.Cell(i + 2, 2).Value = p.Name;
            worksheet.Cell(i + 2, 3).Value = p.Age;
            worksheet.Cell(i + 2, 4).Value = p.Disease;
            worksheet.Cell(i + 2, 5).Value = p.DoctorId;

        }
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        var content = stream.ToArray();
        return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "patients.xlsx");


    }





    [HttpPost]
    public async Task<ActionResult<Patient>> PostPatient(Patient patient)
    {

        patient.CreatedAt = DateTime.UtcNow; // Set CreatedAt field
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
    }





    [HttpPut("{id}")]
    public async Task<IActionResult> PutPatient(int id, Patient patient)
    {
        if (id != patient.Id) return BadRequest();


        patient.UpdatedAt = DateTime.UtcNow; // Set UpdatedAt field
        _context.Entry(patient).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return NoContent();
    }





    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        if (patient == null) return NotFound();

      patient.IsDeleted = true; // Soft delete
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
