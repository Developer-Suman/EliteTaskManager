using AutoMapper;
using Web.Configs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.BLL.Services.Interface;
using Project.DLL.Models;
using System.Text.Json;

namespace Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowAllOrigins")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize]
    public class UtilityController : WebBaseController
    {

        private readonly IProvinceRepository _provinceRepository;
        private readonly IDistrictRepository _districtRepository;
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly IVDCRepository _vdcRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCacheRepository _memoryCacheRepository;

        public UtilityController(
            IMemoryCacheRepository memoryCacheRepository,
            IVDCRepository vDCRepository,
            IMunicipalityRepository municipalityRepository,
            IDistrictRepository districtRepository,IProvinceRepository provinceRepository,
            IMapper mapper,
            UserManager<ApplicationUsers> userManager,
            RoleManager<IdentityRole> roleManager) : base(mapper,userManager, roleManager)
        {
            _municipalityRepository = municipalityRepository;
            _vdcRepository = vDCRepository;
            _districtRepository = districtRepository;
            _memoryCacheRepository = memoryCacheRepository;
            _mapper = mapper;
            _provinceRepository = provinceRepository;

        }

        [HttpGet("Province")]
        public async Task<IActionResult> GetAll()
        {
            var getallProvinceData = await _provinceRepository.GetAll();
            #region switch
            return getallProvinceData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getallProvinceData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getallProvinceData.Errors),
                _ => BadRequest("Invalid some Fields")
            };


            #endregion
        }

        [HttpGet("Province/{ProvinceId}")]
        public async Task<IActionResult> GetById([FromRoute] int ProvinceId)
        {
        
            var getByIdProvinceData = await _provinceRepository.GetById(ProvinceId);

            #region switch
            return getByIdProvinceData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getByIdProvinceData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getByIdProvinceData.Errors),
                _ => BadRequest("Invalid Some Fields")
            };

            #endregion
        }

        [HttpGet("District/{districtId}")]
        public async Task<IActionResult> GetDistrictById([FromRoute] int districtId)
        {
            var getByIdDistrictData = await _districtRepository.GetById(districtId);
            #region Switch
            return getByIdDistrictData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getByIdDistrictData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getByIdDistrictData.Errors),
                _ => BadRequest(" invalid some Fields")
            };
            #endregion
        }

        [HttpGet("District")]
        public async Task<IActionResult> GetAllDistrict()
        {
            var getDistrictData = await _districtRepository.GetAll();

            #region Switch
            return getDistrictData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getDistrictData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getDistrictData.Errors),
                _ => BadRequest("Invalid some Fields")
            };
            #endregion
        }

        [HttpGet("District/Province/{ProvinceId}")]
        public async Task<IActionResult> GetDistrictByProvinceId([FromRoute] int ProvinceId)
        {
            var getDistrictData = await _districtRepository.GetByProvinceId(ProvinceId);

            #region Switch
            return getDistrictData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getDistrictData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess : false, Errors: not null } => HandleFailureResult(getDistrictData.Errors),
                _ => BadRequest("Invalid some Field")
            };;

            #endregion
        }


        [HttpGet("Municipality/District/{DistrictId}")]
        public async Task<IActionResult> GetMunicipalityByDistrictId([FromRoute] int DistrictId)
        {
            var getMunicipalData = await _municipalityRepository.GetByDistrictId(DistrictId);

            #region Switch
            return getMunicipalData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getMunicipalData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getMunicipalData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }


        [HttpGet("Municipality/{MunicipalityId}")]
        public async Task<IActionResult> GetMunicipalityBytId([FromRoute] int MunicipalityId)
        {
            var getMunicipalData = await _municipalityRepository.GetById(MunicipalityId);

            #region Switch
            return getMunicipalData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getMunicipalData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getMunicipalData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }


        [HttpGet("Municipality")]
        public async Task<IActionResult> GetAllMunicipality()
        {
            var getMunicipalData = await _municipalityRepository.GetAll();
            #region Switch
            return getMunicipalData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getMunicipalData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getMunicipalData.Errors),
                _ => BadRequest("Invalid some Field")
            };
            #endregion
        }

        [HttpGet("VDC/District/{DistrictId}")]
        public async Task<IActionResult> GetVDCByDistrictId([FromRoute] int DistrictId)
        {
            var getVDCData = await _vdcRepository.GetByDistrictId(DistrictId);

            #region Switch
            return getVDCData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getVDCData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getVDCData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }



        [HttpGet("VDC")]
        public async Task<IActionResult> GetAllVDC()
        {
            var getVDCData = await _vdcRepository.GetAll();

            #region Switch
            return getVDCData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getVDCData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getVDCData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }



        [HttpGet("VDC/{vdcId}")]
        public async Task<IActionResult> GetVDCBytId([FromRoute] int vdcId)
        {
            var getVDCData = await _vdcRepository.GetById(vdcId);

            #region Switch
            return getVDCData switch
            {
                { IsSuccess: true, Data: not null } => new JsonResult(getVDCData.Data, new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping // Ensure Unicode characters are not escaped
                }),
                { IsSuccess: false, Errors: not null } => HandleFailureResult(getVDCData.Errors),
                _ => BadRequest("Invalid some Field")
            };

            #endregion
        }
    }
}
