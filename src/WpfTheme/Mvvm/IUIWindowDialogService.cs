using System;

namespace SysadminsLV.WPF.OfficeTheme.Toolkit {
    public interface IUIWindowDialogService {
        Boolean? ShowDialog(String title, Object dataContext);
        String ShowFolderBrowser();
    }
}
