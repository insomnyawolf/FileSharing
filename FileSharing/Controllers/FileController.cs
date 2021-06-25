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

namespace Capitales.Controllers
{
    [ApiController]
    [Route("api/[controller]/")]
    public class FileController : BaseController
    {
        private static readonly FileExtensionContentTypeProvider FileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();
        private static readonly string InvalidFileNameCharsString = string.Join(' ', InvalidFileNameChars);
        private readonly string FilePath;
        private readonly int MaxResults;

        public FileController(ILogger<FileController> Logger) : base(Logger)
        {
            FilePath = ConfigurationManager.AppSettings.FilePath;
            MaxResults = ConfigurationManager.AppSettings.MaxResultsInIndex;
        }

        [HttpGet(nameof(Info))]
        public PagedResult<SharedFileInfo> Info(string filename = null, int page = 1)
        {
            if (page < 1)
            {
                throw new BadRequestException($"'{nameof(page)}' should be greater than 0.");
            }

            var info = new DirectoryInfo(FilePath);

            if (string.IsNullOrEmpty(filename))
            {
                filename = string.Empty;
            }
            else
            {
                filename = '*' + filename + '*';
            }
            var files = info.GetFiles(filename);

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

        [HttpGet(nameof(Get))]
        public IActionResult Get([FromQuery] string filename = null)
        {
            if (string.IsNullOrEmpty(filename))
            {
                throw new BadRequestException($"'{nameof(filename)}' can not be empty.");
            }

            var destination = Path.Combine(FilePath, filename);

            if (!System.IO.File.Exists(destination))
            {
                return NotFound();
            }

            var fileStream = System.IO.File.Open(destination, FileMode.Open);

            return File(fileStream: fileStream, contentType: GetContentType(filename), enableRangeProcessing: true/*, fileDownloadName: filename*/); // returns a FileStreamResult
        }

        private static string GetContentType(string filename)
        {
            FileExtensionContentTypeProvider.TryGetContentType(filename, out string contentType);
            return contentType ?? "application/octet-stream";
        }

        [HttpPost(nameof(Upload))]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload()
        {
            var formFiles = Request.Form?.Files;

            for (int i = 0; i < formFiles?.Count; i++)
            {
                var formFile = formFiles[i];

#warning test this
                if (formFile.FileName.IndexOfAny(InvalidFileNameChars) != -1)
                {
                    throw new BadRequestException($"The file '{formFile.FileName}' contains any of the following invalid characters => {InvalidFileNameCharsString}");
                }

                if (formFile.Length > 0)
                {
                    string dest = Path.Combine(FilePath, formFile.FileName);

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
