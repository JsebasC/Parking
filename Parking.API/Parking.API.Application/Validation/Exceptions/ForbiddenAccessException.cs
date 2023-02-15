using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parking.API.Application.Validation.Exceptions
{

    //Esta excepción nos servirá para cuando se intente eliminar algo (o en general, cualquier acción) 
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException() : base() { }
    }
}
