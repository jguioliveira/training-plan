namespace TrainingPlan.App.Client.Code.Services
{
    public class TraningPlanUserService
    {
        private readonly HttpClient _httpClient;

        public TraningPlanUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SendAuthenticateRequestAsync(string username, string password)
        {
            //var response = await _httpClient.GetAsync($"/example-data/{username}.json");
            // string token = string.Empty;

            // if (response.IsSuccessStatusCode)
            // {
            //     token = await response.Content.ReadAsStringAsync();                
            // }

            // return token;

            return await Task.FromResult("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IlNoYWhyaXlhciIsInJvbGUiOiJBZG1pbiIsImlhdCI6MTUxNjIzOTAyMn0.l9E7Oypb-ozndpFUkeVhOYzhtjGEuFmdYdAxhbpXAFY");
        }

    }
}
