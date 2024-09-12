using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using TrainingPlan.Domain.Entities;
using TrainingPlan.Domain.Repositories;
using TrainingPlan.Infrastructure.DbContext;

namespace TrainingPlan.Infrastructure.Repositories
{
    public abstract class BaseRepository<IEntity>(ApplicationDbContext dbContext) : IBaseRepository<IEntity> where IEntity : BaseEntity
    {
        protected readonly ApplicationDbContext _dbContext = dbContext;
        public readonly IDbConnection _dapperConnection = new NpgsqlConnection(dbContext.Database.GetConnectionString());

        protected virtual Task<IEnumerable<IEntity>> GetAllByParentId(Dictionary<string, object> parentIds)
        {
            string where = "WHERE ";

            var dictionary = new Dictionary<string, object>();

            if (parentIds != null && parentIds.Count > 0)
            {
                foreach (var parentId in parentIds)
                {
                    where += $" {nameof(IEntity)}.{parentId.Key} = ${parentId.Key} AND ";
                    dictionary.Add($"${parentId.Key}", parentId.Value);
                }
            }

            where += " 1=1 ";

            var parameters = new DynamicParameters(dictionary);

            string query = $"SELECT * FROM {nameof(IEntity)} {where} ;";

            return _dapperConnection.QueryAsync<IEntity>(query, parameters);
        }

        protected virtual Task<SqlMapper.GridReader> GetPagedAsync(string entityName, int lastId, int pageSize, string direction, Dictionary<string, object> filteredColumns)
        {
            string where = "WHERE ";
            string orderBy = $"ORDER BY \"Id\" ASC";
            string limit = " LIMIT @pageSize ";

            var dictionary = new Dictionary<string, object>
            {
                { "id", lastId } ,
                { "pageSize", pageSize }
            };

            if (filteredColumns != null && filteredColumns.Count > 0)
            {
                foreach (var column in filteredColumns)
                {
                    where += $" \"{column.Key}\" = @{column.Key} AND ";
                    dictionary.Add($"{column.Key}", column.Value);
                }
            }

            where += "\"Id\" > @id ";

            if (direction == "DESC")
            {
                where += "\"Id\" < @id ";
                orderBy = $"ORDER BY \"Id\" DESC";
            }

            var parameters = new DynamicParameters(dictionary);

            string query = $"SELECT * FROM \"{entityName}\" {where} {orderBy} {limit}; ";

            query += $"\n  SELECT COUNT(*) As Total FROM \"{entityName}\" {where};";

            return _dapperConnection.QueryMultipleAsync(query, param: parameters);
        }

        protected Task<SqlMapper.GridReader> GetWithParentsAsync(string query, object param)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "param", param }
            };

            return GetWithParentsAsync(query, dictionary);
        }

        protected Task<SqlMapper.GridReader> GetWithParentsAsync(string query, Dictionary<string, object> filter)
        {
            var parameters = new DynamicParameters(filter);

            using (var connection = new NpgsqlConnection(dbContext.Database.GetConnectionString()))
                return _dapperConnection.QueryMultipleAsync(query, param: parameters);


        }

        protected Task<TResult?> GetAsync<TResult>(string query, object param)
        {
            var dictionary = new Dictionary<string, object>
            {
                { "param", param }
            };

            var parameters = new DynamicParameters(dictionary);

            return _dapperConnection.QuerySingleOrDefaultAsync<TResult>(query, param: parameters);
        }

        public virtual Task<IEntity?> GetAsync(int id, CancellationToken cancellationToken)
        {
            return _dbContext.Set<IEntity>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        protected virtual void Delete(IEntity entity)
        {
            _dbContext.Remove(entity);
        }

        public void Create(IEntity entity)
        {
            _dbContext.Add(entity);
        }

        public void Update(IEntity entity)
        {
            _dbContext.Update(entity);
        }
    }
}
