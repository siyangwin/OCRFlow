using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.MvcCore.Extension.Filter
{
    public class ModelStateFilter: IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                string message = "";
                foreach (var item in context.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        message += error.ErrorMessage + "|";
                    }
                }
                message = message.TrimEnd(new char[] { ' ', '|' });
                throw new Exception(message);
            }
        }
    }
}
