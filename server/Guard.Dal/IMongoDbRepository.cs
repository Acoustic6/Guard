using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Guard.Dal
{
    public interface IMongoDbRepository<T>
    {
        Task DeleteFileAsync(FilterDefinition<GridFSFileInfo> filter);
        Task DeleteFileByIdAsync(ObjectId id);
        Task<DeleteResult> DeleteManyAsync(Expression<Func<T, bool>> filter);
        Task<DeleteResult> DeleteOneAsync(Expression<Func<T, bool>> filter);
        Task<byte[]> DownloadFileAsync(ObjectId id);
        Task DownloadFileAsync(ObjectId id, string destination);
        Task<byte[]> DownloadFileAsync(string source);
        Task DownloadFileAsync(string source, string destination);
        Task<ICollection<T>> FilterAsync(Expression<Func<T, bool>> filter);
        Task<IAsyncCursor<GridFSFileInfo>> FindFileAsync(FilterDefinition<GridFSFileInfo> filter);
        Task SaveAsync(T entity);
        Task SaveManyAsync(ICollection<T> entities);
        Task<ObjectId> UpdateFileAsync(byte[] source, string filename);
        Task<ObjectId> UpdateFileAsync(string source, string filename);
        Task<UpdateResult> UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null);
        Task<UpdateResult> UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null);
        Task<ObjectId> UploadFileAsync(byte[] source, string destination);
        Task<ObjectId> UploadFileAsync(string source, string destination);
    }
}