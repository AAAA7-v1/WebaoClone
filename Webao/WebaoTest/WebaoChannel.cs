using System.Collections.Generic;
using Webao.Attributes;
using Webao.Test.Dto;

namespace Webao.Test
{
    [BaseUrl("https://www.googleapis.com/youtube/v3/")]
    [AddParameter("format", "json")]
    [AddParameter("key", "AIzaSyAYPektgPnDjZoJ_VF2-6xLoIj_edsitHg")]
    public class WebaoChannel : AbstractAccessObject
    {
        public WebaoChannel(IRequest req) : base(req) { }

        [Get("channels?part=snippet&forUsername={Uname}")]
        [Mapping(typeof(DtoChannelInfo), ".Items")]
        public Channel[] GetChannelInfo(string userName) => (Channel[])Request(userName);

    }
}
