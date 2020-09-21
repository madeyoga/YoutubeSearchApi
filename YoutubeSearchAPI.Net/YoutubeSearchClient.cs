﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YoutubeSearchAPI.Modules;

namespace YoutubeSearchAPI
{
    public class YoutubeSearchClient
    {
        private string developerKey;
        private UrlBuilder urlBuilder;
        private readonly HttpClient httpClient;

        public YoutubeSearchClient(string developerKey)
        {
            this.developerKey = developerKey;
            
            this.httpClient = new HttpClient();
            
            urlBuilder = new UrlBuilder(developerKey);
        }

        private async Task <dynamic> HttpRequestGet(string requestUrl)
        {
            var response = await httpClient.GetAsync(requestUrl);

            string jsonString = await response.Content.ReadAsStringAsync();

            dynamic jsonObject = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

            return jsonObject;
        }

        public async Task <dynamic> Search(string query, string part = "snippet", string type = "Video", int maxResults = 5, int videoCategory = 10)
        {
            string requestUrl = urlBuilder.BuildSearchUrl(query, part, type, maxResults, videoCategory);
            
            return await Utils.HttpRequestGet(httpClient, requestUrl);
        }

        public async Task <dynamic> GetPlaylist(string playlistId, int maxResults = 5, string part = "snippet")
        {
            string requestUrl = urlBuilder.BuildPlaylistUrl(playlistId, part: part, maxResults: maxResults);

            return await Utils.HttpRequestGet(httpClient, requestUrl);
        }

        public async Task <dynamic> GetVideoDetails(string videoId, List<string> part)
        {
            string requestUrl = urlBuilder.BuildVideoDetailUrl(videoId: videoId);

            return await Utils.HttpRequestGet(httpClient, requestUrl);
        }

        public async Task <dynamic> GetVideoCommentThreads(string videoId, int maxResults = 5, string textFormat = "plainText", string part = "snippet")
        {
            string requestUrl = urlBuilder.BuildVideoCommentThreadsUrl(videoId: videoId, 
                maxResults: maxResults, textFormat: textFormat, part: part);

            return await Utils.HttpRequestGet(httpClient, requestUrl);
        }

        public async Task <dynamic> GetChannelCommentThreads(string channelId, int maxResults = 5, string textFormat = "plainText", string part = "snippet")
        {
            string requestUrl = urlBuilder.BuildChannelCommentThreadsUrl(channelId: channelId,
                maxResults: maxResults, textFormat: textFormat, part: part);

            return await Utils.HttpRequestGet(httpClient, requestUrl);
        }
    }
}
