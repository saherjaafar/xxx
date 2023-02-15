using Common.ApiRequest.Dto;
using Common.Enums;
using Core.Dto;
using Core.Dto.Admin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Common.ApiRequest
{
    public class ApiRequest : IApiRequest
    {
        public ApiResponseModel responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }


        public ApiRequest(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;
        }

        public async Task<T> SendAsync<T>(ApiRequestModel apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("WedcooApi");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri("https://localhost:5001/api/" + apiRequest.Url);
                if (apiRequest.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.Data), Encoding.UTF8, "application/json");
                    switch (apiRequest.ApiType)
                    {
                        case SD.ApiType.POST:
                            message.Method = HttpMethod.Post; break;
                        case SD.ApiType.PUT:
                            message.Method = HttpMethod.Put; break;
                        case SD.ApiType.DELETE:
                            message.Method = HttpMethod.Delete; break;
                        default:
                            message.Method = HttpMethod.Get; break;
                    }
                }

                HttpResponseMessage apiResponse = null;
                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var ApiResponse = JsonConvert.DeserializeObject<T>(apiContent);
                return ApiResponse;
            }
            catch (Exception ex)
            {
                var dto = new ApiResponseModel
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsSuccess = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var ApiResponse = JsonConvert.DeserializeObject<T>(res);
                return ApiResponse;
            }
        }

        public List<T> GetX<T>(string url)
        {
            var client = httpClient.CreateClient("WedcooApi");
            client.Timeout = TimeSpan.FromHours(1);
            HttpRequestMessage message = new HttpRequestMessage();
            message.Headers.Add("Accept", "application/json");
            message.RequestUri = new Uri(url);
            HttpResponseMessage apiResponse = null;
            apiResponse =  client.Send(message);
            var apiContent =  apiResponse.Content.ReadAsStringAsync();
            List< T > res = JsonConvert.DeserializeObject<List<T>>(apiContent.Result);

            return res;
        }
    }
}
