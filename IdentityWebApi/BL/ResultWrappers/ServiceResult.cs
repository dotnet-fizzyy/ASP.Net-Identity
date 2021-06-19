using IdentityWebApi.BL.Enums;

namespace IdentityWebApi.BL.ResultWrappers
{
    public class ServiceResult
    {
        public ServiceResult() {}

        public ServiceResult(ServiceResultType serviceResultType)
        {
            ServiceResultType = serviceResultType;
        }
        
        public ServiceResult(ServiceResultType serviceResultType, string message)
        {
            ServiceResultType = serviceResultType;
            Message = message;
        }
        
        public ServiceResultType ServiceResultType { get; set; }
        
        public string Message { get; set; }
    }

    public class ServiceResult<T> : ServiceResult where T : class
    {
        public ServiceResult() {}
        
        public ServiceResult(ServiceResultType serviceResultType)
        {
            ServiceResultType = serviceResultType;
        }
        
        public ServiceResult(ServiceResultType serviceResultType, string message)
        {
            ServiceResultType = serviceResultType;
            Message = message;
        }
        
        public ServiceResult(ServiceResultType serviceResultType, T data)
        {
            ServiceResultType = serviceResultType;
            Data = data;
        }
        
        public ServiceResult(ServiceResultType serviceResultType, string message, T data)
        {
            ServiceResultType = serviceResultType;
            Message = message;
            Data = data;
        }
        
        public T Data { get; set; }
    }
}