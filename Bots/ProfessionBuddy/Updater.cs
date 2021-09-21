using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Styx.Helpers;

namespace HighVoltz
{
    public static class Updater
    {
        private const string PbSvnUrl = "http://professionbuddy.googlecode.com/svn/trunk/Professionbuddy/";
        private const string PbChangeLogUrl = "http://code.google.com/p/professionbuddy/source/detail?r=";

        private static readonly Regex _linkPattern = new Regex(@"<li><a href="".+"">(?<ln>.+(?:..))</a></li>",
                                                               RegexOptions.CultureInvariant);

        private static readonly Regex _changelogPattern =
            new Regex(
                "<h4 style=\"margin-top:0\">Log message</h4>\r?\n?<pre class=\"wrap\" style=\"margin-left:1em\">(?<log>.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?.+\r?\n?)</pre>",
                RegexOptions.CultureInvariant);

        public static void CheckForUpdate()
        {
            try
            {
                Professionbuddy.Log("Checking for new version");
				Professionbuddy.Log("No updates found");
            }
            catch (Exception ex)
            {
                Professionbuddy.Err(ex.ToString());
            }
        }

        private static int GetRevision()
        {
           var client = new WebClient();
            string html = client.DownloadString(PbSvnUrl);
            var pattern = new Regex(@" - Revision (?<rev>\d+):", RegexOptions.CultureInvariant);
            Match match = pattern.Match(html);
            if (match.Success && match.Groups["rev"].Success)
                return int.Parse(match.Groups["rev"].Value);
            throw new Exception("Unable to retreive revision");
        }

        private static void DownloadFilesFromSvn(WebClient client, string url)
        {
            string html = client.DownloadString(url);
            MatchCollection results = _linkPattern.Matches(html);

            IEnumerable<Match> matches = from match in results.OfType<Match>()
                                         where match.Success && match.Groups["ln"].Success
                                         select match;
            foreach (Match match in matches)
            {
                string file = RemoveXmlEscapes(match.Groups["ln"].Value);
                string newUrl = url + file;
                if (newUrl[newUrl.Length - 1] == '/') // it's a directory...
                {
                    DownloadFilesFromSvn(client, newUrl);
                }
                else // its a file.
                {
                    string filePath, dirPath;
                    if (url.Length > PbSvnUrl.Length)
                    {
                        string relativePath = url.Substring(PbSvnUrl.Length);
                        dirPath = Path.Combine(Professionbuddy.BotPath, relativePath);
                        filePath = Path.Combine(dirPath, file);
                    }
                    else
                    {
                        dirPath = Environment.CurrentDirectory;
                        filePath = Path.Combine(Professionbuddy.BotPath, file);
                    }
                    Professionbuddy.Debug("Downloading {0}", file);
                    if (!Directory.Exists(dirPath))
                        Directory.CreateDirectory(dirPath);
                    client.DownloadFile(newUrl, filePath);
                }
            }
        }

        private static string RemoveXmlEscapes(string xml)
        {
            return
                xml.Replace("&amp;", "&").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace(
                    "&apos;", "'");
        }

        private static string GetChangeLog(int revision)
        {
            var client = new WebClient();
            string html = client.DownloadString(PbChangeLogUrl + revision);
            Match match = _changelogPattern.Match(html);
            if (match.Success && match.Groups["log"].Success)
                return RemoveXmlEscapes(match.Groups["log"].Value);
            return null;
        }
    }
}

// QmUgY29vbCBhbmQganVzdCBidXkgdGhlIGJvdA==
//!CompilerOption:AddRef:\u0052\u0065\u006D\u006F\u0074\u0069\u006E\u0067\u002E\u0064\u006C\u006C