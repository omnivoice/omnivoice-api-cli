namespace omnivoice {
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    internal static class HttpRequest {
        public static HttpRequestMessage GET(string absolute_uri) {
            var uri = new Uri(absolute_uri, UriKind.Absolute);
            var result = new HttpRequestMessage(HttpMethod.Get, uri);
            return result;
        }

        public static async Task<HttpRequestMessage> POST(string absolute_uri, Stream stream) {
            var uri = new Uri(absolute_uri, UriKind.Absolute);
            var result = new HttpRequestMessage(HttpMethod.Post, uri);
            var form_content = new MultipartFormDataContent();
            using var stream_content = new StreamContent(stream);
            var file_content = new ByteArrayContent(await stream_content.ReadAsByteArrayAsync());

            file_content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            form_content.Add(file_content, "File", $"{Guid.NewGuid()}.wav");

            result.Content = form_content;
            
            return result;
        }

        public static async Task<HttpRequestMessage> POST(string absolute_uri, string file_name) {
            var uri = new Uri(absolute_uri, UriKind.Absolute);
            var result = new HttpRequestMessage(HttpMethod.Post, uri);
            var form_content = new MultipartFormDataContent();
            await using var filestream = File.OpenRead(file_name);
            using var stream_content = new StreamContent(filestream);
            var file_content = new ByteArrayContent(await stream_content.ReadAsByteArrayAsync());

            file_content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            form_content.Add(file_content, "File", Path.GetFileName(file_name));

            result.Content = form_content;
            
            return result;
        }
    };
};