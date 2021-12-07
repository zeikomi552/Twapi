using FollowBackCore.Twitter;
using FollowBackCore.Utilities;
using System;
using System.IO;

namespace FollowBackCore
{
    class Program
    {
        static bool GetKeys(string[] args, ref TwitterKeys keys, ref string command)
        {
            int min_args = 5;

            if (args.Length <= min_args)
            {
                return false;
            }

            command = args[0];

            keys = new TwitterKeys();
            keys.ConsumerKey = args[1];
            keys.ConsumerSecretKey = args[2];
            keys.AccessToken = args[3];
            keys.AccessSecret = args[4];

            return true;
        }

        static bool GetArg(string[] args, int index, ref string arg)
        {
            if (args.Length > index)
            {
                arg = args[index];
                return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            TwitterKeys keys = new TwitterKeys();
            string command = string.Empty;

            if (GetKeys(args, ref keys, ref command))
            {
                switch (command)
                {
                    case "tweet":
                        {
                            string message = string.Empty;
                            if (GetArg(args, 5, ref message))
                            {
                                TwitterAPI.Tweet(keys, message.Replace("\\r\\n", "\r\n"));
                            }
                            break;
                        }
                    case "createfriend":
                        {
                            string screen_name = string.Empty;
                            if (GetArg(args, 5, ref screen_name))
                            {
                                TwitterAPI.CreateFollow(keys, screen_name);
                            }

                            break;
                        }
                    case "breakfriend":
                        {
                            string screen_name = string.Empty;
                            if (GetArg(args, 5, ref screen_name))
                            {
                                TwitterAPI.BreakFollow(keys, screen_name);
                            }

                            break;
                        }
                }
            }
        }

    }
}
