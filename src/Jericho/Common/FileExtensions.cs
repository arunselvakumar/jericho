namespace Jericho.Common
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public static class FileExtensions
    {
        private static readonly IDictionary<string, string> ImageMimeDictionary = new Dictionary<string, string>
        {
            { ".bmp", "image/bmp" },
            { ".dib", "image/bmp" },
            { ".gif", "image/gif" },
            { ".svg", "image/svg+xml" },
            { ".jpe", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".jpg", "image/jpeg" },
            { ".png", "image/png" },
            { ".pnz", "image/png" }
        };

        public static bool IsImage(this string file)
        {
            Ensure.Argument.NotNull(file, nameof(file));
            
            var extension = Path.GetExtension(file);
            return extension != null && ImageMimeDictionary.ContainsKey(extension.ToLowerInvariant());
        }

        public static bool IsGif(this string file)
        {
            Ensure.Argument.NotNull(file, nameof(file));

            string contentType;
            var extension = Path.GetExtension(file);
            if (extension != null && ImageMimeDictionary.TryGetValue(extension.ToLowerInvariant(), out contentType))
            {
                return contentType == @"image/gif";
            }

            return false;
        }

        public static string GetUniqueFileName(this string file)
        {
            var extension = Path.GetExtension(file);
            var fileName = Path.GetFileNameWithoutExtension(file);
            var randomFileName = Path.GetRandomFileName();
            var randomFileNameWithoutExtension = Path.GetFileNameWithoutExtension(randomFileName);

            var uniqueFileName = $"{randomFileNameWithoutExtension}_{fileName}{extension}";
            return uniqueFileName;
        }
    }
}
