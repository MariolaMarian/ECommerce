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

        [HttpGet]
        public async Task<ActionResult<Pagination<OrderToReturnDTO>>> GetOrdersForUser([FromQuery] PaginationSpecParams paginationParams)
        {
            OrderSpecParams orderParams = _mapper.Map<OrderSpecParams>(paginationParams);
            orderParams.Email = HttpContext.User.GetEmailFromPrincipal();

            var orders = await _orderService.GetOrdersForUserAsync(orderParams);
            var data = _mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders);

            var totalCount = await _orderService.GetCountOrdersForUserAsync(orderParams);

            return Ok(new Pagination<OrderToReturnDTO>(orderParams.PageIndex, orderParams.PageSize, totalCount, data));
        }

        [HttpGet("{id}")]
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

        [AllowAnonymous]
        [HttpGet("deliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }

        [HttpPost]
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