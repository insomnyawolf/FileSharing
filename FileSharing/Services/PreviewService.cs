﻿using FileSharing.Configuration;
using FileSharing.Helpers;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace FileSharing.Services
{
    public static class PreviewService
    {
        public static bool TryGetPreview(string path, string contentType, out string previewPath, out string previewContentType)
        {
            contentType = contentType.Split('/', StringSplitOptions.RemoveEmptyEntries)[0].ToLower();

            var fileInfo = new FileInfo(path);

            var cacheSettings = ConfigurationManager.AppSettings.Cache;

            if (fileInfo.Length < cacheSettings.PreviewMinSize)
            {
                // Won't make caché for small files
                previewPath = path;
                previewContentType = contentType;
                return true;
            }

            if (string.IsNullOrEmpty(cacheSettings.PreviewPath))
            {
                cacheSettings.PreviewPath = Path.Combine(fileInfo.DirectoryName, ".prev");
            }

            if (!Directory.Exists(cacheSettings.PreviewPath))
            {
                Directory.CreateDirectory(cacheSettings.PreviewPath);
            }

            switch (contentType)
            {
                case "image":
                    previewPath = GetCacheName(cacheSettings, fileInfo.Name, "png");
                    if (!File.Exists(previewPath))
                    {
                        GenerateImagePreview(fileInfo.FullName, previewPath, cacheSettings);
                    }
                    break;
                // For now previews will only work with images
                default:
                    previewPath = null;
                    previewContentType = null;
                    return false;
            }

            previewContentType = ContentTypeHelper.GetString(previewPath);
            return true;
        }

        private static string GetCacheName(CacheSettings settings, string filename, string extension)
        {
            var filenameWitoutExtension = Path.GetFileNameWithoutExtension(filename);
            return Path.Combine(settings.PreviewPath, $"{filenameWitoutExtension}.{extension}");
        }

        private static void GenerateImagePreview(string originFilePath, string targetPath, CacheSettings settings)
        {
            using var image = new Bitmap(originFilePath);

            var maxSize = new Size
            {
                Height = settings.Image.MaxHeight,
                Width = settings.Image.MaxWidth,
            };

            using var result = ImageResizer.ResizeImage(image, maxSize, settings.Image.Upscale);

#warning To Do Investigate encoder settings to achive greater compression
            result.Save(targetPath, ImageFormat.Png);
        }
    }
}
