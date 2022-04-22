using System.Collections.Generic;
using System.Linq;
using Webao.Test.Dto;
using Webao.Test.WebaoDynamic;

namespace Webao.Test
{
    public class ServiceTracks
    {
        private readonly IWebaoDynTrack webao;
        
        public ServiceTracks() : this(new HttpRequest()) { }
        
        public ServiceTracks(IRequest req) {

            this.webao = (IWebaoDynTrack)WebaoDynBuilder.Build(typeof(IWebaoDynTrack), req);
        }
        
        public IEnumerable<Track> TopTracksFrom(string country) {
            List<Track> tracks;
            int page_nr = 1;
            do
            {
                tracks = webao.GeoGetTopTracks(country, page_nr++);
                foreach (Track track in tracks) yield return track;
            }
            while (tracks.Any());     
        }
    }
}
