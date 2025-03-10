﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cadastro.web.Utils
{
    public static class HttpClientExtansions
    {
        private static MediaTypeHeaderValue _contentType 
            = new MediaTypeHeaderValue("application/json");
        public static async Task<T> ReadContentAs<T>(
            this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) throw
                    new ApplicationException(
                        $"Something went wrong calling API: " +
                        $"{response.ReasonPhrase}");
            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(dataAsString, 
                new JsonSerializerOptions 
                { PropertyNameCaseInsensitive = true });
        }
    public static Task<HttpResponseMessage> PostAsJson<T>(
        this HttpClient httpClient,
        string url,
        T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = _contentType;
            return httpClient.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJson<T>(
            this HttpClient httpClient,
            string url,
            T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = _contentType;
            return httpClient.PutAsync(url, content);
        }

    }
}
