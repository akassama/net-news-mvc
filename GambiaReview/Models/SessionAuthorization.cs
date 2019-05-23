using GambiaReview.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

//https://stackoverflow.com/questions/20787797/how-to-check-session-variable-existence-in-mvc-before-we-do-any-activity-on-page

public class SessionAuthorizeAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var variable_session = httpContext.Session["sessionEmail"];
        var variable_unique_id = httpContext.Session["sessionID"];
        if (httpContext.Session["sessionID"] == null || httpContext.Session["sessionEmail"] == null || httpContext.Session["sessionFullName"] == null)
        {
            return variable_session != null;
        }

        using (var db = new DBConnection())
        {
            string session_unique_id = variable_unique_id.ToString();
            string session_email = variable_session.ToString();
            if (!db.Accounts.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            if (!db.LoginInfo.Any(s => s.Email == session_email && s.LoginSessionID == session_unique_id))
            {
                variable_session = null;
            }
        }
        return variable_session != null;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary
                              {
                                   { "action", "Index" },
                                   { "controller", "Home" }
                              });
    }
}

public class SessionHasRoleAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var variable_session = httpContext.Session["sessionEmail"];
        var variable_unique_id = httpContext.Session["sessionID"];
        if (httpContext.Session["sessionID"] == null || httpContext.Session["sessionEmail"] == null || httpContext.Session["sessionFullName"] == null)
        {
            return variable_session != null;
        }

        using (var db = new DBConnection())
        {
            string session_unique_id = variable_unique_id.ToString();
            string session_email = variable_session.ToString();
            if (!db.Accounts.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            if (!db.LoginInfo.Any(s => s.Email == session_email && s.LoginSessionID == session_unique_id))
            {
                variable_session = null;
            }

            //Validate for User having an Exting Role in group
            if (!db.Groups.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }
        }
        return variable_session != null;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary
                              {
                                   { "action", "Index" },
                                   { "controller", "User" }
                              });
    }
}

public class SessionHasSystemAdminRoleAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var variable_session = httpContext.Session["sessionEmail"];
        var variable_unique_id = httpContext.Session["sessionID"];
        if (httpContext.Session["sessionID"] == null || httpContext.Session["sessionEmail"] == null || httpContext.Session["sessionFullName"] == null)
        {
            return variable_session != null;
        }

        using (var db = new DBConnection())
        {
            string session_unique_id = variable_unique_id.ToString();
            string session_email = variable_session.ToString();
            if (!db.Accounts.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            if (!db.LoginInfo.Any(s => s.Email == session_email && s.LoginSessionID == session_unique_id))
            {
                variable_session = null;
            }

            //Validate for User having an Exting Role in group
            if (!db.Groups.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            int sys_admin_role_id = AppFunctions.GetRoleID("SystemAdmin");
            //Validate for User having an System Admin Role in group
            if (!db.Groups.Any(s => s.Email == session_email && (s.RoleID == sys_admin_role_id)))
            {
                variable_session = null;
            }
        }

        if (variable_session == null)
        {
            httpContext.Session["ErrorMessage"] = "You do not have access to that section";
        }

        return variable_session != null;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary
                              {
                                   { "action", "Index" },
                                   { "controller", "Admin" }
                              });
    }
}

public class SessionHasEditorRoleAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var variable_session = httpContext.Session["sessionEmail"];
        var variable_unique_id = httpContext.Session["sessionID"];
        if (httpContext.Session["sessionID"] == null || httpContext.Session["sessionEmail"] == null || httpContext.Session["sessionFullName"] == null)
        {
            return variable_session != null;
        }

        using (var db = new DBConnection())
        {
            string session_unique_id = variable_unique_id.ToString();
            string session_email = variable_session.ToString();
            if (!db.Accounts.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            if (!db.LoginInfo.Any(s => s.Email == session_email && s.LoginSessionID == session_unique_id))
            {
                variable_session = null;
            }

            //Validate for User having an Exting Role in group
            if (!db.Groups.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            int sys_admin_role_id = AppFunctions.GetRoleID("SystemAdmin");
            int editor_role_id = AppFunctions.GetRoleID("Editor");
            //Validate for User having an Editor Role in group
            if (!db.Groups.Any(s => s.Email == session_email && (s.RoleID == sys_admin_role_id || s.RoleID == editor_role_id)))
            {
                variable_session = null;
            }
        }

        if(variable_session == null)
        {
            httpContext.Session["ErrorMessage"] = "You do not have access to that section";
        }

        return variable_session != null;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary
                              {
                                   { "action", "Index" },
                                   { "controller", "Admin" }
                              });
    }
}


public class SessionHasAuthorRoleAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var variable_session = httpContext.Session["sessionEmail"];
        var variable_unique_id = httpContext.Session["sessionID"];
        if (httpContext.Session["sessionID"] == null || httpContext.Session["sessionEmail"] == null || httpContext.Session["sessionFullName"] == null)
        {
            return variable_session != null;
        }

        using (var db = new DBConnection())
        {
            string session_unique_id = variable_unique_id.ToString();
            string session_email = variable_session.ToString();
            if (!db.Accounts.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            if (!db.LoginInfo.Any(s => s.Email == session_email && s.LoginSessionID == session_unique_id))
            {
                variable_session = null;
            }

            //Validate for User having an Exting Role in group
            if (!db.Groups.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            int sys_admin_role_id = AppFunctions.GetRoleID("SystemAdmin");
            int editor_role_id = AppFunctions.GetRoleID("Editor");
            int author_role_id = AppFunctions.GetRoleID("Author");
            //Validate for User having an Author Role in group
            if (!db.Groups.Any(s => s.Email == session_email && (s.RoleID == sys_admin_role_id || s.RoleID == editor_role_id || s.RoleID == author_role_id)))
            {
                variable_session = null;
            }
        }

        if (variable_session == null)
        {
            httpContext.Session["ErrorMessage"] = "You do not have access to that section";
        }

        return variable_session != null;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary
                              {
                                   { "action", "Index" },
                                   { "controller", "Admin" }
                              });
    }
}


public class SessionHasCensorRoleAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var variable_session = httpContext.Session["sessionEmail"];
        var variable_unique_id = httpContext.Session["sessionID"];
        if (httpContext.Session["sessionID"] == null || httpContext.Session["sessionEmail"] == null || httpContext.Session["sessionFullName"] == null)
        {
            return variable_session != null;
        }

        using (var db = new DBConnection())
        {
            string session_unique_id = variable_unique_id.ToString();
            string session_email = variable_session.ToString();
            if (!db.Accounts.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            if (!db.LoginInfo.Any(s => s.Email == session_email && s.LoginSessionID == session_unique_id))
            {
                variable_session = null;
            }

            //Validate for User having an Exting Role in group
            if (!db.Groups.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            int sys_admin_role_id = AppFunctions.GetRoleID("SystemAdmin");
            int editor_role_id = AppFunctions.GetRoleID("Editor");
            int censor_role_id = AppFunctions.GetRoleID("Censor"); 
            //Validate for User having an Censor Role in group
            if (!db.Groups.Any(s => s.Email == session_email && (s.RoleID == sys_admin_role_id || s.RoleID == editor_role_id || s.RoleID == censor_role_id)))
            {
                variable_session = null;
            }
        }

        if (variable_session == null)
        {
            httpContext.Session["ErrorMessage"] = "You do not have access to that section";
        }

        return variable_session != null;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary
                              {
                                   { "action", "Index" },
                                   { "controller", "Admin" }
                              });
    }
}


//Reset all admin session values
public class SessionReloadAttribute : AuthorizeAttribute
{
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
        var variable_session = httpContext.Session["sessionEmail"];
        var variable_unique_id = httpContext.Session["sessionID"];
        if (httpContext.Session["sessionID"] == null || httpContext.Session["sessionEmail"] == null || httpContext.Session["sessionFullName"] == null)
        {
            return variable_session != null;
        }

        using (var db = new DBConnection())
        {
            string session_unique_id = variable_unique_id.ToString();
            string session_email = variable_session.ToString();
            if (!db.Accounts.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            if (!db.LoginInfo.Any(s => s.Email == session_email && s.LoginSessionID == session_unique_id))
            {
                variable_session = null;
            }

            //Validate for User having an Exting Role in group
            if (!db.Groups.Any(s => s.Email == session_email))
            {
                variable_session = null;
            }

            int sys_admin_role_id = AppFunctions.GetRoleID("SystemAdmin");
            int editor_role_id = AppFunctions.GetRoleID("Editor");
            int author_role_id = AppFunctions.GetRoleID("Author");
            int censor_role_id = AppFunctions.GetRoleID("Censor");
            int advertiser_role_id = AppFunctions.GetRoleID("Advertiser");

            //Set all true
            httpContext.Session["sessionSystemAdmin"] = true;
            httpContext.Session["sessionEditor"] = true;
            httpContext.Session["sessionAuthor"] = true;
            httpContext.Session["sessionCensor"] = true;
            httpContext.Session["sessionAdvertiser"] = true;

            //Reset admin session values
            if (!db.Groups.Any(s => s.Email == session_email && s.RoleID == sys_admin_role_id))
            {
                httpContext.Session["sessionSystemAdmin"] = null;
            }

            if (!db.Groups.Any(s => s.Email == session_email && s.RoleID == editor_role_id))
            {
                httpContext.Session["sessionEditor"] = null;
            }

            if (!db.Groups.Any(s => s.Email == session_email && s.RoleID == author_role_id))
            {
                httpContext.Session["sessionAuthor"] = null;
            }

            if (!db.Groups.Any(s => s.Email == session_email && s.RoleID == censor_role_id))
            {
                httpContext.Session["sessionCensor"] = null;
            }

            if (!db.Groups.Any(s => s.Email == session_email && s.RoleID == advertiser_role_id))
            {
                httpContext.Session["sessionAdvertiser"] = null;
            }

        }


        return variable_session != null;
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
        filterContext.Result = new RedirectToRouteResult(
                              new RouteValueDictionary
                              {
                                   { "action", "Index" },
                                   { "controller", "Home" }
                              });
    }
}