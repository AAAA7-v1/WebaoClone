using System;
using System.Collections.Generic;
using Webao.Test.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Webao.Test.WebaoDynamic;

namespace Webao.Test
{

    [TestClass]
    public class ServiceArtistsTest
    {

        //LAZY
        [TestMethod]
        public void TestLazy()
        {

            HttpRequest req = new HttpRequest();
            ServiceArtists service = new ServiceArtists(req);

            IEnumerator<Artist> artists = service.ArtistSearch("muse").GetEnumerator();

            //Console.WriteLine("NUMBER OF REQUESTS: " + req.GetRequestCounter());
            Assert.AreEqual(0, req.GetRequestCounter());

        }

        //NOT LAZY
        [TestMethod]
        public void TestNotLazy()
        {

            HttpRequest req = new HttpRequest();
            IWebaoDynArtist artistWebao = (IWebaoDynArtist)WebaoDynBuilder.Build(typeof(IWebaoDynArtist), req);
            List<Artist> artists = artistWebao.SearchPage("muse", 1);

            //Console.WriteLine("NUMBER OF REQUESTS: " + req.GetRequestCounter());
            Assert.AreEqual(1, req.GetRequestCounter());

        }

        [TestMethod]
        public void Test3PagesOf20ResultsEachMakes3Request()
        {

            HttpRequest req = new HttpRequest();
            ServiceArtists service = new ServiceArtists(req);

            IEnumerator<Artist> artists = service.ArtistSearch("muse").GetEnumerator();

            for (int i = 0; artists.MoveNext() && i < 50; ++i)
            {
                Console.WriteLine(artists.Current.Name);
            }

            Console.WriteLine("NUMBER OF REQUESTS: " + req.GetRequestCounter());
            Assert.AreEqual(3, req.GetRequestCounter());

        }


        [TestMethod]
        public void TestFirstArtistMakesOneRequest()
        {

            HttpRequest req = new HttpRequest();
            ServiceArtists service = new ServiceArtists(req);

            IEnumerator<Artist> artists = service.ArtistSearch("linkin+park").GetEnumerator();

            if (artists.MoveNext())
            {
                Console.WriteLine(artists.Current.Name);
            }

            //Console.WriteLine("NUMBER OF REQUESTS: " + req.GetRequestCounter());
            Assert.AreEqual(1, req.GetRequestCounter());

        }

        [TestMethod]
        public void TestFirstPageOfResultMakesOneRequest()
        {

            HttpRequest req = new HttpRequest();
            ServiceArtists service = new ServiceArtists(req);

            IEnumerator<Artist> artists = service.ArtistSearch("linkin+park").GetEnumerator();

            for (int i = 0; artists.MoveNext() && i < 19; ++i)
            {
                Console.WriteLine(artists.Current.Name);
            }

            //Console.WriteLine("NUMBER OF REQUESTS: " + req.GetRequestCounter());
            Assert.AreEqual(1, req.GetRequestCounter());

        }

    }
}
