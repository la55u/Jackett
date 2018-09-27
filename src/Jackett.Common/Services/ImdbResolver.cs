﻿using System;
using System.Text;
using System.Threading.Tasks;
using Jackett.Common.Services.Interfaces;
using Jackett.Common.Utils;
using Jackett.Common.Utils.Clients;
using Newtonsoft.Json;

namespace Jackett.Common.Services
{

    public struct Movie
    {
        public string Title;
        public string Year;
    }

    public class OmdbResolver : IImdbResolver
    {
        public OmdbResolver(WebClient webClient, NonNull<string> omdbApiKey, string omdbApiUrl)
        {
            WebClient = webClient;
            apiKey = omdbApiKey;
            url = omdbApiUrl;
        }

        public async Task<Movie> MovieForId(NonNull<string> id)
        {
            string imdbId = id;

            if (!imdbId.StartsWith("tt", StringComparison.Ordinal))
                imdbId = "tt" + imdbId;

            if (string.IsNullOrWhiteSpace(url))
                url = "http://omdbapi.com";

            var request = new WebRequest(url + "/?apikey=" + apiKey + "&i=" + imdbId);
            request.Encoding = Encoding.UTF8;
            var result = await WebClient.GetString(request);
            var movie = JsonConvert.DeserializeObject<Movie>(result.Content);

            return movie;
        }

        private WebClient WebClient;
        private string apiKey;
        private string url;
    }
}
