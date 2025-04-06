using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Model.ViewCms
{
    /// <summary>
    /// 版本控制
    /// </summary>
    public class vm_cms_VersionManagement
    {
        #region Model

        /// <summary>
        /// 自增列
        /// </summary>

        public int Id { get; set; }

        /// <summary>
        /// APP类型： Android：1  IOS：2
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 请求API地址
        /// </summary>
        public string RequestUrl { get; set; }

        /// <summary>
        /// 不提醒：1    提醒更新：2     提醒并强制更新：3
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 版本提示标题中文
        /// </summary>
        public string Title_CN { get; set; }

        /// <summary>
        /// 版本提示标题英文
        /// </summary>
        public string Title_EN { get; set; }

        /// <summary>
        /// 版本提示内容中文
        /// </summary>
        public string Content_CN { get; set; }

        /// <summary>
        /// 版本提示内容英文
        /// </summary>
        public string Content_EN { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }
        #endregion


    }
}
