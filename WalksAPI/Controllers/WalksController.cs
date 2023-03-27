using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkrepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkrepository,IMapper mapper
            ,IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkrepository = walkrepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }
        //walks api
        [HttpGet]
       // [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalks()
        {
            var walks= await walkrepository.GetAllAsync();
            //Domain model to DTO
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);
            return Ok(walksDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetById")]
       // [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetById(Guid id)
        {
            //Get walk domain object from database
            var walk = await walkrepository.GetWalk(id);
            //Domain to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);
        }
        [HttpPost]
        //[Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalk(Models.DTO.AddWalk addWalk)
        {
            //using both fluent validation(normal body validation) and normal validation(bussiness validation)
            if (!await ValidateAddWalk(addWalk))
            {
                return BadRequest(ModelState);
            }
            //DTO to Domain object , if want to use mapper first create profile for that
            var walkDomain = new Models.Domain.Walk
            {
                Name=addWalk.Name,
                Length=addWalk.Length,
                //RegionId=addWalk.RegionId,
                //WalkDifficultyId=addWalk.WalkDifficultyId,

            };
            //pass domain object to repository to add in walk table 
            var walk =await walkrepository.AddWalkAsync(walkDomain);
            //convert domain back to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            //send DTO back to client
            //return Ok(walkDTO);
            return CreatedAtAction(nameof(GetById), new {id=walkDTO.Id},walkDTO);
        }
        [HttpPut]
        //[Authorize(Roles = "writer")]
        //[Route("{id:guid}")]
        // public async Task<IActionResult> UpdateWalk([FromRoute]Guid id,[FromBody]Models.DTO.AddWalk updateWalk)
        public async Task<IActionResult> UpdateWalk(Guid id, Models.DTO.AddWalk updateWalk)
        {
            if(! await ValidateAddWalk(updateWalk))
            {
                return BadRequest(ModelState);
            }
            //AddWalk dto take similar parameter as update so used Models.DTO.AddWalk
            //dto to domain
            var walk = new Models.Domain.Walk
            {
                Name = updateWalk.Name,
                Length = updateWalk.Length,
                RegionId = updateWalk.RegionId,
                WalkDifficultyId = updateWalk.WalkDifficultyId,

            };
            walk = await walkrepository.UpdateAsync(id, walk);
            if (walk == null)
                return NotFound("Sorry " + id + " walk not available in database");
            //domain to dto
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);
        }
        [HttpDelete]
        //[Authorize(Roles = "writer")]
        //[Route("{id:guid}")] use it or not no difference
        public async Task<IActionResult> DeleteWalk(Guid id)
        {
            var walkDb=await walkrepository.DeleteAsync(id);
            //domain to DTO
            if (walkDb == null)
                return NotFound();
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDb);
            return Ok(walkDTO);
        }
        #region Private methods
        private async Task<bool> ValidateAddWalk(Models.DTO.AddWalk addWalk)
        {
            //if(addWalk == null)
            //{
            //    ModelState.AddModelError(nameof(addWalk),
            //        $"{nameof(addWalk)}can't be null");
            //    return false;
            //}

            //if(String.IsNullOrWhiteSpace(addWalk.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalk.Name),
            //        $"{nameof(addWalk.Name)} cann't be null or empty or whitespace");
            //}
            //if(addWalk.Length<0)
            //{
            //    ModelState.AddModelError(nameof(addWalk.Length),
            //        $"{nameof(addWalk.Length)} cannt be negative number");
            //}
            //regioid & walkdifficultyid both should be valid guid and also exist in db-check regionid is available in db
            //regionrepository contain details of region so inject regionrepository in constructor
            //thses are bussiness valiadtion
            var region = await regionRepository.GetAsync(addWalk.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalk.RegionId),
                    $"{nameof(addWalk.RegionId)} is invalid");
            }

            var walkDiffuiculty = await walkDifficultyRepository.GetAsync(addWalk.WalkDifficultyId);
            if (walkDiffuiculty == null)
            {
                ModelState.AddModelError(nameof(addWalk.WalkDifficultyId),
                    $"{nameof(addWalk.WalkDifficultyId)} is invalid");
            }

            if (ModelState.ErrorCount > 0)
                return false;
            return true;

        }
        #endregion

    }
}
