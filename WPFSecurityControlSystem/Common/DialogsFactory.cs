using System.Collections.Generic;
using WPFSecurityControlSystem.Base;
using WPFSecurityControlSystem.Controls;
using WPFSecurityControlSystem.DTO;

namespace WPFSecurityControlSystem
{
    public static class DialogsFactory
    {
        public static BasePropertiesDialog CreateHolidayDialog(int parent_ID)
        {
            HolidayControl ctrlEditHoliday = new HolidayControl();
            BasePropertiesDialog dlg = new BasePropertiesDialog(ctrlEditHoliday);
            dlg.ParentNodeId = parent_ID;// TODO;
            dlg.Title = "Add a new holiday";
            return dlg;
        }

        public static BasePropertiesDialog CreateCardFormatsDialog(List<InfoColumn> listCardFormats)
        {
            CardFormatsControl ctrlAssignCardFormats = new CardFormatsControl(listCardFormats);
            BasePropertiesDialog dlg = new BasePropertiesDialog(ctrlAssignCardFormats);          
            dlg.Title = "More card formats";
            return dlg;
        }

        public static BasePropertiesDialog CreateColumnsPickerDialog(IEnumerable<InfoColumn> allItemsList, string title, string sourceListTitle, string targetListTitle)
        {
            ColumnsPickerControl ctrlColumnsPicker = new ColumnsPickerControl(allItemsList);
            ctrlColumnsPicker.SourceListHeader = sourceListTitle;
            ctrlColumnsPicker.TargetListHeader = targetListTitle;
            BasePropertiesDialog dialog = new BasePropertiesDialog(ctrlColumnsPicker.AllItems, ctrlColumnsPicker);  //dlg.Content = ctrl;   
            dialog.Title = title;
            return dialog;
        }

        public static BasePropertiesDialog CreateColumnsPickerDialog(IEnumerable<InfoColumn> allItemsList)
        {     
            return CreateColumnsPickerDialog(allItemsList,  "View Columns"); 
        }

        public static BasePropertiesDialog CreateColumnsPickerDialog(IEnumerable<InfoColumn> allItemsList, string title)
        {
            return CreateColumnsPickerDialog(allItemsList, title, "Available Items List", "Assigned Items List");                                   
        }

    }
}
