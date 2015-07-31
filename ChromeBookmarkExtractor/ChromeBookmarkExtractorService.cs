using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ChromeBookmarkExtractor
{
    public static class ChromeBookmarkExtractorService
    {
        public static string Extract(string filepath, string destinationpath)
        {
            var errors = 0;
            var processed = 0;

            var rawtext = File.ReadAllText(filepath);

            var o = JObject.Parse(rawtext);

            var barchildren = o["roots"]["bookmark_bar"]["children"];

            var otherchildren = o["roots"]["other"]["children"];

            var totalchildren = barchildren.Union(otherchildren);

            var tempdestinationpath = destinationpath;

            foreach (var child in totalchildren)
            {
                try
                {
                    var safename = GetSafeFilename(child["name"].Value<string>());

                    if (child["type"].Value<string>() == "folder")
                    {
                        Directory.CreateDirectory(safename);

                        tempdestinationpath = destinationpath + @"\" + safename + @"\";

                        continue;
                    }
                    var file = File.OpenWrite(tempdestinationpath + @"\" + safename + ".url");
                    var text = $"[InternetShortcut]{Environment.NewLine}URL={child["url"].Value<string>()}";
                    var bytearray = Encoding.UTF8.GetBytes(text);
                    file.Write(bytearray, 0, bytearray.Length);
                    file.Close();

                    processed++;
                }
                catch (Exception e)
                {
                    errors++;
                    Console.WriteLine(e);
                }
            }

            return $"Processed : {processed}, errors : {errors}";
        }

        private static string GetSafeFilename(string filename)
        {

            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));

        }
    }
}
