using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Identity.Client;

namespace DataverseReader
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Configuration - in production, use environment variables or secret manager
            string tenantId = "YOUR_TENANT_ID_HERE";
            string clientId = "YOUR_CLIENT_ID_HERE";
            string clientSecret = "YOUR_CLIENT_SECRET_HERE";
            string dataverseUrl = "YOUR_DATAVERSE_URL_HERE";

            try
            {
                // 1. Authentication
                var authBuilder = ConfidentialClientApplicationBuilder
                    .Create(clientId)
                    .WithClientSecret(clientSecret)
                    .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}"))
                    .Build();

                var token = await authBuilder.AcquireTokenForClient(
                    new[] { $"{dataverseUrl}.default" })
                    .ExecuteAsync();

                if (string.IsNullOrEmpty(token.AccessToken))
                {
                    Console.WriteLine("Authentication failed: No token received");
                    return;
                }

                // 2. API Setup
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token.AccessToken);
                    client.BaseAddress = new Uri(dataverseUrl);
                    client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
                    client.DefaultRequestHeaders.Add("OData-Version", "4.0");
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    var query = "api/data/v9.2/contacts?$top=20&$select=fullname,emailaddress1,telephone1,contactid&$orderby=fullname asc";
                    // 3. API Call with specific columns
                    string newUrl = $"{dataverseUrl}{query}";


                    //Console.WriteLine($"Requesting: {fullUri}");

                    HttpResponseMessage response = await client.GetAsync(newUrl);

                    // 4. Process Response
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        ODataResponse oData = JsonSerializer.Deserialize<ODataResponse>(responseData);

                        Console.WriteLine($"{responseData}\n\n");

                        //Console.WriteLine($"{oData.value}");
                        JsonDocument doc = JsonDocument.Parse(responseData);

                        //Another option if you don't want to create a Class that mimics the json structure.
                       /* var contacts = doc.RootElement.GetProperty("value");
                        
                        foreach (var contact in contacts.EnumerateArray()) 
                        {
                            var fullname = contact.GetProperty("fullname");
                            var email = contact.GetProperty("emailaddress1");
                            var phone = contact.GetProperty("telephone1");
                            Console.WriteLine("-------------User-------------");
                            Console.WriteLine($"Name:{fullname}");
                            Console.WriteLine($"Email:{email}");
                            Console.WriteLine($"Phone:{phone}\n");
                        }*/

                        foreach (var contact in oData.value)
                        {
                            Console.WriteLine($"{contact.fullname}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        return;
                    }

                    //var responseData = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }
    }
}
