using Microsoft.Data.SqlClient;
using SnowStorm.Extensions;
using SnowStorm.QueryExecutors;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SnowStorm.Users
{
    public class GetAspNetUserQuery : IQueryResultSingle<AspNetUser>
    {
        private string _guid;
        private string _email;

        public GetAspNetUserQuery WithGuid(string guid)
        {
            _guid = guid;
            return this;
        }

        public GetAspNetUserQuery WithEmail(string email)
        {
            _email = email;
            return this;
        }

        public IQueryable<AspNetUser> Get(IQueryableProvider queryableProvider)
        {
            var query = queryableProvider.Query<AspNetUser>();

            if (_guid.HasValue())
                query = query.Where(w => w.Id == _guid);

            if (_email.HasValue())
                query = query.Where(w => w.Email == _email);

            return query.AsQueryable();
        }

        public async Task<string> GetUserGuid(string email)
        {
            SqlCommand cmd = null;
            try
            {
                var db = Container.GetAppDbContext();

                cmd = new SqlCommand("Select Id From dbo.AspNetUsers Where Email = @email", new SqlConnection(db.ConnectionString));

                if (cmd.Connection == null)
                    throw new NullReferenceException("SqlConnection to ASPNet User Db.");
                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    await cmd.Connection.OpenAsync();

                cmd.Parameters.Clear();
                cmd.Parameters.Add(new SqlParameter("@email", email));

                var result = await cmd.ExecuteScalarAsync();

                return result.ToString();
            }
            catch (Exception ex)
            {
                //Log the error and return empty string.             
                return string.Empty;
            }
            finally
            {
                if (cmd != null && cmd.Connection != null && cmd.Connection.State != System.Data.ConnectionState.Closed)
                    cmd.Connection.Close();
            }
        }
    }
}