using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModule;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using Shared.DTOS.BasketModuleDTOs;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Product = DomainLayer.Models.ProductModule.Product;
namespace Service
{
    public class PaymentService(IConfiguration _configuration
        , IUnitOfWork _unitOfWork, IBasketRepository _basketRepository
        , IMapper _mapper) : IPaymentService

    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId)
        {
            //1-Configure Stripe Secret Key :
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            //2-Get Basket From BasketId : 
            var Basket = await _basketRepository.GetBasketAsync(BasketId) ?? throw new BasketNotFoundException(BasketId);

            //3-Get Ammount t : => Get Product =>[Get Price] + DeliveryMethod Cost :

            // * Get Product Price :
            var ProductRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in Basket.Items)
            {
                var Product = await ProductRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                item.Price = Product.Price;
            }
            //* Delivery Method cost :  From DeliveryMethodById => DeliveryMethod.Cost:
            ArgumentNullException.ThrowIfNull(Basket.deliveryMethodId);
            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(Basket.deliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(Basket.deliveryMethodId.Value);
            Basket.shippingPrice = DeliveryMethod.Price;

            // * Calculate Total Ammount :
            var BasketAmount = (long)(Basket.Items.Sum(item => item.Quantity * item.Price) + DeliveryMethod.Price) * 100;


            //4-Create Or Update PaymentIntent :
            var PaymentService = new PaymentIntentService();
            if (Basket.paymentIntentId is null) //Create New PaymentIntent :
            {
                var Options = new PaymentIntentCreateOptions
                {
                    Amount = BasketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };
                var PaymentIntent = await PaymentService.CreateAsync(Options); // return To client Secret + PaymentIntentId => stripe عشان يكلم ال 
                Basket.paymentIntentId = PaymentIntent.Id;
                Basket.clientSecret = PaymentIntent.ClientSecret;

            }
            else //Update PaymentIntent :
            {
                var Options = new PaymentIntentUpdateOptions()
                {
                    Amount = BasketAmount,
                };
                await PaymentService.UpdateAsync(Basket.paymentIntentId, Options);
            }

            //5-Save Changes Basket :
            await _basketRepository.CreateOrUpdateBasketAsync(Basket);

            return _mapper.Map<BasketDto>(Basket);
        }
    }
}
