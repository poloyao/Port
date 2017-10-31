using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using YZXDMS.Models;
using YZXDMS.DataProvider;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class SettingTimeViewModel
    {
        public SystemTime DisplayItem { get; set; }

        public SettingTimeViewModel()
        {
            DisplayItem = new SystemTime();
        }

        public void Save()
        {
            using (SQLiteDBContext db = new SQLiteDBContext())
            {
                
            }
        }
    }
}