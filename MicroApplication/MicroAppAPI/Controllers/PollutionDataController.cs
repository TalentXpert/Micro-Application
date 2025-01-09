using BaseLibrary.Controls.Dashboard;
using MicroAppAPI.Domain;
using MicroAppAPI.Services.Interfaces;

namespace MicroAppAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]

    public class PollutionDataController : BaseController
    {
        public ILoggerFactory LoggerFactory { get; }

        public PollutionDataController(IServiceFactory serviceFactory, ILoggerFactory loggerFactory) : base(serviceFactory, loggerFactory.CreateLogger<PollutionDataController>())
        {
            LoggerFactory = loggerFactory;
        }

        /// <summary>
        /// This API returns user saved filters and filter controls for given global control value if present 
        /// </summary>
        [HttpPost]
        public IActionResult Post([FromBody] PollutionDataVM vm)
        {
            try
            {
                if (vm.Key == Guid.Parse("809E0B38-7D5C-46AB-8A2E-88355AFE4FA1"))
                {
                    var pd = PollutionData.Create(vm.Data, vm.SensorId);
                    if(pd.NoisePollution!=null)
                    {
                        SF.RepositoryFactory.PollutionDataRepository.Add(pd);
                        CommitTransaction();
                    }
                    return Ok(true);
                }
                throw new ValidationException("Provided api key is not valid.");
            }
            catch (Exception exception)
            {
                RollbackTransaction();
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }

        [HttpGet("Get/{key}")]
        public IActionResult Get(Guid key)
        {
            try
            {
                if (key == Guid.Parse("809E0B38-7D5C-46AB-8A2E-88355AFE4FA1"))
                {
                    var result = new List<PollutionDataVM>();
                    var pds = SF.RepositoryFactory.PollutionDataRepository.GetAll().OrderByDescending(d=>d.CreatedOn).Take(100).ToList();
                    foreach ( var p in pds )
                    {
                        result.Add(new PollutionDataVM { Data = p.NoisePollution.ToString(), SensorId = p.SensorId });
                    }
                    CommitTransaction();
                    return Ok(result);
                }
                throw new ValidationException("Provided api key is not valid.");
            }
            catch (Exception exception)
            {
                return HandleException(exception, CodeHelper.CallingMethodInfo());
            }
        }
    }
}