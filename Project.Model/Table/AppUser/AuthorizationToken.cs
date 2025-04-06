using Kogel.Dapper.Extension.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Model.Table
{
    /// <summary>
    /// 授权Token
    /// </summary>
    [Display(Rename = "AuthorizationToken")]
    public class AuthorizationToken : BaseModel
    {
        #region Model
        /// <summary>
        /// 用户Id(关联用户表)
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 登录令牌
        /// </summary>
        public string Token { get; set; }
        #endregion

    }
}
