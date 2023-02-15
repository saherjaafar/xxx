using static Common.Enums.SD;

namespace Common.ApiRequest.Dto
{
    public class ApiRequestModel
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public object Data { get; set; }
    }

}
