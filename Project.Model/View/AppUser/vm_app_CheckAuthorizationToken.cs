using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Model.View
{
    /// <summary>
    /// 检查当前用户的有效Token
    /// </summary>
    public class vm_app_CheckAuthorizationToken
    {
        #region Model

        /// <summary>
        /// 当前用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 当前用户令牌
        /// </summary>
        public string Token { get; set; }

        #endregion
    }
}
