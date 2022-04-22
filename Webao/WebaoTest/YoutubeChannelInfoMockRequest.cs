using System;
using Webao.Test.Dto;

namespace Webao.Test
{
    public class YoutubeChannelInfoMockRequest : MockRequest
    {
        private string path = "channels?part=snippet&forUsername=";
        private Type dest = typeof(DtoChannelInfo);

        public YoutubeChannelInfoMockRequest(params String[] validRequests)
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
