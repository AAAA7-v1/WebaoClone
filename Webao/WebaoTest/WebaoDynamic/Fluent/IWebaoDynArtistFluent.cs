using System.Collections.Generic;
using Webao.Test.Dto;

namespace Webao.Test.WebaoDynamic
{
    public interface IWebaoDynArtistFluent
    {
        Artist GetInfo(string name);
        List<Artist> Search(string name, int page);
    }
}
