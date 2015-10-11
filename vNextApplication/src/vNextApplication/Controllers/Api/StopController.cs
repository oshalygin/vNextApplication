using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Mvc;
using Microsoft.Framework.Logging;
using vNextApplication.Models;
using vNextApplication.Services;
using vNextApplication.ViewModels;

namespace vNextApplication.Controllers.Api
{
    [Route("api/trips/{tripName}/stops")]
    public class StopController : Controller
    {
        private ILogger<StopController> _logger;
        private IWorldRepository _repository;
        private CoordinateService _coordinateService;

        public StopController(IWorldRepository repository, ILogger<StopController> logger,
            CoordinateService coordinateService)
        {
            _repository = repository;
            _logger = logger;
            _coordinateService = coordinateService;
        }

        public JsonResult Get(string tripName)
        {
            try
            {
                var results = _repository.GetTripByName(tripName);

                if (results == null)
                {
                    Response.StatusCode = (int) HttpStatusCode.NotFound;
                    return Json("No trips by that name");
                }


                Response.StatusCode = (int) HttpStatusCode.OK;
                return Json(Mapper.Map<IEnumerable<StopViewModel>>(results.Stops.OrderBy(x => x.Order)));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops for the trip {tripName}", ex);
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json(false);
            }
        }

        [HttpPost("")]
        public async Task<JsonResult> Post(string tripName, [FromBody] StopViewModel vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);


                    var coordinateResult = await _coordinateService.Lookup(newStop.Name);
                    if (!coordinateResult.Success)
                    {
                        Response.StatusCode = (int) HttpStatusCode.NotFound;
                        return Json($"Couldn't locate the city, Message: {coordinateResult.Message}");
                    }

                    newStop.Longitude = coordinateResult.Longitude;
                    newStop.Latitude = coordinateResult.Latitude;

                    _repository.AddStop(newStop, tripName);
                    if (_repository.SaveAll())
                    {
                        Response.StatusCode = (int) HttpStatusCode.Created;
                        return Json(Mapper.Map<StopViewModel>(newStop));
                    }

                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save the new stop", ex);
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                return Json("Failboat");
            }
            return Json(null);
        }
    }
}