using System;
using Webao.Test.Dto;

namespace Webao.Test
{
    public class YoutubeTrendingVideoMockRequest : MockRequest
    {
        private string path = "videos?part=snippet&chart=mostPopular&regionCode=";
        private Type dest = typeof(DtoGeoTrendingVideos);

        public YoutubeTrendingVideoMockRequest(params String[] validRequests)
        {
            this.Path = path;
            this.Dest = dest;
            req.BaseUrl("https://www.googleapis.com/youtube/v3/");
            req.AddParameter("format", "json");
            req.AddParameter("key", "AIzaSyAYPektgPnDjZoJ_VF2-6xLoIj_edsitHg");
            AddResponses(validRequests);
        }
    }
}
