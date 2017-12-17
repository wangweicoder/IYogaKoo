using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IYogaKoo.ViewModel.Commons.Enums
{

    /// <summary>
    /// 瑜友状态
    /// </summary>
    public enum Ustatus
    {
        未激活 = 0,
        开启 = 1,
        冻结 = 2,
        注销 = 3
    }
    /// <summary>
    /// 瑜友类别
    /// </summary>
    public enum UserType
    {
        瑜伽会员 = 0,
        瑜伽导师 = 1,
        瑜伽会馆 = 2,
    }
    /// <summary>
    /// 登录类型
    /// </summary>
    public enum LoginType
    {
        普通 = 0,
        手机 = 1,
        QQ = 2,
        微信 = 3,
        Sina微博 = 4,
    }
    /// <summary>
    /// 图片类型
    /// </summary>
    public enum PictureType
    {
        头像 = 0,
        资质 = 1,
    }
    public enum Gender
    {
        未设置 = 0,
        男 = 1,
        女 = 2
    }
    /// <summary>
    /// 会员级别
    /// </summary>
    public enum MemberLevel
    {
        基础习练者 = 0,
        初级习练者 = 1,
        中级习练者 = 2,
        高级习练者 = 3,
        大师 = 4
    }
    /// <summary>
    /// 老师级别
    /// </summary>
    public enum TeacherLevel
    {
        初级老师 = 1,
        中级老师 = 2,
        高级老师 = 3,
        大师 = 4,
        资深导师=5,
        知名导师=6
    }
    /// <summary>
    /// 老师状态
    /// </summary>
    public enum TeacherStatus
    {
        申请中 = 0,
        普通导师 = 1,
        合约导师 = 3,
        撤销合约 = 4,
        大师 = 5,
        降级为普通习练者 = 9
    }
    /// <summary>
    /// 头像规格
    /// </summary>
    public enum AvatarSize
    {
        original,
        small = 32,
        mid = 64,
        large = 128
    }
    /// <summary>
    /// 图片评论权限
    /// </summary>
    public enum AllowComment
    {
        所有人能评论,
        仅好友能评论,
        所有人不能评论
    }
    /// <summary>
    /// 各种开关
    /// </summary>
    public enum Is
    {
        仅自己可见 = 0,
        好友可见 = 1,
        所有人可见 = 2
    }
    public enum ClassType
    {
        @class = 1,
        activity = 2,
        train = 3
    }
    public enum TimeUnit
    {
        小时 = 1,
        天
    }
    public enum ClassStatus
    {
        审核中 = 1,
        报名中 = 2,
        结束 = 3,
        撤销 = 4
    }
    public enum LevelOrderType
    {
        习练者到老师,
        老师到老师,
        习练者到习练者
    }
    public enum LevelOrderState
    {
        申请中,
        通过,
        未通过
    }
}
 
