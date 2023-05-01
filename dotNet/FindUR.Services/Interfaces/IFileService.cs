using Microsoft.AspNetCore.Http;
using Sabio.Models;
using Sabio.Models.Domain.File;
using Sabio.Models.Requests;
using Sabio.Models.Requests.File;
using Sabio.Web.Core.Configs;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IFileService
    {
        FileModel GetById(int id);
        public Paged<FileModel> GetFilesByHorseId(int pageIndex, int pageSize, int horseId);
        public Paged<FileModel> GetFilesByPagination(int pageSize, int pageIndex, bool deleteQuery);
        public Paged<FileModel> GetFilesByUser(int userId, int pageSize, int pageIndex, bool deleteQuery);
        public Paged<FileModel> GetFilesByName(string name, int pageSize, int pageIndex, bool deleteQuery);
        public Paged<FileModel> GetFilesByNameAndUser(int userId, string name, int pageSize, int pageIndex, bool deleteQuery);
        public Paged<FileModel> GetFilesByType(int typeId, int pageSize, int pageIndex, bool deleteQuery);
        public Paged<FileModel> GetFilesByTypeAndName(int typeId, string name, int pageSize, int pageIndex, bool deleteQuery);
        public Paged<FileModel> GetFilesByTypeAndCurrentUser(int typeId, int userId, int pageSize, int pageIndex, bool deleteQuery);
        public Paged<FileModel> GetFilesByTypeAndNameAndCurrentUser(int typeId, int userId, string name, int pageSize, int pageIndex, bool deleteQuery);
        void Delete(int id);
        void Update(FileUpdateRequest model);
        List<UploadedFile> UploadFile(List<IFormFile> fileList, AWSStorageConfig _awsStorageConfig);
        
    }
}