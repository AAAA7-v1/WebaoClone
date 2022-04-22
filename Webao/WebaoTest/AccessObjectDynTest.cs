using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Webao.Test.Dto;
using Webao.Test.WebaoDynamic;

namespace Webao.Test
{
    [TestClass]
    public class AccessObjectDynTest
    {
        // Custom Attribute
        static readonly IWebaoDynArtist artistDynWebao = (IWebaoDynArtist)WebaoDynBuilder.Build(typeof(IWebaoDynArtist), new HttpRequest());
        static readonly IWebaoDynTrack trackDynWebao = (IWebaoDynTrack)WebaoDynBuilder.Build(typeof(IWebaoDynTrack), new HttpRequest());

        static readonly IWebaoDynVideo videoDynWebao = (IWebaoDynVideo)WebaoDynBuilder.Build(typeof(IWebaoDynVideo), new HttpRequest());
        static readonly IWebaoDynChannel channelDynWebao = (IWebaoDynChannel)WebaoDynBuilder.Build(typeof(IWebaoDynChannel), new HttpRequest());

        // Fluent
        static readonly IWebaoDynArtistFluent artistDynWebaoFluent
            = WebaoDynBuilder
            .For<IWebaoDynArtistFluent>("http://ws.audioscrobbler.com/2.0/")
            .AddParameter("format", "json")
            .AddParameter("api_key", "0d334a19899cbe651d961d8ae8825b36")
            .On("GetInfo")
            .GetFrom("?method=artist.getinfo&artist={name}")
            .Mapping<DtoArtist>(dto => dto.Artist)//.Mapping<DtoArtist>(AccessObjectDynTest.mapDtoArtist)
            .On("Search")
            .GetFrom("?method=artist.search&artist={name}&page={page}")
            .Mapping<DtoSearch>(dto => dto.Results.ArtistMatches.Artist)//.Mapping<DtoSearch>(AccessObjectDynTest.mapDtoSearch)/
            .Build(new HttpRequest());

        static readonly IWebaoDynChannelFluent channelDynWebaoFluent
            = WebaoDynBuilder
            .For<IWebaoDynChannelFluent>("https://www.googleapis.com/youtube/v3/")
            .AddParameter("format", "json")
            .AddParameter("key", "AIzaSyAYPektgPnDjZoJ_VF2-6xLoIj_edsitHg")
            .On("GetChannelInfo")
            .GetFrom("channels?part=snippet&forUsername={Uname}")
            .Mapping<DtoChannelInfo>(dto => dto.Items)//.Mapping<DtoChannelInfo>(AccessObjectDynTest.mapDtoItems)
            .Build(new HttpRequest());

        static readonly IWebaoDynTrackFluent trackDynWebaoFluent
            = WebaoDynBuilder
            .For<IWebaoDynTrackFluent>("http://ws.audioscrobbler.com/2.0/")
            .AddParameter("format", "json")
            .AddParameter("api_key", "0d334a19899cbe651d961d8ae8825b36")
            .On("GeoGetTopTracks")
            .GetFrom("?method=geo.gettoptracks&country={country}&page={page}")
            .Mapping<DtoGeoTopTracks>(dto => dto.Tracks.Track)//.Mapping<DtoGeoTopTracks>(AccessObjectDynTest.mapDtoItems)
            .Build(new HttpRequest());

        static readonly IWebaoDynVideoFluent videoDynWebaoFluent
            = WebaoDynBuilder
            .For<IWebaoDynVideoFluent>("https://www.googleapis.com/youtube/v3/")
            .AddParameter("format", "json")
            .AddParameter("key", "AIzaSyAYPektgPnDjZoJ_VF2-6xLoIj_edsitHg")
            .On("GeoGetTrendingVideos")
            .GetFrom("videos?part=snippet&chart=mostPopular&regionCode={rcode}")
            .Mapping<DtoGeoTrendingVideos>(dto => dto.Items)//.Mapping<DtoGeoTrendingVideos>(AccessObjectDynTest.mapDtoItems)
            .Build(new HttpRequest());

        // Artist
        [TestMethod]
        public void TestWebaoArtistGetInfo()
        {
            Artist artist = artistDynWebao.GetInfo("muse");
            Assert.AreEqual("Muse", artist.Name);
            Assert.AreEqual("fd857293-5ab8-40de-b29e-55a69d4e4d0f", artist.Mbid);
            Assert.AreEqual("https://www.last.fm/music/Muse", artist.Url);
            Assert.AreNotEqual(0, artist.Stats.Listeners);
            Assert.AreNotEqual(0, artist.Stats.Playcount);
        }

        [TestMethod]
        public void TestWebaoArtistSearch()
        {
            List<Artist> artists = artistDynWebao.SearchPage("black", 1);
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        // Fluent Artist
        [TestMethod]
        public void TestWebaoArtistFluentGetInfo()
        {
            Artist artist = artistDynWebaoFluent.GetInfo("muse");
            Assert.AreEqual("Muse", artist.Name);
            Assert.AreEqual("fd857293-5ab8-40de-b29e-55a69d4e4d0f", artist.Mbid);
            Assert.AreEqual("https://www.last.fm/music/Muse", artist.Url);
            Assert.AreNotEqual(0, artist.Stats.Listeners);
            Assert.AreNotEqual(0, artist.Stats.Playcount);
        }

        [TestMethod]
        public void TestWebaoArtistFluentSearch()
        {
            List<Artist> artists = artistDynWebaoFluent.Search("black", 1);
            Assert.AreEqual("Black Sabbath", artists[1].Name);
            Assert.AreEqual("Black Eyed Peas", artists[2].Name);
        }

        // Tracks
        [TestMethod]
        public void TestWebaoTrackGeoGetTopTracks()
        {
            List<Track> tracks = trackDynWebao.GeoGetTopTracks("australia",1);
            Assert.AreEqual("The Less I Know the Better", tracks[0].Name);
            Assert.AreEqual("Mr. Brightside", tracks[1].Name);
            Assert.AreEqual("The Killers", tracks[1].Artist.Name);
        }

        // Tracks Fluent
        [TestMethod]
        public void TestWebaoTrackFluentGeoGetTopTracks()
        {
            List<Track> tracks = trackDynWebaoFluent.GeoGetTopTracks("australia", 1);
            Assert.AreEqual("The Less I Know the Better", tracks[0].Name);
            Assert.AreEqual("Mr. Brightside", tracks[1].Name);
            Assert.AreEqual("The Killers", tracks[1].Artist.Name);
        }
        //Video
        [TestMethod]
        public void TestWebaoVideoGeoGetTrendingVideos() // Might fail if trending video list has changed.
        {
            List<Video> videos1 = videoDynWebao.GeoGetTrendingVideos("PT");
            Assert.AreEqual("Highlights | Resumo: Famalicão 2-1 FC Porto (Liga 19/20 #25)", videos1[0].Snippet.Title);
            Assert.AreEqual("Nelson Freitas - Dpos d' Quarentena", videos1[2].Snippet.Title);

            List<Video> videos2 = videoDynWebao.GeoGetTrendingVideos("US");
            Assert.AreEqual("Video at Buffalo protest shows police pushing 75-year-old man", videos2[0].Snippet.Title);
        }

        // Video Fluent
        [TestMethod]
        public void TestWebaoVideoFluentGeoGetTrendingVideos() // Might fail if trending video list has changed.
        {
            List<Video> videos1 = videoDynWebaoFluent.GeoGetTrendingVideos("PT");
            Assert.AreEqual("Highlights | Resumo: Famalicão 2-1 FC Porto (Liga 19/20 #25)", videos1[0].Snippet.Title);
            Assert.AreEqual("Nelson Freitas - Dpos d' Quarentena", videos1[2].Snippet.Title);

            List<Video> videos2 = videoDynWebaoFluent.GeoGetTrendingVideos("US");
            Assert.AreEqual("Video at Buffalo protest shows police pushing 75-year-old man", videos2[0].Snippet.Title);
        }

        //Channel
        [TestMethod]
        public void TestWebaoChannelGetChannelInfo()
        {
            Channel[] channel1 = channelDynWebao.GetChannelInfo("PewDiePie");
            Assert.AreEqual("I make videos.", channel1[0].Snippet.Description);
            Assert.AreEqual("2010-04-29T10:54:00Z", channel1[0].Snippet.PublishedAt);

            Channel[] channel2 = channelDynWebao.GetChannelInfo("portadosfundos");
            Assert.AreEqual("Porta dos Fundos", channel2[0].Snippet.Title);
            Assert.AreEqual("Esse é o canal Porta dos Fundos. Lançamos vídeos originais, inéditos e exclusivos sempre às segundas, quintas e sábados às 11h. Inscrevam-se e assistam. :-)\n\nCONTATO: contato@portadosfundos.com.br", channel2[0].Snippet.Description);
            Assert.AreEqual("2012-03-12T04:30:27Z", channel2[0].Snippet.PublishedAt);
        }

        //Channel Fluent
        [TestMethod]
        public void TestWebaoChannelFluentGetChannelInfo()
        {
            Channel[] channel1 = channelDynWebaoFluent.GetChannelInfo("PewDiePie");
            Assert.AreEqual("I make videos.", channel1[0].Snippet.Description);
            Assert.AreEqual("2010-04-29T10:54:00Z", channel1[0].Snippet.PublishedAt);

            Channel[] channel2 = channelDynWebaoFluent.GetChannelInfo("portadosfundos");
            Assert.AreEqual("Porta dos Fundos", channel2[0].Snippet.Title);
            Assert.AreEqual("Esse é o canal Porta dos Fundos. Lançamos vídeos originais, inéditos e exclusivos sempre às segundas, quintas e sábados às 11h. Inscrevam-se e assistam. :-)\n\nCONTATO: contato@portadosfundos.com.br", channel2[0].Snippet.Description);
            Assert.AreEqual("2012-03-12T04:30:27Z", channel2[0].Snippet.PublishedAt);
        }
        /*[TestMethod]
        public void TestWebaoArtistSearchWithPageParameter()
        {
            List<Artist> artists1 = artistDynWebao.SearchPage("linkin+park", 1);
            Assert.AreEqual("Linkin Park", artists1[0].Name);

            List<Artist> artists2 = artistDynWebao.SearchPage("linkin+park", 2);
            Assert.AreEqual("Jay Z/Linkin Park", artists2[0].Name);
            Assert.AreEqual("", artists2[0].Mbid);
            Assert.AreEqual("Steve Aoki feat. Linkin Park", artists2[5].Name);
            Assert.AreEqual("", artists2[5].Mbid);

            List<Artist> artists3 = artistDynWebao.SearchPage("muse", 12);
            Assert.AreEqual("Musae", artists3[2].Name);
            Assert.AreEqual("", artists3[2].Mbid);

        }
        */

        public static object mapDtoArtist(DtoArtist dto)
        {
            return dto.Artist;
        }

        public static object mapDtoSearch(DtoSearch dto)
        {
            return dto.Results.ArtistMatches.Artist;
        }
    }
}
