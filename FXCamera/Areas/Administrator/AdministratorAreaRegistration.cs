using System.Web.Mvc;

namespace FXCameraDbConText.Areas.Administrator
{
    public class AdministratorAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Administrator";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //Đăng nhập
            context.MapRoute(
                "Login",
                "Auth/login",
                new { action = "Login", controller = "Auth", id = UrlParameter.Optional }
            );
            //Đăng xuất
            context.MapRoute(
              "Logout",
              "Auth/logout",
              new { action = "Logout", controller = "Auth", id = UrlParameter.Optional }
            );
            //Trang chính
            context.MapRoute(
             "Admin",
             "Admin",
             new { action = "Index", controller = "Admin", id = UrlParameter.Optional }
             );
            //Thêm tài khoản
            context.MapRoute(
             "SignUp",
             "Auth/signup",
             new { action = "SignUp", controller = "Auth", id = UrlParameter.Optional }
             );          
            context.MapRoute(
                "Administrator_default",
                "Administrator/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}