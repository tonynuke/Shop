using System.IO;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace DataAccess.Repositories
{
    /// <summary>
    /// File repository interface.
    /// </summary>
    public interface IFileRepository
    {
        /// <summary>
        /// Uploads a file.
        /// </summary>
        /// <param name="filename">Filename.</param>
        /// <param name="stream">Stream.</param>
        /// <returns>File id.</returns>
        Task<ObjectId> UploadFromStream(string filename, Stream stream);

        /// <summary>
        /// Downloads a file to stream.
        /// </summary>
        /// <param name="fileId">File id.</param>
        /// <param name="stream">Stream.</param>
        /// <returns>Asynchronous operation.</returns>
        Task DownloadFileToStream(ObjectId fileId, Stream stream);

        /// <summary>
        /// Deletes file.
        /// </summary>
        /// <param name="fileId">File id.</param>
        /// <returns>Asynchronous operation.</returns>
        Task Delete(ObjectId fileId);
    }
}