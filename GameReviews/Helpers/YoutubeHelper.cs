using System;
using System.Text.RegularExpressions;

namespace GameReviews.Helpers
{
    public static class YouTubeHelper
    {
        public static string? GetYouTubeEmbedUrl(string? videoUrl)
        {
            if (string.IsNullOrWhiteSpace(videoUrl)) return null;

            var match = Regex.Match(videoUrl,
                @"(?:youtu\.be/|youtube\.com/(?:watch\?v=|embed/|v/|shorts/|.*?[?&]v=))([^""&?/ ]{11})",
                RegexOptions.IgnoreCase);

            if (match.Success)
            {
                return $"https://www.youtube.com/embed/{match.Groups[1].Value}";
            }

            return null;
        }
    }
}

