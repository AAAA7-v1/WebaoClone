using System.Collections.Generic;
using Webao.Test.Dto;

namespace Webao.Test.Dto
{
    public class DtoSearch
    {
        public DtoResults Results { get; set; }

        //added for TP3
        public List<Artist> GetArtistsList() {
            return this.Results.ArtistMatches.Artist;
        }

    }

    public class DtoResults
    {
        public DtoArtistMatches ArtistMatches { get; set; }
    }

    public class DtoArtistMatches
    {
        public List<Artist> Artist { get; set; }
    }
}
