using AcessoChallenge.Domain.Enums;

namespace AcessoChallenge.Domain
{
    public class ServiceResult<T>
    {
        public ServiceResultType Type { get; private set; }
        public T Model { get; private set; }
        public string Message { get; set; }

        public static ServiceResult<T> Success(T model) => new ServiceResult<T>
        {
            Type = ServiceResultType.Success,
            Model = model
        };

        public static ServiceResult<T> Error(string message) => new ServiceResult<T>
        {
            Type = ServiceResultType.Error,
            Message = message
        };

        public bool IsSucccess() => Type == ServiceResultType.Success;
    }
}