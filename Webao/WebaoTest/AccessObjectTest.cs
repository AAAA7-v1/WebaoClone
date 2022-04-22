using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webao.Test.Dto;

namespace Webao.Test
{
    [TestClass]
    public class AccessObjectTest
    {
        static readonly WebaoArtist artistWebao = (WebaoArtist)WebaoBuilder.Build(typeof(WebaoArtist), new HttpRequest());
        static readonly WebaoTrack trackWebao = (WebaoTrack)WebaoBuilder.Build(typeof(WebaoTrack), new HttpRequest());

        static readonly WebaoVideo videoWebao = (WebaoVideo)WebaoBuilder.Build(typeof(WebaoVideo), new HttpRequest());
        static readonly WebaoChannel channelWebao = (WebaoChannel)WebaoBuilder.Build(typeof(WebaoChannel), new HttpRequest());

        [TestMethod]
        public void TestWebaoArtistGetInfo()
        {

            Artist artist = artistWebao.GetInfo("muse");
            Assert.AreEqual("Muse", artist.Name);
            Assert.AreEqual("fd857293-5ab8-40de-b29e-55a69d4e4d0f", artist.Mbid);
            Assert.AreEqual("https://www.last.fm/music/Muse", artist.Url);
            Assert.AreNotEqual(0, artist.Stats.Listeners);
            Assert.AreNotEqual(0, artist.Stats.Playcount);
        }

        [TestMethod]
        public void TestWebaoArtistSearch()
        {
            List<Artist> artists = artistWebao.Search("black");
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        [TestMethod]
        public void TestWebaoTrackGeoGetTopTracks()
        {
            List<Track> tracks = trackWebao.GeoGetTopTracks("australia", 1);
            Assert.AreEqual("The Less I Know the Better", tracks[0].Name);
            Assert.AreEqual("Mr. Brightside", tracks[1].Name);
            Assert.AreEqual("The Killers", tracks[1].Artist.Name);
        }

        [TestMethod]
        public void TestWebaoVideoGeoGetTrendingVideos() // Might fail if trending video list has changed.
        {
            List<Video> videos1 = videoWebao.GeoGetTrendingVideos("PT");
            Assert.AreEqual("Highlights | Resumo: Famalicão 2-1 FC Porto (Liga 19/20 #25)", videos1[0].Snippet.Title);
            Assert.AreEqual("Nelson Freitas - Dpos d' Quarentena", videos1[2].Snippet.Title);

            List<Video> videos2 = videoWebao.GeoGetTrendingVideos("US");
            Assert.AreEqual("Video at Buffalo protest shows police pushing 75-year-old man", videos2[0].Snippet.Title);
        }

        [TestMethod]
        public void TestWebaoChannelGetChannelInfo()
        {
            Channel[] channel1 = channelWebao.GetChannelInfo("PewDiePie");
            Assert.AreEqual("I make videos.", channel1[0].Snippet.Description);
            Assert.AreEqual("2010-04-29T10:54:00Z", channel1[0].Snippet.PublishedAt);

            Channel[] channel2 = channelWebao.GetChannelInfo("portadosfundos");
            Assert.AreEqual("Porta dos Fundos", channel2[0].Snippet.Title);
            Assert.AreEqual("Esse é o canal Porta dos Fundos. Lançamos vídeos originais, inéditos e exclusivos sempre às segundas, quintas e sábados às 11h. Inscrevam-se e assistam. :-)\n\nCONTATO: contato@portadosfundos.com.br", channel2[0].Snippet.Description);
            Assert.AreEqual("2012-03-12T04:30:27.000Z", channel2[0].Snippet.PublishedAt);
        }

        [TestMethod]
        public void TestWebaoArtistSearchWithPageParameter() 
        {
            List<Artist> artists1 = artistWebao.SearchPage("linkin+park", 1);
            Assert.AreEqual("Linkin Park", artists1[0].Name);
           
            List<Artist> artists2 = artistWebao.SearchPage("linkin+park", 2);
            Assert.AreEqual("Linkin Park/JayZ", artists2[0].Name);
            Assert.AreEqual("", artists2[0].Mbid);
            Assert.AreEqual("Linkin Park f.  Godsmack, Disturbed, Pantera, Limp  Bizkit, Tool, Staind, Korn -", artists2[5].Name);
            Assert.AreEqual("", artists2[5].Mbid);

            List<Artist> artists3 = artistWebao.SearchPage("muse", 12);
            Assert.AreEqual("Mouse Trap", artists3[2].Name);
            Assert.AreEqual("", artists3[2].Mbid);

        }

    }
}
