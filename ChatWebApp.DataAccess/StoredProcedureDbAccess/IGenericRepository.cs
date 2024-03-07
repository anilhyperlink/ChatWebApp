using System.Collections.Generic;
using System.Data;

namespace ChatWebApp.StoredProcedureDbAccess
{
    public interface IGenericRepository<out TEntity>
    {
        IDbConnection GetOpenConnection();
        TEntity GetSingle(int aSingleId);
        IEnumerable<TEntity> GetAll();

    }
}
