using System;
using Framework.Core.Configuration;
namespace Framework.Web{
public partial class SiteConfig{
private static SiteAgent SiteAgent{get{return SiteAgent.Instance;  } }
public partial class site{
public partial class page{
/// <summary>
/// 页头后缀  
///类型:string  默认值:page tag
/// </summary>
[Config("页头后缀","page tag","string")]
public static string tag{ get{return (string)SiteAgent.GetConfig("site.page.tag");} }
/// <summary>
/// 页头关键字  
///类型:string  默认值:page keys
/// </summary>
[Config("页头关键字","page keys","string")]
public static string keywords{ get{return (string)SiteAgent.GetConfig("site.page.keywords");} }
/// <summary>
/// 页头作者  
///类型:string  默认值:page head author
/// </summary>
[Config("页头作者","page head author","string")]
public static string author{ get{return (string)SiteAgent.GetConfig("site.page.author");} }
/// <summary>
/// 页头描述  
///类型:string  默认值:page head description
/// </summary>
[Config("页头描述","page head description","string")]
public static string description{ get{return (string)SiteAgent.GetConfig("site.page.description");} }
/// <summary>
/// 页脚备案  
///类型:string  默认值:X ICP - XXX
/// </summary>
[Config("页脚备案","X ICP - XXX","string")]
public static string icp{ get{return (string)SiteAgent.GetConfig("site.page.icp");} }
}
}
public partial class company{
/// <summary>
/// 公司名称  
///类型:string  默认值:company name
/// </summary>
[Config("公司名称","company name","string")]
public static string name{ get{return (string)SiteAgent.GetConfig("company.name");} }
/// <summary>
/// 公司电话  
///类型:string  默认值:0000-00000000
/// </summary>
[Config("公司电话","0000-00000000","string")]
public static string telphone{ get{return (string)SiteAgent.GetConfig("company.telphone");} }
/// <summary>
/// 公司地址  
///类型:string  默认值:company address
/// </summary>
[Config("公司地址","company address","string")]
public static string address{ get{return (string)SiteAgent.GetConfig("company.address");} }
/// <summary>
/// 公司邮编  
///类型:string  默认值:000000
/// </summary>
[Config("公司邮编","000000","string")]
public static string mailcode{ get{return (string)SiteAgent.GetConfig("company.mailcode");} }
/// <summary>
/// 公司传真  
///类型:string  默认值:0000-00000000
/// </summary>
[Config("公司传真","0000-00000000","string")]
public static string fax{ get{return (string)SiteAgent.GetConfig("company.fax");} }
/// <summary>
/// 公司邮箱  
///类型:string  默认值:company@company.com
/// </summary>
[Config("公司邮箱","company@company.com","string")]
public static string email{ get{return (string)SiteAgent.GetConfig("company.email");} }
}
public partial class member{
public partial class user{
/// <summary>
/// 用户默认头像  
///类型:string  默认值:face.jpg
/// </summary>
[Config("用户默认头像","face.jpg","string")]
public static string defaultface{ get{return (string)SiteAgent.GetConfig("member.user.defaultface");} }
}
}
}
}
