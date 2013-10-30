using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace Beauty.Web.Models
{


    public class LoginModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }

    public class UpdatePasswordModel
    {
        [Display(Name = "旧密码")]
        public string OldPassword { get; set; }
        
        [Required]
        [StringLength(30, ErrorMessage = "密码必须在6-30之内", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码确认")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "密码不匹配")]
        public string ConfirmPassword { get; set; }
    }
   

    public class RegisterModel
    {
        [Required(ErrorMessage = "用户名不能空")]
        [Display(Name = "用户名")]
        [Remote("IsExistUsername", "check", ErrorMessage = "用户名已经被别人使用")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能空")]
        [StringLength(30, ErrorMessage = "密码必须在6-30之内", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码确认")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "密码不匹配")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "邮箱不能空")]
        [Display(Name = "邮箱")]
        [StringLength(50,ErrorMessage="邮箱长度不对")]
        [Remote("IsxistEmail", "check", ErrorMessage = "邮箱已经被别人使用")]
        public string Email { get; set; }

        public string QQ { get; set; }

        [Required(ErrorMessage = "手机不能空")]
        [Display(Name = "手机")]
        [StringLength(50, ErrorMessage = "手机长度不对")]
        public string Mobile { get; set; }

        [Display(Name = "省")]
        public string Province { get; set; }
        [Display(Name = "城市")]
        public string City { get; set; }
        [Display(Name = "网店网址")]
        public string ShopAddress { get; set; }

        [Display(Name = "邀请码")]
        [Required(ErrorMessage = "邀请码不能空")]
        public string Refer { get; set; }

        public Decimal Point { get; set; }
        public int Role { get; set; }

        [Required( ErrorMessage="支付宝不能空")]
        [Display(Name = "支付宝")]
        [StringLength(50, ErrorMessage = "支付宝长度不对")]
        public string ZFB { get; set; }

        [Display(Name = "银行卡号")]
        public string Card { get; set; }
        [Display(Name = "银行")]
        public string Bank { get; set; }


    }

    public class WebUser
    {
        
        
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "密码必须在6-30之内", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }


        [Required]
        [Display(Name = "邮箱")]
        [StringLength(50, ErrorMessage = "邮箱长度不对")]
        public string Email { get; set; }

        public string QQ { get; set; }

        [Required]
        [Display(Name = "手机")]
        [StringLength(50, ErrorMessage = "手机长度不对")]
        public string Mobile { get; set; }

        [Display(Name = "省")]
        public string Province { get; set; }
        [Display(Name = "城市")]
        public string City { get; set; }
        [Display(Name = "网店网址")]
        public string ShopAddress { get; set; }
        [Display(Name = "推荐人")]
        public string Refer { get; set; }

        [Display(Name = "积分")]
        public Decimal Point { get; set; }
        [Display(Name = "冻结积分")]
        public Decimal FreezePoint { get; set; }
        [Display(Name = "余额")]
        public Decimal Balance { get; set; }
       [Display(Name = "角色")]
        public string Role { get; set; }

        [Required]
        [Display(Name = "支付宝")]
        [StringLength(50, ErrorMessage = "支付宝长度不对")]
        public string ZFB { get; set; }

        [Display(Name = "银行卡号")]
        public string Card { get; set; }
        [Display(Name = "银行")]
        public string Bank { get; set; }
        [Display(Name = "返利比例")]
        public string Rate { get; set; }

        [Display(Name = "每天最多送积分")]
        public decimal? MaxPoint { get; set; }
        [Display(Name = "每小时送积分")]
        public decimal? TimePoint { get; set; }

    }

    public class ResetPasswordModel
    {
        [Required]
        [StringLength(30, ErrorMessage = "密码必须在6-30之内", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码确认")]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "密码不匹配")]
        public string ConfirmPassword { get; set; }
    }

 
}
