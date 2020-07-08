using System.Collections.Generic;
using System.Threading.Tasks;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductTypesController : BaseApiController
    {
        private readonly IGenericRepository<ProductType> _productTypesRepo;

        public ProductTypesController(IGenericRepository<ProductType> productTypesRepo)
        {
            _productTypesRepo = productTypesRepo;
        }

        ///<summary>
        /// To retrive products types - cached for 10 minutes
        ///</summary>
        [Cached(600)]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            var types = await _productTypesRepo.ListAllAsync();

            return Ok(types);
        }
    }
}