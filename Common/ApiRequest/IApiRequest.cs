using Common.ApiRequest.Dto;
using Core.Dto.Admin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.ApiRequest
{
    public interface IApiRequest
    {
        ApiResponseModel responseModel { get;set; }
        Task<T> SendAsync<T>(ApiRequestModel apiRequest);
        List<T> GetX<T>(string url);
    }
}
