using TrainingPlan.Domain.DTO;
using TrainingPlan.Domain.Entities;

namespace TrainingPlan.Domain.Repositories
{
    public interface IPersonRepository : IBaseRepository<Person>
    {
        Task<AthletesPagedListDTO> GetAthletesAsync(int lastId, int pageSize, string direction, string name);

        Task<AthleteDTO?> GetAthleteAsync(int id);
    }


}
