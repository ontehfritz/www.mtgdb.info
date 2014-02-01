using System;
using System.Collections.Generic;

namespace MtgDb.Info
{
    public interface IRepository
    {
        Guid AddPlaneswalker(string userName, string password, string email);
        void RemovePlaneswalker(Guid id);
        Profile GetProfile(Guid id);
        Profile GetProfile(string name);
        Planeswalker UpdatePlaneswalker(Planeswalker planeswalker);

        UserCard AddUserCard(Guid walkerId, int multiverseId, int amount);
        UserCard[] GetUserCards(Guid walkerId, int[] multiverseIds);
        UserCard[] GetUserCards(Guid walkerId);
        UserCard[] GetUserCards (Guid walkerId, string setId);
        Dictionary<string, int> GetSetCardCounts(Guid walkerId);
//        void UpdatePlaneswalker(Planeswalker planeswalker);
//
//        void SetCardAmount(Guid userId, int multiverseId, int amount);
    }
}

