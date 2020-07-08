using System.Collections.Generic;
using System.Threading.Tasks;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BrandsController : BaseApiController
    {
        private readonly IGenericRepository<ProductBrand> _productBrandsRepo;

        public BrandsController(IGenericRepository<ProductBrand> productBrandsRepo)
        {
            _productBrandsRepo = productBrandsRepo;
        }

        ///<summary>
        /// To retrive products brands - cached for 10 minutes
        ///</summary>
        [Cached(600)]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            var brands = await _productBrandsRepo.ListAllAsync();

            return Ok(brands);
        }
    }
}