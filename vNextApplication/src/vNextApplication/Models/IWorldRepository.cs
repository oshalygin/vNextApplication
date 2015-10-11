﻿using System.Collections.Generic;

namespace vNextApplication.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
        IEnumerable<Trip> GetAllTripsWithStops();
        void AddTrip(Trip newTrip);
        bool SaveAll();
        Trip GetTripByName(string tripName, string username);
        void AddStop(Stop newStop, string username, string tripName);
        IEnumerable<Trip> GetUserTripsWithStops(string name);
    }
}