using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FollowBackCore.Twitter
{
    public class TwitterCommandOptions
    {
        [EndpointParam("-q")]
        public string Q { get; set; }

        [EndpointParam("-geocode")]
        public string Geocode { get; set; }

        [EndpointParam("-lang")]
        public string Lang { get; set; }

        [EndpointParam("-locale")]
        public string Locale { get; set; }

        [EndpointParam("-result_type")]
        public string Result_type { get; set; }

        [EndpointParam("-count")]
        public string Count { get; set; }

        [EndpointParam("-until")]
        public string Until { get; set; }

        [EndpointParam("-since_id")]
        public string Since_id { get; set; }

        [EndpointParam("-max_id")]
        public string Max_id { get; set; }

        [EndpointParam("-include_entities")]
        public string Include_entities { get; set; }

    }
}
