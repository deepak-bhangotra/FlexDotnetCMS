﻿using FrameworkLibrary;
using System;
using System.Linq;
using System.Configuration;
using System.Web;
using System.Web.Script.Serialization;
using MaxMind.GeoIP2.Responses;

/// <summary>
/// Summary description for BaseMedia
/// </summary>

public class FrameworkBaseMedia
{
    //private Media rootMedia;
    //private IMediaDetail rootMediaDetail;
    private Media currentMedia;

    private IMediaDetail currentMediaDetail;
    private Language currentLanguage;
    private Language currentAdminLanguage;
    private static ConnectionStringSettings connectionSettings = null;
    private Language defaultLanguage;
    private string currentVirtualPath = "";

    public static FrameworkBaseMedia GetInstanceByVirtualPath(string virtualPath, bool selectParentIfPossible = false)
    {
        if (FrameworkSettings.Current != null)
            return FrameworkSettings.Current;

        return new FrameworkBaseMedia(virtualPath);
    }

    public static FrameworkBaseMedia GetInstanceByMedia(Media media)
    {
        if (FrameworkSettings.Current != null)
            return FrameworkSettings.Current;

        return new FrameworkBaseMedia(media);
    }

    public static FrameworkBaseMedia GetInstanceByMediaDetail(IMediaDetail mediaDetail)
    {
        if (FrameworkSettings.Current != null)
            return FrameworkSettings.Current;

        return new FrameworkBaseMedia(mediaDetail);
    }

    public static ConnectionStringSettings PrepareConnectionSettings(ConnectionStringSettings connectionStringSettings)
    {
        if (connectionStringSettings != null)
        {
            string connectionString = "metadata=res://*/Model.csdl|res://*/Model.ssdl|res://*/Model.msl;provider=" + connectionStringSettings.ProviderName + ";provider connection string=\"" + connectionStringSettings.ConnectionString + ";MultipleActiveResultSets=True\"";
            return new ConnectionStringSettings(connectionStringSettings.Name, connectionString, connectionStringSettings.ProviderName);
        }

        return null;
    }

    public static void InitConnectionSettings(ConnectionStringSettings connectionStringSettings)
    {
        if (connectionStringSettings != null)
        {
            connectionSettings = PrepareConnectionSettings(connectionStringSettings);
        }
    }

    private FrameworkBaseMedia(string virtualPath)
    {
        Init();
        LoadByVirtualPath(virtualPath);
    }

    private FrameworkBaseMedia(Media media)
    {
        Init();
        LoadByMedia(media);
    }

    private FrameworkBaseMedia(IMediaDetail mediaDetail)
    {
        Init();
        LoadByMediaDetail(mediaDetail);
    }

    private void Init()
    {
        if (connectionSettings == null)
            throw new Exception("You must call FrameworkBaseMedia.InitConnectionSettings first");

        defaultLanguage = LanguagesMapper.GetDefaultLanguage();
        currentLanguage = FrameworkSettings.GetCurrentLanguage();

        if (currentLanguage == null)
            throw new Exception("Error loading default language");

        /*rootMedia = FrameworkSettings.RootMedia;

        if (rootMedia != null)
            rootMediaDetail = FrameworkSettings.RootMediaDetail;*/
    }

    public void LoadByVirtualPath(string virtualPath)
    {
        if ((!virtualPath.StartsWith("http")) && System.IO.File.Exists(HttpContext.Current.Server.MapPath(virtualPath)))
            return;

        currentVirtualPath = virtualPath;

        if (currentLanguage == null)
            throw new Exception("Error loading default language");

        if ((currentVirtualPath == "~/") || currentVirtualPath == URIHelper.ConvertAbsUrlToTilda(URIHelper.ConvertToAbsUrl(currentLanguage.UriSegment)))
        {
            long version = 0;
            long.TryParse(HttpContext.Current.Request["version"], out version);

            currentMediaDetail = WebsitesMapper.GetWebsite(version);
        }
        else
            currentMediaDetail = MediaDetailsMapper.GetByVirtualPath(currentVirtualPath);

        if (currentMediaDetail == null)
            return;

        currentLanguage = currentMediaDetail.Language;
        FrameworkSettings.SetCurrentLanguage(currentMediaDetail.Language);

        //rootMedia = FrameworkSettings.RootMedia;
        //rootMediaDetail = FrameworkSettings.RootMediaDetail;

        currentMedia = currentMediaDetail.Media;
    }

    public void LoadByMedia(Media media)
    {
        currentMedia = media;
        currentMediaDetail = MediaDetailsMapper.GetByMedia(media, CurrentLanguage);
    }

    public void LoadByMediaDetail(IMediaDetail mediaDetail)
    {
        long version = 0;
        long.TryParse(HttpContext.Current.Request["version"], out version);

        if (version == 0 && mediaDetail?.HistoryForMediaDetail != null)
            mediaDetail = mediaDetail.HistoryForMediaDetail;

        currentMediaDetail = mediaDetail;
        currentMedia = currentMediaDetail?.Media;
    }

    public Language CurrentLanguage
    {
        get
        {
            return currentLanguage;
        }
        set
        {
            FrameworkSettings.SetCurrentLanguage(value);
            LoadByVirtualPath(currentVirtualPath);
        }
    }

    public Media CurrentMedia
    {
        get
        {
            return currentMedia;
        }
        set
        {
            currentMedia = value;
            LoadByMedia(currentMedia);
        }
    }

    public Website CurrentWebsite
    {
        get
        {
            return WebsitesMapper.GetWebsite();
        }
    }

    public IMediaDetail CurrentMediaDetail
    {
        get
        {
            return currentMediaDetail;
        }
    }

    public string CurrentVirtualPath
    {
        get
        {
            return currentVirtualPath;
        }
        set
        {
            currentVirtualPath = value;
            LoadByVirtualPath(currentVirtualPath);
        }
    }

    /*public string CurrentMediaVirtualPath
    {
        get
        {
            if (currentMediaDetail != null)
                return currentMediaDetail.VirtualPath;

            return FrameworkSettings.RootMediaDetail.VirtualPath;
        }
    }*/

    public User CurrentUser
    {
        get
        {
            return FrameworkSettings.CurrentUser;
        }
        set
        {
            FrameworkSettings.CurrentUser = value;
        }
    }

    public string CurrentVisitorIP
    {
        get
        {
            var ip = HttpContext.Current.Request.ServerVariables["HTTP_X_CLUSTER_CLIENT_IP"];

            if (ip == null)
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            return ip;
        }
    }

    public CityResponse CurrentVisitorLocation
    {
        get
        {
            return GeoLocationHelper.GetLocation(CurrentVisitorIP);
        }
    }

    public static ConnectionStringSettings ConnectionSettings
    {
        get
        {
            return connectionSettings;
        }
    }
}