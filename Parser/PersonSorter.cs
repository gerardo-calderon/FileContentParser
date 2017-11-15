using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public interface IPersonSorter
    {
        List<Person> Sort(List<Person> persons, SortOptions options);
    }
    public class PersonSorter : IPersonSorter
    {
        public List<Person> Sort(List<Person> persons, SortOptions options)
        {
            IOrderedEnumerable<Person> orderedPersons = null;
            if (options == SortOptions.Gender)
            {
                orderedPersons = persons.OrderBy(person => person.Gender);
            }
            if (options == SortOptions.BirthDate)
            {
                orderedPersons = persons.OrderBy(person => person.BirthDate);
            }
            if (options == SortOptions.LastName)
            {
                orderedPersons = persons.OrderBy(person => person.LastName);
            };

            return orderedPersons.ToList();
        }
    }
}
