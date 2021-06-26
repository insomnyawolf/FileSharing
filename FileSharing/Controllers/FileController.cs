using FileSharing.Configuration;
using FileSharing.Helpers;
using FileSharing.Middleware.CustomExceptions;
using FileSharing.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FileSharing.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class FileController : BaseController
    {
        private static readonly FileExtensionContentTypeProvider FileExtensionContentTypeProvider = InitializeFileExtensionContentTypeProvider();

        private static FileExtensionContentTypeProvider InitializeFileExtensionContentTypeProvider()
        {
            var FileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
            FileExtensionContentTypeProvider.Mappings.Add(".flac", "audio/flac");
            return FileExtensionContentTypeProvider;
        }

        private readonly string FilePath = ConfigurationManager.AppSettings.FilePath;
        private readonly int MaxResults = ConfigurationManager.AppSettings.MaxResultsInIndex;

        public FileController(ILogger<FileController> Logger) : base(Logger)
        {

        }

        [HttpGet(nameof(Info))]
        public PagedResult<SharedFileInfo> Info(string fileName = null, int page = 1)
        {
            if (page < 1)
            {
                throw new BadRequestException($"'{nameof(page)}' should be greater than 0.");
            }


            var info = new DirectoryInfo(FilePath);

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = string.Empty;
            }
            else
            {
                fileName = FileSystemProtection.ValidateFileName(fileName);
                fileName = '*' + fileName + '*';
            }
            var files = info.GetFiles(fileName);

            // Sort by creation-time descending
            Array.Sort(files, (FileInfo f1, FileInfo f2) =>
            {
                return f2.CreationTime.CompareTo(f1.CreationTime);
            });

            // // Works Well For Linq 2 SQL Queries, for in memory data look below 

            //var fileinfo = files.Select(file => new SharedFileInfo
            //{
            //    Name = file.Name,
            //    Date = file.CreationTimeUtc,
            //    Size = file.Length,
            //    ContentType = GetContentType(file.Name),
            //});

            //return new PagedResultHelper<SharedFileInfo>(content: fileinfo, totalResults: files.Length, resultsPerPage: MaxResults, pageRequested: page);

            // This version is more manual but avoid allocations where they aren't needed in that case (everything is already on memory)

            page--;

            var startingItem = MaxResults * page;
            var endingItem = startingItem + MaxResults;

            var items = new List<SharedFileInfo>();

            for (int i = startingItem; i < files.Length && i < endingItem; i++)
            {
                items.Add(new SharedFileInfo
                {
                    Name = files[i].Name,
                    Date = files[i].CreationTimeUtc,
                    Size = files[i].Length,
                    ContentType = GetContentType(files[i].Name),
                });
            }

            return new PagedResult<SharedFileInfo>(files.Length, MaxResults)
            {
                Content = items,
            };
        }

        [HttpGet(nameof(Download))]
        public IActionResult Download([FromQuery] string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new BadRequestException($"'{nameof(fileName)}' can not be empty.");
            }

            fileName = FileSystemProtection.ValidateFileName(fileName);

            var destination = Path.Combine(FilePath, fileName);

            if (!System.IO.File.Exists(destination))
            {
                return NotFound();
            }

            var fileStream = System.IO.File.Open(destination, FileMode.Open, FileAccess.Read, FileShare.Read);

            // returns a FileStreamResult without loading the whole file into memory
            return File(fileStream: fileStream, contentType: GetContentType(fileName), fileDownloadName: fileName, enableRangeProcessing: true);
        }

        private static string GetContentType(string filename)
        {
            if (!FileExtensionContentTypeProvider.TryGetContentType(filename, out string contentType))
            {
                return "application/octet-stream";
            }
            return contentType;
        }

        [HttpPost(nameof(Upload))]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            var formFiles = Request.Form?.Files;

            for (int i = 0; i < formFiles?.Count; i++)
            {
                var formFile = formFiles[i];

                if (formFile.Length > 0)
                {
                    var fileName = FileSystemProtection.ValidateFileName(formFile.FileName);

                    string dest = Path.Combine(FilePath, fileName);

                    if (System.IO.File.Exists(dest))
                    {
                        System.IO.File.Delete(dest);
                    }
                    else if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }

                    using var stream = new FileStream(dest, FileMode.Create);
                    await formFile.CopyToAsync(stream).ConfigureAwait(false);
                }
            }

            return Ok();
        }
    }
}
