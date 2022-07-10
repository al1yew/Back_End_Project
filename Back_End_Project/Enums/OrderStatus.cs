using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Back_End_Project.Enums
{
    public enum OrderStatus
    {
        Pending = 1,
        Accepted,
        Rejected,
        Shipped,
        Courier,
        Delivered
    }
}
