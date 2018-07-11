using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeginScreen
{
    public class ENTValidationError
    {
        public ENTValidationError() { }

        public string ErrorMessage { get; set; }

        #region ENTValidationErrors

        /// <summary>
        /// 一个包含了验证错误信息的自定义类列表，用户可以一次接收到合部验证错误信息
        /// </summary>
        public class ENTValidationErrors : List<ENTValidationError>
        {
            public void Add(string errorMessage)
            {
                base.Add(new ENTValidationError { ErrorMessage = errorMessage });
            }
        }

        #endregion ENTValidationErrors
    }
}