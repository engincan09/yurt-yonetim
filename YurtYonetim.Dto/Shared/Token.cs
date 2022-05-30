using System;
using System.Collections.Generic;
using System.Text;

namespace YurtYonetim.Dto.Shared
{
    public class Token
    {
        public int UserId { get; set; }
        public int[] UserRoleId { get; set; }
        public string FullName { get; set; }
    }
}

