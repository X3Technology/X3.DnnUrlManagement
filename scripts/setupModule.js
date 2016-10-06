Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

function EndRequestHandler(sender, args) {
    setupDnnUrlManagementModule();
}

function setupDnnUrlManagementModule() {

    var hfSelectedTab = $("#hfSelectedTab");
    var selectedTab = hfSelectedTab.val();
    //alert(hfSelectedTab);
    var DnnUrlManagementTabs = $(".dnnForm.DnnUrlManagement").tabs();
    DnnUrlManagementTabs.tabs('option', 'active', selectedTab);

    DnnUrlManagementTabs.tabs({
        activate: function (event, ui) {
            //alert(ui.newTab.index());
            hfSelectedTab.val(ui.newTab.index());
        }
    });
};

$(document).ready(function () {
    setupDnnUrlManagementModule();
});