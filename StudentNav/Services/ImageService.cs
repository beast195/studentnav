using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StudentNav.Services
{
    public class ImageService
    {
        public string Host { get; set; } = "https://api.imgur.com/3/image";

        public async Task<string> GetImage(string imgId)
        {
            return await CreateRequestAsync("GET", null);
        }

        public async Task<string> CreatePhoto(Stream stream)
        {
            var base64 = Convert.ToBase64String(GetBase64Bytes(stream));
            var link = await CreateRequestAsync("POST", base64);
            return link;
        }

        public async Task<string> CreateRequestAsync(string requestType, string base64)
        {
            var client = new HttpClient();
            if (requestType == "GET")
            {
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"{Host}/imagehasg"),
                    Method = HttpMethod.Get
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Client-ID", "edf7276a3e9e8da");
                

                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            else
            {
                var stringPayload = JsonConvert.SerializeObject(new { image = base64});
                var content = new StringContent(stringPayload, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri($"{Host}"),
                    Content = content,
                    Method = HttpMethod.Post
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Client-ID", "edf7276a3e9e8da");
                var response = await client.SendAsync(request);
                var result = JsonConvert.DeserializeObject<ImgResult>(await response.Content.ReadAsStringAsync());

                return result.Data.link;
            }
        }

        
        public byte[] GetBase64Bytes(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }

    public class ImgResult
    {
        public ImgurModel Data { get; set; }
        public string status { get; set; }
        public bool success { get; set; }
    }

    public class ImgurModel
    {
        public string id { get; set; }
        public object title { get; set; }
        public object description { get; set; }
        public int datetime { get; set; }
        public string type { get; set; }
        public bool animated { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int size { get; set; }
        public int views { get; set; }
        public int bandwidth { get; set; }
        public object vote { get; set; }
        public bool favorite { get; set; }
        public object nsfw { get; set; }
        public object section { get; set; }
        public object account_url { get; set; }
        public int account_id { get; set; }
        public bool is_ad { get; set; }
        public bool in_most_viral { get; set; }
        public List<object> tags { get; set; }
        public int ad_type { get; set; }
        public string ad_url { get; set; }
        public bool in_gallery { get; set; }
        public string deletehash { get; set; }
        public string name { get; set; }
        public string link { get; set; }
    }
}