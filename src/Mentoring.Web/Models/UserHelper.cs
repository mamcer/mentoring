using System.Web;
using Mentoring.Core;

namespace Mentoring.Web.Models
{
    public class UserHelper
    {
        internal static UserModel GetCurrentUserInfo()
        {
            return HttpContext.Current.Session["UserInfo"] as UserModel;
        }

        public static string GetDefaultAvatarUrl(HttpRequest request)
        {
            var avatarUri = System.Configuration.ConfigurationManager.AppSettings["DefaultAvatarUri"];
            var avatarUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, avatarUri);
            return avatarUrl;
        }

        internal static void SetUserInfo(User user)
        {
            var currentUser = new UserModel(user);

            HttpContext.Current.Session["UserInfo"] = currentUser;
        }

        internal static void SetUserInfo(UserModel user)
        {
            HttpContext.Current.Session["UserInfo"] = user;
        }
    }
}