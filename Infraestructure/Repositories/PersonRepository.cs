using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PersonRepository : GenericRepository<Person> , IPersonRepository
    {
        public PersonRepository(AdventureWorks2017Context dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<Person>> GetAllPersons()
        {
            var result = await Query().ToListAsync();
            return result;
        }
    }
}
