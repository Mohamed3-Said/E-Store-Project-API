using ServiceAbstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ServiceManagerWithFactoryDeleget(Func<IProductService> ProductFactory ,
        Func<IAuthenticationService> AuthenticationFactory,
        Func<IBasketService>  BasketFactory,
        Func<IOrderService> OrderFactory ,
        Func<IPaymentService> PaymentFactory
        ) : IServiceManager
    {
        public IProductService ProductService => ProductFactory.Invoke();

        public IBasketService BasketService => BasketFactory.Invoke();

        public IAuthenticationService AuthenticationService => AuthenticationFactory.Invoke();

        public IOrderService OrderService => OrderFactory.Invoke();

        public IPaymentService PaymentService => PaymentFactory.Invoke(); 


        // استخدمنا Func<T> عشان نعمل Lazy Loading للخدمات
        // ولازم نسجّل الخدمة نفسها + Func<T> في الـ DI
        // بكده نقدر نعمل Invoke() وننشئ الخدمة وقت ما نحتاجها

    }
}
