using System.Threading.Tasks;
using API.DTOs;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        /// <summary>
        /// To return simplest informations about authorized user who sent the request
        /// </summary>
        [Authorize]
        [HttpGet]
        [Produces("application/json")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var user = await _userManager.FindUserByClaimsPrinciple(HttpContext.User);

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email,
            };
        }

        /// <summary>
        /// Returns true if there is already user registered with this email, returns false if this email is not used by any of registered users
        /// </summary>
        [HttpGet("emailexists")]
        [Produces("text/plain")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return (await _userManager.FindByEmailAsync(email)) != null;
        }

        ///<summary>
        /// To return adress assigned to currently logged user
        ///</summary>
        [Authorize]
        [HttpGet("adress")]
        [Produces("application/json")]
        public async Task<ActionResult<AdressDTO>> GetUserAdress()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAdressAsync(HttpContext.User);

            return _mapper.Map<AdressDTO>(user.Adress);
        }

        ///<summary>
        /// To update currently logged user's adress
        ///</summary>
        ///<response code="200">If adress was successfully changed</response>
        ///<response code="400">If saving new adress did not finished with success</response>
        [Authorize]
        [HttpPut("adress")]
        [Produces("application/json")]

        public async Task<ActionResult<AdressDTO>> UpdateUserAdress(AdressDTO adress)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAdressAsync(HttpContext.User);

            user.Adress = _mapper.Map<Adress>(adress);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return Ok(_mapper.Map<AdressDTO>(user.Adress));
            }

            return BadRequest(new ApiResponse(400, "Problem updating user adress"));
        }

        ///<summary>
        /// To login = user receives authorization token and simplest informations about himself
        ///</summary>
        /// <response code="200">If user is successfully logged</response>
        /// <response code="401">If user with specified username does not exist or password is incorrect for this username </response>
        [HttpPost("login")]
        [Produces("application/json")]

        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401));
            }

            return Ok(new UserDTO
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            });
        }

        ///<summary>
        /// To register new user
        ///</summary>
        ///<response code="200">If user is successfully registered</response>
        ///<response code="400">If email is already used by another user or an error occured while trying to register new user </response>
        [HttpPost("register")]
        [Produces("application/json")]

        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            var user = new AppUser
            {
                DisplayName = registerDTO.DisplayName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email
            };

            if (CheckEmailExistsAsync(registerDTO.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "This email adress is already registered!" } });
            }

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400));
            }

            return new UserDTO
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email,
            };
        }

    }
}