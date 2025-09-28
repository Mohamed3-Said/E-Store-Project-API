using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ErrorModels
{
    public class ValidationError
    {
        public string Failed { get; set; } = default!;
        public IEnumerable<string> Errors { get; set; } = [];
    }
}
