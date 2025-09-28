using DomainLayer.Models.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
     class OrderSpecifications : BaseSpecifications<Order,Guid>
    {
        //Get All Orders By Email:
        public OrderSpecifications(string Email) : base(O=>O.UserEmail==Email)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
  
        }
        //Get Order By Id:
        public OrderSpecifications(Guid id) : base(O=>O.Id==id)
        {
            AddInclude(O => O.DeliveryMethod);
            AddInclude(O => O.Items);
        }

    }
}
