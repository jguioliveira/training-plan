using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Infrastructure.DbContext;
using TrainingPlan.Domain.DTO;

namespace TrainingPlan.Infrastructure.Repositories
{
    public class PersonRepository(ApplicationDbContext dbContext) : BaseRepository<Person>(dbContext), IPersonRepository
    {
        private Task<TResult?> GetPersonAsync<TResult>(int id)
        {
            string query = "SELECT * FROM \"Person\" WHERE \"Id\" = @param;";

            return GetAsync<TResult>(query, id);
        }

        private async Task<TResult> GetPersonsAsync<TResult, TItems>(int lastId, int pageSize, string direction, string name, string type) where TResult : PaginationResult<TItems>
        {
            var filters = new Dictionary<string, object>
            {
                { "Type", type }
            };

            if (!string.IsNullOrEmpty(name))
            {
                filters.Add("Name", name);
            }

            var results = await base.GetPagedAsync("Person", lastId, pageSize, direction, filters);

            var items = await results.ReadAsync<TItems>();
            var total = await results.ReadSingleAsync<int>();

            var response = new PaginationResult<TItems>
            {
                Items = items,
                Total = total,
            };

            return (TResult)response;
        }

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

            var results = await base.GetPagedAsync("Person", lastId, pageSize, direction, filters);

            var items = await results.ReadAsync<AthleteDTO>();
            var total = await results.ReadSingleAsync<int>();

            var athletes = new AthletesPagedListDTO
            {
                Items = items,
                Total = total,
            };

            return athletes;
        }

        public Task<InstructorsPagedListDTO> GetInstructorsAsync(int lastId, int pageSize, string direction, string name)
        {
            var filters = new Dictionary<string, object>
            {
                { "Type", PersonType.INSTRUCTOR }
            };

            if (!string.IsNullOrEmpty(name))
            {
                filters.Add("Name", name);
            }

            return GetPersonsAsync<InstructorsPagedListDTO, InstructorDTO>(lastId, pageSize, direction, name, PersonType.INSTRUCTOR);
        }

        public Task<AthleteDTO?> GetAthleteAsync(int id)
        {
            string query = "SELECT * FROM \"Person\" WHERE \"Id\" = @param;";

            return GetAsync<AthleteDTO>(query, id);
        }

        public Task<InstructorDTO?> GetInstructorAsync(int id)
        {
            return GetPersonAsync<InstructorDTO>(id);
        }
    }
}
