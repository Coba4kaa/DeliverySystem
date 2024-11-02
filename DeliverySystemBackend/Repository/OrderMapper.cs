using DeliverySystemBackend.Repository.Models;
using DeliverySystemBackend.Service.DomainModels;

namespace DeliverySystemBackend.Repository
{
    public static class OrderMapper
    {
        public static OrderDbModel ToDbModel(Order order)
        {
            return new OrderDbModel
            {
                Id = order.Id,
                SenderCity = order.SenderCity,
                SenderAddress = order.SenderAddress,
                RecipientCity = order.RecipientCity,
                RecipientAddress = order.RecipientAddress,
                Weight = order.Weight,
                PickupDate = order.PickupDate
            };
        }

        public static Order ToDomainModel(OrderDbModel orderDbModel)
        {
            return new Order
            {
                Id = orderDbModel.Id,
                SenderCity = orderDbModel.SenderCity,
                SenderAddress = orderDbModel.SenderAddress,
                RecipientCity = orderDbModel.RecipientCity,
                RecipientAddress = orderDbModel.RecipientAddress,
                Weight = orderDbModel.Weight,
                PickupDate = orderDbModel.PickupDate
            };
        }
    }
}