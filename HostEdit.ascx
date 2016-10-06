<%@ Control Language="C#" AutoEventWireup="true" Explicit="True" Inherits="X3.DnnUrlManagement.HostEdit"
    CodeBehind="HostEdit.ascx.cs" %>
<%@ Register TagPrefix="dnn" TagName="Label" Src="~/controls/LabelControl.ascx" %>
<%@ Register TagPrefix="dnn" Assembly="DotNetNuke.Web" Namespace="DotNetNuke.Web.UI.WebControls" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:ValidationSummary runat="server" ID="valSummary" HeaderText="Please correct the following:" DisplayMode="BulletList" CssClass="dnnFormMessage dnnFormValidationSummary" />

<div class="DnnUrlManagement dnnForm">

    <asp:HiddenField runat="server" ID="hfSelectedTab" ClientIDMode="Static" Value="0" />
    <ul class="dnnAdminTabNav">
        <li><a href="#Basic">Basic</a></li>
        <li><a href="#Regex">Regular Expressions</a></li>
    </ul>

    <div id="Basic" class="dnnClear">

        <div class="dnnFormItem">
            <dnn:Label ID="lblRedirectUnfriendly" runat="server" Text="Redirect Unfriendly:" ControlName="cbRedirectUnfriendly" HelpText="Check this box if you want old 'non-friendly' URLs to be redirected to the new URLs." />
            <asp:CheckBox runat="server" ID="cbRedirectUnfriendly" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblEnableCharacterReplacement" runat="server" Text="Enable Character Replacement:" ControlName="cbCharacterReplacement" HelpText="Check this box to enable character replacement. This automatically replaces all spaces in page names." />
            <asp:CheckBox runat="server" ID="cbCharacterReplacement" AutoPostBack="true" OnCheckedChanged="cbCharacterReplacement_CheckedChanged" />
        </div>

        <div class="dnnFormItem" runat="server" id="fiReplacementCharacter">
            <dnn:Label ID="lblReplaceSpaceWith" runat="server" Text="Replacement Character:" ControlName="ddlReplaceSpaceWith" HelpText="Select the replacement character to use." />
            <asp:DropDownList runat="server" ID="ddlReplaceSpaceWith">
                <asp:ListItem Value="-" Text="'-' : e.g. 'About-Us'" />
                <asp:ListItem Value="_" Text="'_' : e.g. 'About_Us'" />
            </asp:DropDownList>
        </div>

        <div class="dnnFormItem" runat="server" id="fiReplaceChars">
            <dnn:Label ID="lblReplaceChars" runat="server" Text="Replace These Characters:" ControlName="txtReplaceChars" HelpText="When the characters in this list appear in a page name, will be replaced with the value specified as the ‘replacement’ character (- or _).  To replace any other character, append it to the end of the list.  To stop any character from being replaced, remove it from the list.<br></br><br></br>[NOTE: The list shown in the ‘replace characters with supplied characters’ is actually the default for this one]" />
            <asp:TextBox runat="server" ID="txtReplaceChars" />
        </div>

        <div class="dnnFormItem" runat="server" id="fiFindReplace">
            <dnn:Label ID="lblFindReplaceTheseCharacters" runat="server" Text="Find/Replace These Characters:" ControlName="txtFindReplaceTheseCharacters" HelpText="To replace a character in a URL with another character, use a comma delimited pair of characters where the first value in the pair is the ‘find’, and the second is the ‘replace’. f,r will find any ‘f’ in a URL and replace it with ‘r’. This can be used for replacing æ with ‘ae’ and similar. This only applies to Page and User Profile URLs and any URLs controlled through URL providers.  You can only replace single characters - the 'find' character can only be a single value, while the 'replace' can be a string of one or more characters." />
            <asp:TextBox runat="server" ID="txtFindReplaceTheseCharacters" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblPageExtenstionHandling" runat="server" Text="Hide Page Extensions:" ControlName="cbPageExtensionHandling" HelpText="Check to hide the page extensions.  This can be useful for SEO and also for masking the underlying technology to make it more difficult for hackers." />
            <asp:CheckBox runat="server" ID="cbPageExtensionHandling" AutoPostBack="true" OnCheckedChanged="cbPageExtensionHandling_CheckedChanged" />
        </div>

        <div class="dnnFormItem" runat="server" id="fiPageExtension">
            <dnn:Label ID="lblPageExtension" runat="server" Text="Page Extension:" ControlName="txtPageExtension" HelpText="Specify the extension to use on the end of every page URL.  The default value is .aspx.  Note: using a custom page extension also requires configuring IIS to handle the custom extension.  If you don't know what this means, leave it alone." />
            <asp:TextBox runat="server" ID="txtPageExtension" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblDeletedTabHandlingType" runat="server" Text="Handle Deleted, Expired, Disabled Pages By:" ControlName="rblDeletedTabHandlingType" HelpText="Select the behavior that should occur when a user browses to a deleted, expired or disabled page." />
            <asp:RadioButtonList runat="server" ID="rblDeletedTabHandlingType">
                <asp:ListItem Value="Do301RedirectToPortalHome" Text="301 Redirect to Site Home Page" />
                <asp:ListItem Value="Do404Error" Text="Show 404 Error" />
            </asp:RadioButtonList>
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblForceLowerCase" runat="server" Text="Convert DNN URLs To Lower Case:" ControlName="cbForceLowerCase" HelpText="When checked, any URL generated by dNN will be converted to all lowercase.  ie. Menus, breadcrumbs, module controls, etc." />
            <asp:CheckBox runat="server" ID="cbForceLowerCase" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblRedirectMixedCase" runat="server" Text="Redirect URLs to lower case:" ControlName="cbRedirectMixedCase" HelpText="When checked, any URL that is not in lower case will be redirected to the lower case version of that URL." />
            <asp:CheckBox runat="server" ID="cbRedirectMixedCase" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblSpaceEncodingValue" runat="server" Text="URL Space Encoding Value:" ControlName="cblSpaceEncodingValue" HelpText="This character is used to replace any spaces in a generalized URL path where parameters with spaces have been supplied." />
            <asp:RadioButtonList runat="server" ID="rblSpaceEncodingValue" CssClass="dnnFormRadioButtons" RepeatDirection="Horizontal">
                <asp:ListItem Text="Hex Encoding (%20)" Value="%20" />
                <asp:ListItem Text="EnglishName (+)" Value="+" />
            </asp:RadioButtonList>
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblAutoAsciiConvert" runat="server" Text="Convert Accented Characters:" ControlName="cbAutoAsciiConvert" HelpText="When checked, any accented (diacritic) characters such as å and è will be converted to their plain-ascii equivalent.  Example : å -> a and è -> e." />
            <asp:CheckBox runat="server" ID="cbAutoAsciiConvert" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblEnableCustomProviders" runat="server" Text="Enable Custom URL Providers:" ControlName="cbEnableCustomProviders" HelpText="When checked, the Custom URL Provider functionality of this site is enabled.  When unchecked, no custom URL providers will be loaded." />
            <asp:CheckBox runat="server" ID="cbEnableCustomProviders" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblCheckForDuplicatedUrls" runat="server" Text="Log Duplicate URL Warnings:" ControlName="cbCheckForDuplicatedUrls" HelpText="When checked, any duplicate URLs found in this site will be reported in the event log.  The system will choose which of the duplicate URLs to show." />
            <asp:CheckBox runat="server" ID="cbCheckForDuplicatedUrls" />
        </div>

    </div>

    <div id="Regex" class="dnnClear">

        <div class="dnnFormItem">
            <dnn:Label ID="lblIgnoreUrlRegex" runat="server" Text="Ignore URL Regular Expressions:" ControlName="txtIgnoreUrlRegex" HelpText="The Ignore URL Regex pattern is used to stop processing of URLs by the URL Rewriting module.  This should be used when the URL in question doesn’t need to be rewritten, redirected or otherwise processed through the URL Rewriter.  Examples include images, css files, pdf files, service requests and requests for resources not associated with DotNetNuke." />
            <asp:TextBox runat="server" ID="txtIgnoreUrlRegex" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblDoNotRewriteRegEx" runat="server" Text="Do Not Rewrite:" ControlName="txtDoNotRewriteRegEx" HelpText="The Do Not Rewrite URL regular expression stops URL Rewriting from occurring on any URL that matches.  Use this value when a URL is being interpreted as a DotNetNuke page, but should not be." />
            <asp:TextBox runat="server" ID="txtDoNotRewriteRegEx" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblSiteUrlsOnlyRegex" runat="server" Text="Site URLs Only:" ControlName="txtSiteUrlsOnlyRegex" HelpText="The Site URLs Only regular expression pattern changes the processing order for matching URLs.  When matched, the URLs are evaluated against any of the regular expressions in the siteURLs.config file, without first being checked against the list of friendly URLs for the site.  Use this pattern to force processing through the siteURLs.config file for an explicit URL Rewrite or Redirect located within that file." />
            <asp:TextBox runat="server" ID="txtSiteUrlsOnlyRegex" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblDoNotRedirectUrlRegex" runat="server" Text="Do Not Redirect:" ControlName="txtDoNotRedirectUrlRegex" HelpText="The Do Not Redirect URL regular expression pattern prevents matching URLs from being redirected in all cases.  Use this pattern when a URL is being redirected incorrectly." />
            <asp:TextBox runat="server" ID="txtDoNotRedirectUrlRegex" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblDoNotRedirectHttpsUrlRegex" runat="server" Text="Do Not Redirect Https:" ControlName="txtDoNotRedirectHttpsUrlRegex" HelpText="The Do Not Redirect https URL regular expression is used to stop unwanted redirects between http and https URLs.  It prevents the redirect for any matching URLs, and works both for http->https and https->http redirects." />
            <asp:TextBox runat="server" ID="txtDoNotRedirectHttpsUrlRegex" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblPreventLowerCaseUrlRegex" runat="server" Text="Prevent Lowercase:" ControlName="txtPreventLowerCaseUrlRegex" HelpText="The Prevent Lowercase URL regular expression stops the automatic conversion to lower case for any matching URLs.  Use this pattern to prevent the lowercase conversion of any URLs which need to remain in mixed/upper case.  This is frequently used to stop the conversion of URLs where the contents of the URL contain an encoded character or case-sensitive value." />
            <asp:TextBox runat="server" ID="txtPreventLowerCaseUrlRegex" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblDoNotUseFriendlyUrlRegex" runat="server" Text="Do Not Use Friendly:" ControlName="txtDoNotUseFriendlyUrlRegex" HelpText="The Do Not Use Friendly URLs regular expression pattern is used to force certain DotNetNuke pages into using a longer URL for the page.  This is normally used to generate behaviour for backwards compatibility." />
            <asp:TextBox runat="server" ID="txtDoNotUseFriendlyUrlRegex" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblKeepInQueryStringRegex" runat="server" Text="Keep In Querystring:" ControlName="txtKeepInQueryStringRegex" HelpText="The Keep in Querystring regular expression allows the matching of part of the friendly URL Path and ensuring that it stays in the querystring.  When a DotNetNuke URL of /pagename/key/value is generated, a ‘Keep in Querystring Regular Expression’ pattern of /key/value will match that part of the path and leave it as part of the querystring for the generated URL; e.g. /pagename?key=value." />
            <asp:TextBox runat="server" ID="txtKeepInQueryStringRegex" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblUrlsWithNoExtensionRegex" runat="server" Text="URLs With No Extension:" ControlName="txtUrlsWithNoExtensionRegex" HelpText="The URLs with no Extension regular expression pattern is used to validate URLs that do not refer to a resource on the server, are not DotNetNuke pages, but can be requested with no URL extension.  URLs matching this regular expression will not be treated as a 404 when a matching DotNetNuke page can not be found for the URL." />
            <asp:TextBox runat="server" ID="txtUrlsWithNoExtensionRegex" />
        </div>

        <div class="dnnFormItem">
            <dnn:Label ID="lblValidFriendlyUrlRegex" runat="server" Text="Valid Friendly URL:" ControlName="txtValidFriendlyUrlRegex" HelpText="This pattern is used to determine whether the characters that make up a page name or URL segment are valid for forming a friendly URL path.  Characters that match the pattern will be removed from page names" />
            <asp:TextBox runat="server" ID="txtValidFriendlyUrlRegex" />
        </div>

    </div>

    <ul class="dnnActions dnnClear">

        <li>
            <asp:LinkButton runat="server" ID="lbSave" Text="Update" CssClass="dnnPrimaryAction" OnClick="lbSave_Click" />
        </li>

    </ul>

</div>
