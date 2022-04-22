using System.Collections.Generic;
using Webao.Attributes;
using Webao.Test.Dto;

namespace Webao.Test.WebaoDynamic
{
    [BaseUrl("https://www.googleapis.com/youtube/v3/")]
    [AddParameter("format", "json")]
    [AddParameter("key", "AIzaSyAYPektgPnDjZoJ_VF2-6xLoIj_edsitHg")]
    public interface IWebaoDynVideo
    {
        [Get("videos?part=snippet&chart=mostPopular&regionCode={rcode}")]
        [Mapping(typeof(DtoGeoTrendingVideos), ".Items")]
        List<Video> GeoGetTrendingVideos(string regionCode);
    }
}
