using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalksAPI.Repositories;

namespace WalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize("Roles=reader")]//reader only can access
        public async Task<IActionResult> GetAll()
        {
            var walkDifficulties= await walkDifficultyRepository.GetAllAsync();

            //domain to DTO
            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);
            return Ok(walkDifficultiesDTO);
        }
        [HttpGet]
        [Route("{id:guid}")] //if route not provided then it won't work
        [ActionName("GetById")]
        [Authorize("Roles=reader")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var walkDifficulty= await walkDifficultyRepository.GetAsync(id);
            if(walkDifficulty==null)
                 return NotFound();
            //domain to DTO
            var walkDifficultyDTO= mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }
        [HttpPost]
        [Authorize("Roles=writer")]
        public async Task<IActionResult> CreateWalkDifficulty(Models.DTO.AddWalkDifficulty addWalkDifficulty)
        {
            //if(! ValidateCreateWalkDifficulty(addWalkDifficulty))
            //{
            //    return BadRequest(ModelState);
            //}
            //dto to model
            //if you want to use mapper then create another domain model with code property only
            var newwalkDifficulty = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficulty.Code
            };
            newwalkDifficulty=await walkDifficultyRepository.CreateAsync(newwalkDifficulty);
            //domain to dto
            var newwalkDifficultyDTO=mapper.Map<Models.DTO.WalkDifficulty>(newwalkDifficulty);
            //Return response
            // return Ok(newwalkDifficultyDTO);
            return CreatedAtAction(nameof(GetById), 
                new { id = newwalkDifficultyDTO.Id }, newwalkDifficultyDTO);
            
        }
        [HttpPut]
        public async Task<IActionResult> Update(Guid id,Models.DTO.UpdateWalkDifficulty updateWalkDifficulty)
        {
            if (!ValidateUpdate(updateWalkDifficulty))
            {
                return BadRequest(ModelState);
            }
            //Dto to domain
            var walkDifficulty = new Models.Domain.WalkDifficulty
            {
                Code=updateWalkDifficulty.Code
            };

            walkDifficulty = await walkDifficultyRepository.UpdateAsync(id, walkDifficulty);
            if (updateWalkDifficulty == null)
                return NotFound();

            //Domian to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);

        }
        [HttpDelete]
        //if route not provided then it will work
        public async Task<IActionResult> DeleteWalkDifficulty(Guid id)
        {
            var walkDifficulty=await walkDifficultyRepository.DeleteAsync(id);
            if (walkDifficulty == null)
                return NotFound();
            //Domain to DTO
            var walkDifficultyDTO=mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }
        #region Private metods
        private bool ValidateCreateWalkDifficulty(Models.DTO.AddWalkDifficulty addWalkDifficulty)
        {
            if(addWalkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkDifficulty),
                    $"{nameof(addWalkDifficulty)} cannot be null");
                return false;
            }
            if(string.IsNullOrWhiteSpace(addWalkDifficulty.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficulty.Code),
                    $"{nameof(addWalkDifficulty.Code)} cannt be null");
            }
            if (ModelState.ErrorCount > 0)
                return false;
            return true;

        }
        private bool ValidateUpdate(Models.DTO.UpdateWalkDifficulty updateWalkDifficulty)
        {
            if (updateWalkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficulty),
                    $"{nameof(updateWalkDifficulty)} cannot be null");
                return false;
            }
            if (string.IsNullOrWhiteSpace(updateWalkDifficulty.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficulty.Code),
                    $"{nameof(updateWalkDifficulty.Code)} cannt be null");
            }
            if (ModelState.ErrorCount > 0)
                return false;
            return true;

        }

        #endregion
    }
}
