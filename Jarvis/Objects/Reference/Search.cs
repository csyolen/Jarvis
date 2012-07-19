using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Jarvis.Objects.Reference
{
    class Search
    {
        public string Link { get; private set; }
        public string Description { get; private set; }

        public Search(string query)
        {
            var google = Google.FromQuery(query);
            var results = google.ResponseData.Results;
            var lucky = results[0];

            Link = lucky.Url;
            Description = lucky.Content.StripHtml();

            var result = results.FirstOrDefault(o => o.Url.ToLower().Contains("imdb"));
            if (result != null)
            {
                var imdb = IMDB.FromQuery(query);
                Description = "{0} is a {1} film and was released in {2} and received a rating of {3}. {4}".Template(imdb.Title, imdb.Genre.ToLower(), imdb.Year, imdb.ImdbRating,
                                                                                        imdb.Plot);
                Link = result.Url;
                return;
            } 

            result = results.FirstOrDefault(o => o.Url.ToLower().Contains("wikipedia"));
            if (result != null)
            {
                Description = new Wikipedia(result.Url) + Environment.NewLine;
                Link = result.Url;
            }
        }

    }
}
