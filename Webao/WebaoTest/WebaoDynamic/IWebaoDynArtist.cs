using System.Collections.Generic;
using Webao.Attributes;
using Webao.Test.Dto;

namespace Webao.Test.WebaoDynamic
{
    [BaseUrl("http://ws.audioscrobbler.com/2.0/")]
    [AddParameter("format", "json")]
    [AddParameter("api_key", "0d334a19899cbe651d961d8ae8825b36")]
    public interface IWebaoDynArtist
    {
        [Get("?method=artist.getinfo&artist={name}")]
        [Mapping(typeof(DtoArtist), ".Artist")]
        Artist GetInfo(string name);

        //added for TP3

        [Get("?method=artist.search&artist={name}&page={page}&limit=20")]
        //[Mapping(typeof(DtoSearch), ".Results.ArtistMatches.Artist")]
        [Mapping(typeof(DtoSearch), With = "Webao.Test.Dto.DtoSearch.GetArtistsList")]
        List<Artist> SearchPage(string name, int page); 

        [Get("?method=artist.search&artist={name}")]
        [Mapping(typeof(DtoSearch), ".Results.ArtistMatches.Artist")]
        List<Artist> Search(string name);
    }
}
