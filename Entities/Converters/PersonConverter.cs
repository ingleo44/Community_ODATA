using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adventureworks.Entities.Util;
using Adventureworks.Entities.ViewModels;
using Infrastructure;

namespace Adventureworks.Entities.Converters
{
    public static class PersonConverter
    {
        public static PersonViewModel Convert(Person person)
        {
            var personViewModel = new PersonViewModel();
            person.CopyPropertiesTo(personViewModel);
            return personViewModel;
        }

        public static List<PersonViewModel> ConvertList(IEnumerable<Person> persons)
        {
            return persons.Select(Convert).ToList();
        }
    }
}
