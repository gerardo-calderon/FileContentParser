using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Parser
{
    [TestFixture]
    public class UnitTests
    {
        #region Content Parser

        [Test]
        [TestCase("Monroe,James,M,Blue,01-01-1809", ',')]
        [TestCase("Monroe James M Blue 01-01-1809", ' ')]
        [TestCase("Monroe|James|M|Blue|01-01-1809", '|')]
        public void ContentParserRawDataToPersonUnitTest(string rawData, char seperator)
        {
            IContentParser contentParser = new ContentParser();
            Person person = contentParser.RawDataToPerson(rawData, seperator);

            Assert.AreEqual("Monroe", person.LastName);
            Assert.AreEqual("James", person.FirstName);
            Assert.AreEqual("M", person.Gender);
            Assert.AreEqual("Blue", person.FavoriteColor);
            Assert.AreEqual("01/01/1809", person.BirthDate.Value.ToString("MM/dd/yyyy"));
        }

        [Test]
        [TestCase("Monroe,James,M,Blue,01/01/1809", ',')]
        [TestCase("Monroe James M Blue 01/01/1809", ' ')]
        [TestCase("Monroe|James|M|Blue|01/01/1809", '|')]
        public void ContentParserPersonToRawDataUnitTest(string expected, char seperator)
        {
            Person person = new Person();
            person.FirstName = "James";
            person.LastName = "Monroe";
            person.Gender = "M";
            person.FavoriteColor = "Blue";
            person.BirthDate = Convert.ToDateTime("01-01-1809");

            IContentParser contentParser = new ContentParser();
            string rawData = contentParser.PersonToRawData(person, seperator);

            Assert.AreEqual(expected, rawData);
        }

        #endregion

        #region FileLoader

        [Test]
        [TestCase(@"D:\_gc\development\Visual Studio 2017\FileContentParser\Parser\TestData\input.commas")]
        public void FileLoaderLoadFilesUnitTest(string filePath)
        {
            //TestCase: Monroe,James,M,Blue,01-01-1809

            FileMetadata fileMetadata = new FileMetadata();
            fileMetadata.FilePath = filePath;
            fileMetadata.Seperator = ',';
            List<FileMetadata> fileMetadataList = new List<FileMetadata>();
            fileMetadataList.Add(fileMetadata);

            IFileLoader fileLoader = new FileLoader();
            List<Person> persons = fileLoader.LoadFiles(fileMetadataList);

            Assert.AreEqual("Monroe", persons[0].LastName);
            Assert.AreEqual("James", persons[0].FirstName);
            Assert.AreEqual("M", persons[0].Gender);
            Assert.AreEqual("Blue", persons[0].FavoriteColor);
            Assert.AreEqual("01/01/1809", persons[0].BirthDate.Value.ToString("MM/dd/yyy"));
        }

        #endregion

        #region PersonSorter

        [Test]
        [TestCase(SortOptions.LastName)]
        [TestCase(SortOptions.BirthDate)]
        [TestCase(SortOptions.Gender)]
        public void PersonSorterSortUnitTest(SortOptions sortOption)
        {
            List<Person> persons = new List<Person>();

            Person person1 = new Person { LastName = "Washington", FirstName = "George", Gender = "M", BirthDate = Convert.ToDateTime("01/01/1800"), FavoriteColor = "Orange" };
            Person person2 = new Person { LastName = "Adams", FirstName = "John", Gender = "M", BirthDate = Convert.ToDateTime("01/01/1900"), FavoriteColor = "Purple" };
            Person person3 = new Person { LastName = "Jane", FirstName = "Doe", Gender = "F", BirthDate = Convert.ToDateTime("01/01/2000"), FavoriteColor = "Green" };

            persons.Add(person1);
            persons.Add(person2);
            persons.Add(person3);

            IPersonSorter personSorter = new PersonSorter();
            List<Person> sortedPersons = personSorter.Sort(persons, sortOption);

            if(sortOption == SortOptions.LastName)
            {
                Assert.AreEqual("Adams", sortedPersons[0].LastName);
            }
            else if(sortOption == SortOptions.Gender)
            {
                Assert.AreEqual("F", sortedPersons[0].Gender);
            }
            else if(sortOption == SortOptions.BirthDate)
            {
                Assert.AreEqual("01/01/1800", sortedPersons[0].BirthDate.Value.ToString("MM/dd/yyyy"));
            }
        }

        #endregion
    }
}
