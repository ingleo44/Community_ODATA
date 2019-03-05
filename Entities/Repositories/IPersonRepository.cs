using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public interface IPersonRepository
    {
        Task<ICollection<Person>> GetAllPersons();

    }
}