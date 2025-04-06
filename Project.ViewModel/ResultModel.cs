using System.Collections.Generic;

namespace Project.ViewModel
{
    /// <summary>
    /// API返回信息
    /// </summary>
    public class ResultModels<T>
    {
        /// <summary>
        /// 返回類型
        /// </summary>
        public ResultModels()
        {
            this.api_version = "v1";
            code = "200";
            this.success = true;
        }
        /// <summary>
        /// 版本號
        /// </summary>
        public string api_version { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 總數量
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 返回數據集合
        /// </summary>
        public List<T> data { get; set; }
    }
    /// <summary>
    /// API返回信息
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 返回類型
        /// </summary>
        public ResultModel()
        {
            this.api_version = "v1";
            code = "200";
            this.success = true;
        }
        /// <summary>
        /// 版本號
        /// </summary>
        public string api_version { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
    }
    /// <summary>
    /// API返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultModel<T>
    {
        /// <summary>
        /// 返回類型
        /// </summary>
        public ResultModel()
        {
            this.api_version = "v1";
            code = "200";
            this.success = true;
        }
        /// <summary>
        /// 版本號
        /// </summary>
        public string api_version { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 返回數據集合
        /// </summary>
        public T data { get; set; }
    }

    public class ResultPageModel<T>
    {
        /// <summary>
        /// 返回類型
        /// </summary>
        public ResultPageModel()
        {
            this.api_version = "v1";
            code = "200";
            this.success = true;
        }
        /// <summary>
        /// 版本號
        /// </summary>
        public string api_version { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 分頁數據
        /// </summary>
        public PageJson<T> data { get; set; }
    }
    /// <summary>
    /// 積分記錄專用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResultPageModel_Point<T>
    {
        /// <summary>
        /// 返回類型
        /// </summary>
        public ResultPageModel_Point()
        {
            this.api_version = "v1";
            code = "200";
            this.success = true;
        }
        /// <summary>
        /// 版本號
        /// </summary>
        public string api_version { get; set; }
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// code
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 分頁數據
        /// </summary>
        public PageJson_Point<T> data { get; set; }
    }
}
