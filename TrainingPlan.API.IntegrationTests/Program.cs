using Bogus;
using System.Text;
using System.Text.Json;
using TrainingPlan.API.Application.Features.WorkoutFeatures.CreateWorkout;
using TrainingPlan.API.Application.Features.WorkoutFeatures.UpdateWorkout;
using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;

class Program
{
    private static readonly HttpClient client = new HttpClient { BaseAddress = new Uri("http://localhost:5276/") };

    private static readonly JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,  // Use camelCase naming for properties
    };

    static async Task Main(string[] args)
    {
        try
        {
            Task.Delay(5000).Wait(); // Wait for the server to start

            await CreateTeamAsync();
            await UpdateTeamAsync();
            await GetTeamAsync();
            await SaveTeamSocialMediaAsync();
            await SaveTeamSettingsAsync();

            await CreateAthletesAsync();
            await UpdateAthleteAsync();
            await GetAthleteAsync();
            var athletes = await GetAthletesAsync();

            await CreatePlansForAthletesAsync();
            await UpdatePlanAsync();
            await GetPlanAsync();
            var randomAthlete = new Random();
            int indexAthlete = randomAthlete.Next(athletes.Items.Count());
            var athlete = athletes.Items.ElementAt(indexAthlete);
            var plan = await GetPlanAsync(athlete.Id);


            // ##### WORKOUTS #####
            await CreateWorkoutsForPlansAsync();
            plan = await GetPlanAsync(athlete.Id);
            await UpdateWorkoutAsync(plan);

            var randomWorkouts = new Random();
            int randomIndex = randomWorkouts.Next(plan.Workouts.Count());
            var workout = plan.Workouts.ElementAt(randomIndex);
            

            await CreateCommentAsync(workout.Id, athlete.Id);
            await CreateCommentAsync(workout.Id, 1); //Anderson Teles
            await CreateCommentAsync(workout.Id, athlete.Id);

            workout = await GetWorkoutAsync(workout.Id);

            await UpdateCommentAsync(workout);
            await RemoveCommentAsync(workout);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.ReadLine();
    }

    private static async Task CreateTeamAsync()
    {
        var newTeam = new
        {
            Name = "Anderson Teles",
            Email = "anderson@andersonteles.com"
        };

        var response = await client.PostAsync("api/Teams", new StringContent(JsonSerializer.Serialize(newTeam), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var createdTeam = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Created Team: {createdTeam}");
    }

    private static async Task UpdateTeamAsync()
    {
        var updateTeam = new
        {
            Id = 1, // Assuming the team ID is 1
            Name = "Anderson Teles Updated",
            Email = "anderson@andersonteles.com"
        };

        var response = await client.PutAsync("api/Teams", new StringContent(JsonSerializer.Serialize(updateTeam), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var updatedTeam = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Updated Team: {updatedTeam}");
    }

    private static async Task GetTeamAsync()
    {
        var response = await client.GetAsync("api/Teams/1"); // Assuming the team ID is 1
        response.EnsureSuccessStatusCode();
        var team = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Retrieved Team: {team}");
    }

    private static async Task SaveTeamSocialMediaAsync()
    {
        var socialMedia = new
        {
            Name = "Twitter",
            Account = "@anderson_teles"
        };

        var body = JsonSerializer.Serialize(socialMedia);

        var response = await client.PostAsync("api/Teams/1/social-media", new StringContent(body, Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var savedSocialMedia = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Saved Social Media: {savedSocialMedia}");
    }

    private static async Task SaveTeamSettingsAsync()
    {
        var settings = new Dictionary<string, string>
            {
                { "LogoImage", "https://example.com/logo.png" },
                { "BackgroundColor", "#FFFFFF" }
            };

        var response = await client.PostAsync("api/Teams/1/settings", new StringContent(JsonSerializer.Serialize(settings), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var savedSettings = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Saved Settings: {savedSettings}");
    }

    // #####

    private static async Task CreateAthletesAsync()
    {
        var faker = new Faker("en");
        var athletes = new List<dynamic>();

        for (int i = 1; i <= 20; i++)
        {
            var newAthlete = new
            {
                Email = faker.Internet.Email(),
                Name = faker.Name.FullName(),
                Birth = faker.Date.Past(30, DateTime.Now.AddYears(-20)),
                Phone = faker.Phone.PhoneNumber(),
                Password = faker.Internet.Password(length: 12, prefix: "@Al2024")
            };

            athletes.Add(newAthlete);
        }

        foreach (var athlete in athletes)
        {
            var response = await client.PostAsync("api/Athletes", new StringContent(JsonSerializer.Serialize(athlete), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var createdAthlete = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
            Console.WriteLine($"Created Athlete: {createdAthlete}");
        }
    }

    private static async Task UpdateAthleteAsync()
    {
        var faker = new Faker("en");
        var athletes = await GetAthletesAsync();

        var athleteToUpdate = athletes.Items.FirstOrDefault(i => i.Id == athletes.Items.Max(i =>i.Id)); // Assuming the first athlete in the list

        var updateAthlete = new
        {
            athleteToUpdate.Id, // Assuming the athlete ID is 21
            Name = $"{athleteToUpdate.Name } Updated",
            Birth = faker.Date.Past(30, DateTime.Now.AddYears(-20)),
            Phone = faker.Phone.PhoneNumber(),
        };

        var response = await client.PutAsync("api/Athletes", new StringContent(JsonSerializer.Serialize(updateAthlete), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var updatedAthlete = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Updated Athlete: {updatedAthlete}");
    }

    private static async Task GetAthleteAsync()
    {
        var athletes = await GetAthletesAsync();
        var athleteToSelect = athletes.Items.FirstOrDefault(i => i.Id == athletes.Items.Max(i => i.Id)); // Assuming the first athlete in the list

        var response = await client.GetAsync($"api/Athletes/{athleteToSelect.Id}"); // Assuming the athlete ID is 21
        response.EnsureSuccessStatusCode();
        var athlete = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Retrieved Athlete: {athlete}");
    }

    private static async Task<AthletesPagedListDTO> GetAthletesAsync()
    {
        var response = await client.GetAsync("api/Athletes?pageSize=40&lastId=0&direction=ASC");
        response.EnsureSuccessStatusCode();        

        var strResponse = await response.Content.ReadAsStringAsync();
        var athletes = JsonSerializer.Deserialize<AthletesPagedListDTO>(strResponse, options);
        Console.WriteLine($"Retrieved athletes: \n {athletes}");
        return athletes;
    }

    // #####

    private static async Task CreatePlansForAthletesAsync()
    {
        var athletes = await GetAthletesAsync();
        var faker = new Faker("en");

        foreach (var athlete in athletes.Items)
        {
            var newPlan = new
            {
                Name = faker.Name.JobDescriptor(),
                Description = faker.Lorem.Sentence(6),
                Goal = faker.Lorem.Sentence(6),
                InstructorId = 1, //faker.Random.Int(1, 10), // Assuming instructor IDs are between 1 and 10
                AthleteId = athlete.Id
            };

            var response = await client.PostAsync("api/Plans", new StringContent(JsonSerializer.Serialize(newPlan), Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var createdPlan = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
            Console.WriteLine($"Created Plan for Athlete {athlete.Id}: {createdPlan}");
        }
    }

    private static async Task UpdatePlanAsync()
    {
        var faker = new Faker("en");
        var plans = await GetPlansAsync();

        var planToUpdate = plans.Items.FirstOrDefault(); // Assuming the first plan in the list

        var updatePlan = new
        {
            planToUpdate.Id,
            Name = $"{planToUpdate.Name} Updated",
            Description = faker.Lorem.Sentence(6),
            Goal = faker.Lorem.Sentence(5),
            InstructorId = 1//faker.Random.Int(1, 1) // Assuming instructor IDs are between 1 and 10
        };

        var response = await client.PutAsync("api/Plans", new StringContent(JsonSerializer.Serialize(updatePlan), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var updatedPlan = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Updated Plan: {updatedPlan}");
    }

    private static async Task GetPlanAsync()
    {
        var athletes = await GetAthletesAsync();
        var athleteToSelect = athletes.Items.FirstOrDefault(); // Assuming the first plan in the list

        var response = await client.GetAsync($"api/Plans/athlete/{athleteToSelect.Id}");
        response.EnsureSuccessStatusCode();
        var plan = JsonSerializer.Deserialize<PlanDTO>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Retrieved Plan: {plan}");
    }

    private static async Task<PlanDTO> GetPlanAsync(int athleteId)
    {
        var response = await client.GetAsync($"api/Plans/athlete/{athleteId}");
        response.EnsureSuccessStatusCode();
        var plan = JsonSerializer.Deserialize<PlanDTO>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Retrieved Plan: {plan}");
        return plan;
    }

    private static async Task<PlansPagedListDTO> GetPlansAsync()
    {
        var response = await client.GetAsync("api/Plans?pageSize=40&lastId=0&direction=ASC");
        response.EnsureSuccessStatusCode();
        var plans = JsonSerializer.Deserialize<PlansPagedListDTO>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Retrieved plans: \n {plans}");
        return plans;
    }



    // #####


    private static async Task CreateWorkoutsForPlansAsync()
    {
        var plans = await GetPlansAsync();
        var faker = new Faker("en");

        foreach (var plan in plans.Items)
        {
            for (int i = 0; i < 20; i++)
            {
                var newWorkout = new
                {
                    PlanId = plan.Id,
                    Date = faker.Date.Between(DateTime.Now.AddDays(1), DateTime.Now.AddDays(31)),
                    Description = faker.Lorem.Sentence(6),
                    ContentId = (int?)null // Ignoring ContentId for now
                };

                var response = await client.PostAsync("api/Workouts", new StringContent(JsonSerializer.Serialize(newWorkout), Encoding.UTF8, "application/json"));
                response.EnsureSuccessStatusCode();
                var createdWorkout = JsonSerializer.Deserialize<CreateWorkoutResponse>(await response.Content.ReadAsStringAsync(), options);
                Console.WriteLine($"Created Workout for Plan {plan.Id}: {createdWorkout}");
            }
        }
    }

    private static async Task UpdateWorkoutAsync(PlanDTO plan)
    {
        var faker = new Faker("en");

        var randomWorkouts = new Random();
        int randomIndex = randomWorkouts.Next(plan.Workouts.Count());
        var workoutToUpdate = plan.Workouts.ElementAt(randomIndex);

        var updateWorkout = new
        {
            workoutToUpdate.Id,
            Date = DateTime.Now,
            Description = $"{workoutToUpdate.Description} Updated",
            ContentId = (int?)null // Ignoring ContentId for now
        };

        var response = await client.PutAsync("api/Workouts", new StringContent(JsonSerializer.Serialize(updateWorkout), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var updatedWorkout = JsonSerializer.Deserialize<UpdateWorkoutResponse>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Updated Workout: {updatedWorkout}");
    }

    private static async Task<WorkoutDTO> GetWorkoutAsync(int workoutId)
    {
        var response = await client.GetAsync($"api/Workouts/{workoutId}");
        response.EnsureSuccessStatusCode();
        var workout = JsonSerializer.Deserialize<WorkoutDTO>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Retrieved Workout: {workout}");

        return workout;
    }

    private static async Task CreateCommentAsync(int workoutId, int personId)
    {
        var faker = new Faker("en");

        var newComment = new
        {
            PersonId = personId,
            WorkoutId = workoutId,
            Text = faker.Lorem.Sentence(10)
        };

        var response = await client.PostAsync($"api/Workouts/{workoutId}/comments", new StringContent(JsonSerializer.Serialize(newComment), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var createdComment = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Created Comment for Workout {workoutId}: {createdComment}");
    }

    private static async Task UpdateCommentAsync(WorkoutDTO workout)
    {
        var faker = new Faker("en");

        var commentToUpdate = workout.Comments.FirstOrDefault();

        var updateComment = new
        {
            Text = $"{commentToUpdate.Text} Updated"
        };

        var response = await client.PutAsync($"api/Workouts/{workout.Id}/comments/{commentToUpdate.Id}", new StringContent(JsonSerializer.Serialize(updateComment), Encoding.UTF8, "application/json"));
        response.EnsureSuccessStatusCode();
        var updatedComment = JsonSerializer.Deserialize<dynamic>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Updated Comment: {updatedComment}");
    }

    private static async Task RemoveCommentAsync(WorkoutDTO workout)
    {
        var commentToRemove = workout.Comments.FirstOrDefault(); // Assuming the first comment in the list

        var response = await client.DeleteAsync($"api/Workouts/{workout.Id}/comments/{commentToRemove.Id}");
        response.EnsureSuccessStatusCode();
        Console.WriteLine($"Removed Comment: {commentToRemove.Id}");

    }

    private static async Task<List<dynamic>> GetWorkoutsAsync()
    {
        var response = await client.GetAsync("api/Workouts");
        response.EnsureSuccessStatusCode();
        var workouts = JsonSerializer.Deserialize<List<dynamic>>(await response.Content.ReadAsStringAsync(), options);
        Console.WriteLine($"Retrieved workouts: \n {workouts}");
        return workouts;
    }

}
