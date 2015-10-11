using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using Microsoft.Framework.Logging;

namespace vNextApplication.Models
{
    public class WorldRepository : IWorldRepository
    {
        private WorldContext _context;
        private ILogger<WorldRepository> _logger;

        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            try
            {
                return _context.Trips
                    .OrderBy(x => x.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips from database", ex);
                return null;
            }
        }

        public IEnumerable<Trip> GetAllTripsWithStops()
        {
            try
            {
                return _context.Trips.Include(x => x.Stops)
                    .OrderBy(x => x.Name)
                    .ToList();
            }

            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }

        public void AddTrip(Trip newTrip)
        {
            _context.Add(newTrip);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }

        public Trip GetTripByName(string tripName)
        {
            return _context.Trips
                .Include(x => x.Stops)
                .FirstOrDefault(x => x.Name == tripName);

        }

        public void AddStop(Stop newStop, string tripName)
        {
            var theTrip = GetTripByName(tripName);
            newStop.Order = theTrip.Stops.Max(x => x.Order) + 1;
            theTrip.Stops.Add(newStop);
            _context.Stops.Add(newStop);
        }
    }
}