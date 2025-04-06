using System;
using System.Collections.Generic;
using System.Text;

namespace Project.ViewModel
{
    /// <summary>
    /// APP請求實體
    /// </summary>
    public class PageListReqAppDto
    {
        /// <summary>
		/// 當前頁
		/// </summary>
		public int pageIndex { get; set; } = 1;
        /// <summary>
        /// 顯示數
        /// </summary>
        public int pageSize { get; set; } = 20;
        /// <summary>
        /// 排序字段
        /// </summary>
        public string sort { get; set; } = "Id";
        /// <summary>
        /// 排序方式
        /// </summary>
        public string sortOrder { get; set; } = "asc";
    }
}
