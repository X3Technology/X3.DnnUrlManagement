using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
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
    public partial class PortalEdit : PortalModuleBase
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

                FriendlyUrlSettings fs = new DotNetNuke.Entities.Urls.FriendlyUrlSettings(PortalId);

                // AUM_UrlFormat
                //cbURLFormat.Checked = (fs.UrlFormat == UrlFormatType.Advanced.ToString().ToLower());

                // AUM_DeletedTabHandlingType
                ListItem liDeletedTabHandling = rblDeletedTabHandlingType.Items.FindByValue(fs.DeletedTabHandlingType.ToString());
                if (liDeletedTabHandling != null)
                {
                    rblDeletedTabHandlingType.ClearSelection();
                    liDeletedTabHandling.Selected = true;
                }

                // AUM_ErrorPage404                
                if (fs.TabId404 > 0)
                {
                    ddlErrorPage404.SelectedPage = tabs.Where(i => i.TabID == portal.Custom404TabId).SingleOrDefault();
                }

                // AUM_ErrorPage500
                if (fs.TabId500 > 0)
                {
                    ddlErrorPage500.SelectedPage = tabs.Where(i => i.TabID == portal.Custom500TabId).SingleOrDefault();
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
                var portalController = new PortalController();
                var portal = portalController.GetPortal(PortalId, SelectedCultureCode);

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
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.DeletedTabHandlingTypeSetting, rblDeletedTabHandlingType.SelectedValue);

                // AUM_ErrorPage404          
                // pre 7.2.2.
                //DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.ErrorPage404Setting, ddlErrorPage404.SelectedItemValueAsInt.ToString());
                //7.2.2+
                portal.Custom404TabId = ddlErrorPage404.SelectedItemValueAsInt;

                // AUM_ErrorPage500
                // pre 7.2.2.
                //DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.ErrorPage500Setting, ddlErrorPage500.SelectedItemValueAsInt.ToString());
                portal.Custom500TabId = ddlErrorPage500.SelectedItemValueAsInt;

                portalController.UpdatePortalInfo(portal);

                // AUM_ForceLowerCase
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.ForceLowerCaseSetting, cbForceLowerCase.Checked.ToString());

                // AUM_PageExtension
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.PageExtensionSetting, txtPageExtension.Text);

                // AUM_PageExtensionUsage
                if (!cbPageExtensionHandling.Checked)
                {
                    DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.PageExtensionUsageSetting, DotNetNuke.Entities.Urls.PageExtensionUsageType.AlwaysUse.ToString());
                }
                else
                {
                    DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.PageExtensionUsageSetting, DotNetNuke.Entities.Urls.PageExtensionUsageType.Never.ToString());
                }

                // AUM_RedirectOldProfileUrl
                // AUM_RedirectUnfriendly
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.RedirectUnfriendlySetting, cbRedirectUnfriendly.Checked.ToString());

                // AUM_ReplaceSpaceWith, // AUM_ReplaceChars, // AUM_ReplaceCharWithChar
                if (cbCharacterReplacement.Checked)
                {
                    DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.ReplaceSpaceWithSetting, ddlReplaceSpaceWith.SelectedValue);
                    DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.ReplaceCharsSetting, txtReplaceChars.Text);
                    DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.ReplaceCharWithCharSetting, txtFindReplaceTheseCharacters.Text);
                }
                else
                {
                    DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.ReplaceSpaceWithSetting, FriendlyUrlSettings.ReplaceSpaceWithNothing);
                    DotNetNuke.Entities.Portals.PortalController.DeletePortalSetting(PortalId, FriendlyUrlSettings.ReplaceCharsSetting);
                    DotNetNuke.Entities.Portals.PortalController.DeletePortalSetting(PortalId, FriendlyUrlSettings.ReplaceCharWithCharSetting);
                }

                // AUM_RedirectMixedCase
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.RedirectMixedCaseSetting, cbRedirectMixedCase.Checked.ToString());

                // AUM_SpaceEncodingValue
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.SpaceEncodingValueSetting, rblSpaceEncodingValue.SelectedValue);

                // AUM_AutoAsciiConvert
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.AutoAsciiConvertSetting, cbAutoAsciiConvert.Checked.ToString());

                // AUM_CheckForDuplicatedUrls 
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.CheckForDuplicatedUrlsSetting, cbCheckForDuplicatedUrls.Checked.ToString());

                // AUM_FriendlyAdminHostUrls - skip

                // AUM_EnableCustomProviders
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.EnableCustomProvidersSetting, cbEnableCustomProviders.Checked.ToString());

                // AUM_IgnoreUrlRegex
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.IgnoreRegexSetting, txtIgnoreUrlRegex.Text);

                // AUM_DoNotRewriteRegEx
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.DoNotRewriteRegExSetting, txtDoNotRewriteRegEx.Text);

                // AUM_SiteUrlsOnlyRegex
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.SiteUrlsOnlyRegexSetting, txtSiteUrlsOnlyRegex.Text);

                // AUM_DoNotRedirectUrlRegex
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.DoNotRedirectUrlRegexSetting, txtDoNotRedirectUrlRegex.Text);

                // AUM_DoNotRedirectHttpsUrlRegex
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.DoNotRedirectHttpsUrlRegexSetting, txtDoNotRedirectHttpsUrlRegex.Text);

                // AUM_PreventLowerCaseUrlRegex
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.PreventLowerCaseUrlRegexSetting, txtPreventLowerCaseUrlRegex.Text);

                // AUM_DoNotUseFriendlyUrlRegex
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.DoNotUseFriendlyUrlRegexSetting, txtDoNotUseFriendlyUrlRegex.Text);

                // AUM_KeepInQueryStringRegex
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.KeepInQueryStringRegexSetting, txtKeepInQueryStringRegex.Text);

                // AUM_UrlsWithNoExtensionRegex
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.UrlsWithNoExtensionRegexSetting, txtUrlsWithNoExtensionRegex.Text);

                // AUM_ValidFriendlyUrlRegex
                DotNetNuke.Entities.Portals.PortalController.UpdatePortalSetting(PortalId, FriendlyUrlSettings.ValidFriendlyUrlRegexSetting, txtValidFriendlyUrlRegex.Text);

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

        protected void ddlPage_SelectionChanged(object sender, EventArgs e)
        {
            gvCustomUrls.MasterTableView.IsItemInserted = false;
            gvCustomUrls.Rebind();

            int tabID = ddlPage.SelectedItemValueAsInt;
            TabController tc = new TabController();
            TabInfo tab = tc.GetTab(tabID, PortalId, false);

            if (tab != null)
            {
                var baseUrl = Globals.AddHTTP(PortalAlias.HTTPAlias) + "/Default.aspx?TabId=" + tab.TabID;
                var path = AdvancedFriendlyUrlProvider.ImprovedFriendlyUrl(tab,
                    baseUrl,
                    Globals.glbDefaultPage,
                    PortalAlias.HTTPAlias,
                    true,
                    new DotNetNuke.Entities.Urls.FriendlyUrlSettings(PortalId),
                    Guid.Empty);

                hypSystemUrl.NavigateUrl = path;
                hypSystemUrl.Text = path;
            }
        }

        protected void gvCustomUrls_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            int tabID = ddlPage.SelectedItemValueAsInt;
            TabInfo ti = FriendlyUrlController.GetTab(tabID, true);
            if (ti != null)
            {
                gvCustomUrls.DataSource = ti.TabUrls;
            }
        }

        protected void gvCustomUrls_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                DropDownList ddlPortalAliasID = (DropDownList)dataItem["PortalAliasId"].FindControl("ddlPortalAliasId");
                BindPortalAliasId(ddlPortalAliasID);
            }
        }

        protected void gvCustomUrls_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                int tabID = ddlPage.SelectedItemValueAsInt;
                int seqnum = (int)((GridDataItem)e.Item).GetDataKeyValue("SeqNum");

                List<TabUrlInfo> tabUrls = DotNetNuke.Entities.Tabs.Internal.TestableTabController.Instance.GetTabUrls(tabID, PortalId);
                TabUrlInfo tabUrl = tabUrls.Where(i => i.SeqNum == seqnum).FirstOrDefault();

                if (tabUrl != null)
                {
                    DotNetNuke.Entities.Tabs.Internal.TestableTabController.Instance.DeleteTabUrl(tabUrl, PortalId, true);
                }

                gvCustomUrls.Rebind();
            }
            catch (Exception exc)
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        protected void gvCustomUrls_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem && (e.Item.DataItem.GetType() != typeof(GridInsertionObject)))
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                TabUrlInfo item = (TabUrlInfo)dataItem.DataItem;

                DropDownList ddlPortalAliasId = (DropDownList)dataItem.FindControl("ddlPortalAliasId");
                DropDownList ddlPortalAliasUage = (DropDownList)dataItem.FindControl("ddlPortalAliasUsage");
                DropDownList ddlHttpStatus = (DropDownList)dataItem.FindControl("ddlHttpStatus");

                if (ddlPortalAliasId != null)
                {
                    ListItem liPortalAliasId = ddlPortalAliasId.Items.FindByValue(item.PortalAliasId.ToString());
                    if (liPortalAliasId != null)
                    {
                        ddlPortalAliasId.ClearSelection();
                        liPortalAliasId.Selected = true;
                    }
                }

                if (ddlPortalAliasUage != null)
                {
                    int portalAliasUage = (int)item.PortalAliasUsage;

                    ListItem liPortalAliasUage = ddlPortalAliasUage.Items.FindByValue(portalAliasUage.ToString());
                    if (liPortalAliasUage != null)
                    {
                        ddlPortalAliasUage.ClearSelection();
                        liPortalAliasUage.Selected = true;
                    }
                }

                if (ddlHttpStatus != null)
                {
                    ListItem liHttpStatus = ddlHttpStatus.Items.FindByValue(item.HttpStatus);
                    if (liHttpStatus != null)
                    {
                        ddlHttpStatus.ClearSelection();
                        liHttpStatus.Selected = true;
                    }
                }
            }
        }

        protected void gvCustomUrls_InsertCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;

            DropDownList ddlPortalAliasId = editedItem.FindControl("ddlPortalAliasId") as DropDownList;
            DropDownList ddlPortalAliasUsage = editedItem.FindControl("ddlPortalAliasUsage") as DropDownList;
            DropDownList ddlHttpStatus = editedItem.FindControl("ddlHttpStatus") as DropDownList;

            Hashtable ht = new Hashtable();
            editedItem.ExtractValues(ht);

            // increment existing tabUrls
            List<TabUrlInfo> tabUrls = DotNetNuke.Entities.Tabs.Internal.TestableTabController.Instance.GetTabUrls(ddlPage.SelectedItemValueAsInt, PortalId);
            foreach (TabUrlInfo tu in tabUrls)
            {
                tu.SeqNum++;
                DotNetNuke.Entities.Tabs.Internal.TestableTabController.Instance.SaveTabUrl(tu, PortalId, false);
            }

            // create new tabUrl
            TabUrlInfo tabUrl = new TabUrlInfo();

            tabUrl.TabId = ddlPage.SelectedItemValueAsInt;
            tabUrl.SeqNum = 0;
            tabUrl.CultureCode = this.PortalSettings.CultureCode;
            if (!string.IsNullOrEmpty(ddlPortalAliasId.SelectedValue))
            {
                tabUrl.PortalAliasId = int.Parse(ddlPortalAliasId.SelectedValue);
            }

            if (!string.IsNullOrWhiteSpace(ddlPortalAliasUsage.SelectedValue))
            {
                tabUrl.PortalAliasUsage = (PortalAliasUsageType)Enum.Parse(typeof(PortalAliasUsageType), ddlPortalAliasUsage.SelectedValue);
            }

            if (ht["Url"] != null)
            {
                tabUrl.Url = FriendlyUrlController.EnsureLeadingChar("/", ht["Url"].ToString());
            }
            if (ht["QueryString"] != null)
            {
                tabUrl.QueryString = ht["QueryString"].ToString();
            }
            tabUrl.HttpStatus = ddlHttpStatus.SelectedValue;
            tabUrl.IsSystem = false;

            DotNetNuke.Entities.Tabs.Internal.TestableTabController.Instance.SaveTabUrl(tabUrl, PortalId, true);
            DotNetNuke.Common.Utilities.DataCache.ClearCache();
        }

        protected void gvCustomUrls_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editedItem = e.Item as GridEditableItem;
            var SeqNum = (int)editedItem.GetDataKeyValue("SeqNum");

            DropDownList ddlPortalAliasId = editedItem.FindControl("ddlPortalAliasId") as DropDownList;
            DropDownList ddlPortalAliasUsage = editedItem.FindControl("ddlPortalAliasUsage") as DropDownList;
            DropDownList ddlHttpStatus = editedItem.FindControl("ddlHttpStatus") as DropDownList;

            List<TabUrlInfo> tabUrls = DotNetNuke.Entities.Tabs.TabController.Instance.GetTabUrls(ddlPage.SelectedItemValueAsInt, PortalId);
            TabUrlInfo tabUrl = tabUrls.Where(i => i.SeqNum == SeqNum).FirstOrDefault();

            Hashtable ht = new Hashtable();
            editedItem.ExtractValues(ht);

            if (!string.IsNullOrEmpty(ddlPortalAliasId.SelectedValue))
            {
                tabUrl.PortalAliasId = int.Parse(ddlPortalAliasId.SelectedValue);
            }
            if (!string.IsNullOrWhiteSpace(ddlPortalAliasUsage.SelectedValue))
            {
                tabUrl.PortalAliasUsage = (PortalAliasUsageType)Enum.Parse(typeof(PortalAliasUsageType), ddlPortalAliasUsage.SelectedValue);
            }
            if (ht["Url"] != null)
            {
                tabUrl.Url = FriendlyUrlController.EnsureLeadingChar("/", ht["Url"].ToString());
            }
            if (ht["QueryString"] != null)
            {
                tabUrl.QueryString = ht["QueryString"].ToString();
            }
            tabUrl.HttpStatus = ddlHttpStatus.SelectedValue;

            DotNetNuke.Entities.Tabs.TabController.Instance.SaveTabUrl(tabUrl, PortalId, true);
            DotNetNuke.Common.Utilities.DataCache.ClearCache();
        }

        protected List<ListItem> GetPortalAliases()
        {
            List<ListItem> listItems = new List<ListItem>();

            List<PortalAliasInfo> portalAliases = DotNetNuke.Entities.Portals.Internal.TestablePortalAliasController.Instance.GetPortalAliasesByPortalId(PortalId).ToList();
            foreach (PortalAliasInfo portalAlias in portalAliases)
            {
                ListItem li = new ListItem(portalAlias.HTTPAlias, portalAlias.PortalAliasID.ToString());
                listItems.Add(li);
            }
            listItems.Insert(0, new ListItem("-any-", string.Empty));

            return listItems;
        }

        protected string GetPortalAlias(int portalAliasId)
        {
            StringBuilder sb = new StringBuilder();

            if (portalAliasId >= 0)
            {
                DotNetNuke.Entities.Portals.PortalAliasController pac = new PortalAliasController();
                PortalAliasInfo pa = pac.GetPortalAliasByPortalAliasID(portalAliasId);
                if (pa != null)
                {
                    sb.Append(pa.HTTPAlias);
                }
            }

            return sb.ToString();
        }

        protected string GetTabUrl(string url, int portalAliasId)
        {
            StringBuilder sb = new StringBuilder();

            if (portalAliasId > 0)
            {
                DotNetNuke.Entities.Portals.PortalAliasController pac = new PortalAliasController();
                PortalAliasInfo pa = pac.GetPortalAliasByPortalAliasID(portalAliasId);
                if (pa != null)
                {
                    sb.Append(pa.HTTPAlias);
                }
            }

            sb.Append(url);

            return sb.ToString();
        }

        protected string GetTabUrlType(string urlType)
        {
            StringBuilder sb = new StringBuilder();

            switch (urlType)
            {
                case "200":
                    sb.Append("Active (200)");
                    break;
                case "301":
                    sb.Append("Redirect (301)");
                    break;
                default:
                    break;
            }

            return sb.ToString();
        }

        private void BindPortalAliasId(DropDownList ddlPortalAliasId)
        {
            List<PortalAliasInfo> portalAliases = DotNetNuke.Entities.Portals.Internal.TestablePortalAliasController.Instance.GetPortalAliasesByPortalId(PortalId).ToList();

            ddlPortalAliasId.DataTextField = "HTTPAlias";
            ddlPortalAliasId.DataValueField = "PortalAliasID";
            ddlPortalAliasId.DataSource = portalAliases;
            ddlPortalAliasId.DataBind();

            ddlPortalAliasId.Items.Insert(0, new ListItem("-any-", string.Empty));
        }
    }
}
