using System.Collections.Generic;
using Webao.Attributes;
using Webao.Test.Dto;

namespace Webao.Test
{
[BaseUrl("http://ws.audioscrobbler.com/2.0/")]
[AddParameter("format", "json")]
[AddParameter("api_key", "0d334a19899cbe651d961d8ae8825b36")]
public class WebaoArtist : AbstractAccessObject
{
    public WebaoArtist(IRequest req) : base(req) {}

    [Get("?method=artist.getinfo&artist={name}")]
    [Mapping(typeof(DtoArtist), ".Artist")]
    public Artist GetInfo(string name) => (Artist)Request(name); 
        

    [Get("?method=artist.search&artist={name}")]
    [Mapping(typeof(DtoSearch), ".Results.ArtistMatches.Artist")]
    public List<Artist> Search(string name) => (List<Artist>)Request(name);

    [Get("?method=artist.search&artist={name}&page={page}&limit=20")]
    [Mapping(typeof(DtoSearch), ".Results.ArtistMatches.Artist")]
    public List<Artist> SearchPage(string name, int page) => (List<Artist>)Request(name, page);
    

    }
}
