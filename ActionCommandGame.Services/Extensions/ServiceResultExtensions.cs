using System.Collections.Generic;
using ActionCommandGame.Services.Model.Core;

namespace ActionCommandGame.Services.Extensions
{
    public static class ServiceResultExtensions
    {
        public static ServiceResult NotFound(this ServiceResult serviceResult)
        {
            var message = new ServiceMessage
            {
                Code = "NotFound",
                Message = "What you have been looking for is not here.",
                MessagePriority = MessagePriority.Error
            };
            serviceResult.Messages.Add(message);

            return serviceResult;
        }

        public static ServiceResult<T> NotFound<T>(this ServiceResult<T> serviceResult)
        {
            var notFoundResult = serviceResult.NotFound();
            serviceResult.Messages = notFoundResult.Messages;
            return serviceResult;
        }

        public static ServiceResult<T> PlayerNotFound<T>(this ServiceResult<T> serviceResult)
        {
            var message = new ServiceMessage
            {
                Code = "PlayerNotFound",
                Message = "Who? I don't know this player. Come back with valid identification.",
                MessagePriority = MessagePriority.Error
            };
            serviceResult.Messages.Add(message);

            return serviceResult;
        }

        public static ServiceResult<T> ItemNotFound<T>(this ServiceResult<T> serviceResult)
        {
            var message = new ServiceMessage
            {
                Code = "ItemNotFound",
                Message = "What? I don't know what item you are talking about. Come back when you have more information about this item.",
                MessagePriority = MessagePriority.Error
            };
            serviceResult.Messages.Add(message);

            return serviceResult;
        }

        public static ServiceResult<T> NotEnoughMoney<T>(this ServiceResult<T> serviceResult)
        {
            var message = new ServiceMessage
            {
                Code = "NotEnoughMoney",
                Message = "You cannot afford that.",
                MessagePriority = MessagePriority.Error
            };
            serviceResult.Messages.Add(message);

            return serviceResult;
        }

        public static ServiceResult<T> WithMessages<T>(this ServiceResult<T> serviceResult,
            IList<ServiceMessage> messages)
        {
            foreach (var serviceMessage in messages)
            {
                serviceResult.Messages.Add(serviceMessage);
            }

            return serviceResult;
        }
    }
}
