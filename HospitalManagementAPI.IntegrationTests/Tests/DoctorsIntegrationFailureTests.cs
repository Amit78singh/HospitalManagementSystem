using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace HospitalManagementAPI.IntegrationTests.Tests
{
    public class DoctorsIntegrationFailureTests
    {
        private readonly HttpClient _client;

        public DoctorsIntegrationFailureTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7064/")
            };

            var token = GetJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private string GetJwtToken()
        {
            return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5AaG9zcGl0YWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NTIyMTc5MDR9.cIQA9nLhP7ulkEGm1DN8XPabGBKA6ONmqaj-HSCvd74"; // Replace with real one
        }

        [Fact]
        public async Task PostDoctor_ShouldReturnBadRequest_WhenNameIsMissing()
        {
            var invalidDoctor = new
            {
                Name = "", // Missing name
                Specialty = "Cardiology"
            };

            var json = JsonSerializer.Serialize(invalidDoctor);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/doctors", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PutDoctor_ShouldReturnBadRequest_WhenIdMismatch()
        {
            var doctor = new
            {
                Id = 10,
                Name = "Mismatch Doctor",
                Specialty = "Neurology"
            };

            var json = JsonSerializer.Serialize(doctor);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Mismatch between route ID and body ID
            var response = await _client.PutAsync("api/doctors/99", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task DeleteDoctor_ShouldReturnNotFound_WhenDoctorDoesNotExist()
        {
            var response = await _client.DeleteAsync("api/doctors/99999"); // Non-existent ID

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
