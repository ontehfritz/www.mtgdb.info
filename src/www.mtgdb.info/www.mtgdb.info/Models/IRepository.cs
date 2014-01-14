using System;

namespace MtgDb.Info
{
    public interface IRepository
    {
        Planeswalker AddPlaneswalker(Planeswalker planeswalker);
        void RemovePlaneswalker(Guid Id);
        Planeswalker GetPlaneswalker(Guid Id);
//        void UpdatePlaneswalker(Planeswalker planeswalker);
//
//        void SetCardAmount(Guid userId, int multiverseId, int amount);
    }
}

