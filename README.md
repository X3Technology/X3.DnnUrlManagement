# X3.DnnUrlManagement
## Project Description

This DNN module provides a management UI for the advanced URL management functionality introduced in DNN v7.1

The new functionality is controlled by setting a series of variable prefaced with "AUM_" (presumably for Advanced Url Management) in HostSettings and PortalSettings.

This module does not actually change the behavior / functionality of DNN's URL rewritter or add any new functionality, it simply sets the variables which affect its behavior.  As such, any unexpected behavior is likely a bug in DNN's URL rewritter functionality.

To use it, simply install the Install package, then place the module "X3.DnnUrlManagement" on a page. You likely want to put it on a page under the Admin section.

When you put the module on a page, you will get both the Portal and Host versions.  Simply delete the one you don't want on the page.  The Portal version should go on a page under Admin and the Host version on a page under Host.

Let me know if you experience any issues.

### Note:
* Use version v14.7.16+ for DNN 7.2.2+
* Use the v14.1.31 for DNN 7.1 through 7.2.1
