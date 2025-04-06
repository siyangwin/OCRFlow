using Project.Core;
using Project.IService;
using Project.IService.App;
using Project.Model.EnumModel;
using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PaddleOCRSharp;
using System.Drawing;
using System.Security.Cryptography;
using Spire.Pdf.Exporting.XPS.Schema;
using System.Net.Http;

namespace Project.Service.App
{
    /// <summary>
    /// OCR-Demo
    /// </summary>
    public class OCRService: IOCRService
    {
        //数据库
        private IRepository connection;

        //语言包
        ResourceManage languageResource;

        //读取配置文件
        IConfigService configService;

        /// <summary>
        /// 注入
        /// </summary>
        public OCRService(IRepository connection, ResourceManage languageResource, IConfigService configService)
        {
            this.connection = connection;
            this.languageResource = languageResource;
            this.configService = configService;
        }

        //初始化普通OCR组件
        private PaddleOCREngine engine;

        //初始化表格OCR组件
        private PaddleStructureEngine structengine;


        //string outpath = Path.Combine(Environment.CurrentDirectory, "out");

        /// <summary>
        ///  测试OCR
        /// </summary>
        /// <param name="type">1.使用普通模式识别  2.使用表格模式识别</param>
        /// <param name="path">图片路径</param>
        /// <returns></returns>
        public ResultModel<string> CheckOcr(int type,string path)
        {
            ResultModel<string> result = new ResultModel<string>();

            ////创建文件夹
            //if (!Directory.Exists(outpath))
            //{ Directory.CreateDirectory(outpath); }


            //自带轻量版中英文模型V4模型
            OCRModelConfig config = null;

            ////服务器中英文模型v2
            //// OCRModelConfig config = new OCRModelConfig();
            ////string root = PaddleOCRSharp.EngineBase.GetRootDirectory();
            ///你的模型绝对路径文件夹
            ////string modelPathroot = root + @"\inferenceserver";
            ////config.det_infer = modelPathroot + @"\ch_ppocr_server_v2.0_det_infer";
            ////config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            ////config.rec_infer = modelPathroot + @"\ch_ppocr_server_v2.0_rec_infer";
            ////config.keys = modelPathroot + @"\ppocr_keys.txt";

            ////英文和数字模型V3
            ////OCRModelConfig config = new OCRModelConfig();
            ////string root = PaddleOCRSharp.EngineBase.GetRootDirectory();
            ///你的模型绝对路径文件夹
            ////string modelPathroot = root + @"\en_v3";
            ////config.det_infer = modelPathroot + @"\en_PP-OCRv3_det_infer";
            ////config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            ////config.rec_infer = modelPathroot + @"\en_PP-OCRv3_rec_infer";
            ////config.keys = modelPathroot + @"\en_dict.txt";

            ////中英文模型V3
            ////config = new OCRModelConfig();
            ////string root = EngineBase.GetRootDirectory();
            ///你的模型绝对路径文件夹
            ////string modelPathroot = root + @"\inference";
            ////config.det_infer = modelPathroot + @"\ch_PP-OCRv3_det_infer";
            ////config.cls_infer = modelPathroot + @"\ch_ppocr_mobile_v2.0_cls_infer";
            ////config.rec_infer = modelPathroot + @"\ch_PP-OCRv3_rec_infer";
            ////config.keys = modelPathroot + @"\ppocr_keys.txt";



            //使用默认参数
            PaddleOCRSharp.OCRParameter oCRParameter = new PaddleOCRSharp.OCRParameter();
   
            //初始化OCR引擎
            //建议程序全局初始化一次即可，不必每次识别都初始化，容易报错。     
            //PaddleOCRSharp.PaddleOCREngine engine = new PaddleOCRSharp.PaddleOCREngine(config, oCRParameter);
            engine = new PaddleOCREngine(config, "");

            //识别结果对象
            PaddleOCRSharp.OCRResult ocrResult = new PaddleOCRSharp.OCRResult();


            //模型配置，使用默认值
            StructureModelConfig structureModelConfig = null;

            //表格识别参数配置，使用默认值
            StructureParameter structureParameter = new StructureParameter();
            //初始化表格识别引擎
            structengine = new PaddleStructureEngine(structureModelConfig, structureParameter);


            try
            {

                //判断是否有内容
                if (string.IsNullOrWhiteSpace(path))
                {
                    result.data = "没找到本地图片";
                    return result;
                }

                byte[] imagebyte = null;
                if (File.Exists(path)) // 本地文件
                {
                    imagebyte= File.ReadAllBytes(path);
                }
                else if (Uri.IsWellFormedUriString(path, UriKind.Absolute)) // 网络图片
                {
                    using (HttpClient client = new HttpClient())
                    {
                        try
                        {
                            imagebyte= client.GetByteArrayAsync(path).Result;
                        }
                        catch (Exception)
                        {
                            result.data = "没找到网络图片";
                            return result;
                        }
                    }
                }


                //记录开始时间
                DateTime Startdt = DateTime.Now;
                if(type == 1)
                {
                    //实现OCR文字识别  //识别结果对象
                    ocrResult = engine.DetectText(imagebyte);

                    //判断是否有内容
                    if (ocrResult == null)
                    {
                        result.data = "没有识别出内容";
                        return result;
                    }

                    result.data = ocrResult.JsonText;
                }
                else if(type == 2)
                {
                    //表格识别，返回结果是html格式的表格形式
                    string structresult = structengine.StructureDetect(imagebyte);

                    //判断是否有内容
                    if (structresult == null)
                    {
                        result.data = "没有识别出内容";
                        return result;
                    }

                    //添加边框线，方便查看效果
                    string css = "<style>table{ border-spacing: 0pt;} td { border: 1px solid black;}</style>";
                    structresult = structresult.Replace("<html>", "<html>" + css);
                    result.data = structresult;
                }
                else
                {
                    result.data = "未选择正确的模型";
                    return result;
                }
               
                //记录结束时间
                DateTime Enddt = DateTime.Now;


                //输出耗费时间
                result.message = $"耗时：{(Enddt - Startdt).TotalMilliseconds}ms\n";
                
            }
            catch (Exception ex)
            {

                result.data=ex.Message;
            }
           
            return result;
        }
    }
}
