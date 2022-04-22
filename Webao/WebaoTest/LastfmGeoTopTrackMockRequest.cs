using System;
using Webao.Test.Dto;

namespace Webao.Test
{
    public class LastfmGeoTopTrackMockRequest : MockRequest
    {
        private string path = "?method=geo.gettoptracks&country=";
        private Type dest = typeof(DtoGeoTopTracks);

        public LastfmGeoTopTrackMockRequest(params String[] validRequests)
        {
            this.Path = path;
            this.Dest = dest;
            req.BaseUrl("http://ws.audioscrobbler.com/2.0/");
            req.AddParameter("format", "json");
            req.AddParameter("api_key", "0d334a19899cbe651d961d8ae8825b36");
            AddResponses(validRequests);
        }
    }
}
