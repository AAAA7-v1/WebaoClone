using Webao.Test;
using Webao.Test.WebaoDynamic;

namespace Webao.WebaoBench
{
    class Program
    {
        static readonly WebaoArtist artistWebaoInfo = (WebaoArtist)WebaoBuilder.Build(typeof(WebaoArtist), new LastfmArtistInfoMockRequest("muse"));
        static readonly IWebaoDynArtist artistDynWebaoInfo = (IWebaoDynArtist)WebaoDynBuilder.Build(typeof(IWebaoDynArtist), new LastfmArtistInfoMockRequest("muse"));

        static readonly WebaoArtist artistWebaoSearch = (WebaoArtist)WebaoBuilder.Build(typeof(WebaoArtist), new LastfmArtistSearchMockRequest("muse"));
        static readonly IWebaoDynArtist artistDynWebaoSearch = (IWebaoDynArtist)WebaoDynBuilder.Build(typeof(IWebaoDynArtist), new LastfmArtistSearchMockRequest("muse"));

        static readonly WebaoTrack trackWebao = (WebaoTrack)WebaoBuilder.Build(typeof(WebaoTrack), new LastfmGeoTopTrackMockRequest("australia"));
        static readonly IWebaoDynTrack trackDynWebao = (IWebaoDynTrack)WebaoDynBuilder.Build(typeof(IWebaoDynTrack), new LastfmGeoTopTrackMockRequest("australia"));

        static readonly WebaoChannel channelWebao = (WebaoChannel)WebaoBuilder.Build(typeof(WebaoChannel), new YoutubeChannelInfoMockRequest("codyko69"));
        static readonly IWebaoDynChannel channelDynWebao = (IWebaoDynChannel)WebaoDynBuilder.Build(typeof(IWebaoDynChannel), new YoutubeChannelInfoMockRequest("codyko69"));


        static void Main(string[] args)
        {   
            // Builds
            NBench.Bench(BenchWebaoBuild, "Webao Build");

            NBench.Bench(BenchWebaoDynBuild, "Webao Dynamic Build");

            // Array of class
            NBench.Bench(BenchWebaoGetChannelInfo, "Webao GetChannelInfo");

            NBench.Bench(BenchWebaoDynGetChannelInfo, "Webao Dyn GetChannelInfo");

            // List of struct
            NBench.Bench(BenchWebaoGetGeoTopTracks, "Webao GeoTop");

            NBench.Bench(BenchWebaoDynGetGeoTopTracks, "Webao Dyn GeoTop");

            // List of class
            NBench.Bench(BenchWebaoSearch, "Webao Search");

            NBench.Bench(BenchWebaoDynSearch, "Webao Dynamic Search");

            // Class
            NBench.Bench(BenchWebaoGetInfo, "Webao GetInfo");

            NBench.Bench(BenchWebaoDynGetInfo, "Webao Dyn GetInfo");
        }

        static void BenchWebaoBuild()
        {
            WebaoBuilder.Build(typeof(WebaoArtist), new HttpRequest());
        }
        static void BenchWebaoDynBuild()
        {
            WebaoDynBuilder.Build(typeof(IWebaoDynArtist), new HttpRequest());
        }
        static void BenchWebaoGetGeoTopTracks()
        {
            trackWebao.GeoGetTopTracks("australia");
        }
        static void BenchWebaoDynGetGeoTopTracks()
        {
            trackDynWebao.GeoGetTopTracks("australia");
        }
        static void BenchWebaoGetInfo() 
        {
            artistWebaoInfo.GetInfo("muse");
        }
        static void BenchWebaoDynGetInfo() 
        {
            artistDynWebaoInfo.GetInfo("muse");
        }
        static void BenchWebaoSearch()
        {
            artistWebaoSearch.Search("muse");
        }
        static void BenchWebaoDynSearch()
        {
            artistDynWebaoSearch.Search("muse");
        }
        static void BenchWebaoGetChannelInfo()
        {
            channelWebao.GetChannelInfo("codyko69");
        }
        static void BenchWebaoDynGetChannelInfo()
        {
            channelDynWebao.GetChannelInfo("codyko69");
        }
    }
}
