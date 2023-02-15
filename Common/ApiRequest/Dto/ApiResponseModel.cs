using System.Collections.Generic;
using System.Net;

namespace Common.ApiRequest.Dto
{
    public class ApiResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public object Result { get; set; }
    }

    public class ApiResponseModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; }
        public List<string> ErrorMessages { get; set; }
        public T Result { get; set; }
        public List<T> ListResult { get; set; }
    }
}
