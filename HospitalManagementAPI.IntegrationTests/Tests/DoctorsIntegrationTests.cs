using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace HospitalManagementAPI.IntegrationTests.Tests
{
    public class DoctorsIntegrationTests
    {
        private readonly HttpClient _client;

        public DoctorsIntegrationTests()
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
            return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5AaG9zcGl0YWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NTIyMzY2ODh9.gqAa2JrWrZBqG4IPHC2WWHKvbx6is2NSBXl5cxPjQqA";
        }

        [Fact]
        public async Task PostDoctor_ShouldCreateDoctor()
        {
            var newDoctor = new
            {
                Name = "Dr. Integration",
                Specialty = "Dermatology"
            };

            var json = JsonSerializer.Serialize(newDoctor);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/doctors", content);
            response.EnsureSuccessStatusCode();

            var body = await response.Content.ReadAsStringAsync();
            body.Should().Contain("Dr. Integration");
        }

        [Fact]
        public async Task PutDoctor_ShouldUpdateDoctor()
        {
            var updateDoctor = new
            {
                Id = 5, // use a valid existing doctor ID
                Name = "Dr. Updated",
                Specialty = "Cardiology",
                IsDeleted= false,
                CreatedAt=DateTime.UtcNow,
                UpdatedAt= DateTime.UtcNow,
            };

            var json = JsonSerializer.Serialize(updateDoctor);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("api/doctors/5", content); // match ID in URL and payload
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task DeleteDoctor_ShouldRemoveDoctor()
        {
            var response = await _client.DeleteAsync("api/doctors/5"); // use a real doctor ID
            response.EnsureSuccessStatusCode();
        }
    }
}
