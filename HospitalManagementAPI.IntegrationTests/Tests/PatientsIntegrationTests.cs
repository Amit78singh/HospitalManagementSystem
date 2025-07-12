using DocumentFormat.OpenXml.Wordprocessing;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;

namespace HospitalManagementAPI.IntegrationTests.Tests
{
    public class PatientsIntegrationTests
    {
        private readonly HttpClient _client;

        public PatientsIntegrationTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7064/") 
            };

            // Authorization if needed
            var token = GetJwtToken();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        }

        private string GetJwtToken()
        {
            
            return "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW5AaG9zcGl0YWwuY29tIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiQWRtaW4iLCJleHAiOjE3NTIyMTc5MDR9.cIQA9nLhP7ulkEGm1DN8XPabGBKA6ONmqaj-HSCvd74";
        }


        [Fact]
        public async Task GetPatients_ShouldReturnSuccess()
        {
            var response = await _client.GetAsync("api/patients?pageNumber=1&pageSize=5");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var result = JsonSerializer.Deserialize<PatientsResponse>(content, options);

            result.Should().NotBeNull();
            result!.data.Should().NotBeEmpty();
            result.data[0].Name.Should().NotBeNullOrEmpty();
        }

        // Helper class
        public class PatientsResponse
        {
            public List<Patient> data { get; set; } = new();
            public int totalPages { get; set; }
        }

        public class Patient
        {
            public int Id { get; set; }
            public string Name { get; set; } = "";
            public int Age { get; set; }
            public string Disease { get; set; } = "";
            public int DoctorId { get; set; }
        }


        [Fact]
        public async Task PostPatient_ShouldCreatePatient()
        {
            var newPatient = new
            {
                Name = "Test Patient",
                Age = 30,
                Disease = "Flu",
                DoctorId = 5
            };

            var json = JsonSerializer.Serialize(newPatient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/patients", content);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            responseBody.Should().Contain("Test Patient");
        }


        [Fact]
        public async Task PutPatient_ShouldUpdatePatient()
        {
            var updatePatient = new
            {
                Id = 14,
                Name = "Updated Patient",
                Age = 35,
                Disease = "Updated Disease",
                DoctorId= 5,
                IsDeleted= false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
               


            };

            var json = JsonSerializer.Serialize(updatePatient);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _client.PutAsync("api/patients/14", content);
            response.EnsureSuccessStatusCode();
        }


        [Fact]
        public async Task DeletePatient_ShouldRemovePatient()
        {
            var response = await _client.DeleteAsync("api/patients/10");
            response.EnsureSuccessStatusCode();
        }


    }
}
