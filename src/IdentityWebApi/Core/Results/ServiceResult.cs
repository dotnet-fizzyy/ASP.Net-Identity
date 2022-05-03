using IdentityWebApi.Core.Enums;

namespace IdentityWebApi.Core.Results
{
    public class ServiceResult
    {
        #region Properties

        public ServiceResultType Result { get; set; }

        public string Message { get; set; }

        #endregion

        #region Lambdas

        public bool IsResultFailed => Result is not ServiceResultType.Success;

        public bool IsResultInvalidData => Result is ServiceResultType.InvalidData;

        public bool IsResultNotFound => Result is ServiceResultType.NotFound;

        public bool IsResultInternalError => Result is ServiceResultType.InternalError;

        #endregion

        #region Constuctors
        protected ServiceResult() {}

        public ServiceResult(ServiceResultType result)
        {
            Result = result;
        }

        public ServiceResult(ServiceResultType result, string message)
        {
            Result = result;
            Message = message;
        }
        #endregion
    }

    public class ServiceResult<T> : ServiceResult
    {
        #region Properties

        public T Data { get; set; }

        #endregion

        #region Constructors

        public ServiceResult() {}

        public ServiceResult(ServiceResultType result)
        {
            Result = result;
        }

        public ServiceResult(ServiceResultType result, string message)
        {
            Result = result;
            Message = message;
        }

        public ServiceResult(ServiceResultType result, T data)
        {
            Result = result;
            Data = data;
        }

        public ServiceResult(ServiceResultType result, string message, T data)
        {
            Result = result;
            Message = message;
            Data = data;
        }

        #endregion
    }
}