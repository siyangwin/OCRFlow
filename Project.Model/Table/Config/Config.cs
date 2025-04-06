using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Model.Table
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class Config:BaseModel
    {
        #region Model
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
