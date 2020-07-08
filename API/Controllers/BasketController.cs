using System.Threading.Tasks;
using API.DTOs;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        ///<summary>
        /// To reveive products in basket, if there is no basket with specified it, new one is created and returned
        ///</summary>
        [HttpGet]        
        [Produces("application/json")]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        ///<summary>
        /// To update existing basket or set new basket
        ///</summary>
        ///<response code="200">Returns updated or created basket</response>
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDTO basket)
        {
            var updatedBasket = await _basketRepository.UpdateBasketAsync(_mapper.Map<CustomerBasket>(basket));

            return Ok(updatedBasket);
        }

        ///<summary>
        /// To delete basket specified by id
        ///</summary>
        ///<response code="200">Returns even if basket with this id did not exist, OK means there is no basket with this id</response>
        [HttpDelete]
        public async Task<IActionResult> DeleteBasketAsync(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);

            return Ok();
        }
    }
}