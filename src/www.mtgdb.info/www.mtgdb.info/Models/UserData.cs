using System;
using System.Collections.Generic;

namespace mtgdb.info
{
    public class UserData
    {
        public Guid Id { get; set; }
        public List<UserCard> Cards { get; set; }
    }
}

