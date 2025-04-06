using Project.Model.EnumModel;
using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.IService.App
{
    /// <summary>
    /// OCR-Demo
    /// </summary>
    public interface IOCRService
    {
        /// <summary>
        ///  测试OCR
        /// </summary>
        /// <param name="type">1.使用普通模式识别  2.使用表格模式识别</param>
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        ResultModel<string> CheckOcr(int type,string path);
    }
}
