using Parser;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace ParserConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //assumptions are that the file name will contain following extensions: commas,pipes,spaces

            if (args == null || args.Length <= 0)
                return;

            //default sort option
            SortOptions sortOption = SortOptions.LastName;
            if(args.Length > 3)
            {
                sortOption = (SortOptions) Enum.Parse(typeof(SortOptions),args[3]);
            }

            //gather information about the files that we are going load
            List<FileMetadata> fileMetadataList = new List<FileMetadata>();
            for (int arg = 0; arg < 3; arg++)
            {
                try
                {
                    FileMetadata metadata = new FileMetadata()
                    {
                        FilePath = args[arg],
                        Seperator = GetFileContentSeperator(args[arg])
                    };

                    fileMetadataList.Add(metadata);
                }
                catch
                {
                    continue;
                }
            }

            //parse data into a collection of persons
            FileLoader fileLoader = new FileLoader();
            List<Person> persons = fileLoader.LoadFiles(fileMetadataList);

            //perform sorting
            PersonSorter sorter = new PersonSorter();
            persons = sorter.Sort(persons, sortOption);

            ContentParser contentParser = new ContentParser();
            char outputSeperator = ConfigurationManager.AppSettings["OutputSeperator"].ToCharArray()[0];
            foreach (Person person in persons)
            {                                
                var parsedConent = contentParser.PersonToRawData(person, outputSeperator);

                Console.WriteLine(parsedConent);
            }
            Console.ReadLine();
        }

        private static char GetFileContentSeperator(string filePath)
        {
            string extension = Path.GetExtension(filePath);

            switch (extension)
            {
                case ".pipes":
                    return '|';
                case ".spaces":
                    return ' ';
                case ".commas":
                    return ',';
                default:
                    throw new ArgumentException("Failed to determine file seperator.");
            }
        }
    }
}
