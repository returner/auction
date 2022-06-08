using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Net.Mime;

namespace Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[WebApiResultMiddleware]
    [Produces(MediaTypeNames.Application.Json)]
    //[RequestDurationLog]
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "성공을 제외하고 정의되지 않은 모든 오류")]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "인증과 관련된 모든 오류")]
    [SwaggerResponse((int)HttpStatusCode.Forbidden, "(외부호출)인증이 필요한 API를 가입되지 않은 사용자가 호출하는경우")]
    public class ApiControllerBase : ControllerBase
    {
    }
}
