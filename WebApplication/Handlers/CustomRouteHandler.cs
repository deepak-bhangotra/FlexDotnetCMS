﻿using FrameworkLibrary;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WebApplication.Handlers
{
    public class CustomRouteHandler : IRouteHandler, IRequiresSessionState
    {
        private string virtualPath = "";

        private HttpRequest Request
        {
            get { return HttpContext.Current.Request; }
        }

        private HttpResponse Response
        {
            get { return HttpContext.Current.Response; }
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            virtualPath = URIHelper.GetCurrentVirtualPath().ToLower();

            if ((virtualPath != "~/") && (!virtualPath.EndsWith("/")))
                HttpContext.Current.Response.RedirectPermanent(virtualPath + "/");

            var segments = URIHelper.GetUriSegments(virtualPath).ToList();

            /*if (segments.Count > 0)
            {
                var languageSegment = segments.FirstOrDefault();
                var language = LanguagesMapper.GetDataModel().Languages.FirstOrDefault(i => i.UriSegment == languageSegment);
            }*/

            string firstSegment = "";

            if (segments.Count > 0)
            {
                firstSegment = segments[0];

                var language = LanguagesMapper.GetAllActive().SingleOrDefault(i => i.UriSegment == firstSegment);

                FrameworkSettings.SetCurrentLanguage(language);
            }

            if (AppSettings.EnableUrlRedirectRules)
            {
                var redirectRule = UrlRedirectRulesMapper.GetRuleForUrl(virtualPath);

                if (redirectRule != null)
                {
                    var newUrl = URIHelper.ConvertToAbsUrl(redirectRule.RedirectToUrl);

                    if (Request.QueryString.Count > 0)
                        newUrl += "?" + Request.QueryString;

                    if (redirectRule.Is301Redirect)
                        Response.RedirectPermanent(newUrl);
                    else
                        Response.Redirect(newUrl);
                }
            }

            if (!File.Exists(HttpContext.Current.Server.MapPath(virtualPath)) && !virtualPath.Contains(ParserHelper.OpenToken) && !virtualPath.Contains(ParserHelper.CloseToken))
            {
                
                if ((virtualPath != "~/login/") && (virtualPath != "~/admin/"))
                {
                    var settings = SettingsMapper.GetSettings();

                    if (!virtualPath.Contains(settings.SiteOfflineUrl) && (!settings.IsSiteOnline()) && (virtualPath != settings.SiteOfflineUrl))
                        Response.Redirect(settings.SiteOfflineUrl);
                }

                string viewPath = "";

                FrameworkSettings.CurrentFrameworkBaseMedia = FrameworkBaseMedia.GetInstance(virtualPath, true);
                MediaDetail detail = (MediaDetail)FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail;

                if (detail != null && !detail.CanUserAccessSection(FrameworkSettings.CurrentUser))
                {
                    FormsAuthentication.RedirectToLoginPage();
                }

                if ((detail != null) && (detail.ForceSSL))
                    URIHelper.ForceSSL();
                else
                    URIHelper.ForceNonSSL();

                if ((detail == null) || (!IsValidRequest(detail)))
                {
                    IMediaDetail pageNotFoundHandler = MediaDetailsMapper.GetByVirtualPath(FrameworkSettings.GetPageNotFoundHandler(FrameworkSettings.GetCurrentLanguage()), false);

                    if (pageNotFoundHandler == null)
                        pageNotFoundHandler = MediaDetailsMapper.GetByVirtualPath(FrameworkSettings.GetPageNotFoundHandler(LanguagesMapper.GetDefaultLanguage()), false);

                    if (pageNotFoundHandler != null)
                    {
                        Response.Redirect(pageNotFoundHandler.AutoCalculatedVirtualPath + "?requestVirtualPath=" + virtualPath);
                        Response.End();
                    }
                }

                if (detail != null)
                {
                    if (detail.RedirectToFirstChild)
                    {
                        var items = MediaDetailsMapper.GetAllChildMediaDetails(detail.Media, detail.Language).ToList();

                        if (items.Count > 0)
                            HttpContext.Current.Response.Redirect(items[0].AutoCalculatedVirtualPath);
                    }

                    viewPath = FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.Handler;

                    if ((viewPath == null) || (viewPath.Trim() == ""))
                        viewPath = MediaTypesMapper.GetByID(FrameworkSettings.CurrentFrameworkBaseMedia.CurrentMediaDetail.MediaTypeID).MediaTypeHandler;

                    return CreateInstanceFromVirtualPath(viewPath, typeof(BasePage));
                }
            }

            return new DefaultHttpHandler();
        }

        private bool IsValidRequest(IMediaDetail detail)
        {
            if (detail == null)
                return false;

            if ((!detail.CanRender) && (Request["version"] == null))
                return false;

            return true;
        }

        private IHttpHandler CreateInstanceFromVirtualPath(string viewPath, Type type)
        {
            var display = BuildManager.CreateInstanceFromVirtualPath(viewPath, type);
            return (IHttpHandler)display;
        }
    }
}