using System;
namespace Parser
{
    public interface IContentParser
    {
        Person RawDataToPerson(string rawData, char seperator);
        string PersonToRawData(Person person, char seperator);
    }

    public class ContentParser : IContentParser
    {
        public Person RawDataToPerson(string rawData, char seperator)
        {
            if (string.IsNullOrWhiteSpace(rawData))
                return null;

            var fields = rawData.Split(new[] { seperator }, StringSplitOptions.RemoveEmptyEntries);

            if (fields == null || fields.Length != 5)
                return null;

            return new Person
            {
                LastName = fields[0],
                FirstName = fields[1],
                Gender = fields[2],
                FavoriteColor = fields[3],
                BirthDate = Convert.ToDateTime(fields[4])
            };
        }

        public string PersonToRawData(Person person, char seperator)
        {
            return string.Format($"{person.LastName}" +
                $"{seperator}" +
                $"{person.FirstName}" +
                $"{seperator}" +
                $"{person.Gender}" +
                $"{seperator}" +
                $"{person.FavoriteColor}" +
                $"{seperator}" +
                $"{person.BirthDate.Value.ToString("MM/dd/yyyy")}");
        }
    }
}
