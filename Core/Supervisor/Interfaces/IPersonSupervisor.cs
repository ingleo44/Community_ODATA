using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure;

namespace Adventureworks.Core.Supervisor.Interfaces
{
    public interface IPersonSupervisor
    {
        Task<ICollection<Person>> Getall();
    }
}