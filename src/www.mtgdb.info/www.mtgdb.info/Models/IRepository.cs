using System;

namespace MtgDb.Info
{
    public interface IRepository
    {
        void AddPlaneswalker(string userName, string password, string email);
        void RemovePlaneswalker(Guid Id);
        Profile GetProfile(Guid Id);
        Planeswalker UpdatePlaneswalker(Planeswalker planeswalker);

//        void UpdatePlaneswalker(Planeswalker planeswalker);
//
//        void SetCardAmount(Guid userId, int multiverseId, int amount);
    }
}

