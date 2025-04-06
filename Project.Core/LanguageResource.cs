using Project.Model.EnumModel;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using System.Xml;

namespace Project.Core
{
    /// <summary>
    /// 语言资源
    /// </summary>
    public class LanguageResource
    {
        public LanguageResource()
        {

        }
        private ResourceManager resourceManager;
        public LanguageResource(ResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }

        /// <summary>
        /// 根据语言获取资源内容
        /// </summary>
        /// <param name="name"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string Get(string name, LanguageEnum language)
        {
            string result = string.Empty;
            if (language == LanguageEnum.CN)
            {
                //result = name;
                result = GetByXml(name);
            }
            else
            {
                result = resourceManager.GetString(name);
            }
            return !string.IsNullOrEmpty(result) ? result : name;
        }

        /// <summary>
        /// 根据xml获取中文
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetByXml(string name)
        {
            string result = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("Config/LanguageResource.resx");
            var nodes = xmlDoc.SelectNodes("root/data");
            foreach (XmlNode row in nodes)
            {
                if (row.Attributes["name"].InnerText == name)
                {
                    result = row.SelectSingleNode("comment")?.InnerText;
                    break;
                }
            }
            return result;
        }
    }
}
