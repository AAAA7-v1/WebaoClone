using System.Collections.Generic;
using Webao.Attributes;
using Webao.Test.Dto;

namespace Webao.Test
{
    [BaseUrl("https://www.googleapis.com/youtube/v3/")]
    [AddParameter("format", "json")]
    [AddParameter("key", "AIzaSyAYPektgPnDjZoJ_VF2-6xLoIj_edsitHg")]
    public class WebaoVideo : AbstractAccessObject
    {
        public WebaoVideo(IRequest req) : base(req) { }


        [Get("videos?part=snippet&chart=mostPopular&regionCode={rcode}")]
        [Mapping(typeof(DtoGeoTrendingVideos), ".Items")]
        public List<Video> GeoGetTrendingVideos(string regionCode) => (List<Video>) Request(regionCode);
    }
}