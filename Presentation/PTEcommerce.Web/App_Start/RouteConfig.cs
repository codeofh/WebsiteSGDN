using System.Web.Mvc;
using System.Web.Routing;

namespace PTEcommerce.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Sign in",
                url: "accounts/sign-in",
                defaults: new { controller = "Login", action = "Index" },
                namespaces: new string[] { "PTEcommerce.Web.Controllers" }
            );
            routes.MapRoute(
                name: "Sign up",
                url: "accounts/sign-up",
                defaults: new { controller = "Login", action = "Register" },
                namespaces: new string[] { "PTEcommerce.Web.Controllers" }
            );
            routes.MapRoute(
               name: "Sign out",
               url: "accounts/sign-out",
               defaults: new { controller = "Customer", action = "Logout" },
               namespaces: new string[] { "PTEcommerce.Web.Controllers" }
           );
            routes.MapRoute(
               name: "Reset password",
               url: "accounts/reset-password",
               defaults: new { controller = "Login", action = "Recovery" },
               namespaces: new string[] { "PTEcommerce.Web.Controllers" }
           );
            routes.MapRoute(
               name: "Contacts",
               url: "contacts",
               defaults: new { controller = "Home", action = "Contacts" },
               namespaces: new string[] { "PTEcommerce.Web.Controllers" }
           );
            routes.MapRoute(
              name: "SocialResponsibility",
              url: "vi/social-responsibility",
              defaults: new { controller = "Vi", action = "SocialResponsibility" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          ); routes.MapRoute(
              name: "regulation",
              url: "vi/regulation",
              defaults: new { controller = "Vi", action = "regulation" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          );
            routes.MapRoute(
              name: "media-room",
              url: "vi/media-room",
              defaults: new { controller = "Vi", action = "mediaroom" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          );
            routes.MapRoute(
              name: "financial-reports",
              url: "vi/financial-reports",
              defaults: new { controller = "Vi", action = "financialreports" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          ); 
            routes.MapRoute(
              name: "legal-documents",
              url: "vi/legal-documents",
              defaults: new { controller = "Vi", action = "legaldocuments" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          );
            routes.MapRoute(
              name: "compensation-fund",
              url: "vi/compensation-fund",
              defaults: new { controller = "Vi", action = "compensationfund" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          );
            routes.MapRoute(
              name: "standard-accounts",
              url: "vi/standard-accounts",
              defaults: new { controller = "Vi", action = "standardaccounts" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          ); routes.MapRoute(
              name: "pro-accounts",
              url: "vi/pro-accounts",
              defaults: new { controller = "Vi", action = "proaccounts" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          ); routes.MapRoute(
              name: "deposits-and-withdrawals",
              url: "vi/deposits-and-withdrawals",
              defaults: new { controller = "Vi", action = "depositsandwithdrawals" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          )
                ; routes.MapRoute(
              name: "exness-trader-app",
              url: "vi/exness-trader-app",
              defaults: new { controller = "Vi", action = "traderapp" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          ); 
            routes.MapRoute(
              name: "exness-webterminal",
              url: "vi/exness-webterminal",
              defaults: new { controller = "Vi", action = "webterminal" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          ); 
            routes.MapRoute(
              name: "metatrader-5",
              url: "vi/metatrader-5",
              defaults: new { controller = "Vi", action = "metatrade5" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          ); 
            routes.MapRoute(
              name: "metatrader-4",
              url: "vi/metatrader-4",
              defaults: new { controller = "Vi", action = "metatrade4" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          );
            routes.MapRoute(
              name: "metatrader-webterminal",
              url: "vi/metatrader-webterminal",
              defaults: new { controller = "Vi", action = "metatrader" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          ); 
            routes.MapRoute(
              name: "metatrader-mobile",
              url: "vi/metatrader-mobile",
              defaults: new { controller = "Vi", action = "metatradermobile" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          ); 
            routes.MapRoute(
              name: "analytical-tools",
              url: "vi/analytical-tools",
              defaults: new { controller = "Vi", action = "analytic" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          );
            routes.MapRoute(
              name: "currency-converter",
              url: "vi/currency-converter",
              defaults: new { controller = "Vi", action = "converter" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          ); 
            routes.MapRoute(
              name: "tick-history",
              url: "vi/tick-history",
              defaults: new { controller = "Vi", action = "history" },
              namespaces: new string[] { "PTEcommerce.Web.Controllers" }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "PTEcommerce.Web.Controllers" }
            );
        }
    }
}
