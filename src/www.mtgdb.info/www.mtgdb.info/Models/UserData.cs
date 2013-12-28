using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public class UserData
    {
        public Guid Id { get; set; }
        public List<UserCard> Cards { get; set; }
    }
}

