using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using DataAccess.Repositories;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using TestUtils;
using TestUtils.Integration;
using Xunit;
using Xunit.Abstractions;

namespace Catalog.Tests.Integration
{
    public class FileRepositoryTests : MongoClientFixture
    {
        private readonly FileRepository _fileRepository;

        public FileRepositoryTests(ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            //_fileRepository = new DocumentFileRepository(Database);
        }

        //[Fact]
        //public async Task Can_download_a_file()
        //{
        //    string fileName = "content.txt";
        //    string expectedContent = "content";
        //    var bytes = Encoding.UTF8.GetBytes(expectedContent);
        //    ObjectId fileId;
        //    await using (var stream = new MemoryStream(bytes))
        //    {
        //        fileId = await _fileRepository.UploadFromStream(fileName, stream);
        //    }

        //    await using (var stream = new MemoryStream())
        //    {
        //        await _fileRepository.DownloadFileToStream(fileId, stream);
        //        string actualContent = Encoding.UTF8.GetString(stream.ToArray());
        //        actualContent.Should().Be(expectedContent);
        //    }
        //}
    }
}