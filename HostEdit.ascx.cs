using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Controllers;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Urls;
using DotNetNuke.Framework.Providers;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.Skins.Controls;
using DotNetNuke.Web.Client.ClientResourceManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using System.Text;
using Telerik.Web.UI;
using System.Collections;

namespace X3.DnnUrlManagement
{
    public partial class HostEdit : PortalModuleBase
    {
        private string SelectedCultureCode
        {
            get
            {
                return LocaleController.Instance.GetCurrentLocale(PortalId).Code;
            }
        }

        private void Page_Load(System.Object sender, System.EventArgs e)
        {
            try
            {
                //JS                
                ClientResourceManager.RegisterScript(this.Page, ResolveUrl("scripts/setupModule.js"));

                if (Page.IsPostBack == false)
                {
                    CheckForAdvanced();

                    LoadSettings();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void LoadSettings()
        {
            try
            {
                var portalController = new PortalController();
                var portal = portalController.GetPortal(PortalId, SelectedCultureCode);

                //Set up special page lists
                List<TabInfo> listTabs = TabController.GetPortalTabs(TabController.GetTabsBySortOrder(portal.PortalID, SelectedCultureCode, true),
                                                                     Null.NullInteger,
                                                                     true,
                                                                     "<" + Localization.GetString("None_Specified") + ">",
                                                                     true,
                                                                     false,
                                                                     false,
                                                                     false,
                                                                     false);

                var tabs = listTabs.Where(t => t.DisableLink == false).ToList();

                FriendlyUrlSettings fs = new DotNetNuke.Entities.Urls.FriendlyUrlSettings(-1);


                // AUM_UrlFormat
                //cbURLFormat.Checked = (fs.UrlFormat == UrlFormatType.Advanced.ToString().ToLower());

                // AUM_DeletedTabHandlingType
                ListItem liDeletedTabHandling = rblDeletedTabHandlingType.Items.FindByValue(fs.DeletedTabHandlingType.ToString());
                if (liDeletedTabHandling != null)
                {
                    rblDeletedTabHandlingType.ClearSelection();
                    liDeletedTabHandling.Selected = true;
                }

                // AUM_ForceLowerCase
                cbForceLowerCase.Checked = fs.ForceLowerCase;

                // AUM_PageExtension
                txtPageExtension.Text = fs.PageExtension;

                // AUM_PageExtensionUsage - reverse boolean because label is 'hide'
                if (fs.PageExtensionUsageType == DotNetNuke.Entities.Urls.PageExtensionUsageType.AlwaysUse)
                {
                    cbPageExtensionHandling.Checked = false;
                }
                else
                {
                    cbPageExtensionHandling.Checked = true;
                }

                // AUM_RedirectOldProfileUrl - skip

                // AUM_RedirectUnfriendly
                cbRedirectUnfriendly.Checked = fs.RedirectUnfriendly;


                // AUM_ReplaceSpaceWith, // AUM_ReplaceChars , // AUM_ReplaceCharWithChar
                if (fs.ReplaceSpaceWith == FriendlyUrlSettings.ReplaceSpaceWithNothing)
                {
                    cbCharacterReplacement.Checked = false;
                    txtReplaceChars.Text = fs.ReplaceChars;
                }
                else
                {
                    cbCharacterReplacement.Checked = true;

                    ListItem liReplaceSpaceWith = ddlReplaceSpaceWith.Items.FindByValue(fs.ReplaceSpaceWith);
                    if (liReplaceSpaceWith != null)
                    {
                        liReplaceSpaceWith.Selected = true;
                    }

                    txtReplaceChars.Text = fs.ReplaceChars;
                }
                txtFindReplaceTheseCharacters.Text = DotNetNuke.Entities.Portals.PortalController.GetPortalSetting(FriendlyUrlSettings.ReplaceCharWithCharSetting, PortalId, string.Empty);

                // AUM_RedirectMixedCase
                cbRedirectMixedCase.Checked = fs.RedirectWrongCase;

                // AUM_SpaceEncodingValue
                ListItem liSpaceEncodingValue = rblSpaceEncodingValue.Items.FindByValue(fs.SpaceEncodingValue);
                if (liSpaceEncodingValue != null)
                {
                    rblSpaceEncodingValue.ClearSelection();
                    liSpaceEncodingValue.Selected = true;
                }

                // AUM_AutoAsciiConvert
                cbAutoAsciiConvert.Checked = fs.AutoAsciiConvert;

                // AUM_CheckForDuplicatedUrls
                cbCheckForDuplicatedUrls.Checked = fs.CheckForDuplicateUrls;

                // AUM_FriendlyAdminHostUrls - skip

                // AUM_EnableCustomProviders
                cbEnableCustomProviders.Checked = fs.EnableCustomProviders;


                // AUM_IgnoreUrlRegex
                txtIgnoreUrlRegex.Text = fs.IgnoreRegex;

                // AUM_DoNotRewriteRegEx
                txtDoNotRewriteRegEx.Text = fs.DoNotRewriteRegex;

                // AUM_SiteUrlsOnlyRegex
                txtSiteUrlsOnlyRegex.Text = fs.UseSiteUrlsRegex;

                // AUM_DoNotRedirectUrlRegex
                txtDoNotRedirectUrlRegex.Text = fs.DoNotRedirectRegex;

                // AUM_DoNotRedirectHttpsUrlRegex
                txtDoNotRedirectHttpsUrlRegex.Text = fs.DoNotRedirectSecureRegex;

                // AUM_PreventLowerCaseUrlRegex
                txtPreventLowerCaseUrlRegex.Text = fs.ForceLowerCaseRegex;

                // AUM_DoNotUseFriendlyUrlRegex
                txtDoNotUseFriendlyUrlRegex.Text = fs.NoFriendlyUrlRegex;

                // AUM_KeepInQueryStringRegex
                txtKeepInQueryStringRegex.Text = fs.DoNotIncludeInPathRegex;

                // AUM_UrlsWithNoExtensionRegex
                txtUrlsWithNoExtensionRegex.Text = fs.ValidExtensionlessUrlsRegex;

                // AUM_ValidFriendlyUrlRegex
                txtValidFriendlyUrlRegex.Text = fs.RegexMatch;



                // AUM_UsePortalDefaultLanguage
                // AUM_AllowDebugCode
                // AUM_LogCacheMessages

                CheckPageExtension();
                CheckCharacterReplacement();

            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void lbSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    Save();
                }
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void Save()
        {
            try
            {
                // AUM_UrlFormat
                //if (cbURLFormat.Checked)
                //{
                //    DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.UrlFormatSetting, UrlFormatType.Advanced.ToString().ToLower());
                //}
                //else
                //{
                //    DotNetNuke.Entities.Portals.PortalController.DeletePortalSetting(PortalId, FriendlyUrlSettings.UrlFormatSetting);
                //}

                // AUM_DeletedTabHandlingType
                HostController.Instance.Update(FriendlyUrlSettings.DeletedTabHandlingTypeSetting, rblDeletedTabHandlingType.SelectedValue);

                // AUM_ForceLowerCase                
                HostController.Instance.Update(FriendlyUrlSettings.ForceLowerCaseSetting, cbForceLowerCase.Checked.ToString());

                // AUM_PageExtension
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.PageExtensionSetting, txtPageExtension.Text);
                HostController.Instance.Update(FriendlyUrlSettings.PageExtensionSetting, txtPageExtension.Text);

                // AUM_PageExtensionUsage
                if (!cbPageExtensionHandling.Checked)
                {
                    HostController.Instance.Update(FriendlyUrlSettings.PageExtensionUsageSetting, DotNetNuke.Entities.Urls.PageExtensionUsageType.AlwaysUse.ToString());
                }
                else
                {
                    HostController.Instance.Update(FriendlyUrlSettings.PageExtensionUsageSetting, DotNetNuke.Entities.Urls.PageExtensionUsageType.Never.ToString());
                }

                // AUM_RedirectOldProfileUrl
                // AUM_RedirectUnfriendly
                HostController.Instance.Update(FriendlyUrlSettings.RedirectUnfriendlySetting, cbRedirectUnfriendly.Checked.ToString());

                // AUM_ReplaceSpaceWith, // AUM_ReplaceChars, // AUM_ReplaceCharWithChar
                if (cbCharacterReplacement.Checked)
                {
                    HostController.Instance.Update(FriendlyUrlSettings.ReplaceSpaceWithSetting, ddlReplaceSpaceWith.SelectedValue);
                    HostController.Instance.Update(FriendlyUrlSettings.ReplaceCharsSetting, txtReplaceChars.Text);
                    HostController.Instance.Update(FriendlyUrlSettings.ReplaceCharWithCharSetting, txtFindReplaceTheseCharacters.Text);
                }
                else
                {
                    HostController.Instance.Update(FriendlyUrlSettings.ReplaceSpaceWithSetting, string.Empty);
                    HostController.Instance.Update(FriendlyUrlSettings.ReplaceCharsSetting, string.Empty);
                    HostController.Instance.Update(FriendlyUrlSettings.ReplaceCharWithCharSetting, string.Empty);
                }

                // AUM_RedirectMixedCase
                HostController.Instance.Update(FriendlyUrlSettings.RedirectMixedCaseSetting, cbRedirectMixedCase.Checked.ToString());

                // AUM_SpaceEncodingValue
                HostController.Instance.Update(FriendlyUrlSettings.SpaceEncodingValueSetting, rblSpaceEncodingValue.SelectedValue);

                // AUM_AutoAsciiConvert
                HostController.Instance.Update(FriendlyUrlSettings.AutoAsciiConvertSetting, cbAutoAsciiConvert.Checked.ToString());

                // AUM_CheckForDuplicatedUrls 
                HostController.Instance.Update(FriendlyUrlSettings.CheckForDuplicatedUrlsSetting, cbCheckForDuplicatedUrls.Checked.ToString());

                // AUM_FriendlyAdminHostUrls - skip

                // AUM_EnableCustomProviders
                HostController.Instance.Update(FriendlyUrlSettings.EnableCustomProvidersSetting, cbEnableCustomProviders.Checked.ToString());

                // AUM_IgnoreUrlRegex
                HostController.Instance.Update(FriendlyUrlSettings.IgnoreRegexSetting, txtIgnoreUrlRegex.Text);

                // AUM_DoNotRewriteRegEx
                HostController.Instance.Update(FriendlyUrlSettings.DoNotRewriteRegExSetting, txtDoNotRewriteRegEx.Text);

                // AUM_SiteUrlsOnlyRegex
                HostController.Instance.Update(FriendlyUrlSettings.SiteUrlsOnlyRegexSetting, txtSiteUrlsOnlyRegex.Text);

                // AUM_DoNotRedirectUrlRegex
                HostController.Instance.Update(FriendlyUrlSettings.DoNotRedirectUrlRegexSetting, txtDoNotRedirectUrlRegex.Text);

                // AUM_DoNotRedirectHttpsUrlRegex
                HostController.Instance.Update(FriendlyUrlSettings.DoNotRedirectHttpsUrlRegexSetting, txtDoNotRedirectHttpsUrlRegex.Text);

                // AUM_PreventLowerCaseUrlRegex
                HostController.Instance.Update(FriendlyUrlSettings.PreventLowerCaseUrlRegexSetting, txtPreventLowerCaseUrlRegex.Text);

                // AUM_DoNotUseFriendlyUrlRegex
                HostController.Instance.Update(FriendlyUrlSettings.DoNotUseFriendlyUrlRegexSetting, txtDoNotUseFriendlyUrlRegex.Text);

                // AUM_KeepInQueryStringRegex
                HostController.Instance.Update(FriendlyUrlSettings.KeepInQueryStringRegexSetting, txtKeepInQueryStringRegex.Text);

                // AUM_UrlsWithNoExtensionRegex
                HostController.Instance.Update(FriendlyUrlSettings.UrlsWithNoExtensionRegexSetting, txtUrlsWithNoExtensionRegex.Text);

                // AUM_ValidFriendlyUrlRegex
                HostController.Instance.Update(FriendlyUrlSettings.ValidFriendlyUrlRegexSetting, txtValidFriendlyUrlRegex.Text);

                // AUM_UsePortalDefaultLanguage
                // AUM_AllowDebugCode
                // AUM_LogCacheMessages

                CacheController.FlushFriendlyUrlSettingsFromCache();
                CacheController.FlushPageIndexFromCache();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void cbPageExtensionHandling_CheckedChanged(object sender, EventArgs e)
        {
            CheckPageExtension();
        }

        private void CheckPageExtension()
        {
            fiPageExtension.Visible = !cbPageExtensionHandling.Checked;
        }

        protected void cbCharacterReplacement_CheckedChanged(object sender, EventArgs e)
        {
            CheckCharacterReplacement();
        }

        private void CheckCharacterReplacement()
        {
            fiReplacementCharacter.Visible = cbCharacterReplacement.Checked;
            fiFindReplace.Visible = cbCharacterReplacement.Checked;
            fiReplaceChars.Visible = cbCharacterReplacement.Checked;
        }

        private void CheckForAdvanced()
        {
            bool advancedEnabled = false;

            ProviderConfiguration fupConfig = ProviderConfiguration.GetProviderConfiguration("friendlyUrl");
            if (fupConfig != null)
            {
                string defaultFriendlyUrlProvider = fupConfig.DefaultProvider;
                var provider = (Provider)fupConfig.Providers[defaultFriendlyUrlProvider];
                string urlFormat = provider.Attributes["urlFormat"];
                if (!string.IsNullOrEmpty(urlFormat))
                {
                    if (urlFormat.ToLower() == UrlFormatType.Advanced.ToString().ToLower())
                    {
                        advancedEnabled = true;
                    }
                }
            }

            if (!advancedEnabled)
            {
                DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, "<h3>Advanced Mode Not Enabled</h3><p>You must enable the DNN Advanced URL Rewritting capabilities in the web.config.</p><p><a href='http://www.dnnsoftware.com/wiki/Page/Activating-Advanced-Url-Management'>See Wiki</a></p>", ModuleMessage.ModuleMessageType.YellowWarning);
            }
        }


    }
}
