using System.Collections.Generic;
using Webao.Attributes;
using Webao.Test.Dto;

namespace Webao.Test.WebaoDynamic
{
    [BaseUrl("http://ws.audioscrobbler.com/2.0/")]
    [AddParameterAttribute("format", "json")]
    [AddParameterAttribute("api_key", "0d334a19899cbe651d961d8ae8825b36")]
    public interface IWebaoDynTrack
    {
        [Get("?method=geo.gettoptracks&country={country}&page={page}&limit=20")]
        //[Mapping(typeof(DtoGeoTopTracks), ".Tracks.Track")]
        [Mapping(typeof(DtoGeoTopTracks), With = "Webao.Test.Dto.DtoGeoTopTracks.GetTrackList")]
        List<Track> GeoGetTopTracks(string country, int page);
    }
}
