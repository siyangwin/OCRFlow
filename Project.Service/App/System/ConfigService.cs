using Project.IService;
using Project.Model;
using Project.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Service
{
    public class ConfigService : IConfigService
    {
        IRepository repository;
        public ConfigService(IRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 根据名称获取配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="isLike">是否模糊查询</param>
        /// <returns></returns>
        public List<ConfigResDto> Get(string name, bool isLike)
        {
            return repository.QuerySet<vm_app_Config_List>()
                .WhereIf(isLike, x => x.Name.Contains(name), x => x.Name == name)
                .ToList<ConfigResDto>();
        }

    }
}
