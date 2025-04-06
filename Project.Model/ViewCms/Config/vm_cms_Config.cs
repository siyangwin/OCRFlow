using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Model.ViewCms
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class vm_cms_Config
    {
        #region Model
        /// <summary>
        /// 自增列
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 配置项目
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 对应的值
        /// </summary>
        public string Values { get; set; }

        /// <summary>
        /// 备注，添加当前配置的说明
        /// </summary>
        public string Remark { get; set; }

        #endregion

    }
}
