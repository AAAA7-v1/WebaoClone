using System.Collections.Generic;
using Webao.Test.Dto;

namespace Webao.Test.WebaoDynamic
{
    public interface IWebaoDynVideoFluent
    {
        List<Video> GeoGetTrendingVideos(string regionCode);
    }
}
