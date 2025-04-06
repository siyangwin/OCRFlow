using Project.Core;
using Project.Model.Table;
using Project.Model.View;
using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Service
{
    /// <summary>
    /// 授权令牌 Core校验
    /// </summary>
    public class AuthorizationTokenCoreService : IAuthorizationTokenCoreService
    {

        public AuthorizationTokenCoreService()
        {
        }

        /// <summary>
        /// 校验Token是否有效
        /// </summary>
        /// <param name="Token">传入Token</param>
        /// <returns></returns>
        public ResultModel<AuthorizationTokenResDto> CheckAuthorizationToken(string Token)
        {
            Repository repository = new Repository();
            ResultModel<AuthorizationTokenResDto> resultModel = new ResultModel<AuthorizationTokenResDto>();
            //获取当前用户有效的Token
            var AuthorizationToken = repository.QuerySet<vm_app_CheckAuthorizationToken>()
              .Where(x => x.Token == Token)
              .Get(x => new AuthorizationTokenResDto
              {
                  UserId = x.UserId,
                  Token = x.Token
              });

            //验证用户是否有Token
            if (AuthorizationToken != null)
            {
                resultModel.data = AuthorizationToken;
            }
            else
            {
                resultModel.success = false;
            }

            return resultModel;
        }



        /// <summary>
        /// 登出的时候，清除数据库有效Token
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public bool DeleteAuthorizationToken(int UserId)
        {
            Repository repository = new Repository();
            bool res = false;
            //删除之前的用户Token数据
            bool result = repository.CommandSet<AuthorizationToken>()
            .Where(x => x.IsDelete == false && x.UserId == UserId)
            .Update(x => new AuthorizationToken
            {
                IsDelete = true,
                UpdateTime = DateTime.Now,
                UpdateUser = UserId.ToString()
            }) > 0;

           return res;
        }
    }
}
