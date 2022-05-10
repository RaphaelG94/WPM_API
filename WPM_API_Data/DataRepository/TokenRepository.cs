using WPM_API.Common.Utils;
using WPM_API.Data.DataContext.Entities;
using WPM_API.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace  WPM_API.Data.DataRepository
{
    public class TokenRepository : RepositoryEntityBase<Token>
    {
        public TokenRepository(DataContextProvider context) : base(context)
        {
        }

        public string GenerateToken(string customerId, DateTime validTo)
        {
            string guid = Guid.NewGuid().ToString();
            Context.Set<Token>().Add(new Token() { CustomerId = customerId, Hash = PasswordHash.HashPassword(guid), ValidTo = validTo });
            return guid;
        }
        public List<Token> GetByCustomerId(string customerId)
        {
           return  Context.Set<Token>().Where(x=>x.CustomerId.Equals(customerId) && x.ValidTo>DateTime.Now).ToList();
        }
    }
}
