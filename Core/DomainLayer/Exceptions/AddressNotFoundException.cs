using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public sealed class AddressNotFoundException(string UserName) : NotFoundException($"Address for user '{UserName}' is not found !")
    {
    }
}
