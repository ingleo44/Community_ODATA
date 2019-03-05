using System.Collections.Generic;
using System.Threading.Tasks;
using Adventureworks.Core.Supervisor.Interfaces;
using Infrastructure;
using Infrastructure.Repositories;

namespace Adventureworks.Core.Supervisor.Classes
{
    public class PersonSupervisor : IPersonSupervisor
    {
        private IPersonRepository _personRepository;

        public PersonSupervisor(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<ICollection<Person>> Getall()
        {
            return await _personRepository.GetAllPersons();
        }
    }
}
