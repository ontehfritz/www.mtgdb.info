using System;

namespace MtgDb.Info
{
    public class Planeswalker 
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public Planeswalker()
        {

        }
    }
}

