using Parser;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;

namespace ParserWebComponent.Controllers
{
    public class RecordsController : ApiController
    {
        //this would never be here in "real code" we're just assuming we have some data store
        private const string DataStorePath = @"d:\_gc\development\visual studio 2017\filecontentparser\parser\testdata\data.dataset";

        [HttpGet()]        
        public IEnumerable<Person> Get()
        {
            return GetMockedDataStorePersonData();
        }

        [HttpGet()]
        [Route("api/records/birthdate")]
        public IEnumerable<Person> GetByBirthDate()
        {
            return GetMockedDataStorePersonData(SortOptions.BirthDate);
        }

        [HttpGet()]
        [Route("api/records/name")]
        public IEnumerable<Person> GetByName()
        {
            return GetMockedDataStorePersonData(SortOptions.LastName);
        }

        [HttpGet()]
        [Route("api/records/gender")]
        public IEnumerable<Person> GetByGender()
        {
            return GetMockedDataStorePersonData(SortOptions.Gender);
        }
               
        [HttpPost()]        
        public IHttpActionResult Post(Person person)
        {
            IContentParser contentParser = new ContentParser();
            string rawData = contentParser.PersonToRawData(person, ',');
            File.AppendAllText(DataStorePath, $"{rawData}");

            return Ok();
        }

        private IEnumerable<Person> GetMockedDataStorePersonData(SortOptions option = SortOptions.LastName)
        {
            IFileLoader fileLoader = new FileLoader();
            List<FileMetadata> fileMetadataList = new List<FileMetadata>();
            FileMetadata fileMetadata = new FileMetadata();
            fileMetadata.FilePath = DataStorePath;
            fileMetadata.Seperator = ',';
            fileMetadataList.Add(fileMetadata);
            List<Person> persons = fileLoader.LoadFiles(fileMetadataList);

            IPersonSorter personSorter = new PersonSorter();
            persons = personSorter.Sort(persons, option);

            return persons;
        }
    }
}
