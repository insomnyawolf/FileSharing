namespace FileSharing.Configuration
{
    public class AppSettings
    {
        public string FilePath { get; set; }
        public int MaxResultsPerPage { get; set; }
        public CacheSettings Cache { get; set; }
    }

    public class CacheSettings
    {
        public string PreviewPath { get; set; }
        public long PreviewMinSize { get; set; }
        public ImageCacheSettings Image { get; set; }
    }

    public class ImageCacheSettings
    {
        public int MaxHeight { get; set; }
        public int MaxWidth { get; set; }
        public bool Upscale { get; set; }
        public int MaxColors { get; set; }
    }
}
