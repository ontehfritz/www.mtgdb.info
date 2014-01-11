using System;

namespace MtgDb.Info
{
    public interface IRepository
    {
        void AddPlaneswalker(Planeswalker planeswalker);
        void DeletePlaneswalker(Guid Id);
        void UpdatePlaneswalker(Planeswalker planeswalker);

        void AddUserCard();
    }
}

