using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Webao.Test.Dto;
using Webao.Test.WebaoDynamic;

namespace Webao.Test
{
    [TestClass]
    public class MockRequestDynTest
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMockArtistInfoRequest()
        {
            IWebaoDynArtist artistDynWebao = (IWebaoDynArtist)WebaoDynBuilder.Build(typeof(IWebaoDynArtist), new LastfmArtistInfoMockRequest("muse", "gorillaz"));

            Artist artist1 = artistDynWebao.GetInfo("muse");
            Assert.AreEqual("Muse", artist1.Name);
            Assert.AreEqual("https://www.last.fm/music/Muse", artist1.Url);

            Artist artist2 = artistDynWebao.GetInfo("gorillaz");
            Assert.AreEqual("Gorillaz", artist2.Name);
            Assert.AreEqual("e21857d5-3256-4547-afb3-4b6ded592596", artist2.Mbid);

            Artist artist3 = artistDynWebao.GetInfo("kendrick+lamar"); // Throws System.ArgumentException
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMockArtistSearchRequest()
        {
            IWebaoDynArtist artistDynWebao = (IWebaoDynArtist)WebaoDynBuilder.Build(typeof(IWebaoDynArtist), new LastfmArtistSearchMockRequest("muse", "eminem"));

            List<Artist> search1 = artistDynWebao.SearchPage("muse", 1);
            Assert.AreEqual("Muse", search1[0].Name);
            Assert.AreEqual("Mouse on Mars", search1[3].Name);

            List<Artist> search2 = artistDynWebao.SearchPage("eminem", 1);
            Assert.AreEqual("Eminem", search2[0].Name);

            List<Artist> search3 = artistDynWebao.SearchPage("linkin+park", 1); // Throws System.ArgumentException

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMockGeoTopTracksRequest()
        {
            IWebaoDynTrack trackDynWebao = (IWebaoDynTrack)WebaoDynBuilder.Build(typeof(IWebaoDynTrack), new LastfmGeoTopTrackMockRequest("australia", "france"));

            List<Track> tracks1 = trackDynWebao.GeoGetTopTracks("australia", 1);
            Assert.AreEqual("The Less I Know the Better", tracks1[0].Name);
            Assert.AreEqual("Mr. Brightside", tracks1[1].Name);
            Assert.AreEqual("The Killers", tracks1[1].Artist.Name);

            List<Track> tracks2 = trackDynWebao.GeoGetTopTracks("france", 1);
            Assert.AreEqual("Smells Like Teen Spirit", tracks2[0].Name);

            List<Track> tracks3 = trackDynWebao.GeoGetTopTracks("portugal", 1); // Throws System.ArgumentAxception

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestMockTrendingVideoRequest() // Might fail if trending video list has changed.
        {
            IWebaoDynVideo videoDynWebao = (IWebaoDynVideo)WebaoDynBuilder.Build(typeof(IWebaoDynVideo), new YoutubeTrendingVideoMockRequest("PT", "US"));

            List<Video> trending1 = videoDynWebao.GeoGetTrendingVideos("PT");
            Assert.AreEqual("Highlights | Resumo: Famalicão 2-1 FC Porto (Liga 19/20 #25)", trending1[0].Snippet.Title);
            Assert.AreEqual("q5xIoeG4uVI", trending1[1].Id);

            List<Video> trending2 = videoDynWebao.GeoGetTrendingVideos("US");
            Assert.AreEqual("Brianna", trending2[2].Snippet.ChannelTitle);

            List<Video> trending3 = videoDynWebao.GeoGetTrendingVideos("IN"); // Throws System.ArgumentException

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]

        public void TestMockChannelInfoRequest()
        {
            IWebaoDynChannel channelDynWebao = (IWebaoDynChannel)WebaoDynBuilder.Build(typeof(IWebaoDynChannel), new YoutubeChannelInfoMockRequest("PewDiePie", "codyko69"));

            Channel[] channels1 = channelDynWebao.GetChannelInfo("PewDiePie");
            Assert.AreEqual("I make videos.", channels1[0].Snippet.Description);
            Assert.AreEqual("2010-04-29T10:54:00.000Z", channels1[0].Snippet.PublishedAt);

            Channel[] channels2 = channelDynWebao.GetChannelInfo("codyko69");
            Assert.AreEqual("Cody Ko", channels2[0].Snippet.Title);
            Assert.AreEqual("UCfp86n--4JvqKbunwSI2lYQ", channels2[0].Id);

            Channel[] channels3 = channelDynWebao.GetChannelInfo("cmbroad44"); // Throws System.ArgumentException
        }

    }
}
