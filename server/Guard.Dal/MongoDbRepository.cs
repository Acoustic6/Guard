using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Guard.Domain.Repositories;

namespace Guard.Dal
{
    public class MongoDBRepository<T> : IMongoDBRepository<T>
    {
        private readonly string _dbName;
        private readonly MongoDBContext _dBContext;

        public MongoDBRepository(string dbName, MongoDBContext dBContext)
        {
            _dbName = dbName;
            _dBContext = dBContext;
        }

        public async Task SaveManyAsync(ICollection<T> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            await GetCollection().InsertManyAsync(entities);
        }

        public async Task SaveAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await GetCollection().InsertOneAsync(entity);
        }

        public async Task<ICollection<T>> FilterAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return await GetCollection().Find(filter).ToListAsync();
        }

        public async Task<UpdateResult> UpdateOneAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (update == null) throw new ArgumentNullException(nameof(update));

            return await GetCollection().UpdateOneAsync(filter, update, options);
        }

        public async Task<UpdateResult> UpdateManyAsync(Expression<Func<T, bool>> filter, UpdateDefinition<T> update, UpdateOptions options = null)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));
            if (update == null) throw new ArgumentNullException(nameof(update));

            return await GetCollection().UpdateManyAsync(filter, update, options);
        }

        public async Task<DeleteResult> DeleteOneAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return await GetCollection().DeleteOneAsync(filter);
        }

        public async Task<DeleteResult> DeleteManyAsync(Expression<Func<T, bool>> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return await GetCollection().DeleteManyAsync(filter);
        }

        public async Task<ObjectId> UploadFileAsync(string source, string destination)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            using (var fs = new FileStream(source, FileMode.Open))
            {
                return await _dBContext.GridFs.UploadFromStreamAsync(destination, fs);
            }
        }

        public async Task<ObjectId> UploadFileAsync(byte[] source, string destination)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            return await _dBContext.GridFs.UploadFromBytesAsync(destination, source);
        }

        public async Task DownloadFileAsync(string source, string destination)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            using (Stream fs = new FileStream(destination, FileMode.OpenOrCreate))
            {
                await _dBContext.GridFs.DownloadToStreamByNameAsync(source, fs);
            }
        }

        public async Task DownloadFileAsync(ObjectId id, string destination)
        {
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            using (Stream fs = new FileStream(destination, FileMode.OpenOrCreate))
            {
                await _dBContext.GridFs.DownloadToStreamAsync(id, fs);
            }
        }

        public async Task<byte[]> DownloadFileAsync(string source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return await _dBContext.GridFs.DownloadAsBytesByNameAsync(source);
        }

        public async Task<byte[]> DownloadFileAsync(ObjectId id)
        {
            return await _dBContext.GridFs.DownloadAsBytesAsync(id);
        }

        public async Task<IAsyncCursor<GridFSFileInfo>> FindFileAsync(FilterDefinition<GridFSFileInfo> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            return await _dBContext.GridFs.FindAsync(filter);
        }

        public async Task<ObjectId> UpdateFileAsync(string source, string filename)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            using (Stream fs = new FileStream(source, FileMode.Open))
            {
                return await _dBContext.GridFs.UploadFromStreamAsync(
                    filename,
                    fs,
                    new GridFSUploadOptions {Metadata = new BsonDocument("filename", filename)});
            }
        }

        public async Task<ObjectId> UpdateFileAsync(byte[] source, string filename)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (filename == null) throw new ArgumentNullException(nameof(filename));

            return await _dBContext.GridFs.UploadFromBytesAsync(
                filename,
                source,
                new GridFSUploadOptions {Metadata = new BsonDocument("filename", filename)});
        }

        public async Task DeleteFileByIdAsync(ObjectId id)
        {
            await _dBContext.GridFs.DeleteAsync(id);
        }

        public async Task DeleteFileAsync(FilterDefinition<GridFSFileInfo> filter)
        {
            if (filter == null) throw new ArgumentNullException(nameof(filter));

            var fileInfos = await _dBContext.GridFs.FindAsync(filter);
            var fileInfo = fileInfos.FirstOrDefault();

            if (fileInfo != null)
                await _dBContext.GridFs.DeleteAsync(fileInfo.Id);
        }

        private IMongoCollection<T> GetCollection()
        {
            if(_dBContext == null) throw new ArgumentNullException(nameof(_dBContext));

            return _dBContext.Database.GetCollection<T>(_dbName);
        }
    }
}
