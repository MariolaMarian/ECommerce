using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities.OrderAgregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        ///<summary>
        /// To retrive paginated orders created by currently logged user
        ///</summary>
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<Pagination<OrderToReturnDTO>>> GetOrdersForUser([FromQuery] PaginationSpecParams paginationParams)
        {
            OrderSpecParams orderParams = _mapper.Map<OrderSpecParams>(paginationParams);
            orderParams.Email = HttpContext.User.GetEmailFromPrincipal();

            var orders = await _orderService.GetOrdersForUserAsync(orderParams);
            var data = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);

            var totalCount = await _orderService.GetCountOrdersForUserAsync(orderParams);

            return Ok(new Pagination<OrderToReturnDTO>(orderParams.PageIndex, orderParams.PageSize, totalCount, data));
        }

        ///<summary>
        /// To retrive product by it's id, user can only see his order
        ///</summary>
        ///<response code="200">If order was found</response>
        ///<response code="400">If order was not found - order with this id does not exist or it does not belong to currently logged user</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrderToReturnDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderByIdForUser(int id)
        {
            var email = HttpContext.User.GetEmailFromPrincipal();

            var order = await _orderService.GetOrderByIdAsync(id,email);

            if(order == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(_mapper.Map<OrderToReturnDTO>(order));
        }

        ///<summary>
        /// To retrive delivery methods
        ///</summary>
        [AllowAnonymous]
        [HttpGet("deliveryMethods")]
        [Produces("application/json")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }

        ///<summary>
        /// To place new order
        ///</summary>
        ///<response code="200">If order was successfully created</response>
        ///<response code="400">If an error occured while trying to create new order</response>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(OrderDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Order>> CreateOrder(OrderDTO orderDTO)
        {
            var email = HttpContext.User.GetEmailFromPrincipal();

            var adress = _mapper.Map<AdressDTO, Adress>(orderDTO.ShipToAdress);
            var order = await _orderService.CreateOrderAsync(email, orderDTO.DeliveryMethodId, orderDTO.BasketId, adress);

            if(order == null )
            {
                return BadRequest(new ApiResponse(400, "Problem creating order"));
            }

            return Ok(order);
        }
    }
}