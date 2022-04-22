using System.Collections.Generic;
using System.Linq;
using Webao.Test.Dto;
using Webao.Test.WebaoDynamic;

namespace Webao.Test
{
    public class ServiceArtists
    {
        private readonly IWebaoDynArtist webao;

        public ServiceArtists() : this(new HttpRequest()) { }

        public ServiceArtists(IRequest req)
        {
            this.webao = (IWebaoDynArtist)WebaoDynBuilder.Build(typeof(IWebaoDynArtist), req);
        }

        public IEnumerable<Artist> ArtistSearch(string name)
        {
            List<Artist> artists;
            int page_nr = 1;
            do
            {
                artists = webao.SearchPage(name, page_nr++);
                foreach (Artist artist in artists) yield return artist;
            }
            while (artists.Any());
        }
    }
}
