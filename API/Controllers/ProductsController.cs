using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<ProductType> productTypesRepo, IGenericRepository<ProductBrand> productBrandsRepo, IGenericRepository<Product> productsRepo, IMapper mapper)
        {
            _productsRepo = productsRepo;
            _mapper = mapper;
        }

        ///<summary>
        /// To retrive paginated products list - cached for 10 minutes
        ///</summary>
        [Cached(600)]
        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDTO>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);

            var products = await _productsRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDTO>>(products);

            return Ok(new Pagination<ProductToReturnDTO>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        ///<summary>
        /// To retrive product specified by it's id - cached for 10 minutes
        ///</summary>
        [Cached(600)]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if(product == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<Product, ProductToReturnDTO>(product));
        }

    }
}