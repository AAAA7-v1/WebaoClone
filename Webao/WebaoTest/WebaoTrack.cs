using System.Collections.Generic;
using Webao.Attributes;
using Webao.Test.Dto;

namespace Webao.Test
{
    [BaseUrl("http://ws.audioscrobbler.com/2.0/")]
    [AddParameterAttribute("format", "json")]
    [AddParameterAttribute("api_key", "0d334a19899cbe651d961d8ae8825b36")]

    public class WebaoTrack : AbstractAccessObject
    {
        public WebaoTrack(IRequest req) : base(req) { }

        [Get("?method=geo.gettoptracks&country={country}&page={page}&limit=20")]
        [Mapping(typeof(DtoGeoTopTracks), ".Tracks.Track")]
        public List<Track> GeoGetTopTracks(string country, int page) {
            return (List<Track>) Request(country, page);
        }

    }
}