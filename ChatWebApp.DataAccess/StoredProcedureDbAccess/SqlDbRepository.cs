using ChatWebApp.StoredProcedureDbAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ChatWebApp.WebedureDbAccess
{
    public abstract class SqlDbRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly string _connectionString;
        protected SqlDbRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public IDbConnection GetOpenConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public TEntity GetSingle(int aSingleId)
        {
            throw new NotImplementedException();
        }
    }
}
