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
    }
}