using SnowStorm.Extensions;
using SnowStorm.QueryExecutors;
using SnowStorm.Users;
using System.Linq;

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
    }
}
