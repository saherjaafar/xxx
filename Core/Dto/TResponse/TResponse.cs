using System.Collections.Generic;

namespace Core.Dto.TResponse
{
    public class TResponseVM<T>
    {
        public bool HasError { get; set; }
        public int StatusCode { get; set; }
        public T obj { get; set; }
        public List<T> ListObj { get; set; }
        public string Message { get; set; }
    }

    public class ResponseVM
    {
        public bool HasError { get; set; }
        public int StatusCode { get; set; }
        public object obj { get; set; }
        public List<object> ListObj { get; set; }
        public string Message { get; set; }
    }
}
