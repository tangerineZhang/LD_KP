//******************************************************************************
//文 件 名： UserJsonAttrs
//版权所有： 蓝思创工作室
//创 建 人： 蓝思创
//创建日期： 2016-05-08
//网    址：https://shop112893715.taobao.com/
//功能描述： 用户的扩展信息列表
//--------------------------------------------------------------------------------
//修改人：
//修改原因：
//修改日期:

//******************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlueThink.User.Class
{
    public class UserJsonAttrs
    {

        public UserJsonAttrs()
        {
            this.attrs = new List<UserJsonAttrsInfo>();
        }


        /// <summary>
        /// 扩展信息
        /// </summary>
        public List<UserJsonAttrsInfo> attrs { get; set; }
    }
}
