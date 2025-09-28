using Shared.DTOS.BasketModuleDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public interface IPaymentService
    {
        public Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string BasketId);
    }
}
