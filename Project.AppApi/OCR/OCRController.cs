using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.AppApi.Controllers;
using Project.IService.App;
using Project.Model.EnumModel;
using Project.Service.App;
using Project.ViewModel;

namespace Project.AppApi.Areas.OCR
{
    public class OCRController : BaseController
    {
        //获取Service
        IOCRService oCRService;

        /// <summary>
        ///  注入
        /// </summary>
        /// <param name="oCRService"></param>
        public OCRController(IOCRService oCRService)
        {
            this.oCRService = oCRService;
        }

        /// <summary>
        /// 测试OCR
        /// </summary>
        /// <param name="type">1.使用普通模式识别  2.使用表格模式识别</param>
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        [Route("/api/ocr/Check")]
        [HttpGet]
        [AllowAnonymous]
        public ResultModel<string> CheckOcr(int type,string path)
        {
            return oCRService.CheckOcr(type, path);
        }

    }
}
