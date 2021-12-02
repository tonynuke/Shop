using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Common.MongoDb.Repositories
{
    /// <summary>
    /// File repository.
    /// </summary>
    public abstract class FileRepository : IFileRepository
    {
        private readonly GridFSBucket _gridFsBucket;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileRepository"/> class.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="bucketName">Bucket name.</param>
        protected FileRepository(IMongoDatabase database, string bucketName)
        {
            var gridFsBucketOptions = new GridFSBucketOptions
            {
                BucketName = bucketName,
                ChunkSizeBytes = 255,
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Secondary
            };
            _gridFsBucket = new GridFSBucket(database, gridFsBucketOptions);
        }

        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <param name="stream">Stream.</param>
        /// <returns>File id.</returns>
        public Task<ObjectId> UploadFromStream(string filename, Stream stream)
        {
            return _gridFsBucket.UploadFromStreamAsync(filename, stream);
        }

        /// <summary>
        /// Downloads a file to stream.
        /// </summary>
        /// <param name="fileId">File id.</param>
        /// <param name="stream">Stream.</param>
        /// <returns>Asynchronous operation.</returns>
        public Task DownloadFileToStream(ObjectId fileId, Stream stream)
        {
            return _gridFsBucket.DownloadToStreamAsync(fileId, stream);
        }

        /// <summary>
        /// Deletes file.
        /// </summary>
        /// <param name="fileId">File id.</param>
        /// <returns>Asynchronous operation.</returns>
        public Task Delete(ObjectId fileId)
        {
            return _gridFsBucket.DeleteAsync(fileId);
        }
    }
}