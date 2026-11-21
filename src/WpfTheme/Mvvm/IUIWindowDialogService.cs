using System;

namespace SysadminsLV.WPF.Mvvm {
    public interface IUIWindowDialogService {
        Boolean? ShowDialog(String title, Object dataContext);
        String ShowFolderBrowser();
    }
}
