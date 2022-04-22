using System;
using System.Collections.Generic;
using Webao.Test.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webao.Test.WebaoDynamic;

namespace Webao.Test
{

    [TestClass]
    public class ServiceTracksTest
    {

        //LAZY
        [TestMethod]
        public void TestLazy()
        {

            HttpRequest req = new HttpRequest();
            ServiceTracks service = new ServiceTracks(req);

            IEnumerator<Track> tracks = service.TopTracksFrom("australia").GetEnumerator();

            //Console.WriteLine("NUMBER OF REQUESTS: " + req.GetRequestCounter());
            Assert.AreEqual(0, req.GetRequestCounter());

        }

        //NOT LAZY
        [TestMethod]
        public void TestNotLazy()
        {

            HttpRequest req = new HttpRequest();
            IWebaoDynTrack trackWebao = (IWebaoDynTrack)WebaoDynBuilder.Build(typeof(IWebaoDynTrack), req);
            List<Track> tracks = trackWebao.GeoGetTopTracks("australia", 1);

            //Console.WriteLine("NUMBER OF REQUESTS: " + req.GetRequestCounter());
            Assert.AreEqual(1, req.GetRequestCounter());

        }

        [TestMethod]
        public void Test3PagesOf20ResultsEachMakes3Request()
        {

            HttpRequest req = new HttpRequest();
            ServiceTracks service = new ServiceTracks(req);

            IEnumerator<Track> tracks = service.TopTracksFrom("australia").GetEnumerator();

            for (int i = 0; tracks.MoveNext() && i < 50; ++i)
            {
                Console.WriteLine(tracks.Current.Name);
            }

            //Console.WriteLine("NUMBER OF REQUESTS: " + req.GetRequestCounter());
            Assert.AreEqual(3, req.GetRequestCounter());

        }


        [TestMethod]
        public void TestFirstTrackMakesOneRequest()
        {

            HttpRequest req = new HttpRequest();
            ServiceTracks service = new ServiceTracks(req);

            IEnumerator<Track> tracks = service.TopTracksFrom("france").GetEnumerator();

            if (tracks.MoveNext())
            {
                Console.WriteLine(tracks.Current.Name);
            }

            //Console.WriteLine("NUMBER OF REQUESTS: " + req.GetRequestCounter());
            Assert.AreEqual(1, req.GetRequestCounter());

        }

        [TestMethod]
        public void TestFirstPageOfResultMakesOneRequest()
        {

            HttpRequest req = new HttpRequest();
            ServiceTracks service = new ServiceTracks(req);

            IEnumerator<Track> tracks = service.TopTracksFrom("portugal").GetEnumerator();

            for (int i = 0; tracks.MoveNext() && i < 19; ++i)
            {
                Console.WriteLine(tracks.Current.Name);
            }

            //Console.WriteLine("NUMBER OF REQUESTS: " + req.GetRequestCounter());
            Assert.AreEqual(1, req.GetRequestCounter());

        }

    }
}
