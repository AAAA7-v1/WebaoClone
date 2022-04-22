using System.Collections.Generic;
using Webao.Test.Dto;

namespace Webao.Test.WebaoDynamic
{
    public interface IWebaoDynTrackFluent
    {
        List<Track> GeoGetTopTracks(string country, int page);
    }
}
