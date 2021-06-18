using IdentityWebApi.BL.Enums;

namespace IdentityWebApi.BL.ResultWrappers
{
    public class ServiceResult
    {
        public ServiceResultType ServiceResultType { get; set; }
        
        public string Message { get; set; }
    }

    public class ServiceResult<T> : ServiceResult where T : class
    {
        public T Data { get; set; }
    }
}