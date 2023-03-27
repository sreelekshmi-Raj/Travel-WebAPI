using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.Models.Domain;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers
{
    [ApiController]
    //[Route("walk-region")]//or use [Route("[controller]")]
    [Route("[controller]")]
    //to use authentication use authorize attribute -this tell client that we need a valid token to access the resources
    //authorize attribute to block request from any user who didn't have a token- block all end points from use
    //use in controller level or method level
    //[Authorize]//401 -unauthorized
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;
        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository= regionRepository;
            this.mapper=mapper;
        }
        //not passing any value so no need of validations- validation do for data coming to db
        //validations for protecting the APIs
        [HttpGet]
        [Authorize(Roles ="reader")]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions=await regionRepository.GetAllAsync();
            // return Ok(regions);
            //using DTO to transfer domain model values

            //var regionsDTO =new List<Models.DTO.Region>();
            //foreach(var item in regions)
            //{
            //    var regionDTO=new Models.DTO.Region()
            //    {
            //        Id=item.Id,
            //        Code=item.Code,
            //        Name=item.Name,
            //        Area=item.Area,
            //        Lat=item.Lat,
            //        Long=item.Long,
            //        Population=item.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //}
            //return Ok(regionsDTO);

            //change above code using automapper
            var regionsDTO=mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]//expecting single id field -guid validator in the route so no other validation
        [ActionName("GetRegionAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetRegion(Guid id)
        {
            //domain value
            var region = await regionRepository.GetAsync(id);
            if(region == null)
                return NotFound();
            var regionDTO=mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]//add region request contain some property so validation for that property needed
                  // [Authorize]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddRegion(Models.DTO.AddRegion addRegion)
        {
            //validate addregion request
            //if(!ValidateAddRegion(addRegion))//if it false ->validation fails
            //    return BadRequest();//it automatically bind model state error and show only 400 bad request

            //if(!ValidateAddRegion(addRegion))
            //{
            //    return BadRequest(ModelState);
            //}commented because using fluent validation

            //convert DTO to domain model
            // var region=mapper.Map<Models.Domain.Region>(addRegion);

            var region = new Models.Domain.Region()
            {
                Name = addRegion.Name,
                Code = addRegion.Code,
                Area = addRegion.Area,
                Lat = addRegion.Lat,
                Long = addRegion.Long,
                Population = addRegion.Population,
            };

            //pass data to repository to add in database
            var result = await regionRepository.AddAsync(region);

            //convert back to DTO
            // var regionDTO = mapper.Map<Models.DTO.AddRegion>(result);
            var regionDTO = new Models.DTO.Region()
            {
                Id = result.Id,
                Name = result.Name,
                Code = result.Code,
                Area = result.Area,
                Lat = result.Lat,
                Long = result.Long,
                Population = result.Population,
            };
            //  return Ok(regionDTO);

            //created resource so use CreateAtAction() return http 201 status back to client- action name-GetAllRegions,passing object value
            //send whole object back to regionDTO
            //passing response back to client
            return CreatedAtAction(nameof(GetAllRegions), new { id = regionDTO.Id }, regionDTO);
        
        }
        [HttpDelete]
        [Route("{id:guid}")]//id value taken from route parameter
        //strongly typed id to a guid so only accepting valid guid  ,if id not found then return notfound() error
       // [Authorize]
       [Authorize(Roles ="writer,reader")]
        public async Task<IActionResult> DeleteRegion(Guid id)
        {
            //get region from database
            var region = await regionRepository.DeleteAsync(id);


            //if null return notfound
            if (region == null)
                return NotFound();

            //convert response back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id=region.Id,
                Name=region.Name,
                Code=region.Code,
                Area = region.Area,
                Long = region.Long,
                Lat = region.Lat,
                Population = region.Population,
            };

            //return OK response
           
            return Ok(regionDTO);  

        }
        [HttpPut]
        [Route("{id:guid}")]
        //[Authorize]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute]Guid id,[FromBody]Models.DTO.UpdateRegion updateRegion)
        {
            //either below code for validation or fluent validation can be use
            //if(!ValidateUpdateRegion(updateRegion))
            //{
            //    return BadRequest(ModelState);
            //}
            //convert DTO to Domain model
            var region = new Models.Domain.Region()
            {
                Name = updateRegion.Name,
                Code=updateRegion.Code,
                Area = updateRegion.Area,
                Lat=updateRegion.Lat,
                Long=updateRegion.Long,
                Population=updateRegion.Population

            };

            //update region using repository
             region=await regionRepository.UpdateAsync(id,region);

            //if null then notfound
            if (region == null)
                return NotFound();

            //convert domain back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Name=region.Name,
                Code =region.Code,
                Area=region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };

            //Return Ok response
            return Ok(regionDTO);
        }


        //validation for addregion request-creating private method
        //create a region 
        //private methods are private to this class nobody else can access it
        #region Private methods
        //validate (fullname of method)
        private bool ValidateAddRegion(Models.DTO.AddRegion addRegion)
        {
                //check each propery in addRegion
   
                //check if the whole object is null
                if(addRegion==null)
                {
                    ModelState.AddModelError(nameof(addRegion),
                        $"Add Region Data is required.");
                    return false;
                }
                //use modelstate attribute provided by Asp.net and tell the  error to the user/client
                //it take key and error message - key is parameter name and ten error msg
                //check code is either null /empty/white space
                if (string.IsNullOrWhiteSpace(addRegion.Code))
                {
                    ModelState.AddModelError(nameof(addRegion.Code),
                        $"{ nameof(addRegion.Code)} cannot be null or empty or white space");
                    // return false;
                }
                if (string.IsNullOrWhiteSpace(addRegion.Name))
                {
                    ModelState.AddModelError(nameof(addRegion.Name),
                        $"{nameof(addRegion.Name)} cannot be null or empty or white space");
                }
                if (addRegion.Area<=0)
                {
                    ModelState.AddModelError(nameof(addRegion.Area),
                        $"{nameof(addRegion.Area)} cannot be less than or equal to zero");
                }
                if (addRegion.Lat <= 0)
                {
                    ModelState.AddModelError(nameof(addRegion.Lat),
                        $"{nameof(addRegion.Lat)} cannot be less than or equal to zero");
                }
                if (addRegion.Long <= 0)
                {
                    ModelState.AddModelError(nameof(addRegion.Long),
                        $"{nameof(addRegion.Long)} cannot be less than or equal to zero");
                }
                //population can be 0 but not negative
                if (addRegion.Population < 0)
                {
                    ModelState.AddModelError(nameof(addRegion.Population),
                        $"{nameof(addRegion.Population)} cannot be less than  zero");
                }
                //if there is no error in the validation then pass a true
             
                if(ModelState.ErrorCount>0)//if some error in the model then return false
                {
                    return false;
                }
                return true;
        }
        private bool ValidateUpdateRegion(Models.DTO.UpdateRegion updateRegion)
        {
            if (updateRegion == null)
            {
                ModelState.AddModelError(nameof(updateRegion),
                    $"Add Region Data is required.");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateRegion.Code))
            {
                ModelState.AddModelError(nameof(updateRegion.Code),
                    $"{nameof(updateRegion.Code)} cannot be null or empty or white space");
            }
            if (string.IsNullOrWhiteSpace(updateRegion.Name))
            {
                ModelState.AddModelError(nameof(updateRegion.Name),
                    $"{nameof(updateRegion.Name)} cannot be null or empty or white space");
            }
            if (updateRegion.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegion.Area),
                    $"{nameof(updateRegion.Area)} cannot be less than or equal to zero");
            }
            if (updateRegion.Lat <= 0)
            {
                ModelState.AddModelError(nameof(updateRegion.Lat),
                    $"{nameof(updateRegion.Lat)} cannot be less than or equal to zero");
            }
            if (updateRegion.Long <= 0)
            {
                ModelState.AddModelError(nameof(updateRegion.Long),
                    $"{nameof(updateRegion.Long)} cannot be less than or equal to zero");
            }
            if (updateRegion.Population < 0)
            {
                ModelState.AddModelError(nameof(updateRegion.Population),
                    $"{nameof(updateRegion.Population)} cannot be less than  zero");
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;

        }

        #endregion


    }
}
