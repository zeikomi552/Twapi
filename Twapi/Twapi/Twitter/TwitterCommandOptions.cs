using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twapi.Twitter
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

        [EndpointParam("-cursor")]
        public string Cursor { get; set; }

        [EndpointParam("-skip_status")]
        public string Skip_status { get; set; }

        [EndpointParam("-include_ext_alt_text")]
        public string Include_ext_alt_text { get; set; }

        [EndpointParam("-user_id")]
        public string User_id { get; set; }

        [EndpointParam("-screen_name")]
        public string Screen_name { get; set; }

        [EndpointParam("-include_user_entities")]
        public string Include_user_entities { get; set; }

        [EndpointParam("-tweet_mode")]
        public string Tweet_mode { get; set; }

        [EndpointParam("-status")]
        public string Status { get; set; }

        [EndpointParam("-trim_user")]
        public string Trim_User { get; set; }
        [EndpointParam("-exclude_replies")]
        public string Exclude_replies { get; set; }
        [EndpointParam("-contributor_details")]
        public string Contributor_details { get; set; }
    }
}
