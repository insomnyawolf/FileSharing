using FileSharing.Middleware.CustomExceptions;
using System.IO;

namespace FileSharing.Helpers
{
    public static class FileSystemProtectionHelper
    {
        private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();
        private static readonly string InvalidFileNameCharsString = string.Join(' ', InvalidFileNameChars);

        public static string SanitizePathString(string path)
        {
            // Prevents Directory Escalation
            const string dangerousString = "../";
            const string safeString = "./";
            while (path.Contains(dangerousString))
            {
                path = path.Replace(dangerousString, safeString);
            }
            return path;
        }

#warning optimize this maybe?
        // Return value and assign may not be needed ?
        public static string ValidateFileName(string path)
        {
            // Test Invalid Characters
            if (path.IndexOfAny(InvalidFileNameChars) != -1)
            {
                throw new BadRequestException($"The file '{path}' contains any of the following invalid characters => {InvalidFileNameCharsString}");
            }

            return path;
        }
    }
}
