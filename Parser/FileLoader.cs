using System.Collections.Generic;
using System.IO;

namespace Parser
{
    public interface IFileLoader
    {
        List<Person> LoadFiles(List<FileMetadata> fileMetadataList);
    }

    public class FileLoader : IFileLoader
    {
        private IContentParser _contentParser = new ContentParser();

        public List<Person> LoadFiles(List<FileMetadata> fileMetadataList)
        {
            List<Person> persons = new List<Person>();

            if (fileMetadataList == null || fileMetadataList.Count <= 0)
                return persons;

            for (int i = 0; i < fileMetadataList.Count; i++)
            {
                string[] fileLines = File.ReadAllLines(fileMetadataList[i].FilePath);
                if (fileLines == null || fileLines.Length <= 0)
                    continue;

                for (int fileLine = 0; fileLine < fileLines.Length; fileLine++)
                {
                    Person person = _contentParser.RawDataToPerson(fileLines[fileLine], fileMetadataList[i].Seperator);
                    persons.Add(person);
                }
            }
            return persons;
        }
    }
}
