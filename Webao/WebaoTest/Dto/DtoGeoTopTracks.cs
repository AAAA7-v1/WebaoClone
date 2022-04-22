using System.Collections.Generic;

namespace Webao.Test.Dto
{
    public struct DtoGeoTopTracks
    {
        public DtoTracks Tracks { get; set; }

        //added for TP3
        public List<Track> GetTrackList()
        {
            return this.Tracks.Track;
        }

    }

    public struct DtoTracks
    {
        public List<Track> Track { get; set; }
    }

}