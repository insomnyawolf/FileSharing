using Microsoft.AspNetCore.StaticFiles;

namespace FileSharing.Helpers
{
    public static class ContentTypeHelper
    {
        private static readonly FileExtensionContentTypeProvider FileExtensionContentTypeProvider = InitializeFileExtensionContentTypeProvider();

        private static FileExtensionContentTypeProvider InitializeFileExtensionContentTypeProvider()
        {
            var FileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
            FileExtensionContentTypeProvider.Mappings.Add(".flac", "audio/flac");
            return FileExtensionContentTypeProvider;
        }

        public static string GetString(string filename)
        {
            if (!FileExtensionContentTypeProvider.TryGetContentType(filename, out string contentType))
            {
                return "application/octet-stream";
            }
            return contentType;
        }
    }
}
