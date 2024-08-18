using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Infrastructure.DbContext;
using TrainingPlan.Domain.DTO;
using System.Threading;

namespace TrainingPlan.Infrastructure.Repositories
{
    public class PersonRepository(ApplicationDbContext dbContext) : BaseRepository<Person>(dbContext), IPersonRepository
    {
        public async Task<AthletesPagedListDTO> GetAthletesAsync(int lastId, int pageSize, string direction, string name)
        {
            var filters = new Dictionary<string, object>
            {
                { "Type", PersonType.ATHLETE }
            };

            if (!string.IsNullOrEmpty(name)) 
            {
                filters.Add("Name", name);
            }

            var results = await base.GetPagedAsync(lastId, pageSize, direction, filters);

            var items = await results.ReadAsync<AthleteDTO>();
            var total = await results.ReadSingleAsync<int>();

            var athletes = new AthletesPagedListDTO
            {
                Items = items,
                Total = total,
            };

            return athletes;
        }

        public Task<AthleteDTO?> GetAthleteAsync(int id)
        {
            string query = "SELECT * FROM \"Person\" WHERE \"Id\" = @param;";

            return GetAsync<AthleteDTO>(query, id);
        }
    }
}
