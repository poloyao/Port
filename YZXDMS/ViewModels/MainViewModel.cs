using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.Collections.Generic;
using DevExpress.Mvvm.POCO;
using System.Linq;
using DevExpress.Utils;
using System.Windows.Threading;

namespace YZXDMS.ViewModels
{
    /// <summary>
    /// 主界面控制器
    /// 初始化界面模块与可显示界面
    /// </summary>
    [POCOViewModel]
    public class MainViewModel
    {
        /// <summary>
        /// 模块组
        /// </summary>
        public virtual IEnumerable<ModuleGroup> ModuleGroups { get; protected set; }
        public virtual ModuleInfo SelectedModuleInfo { get; set; }
        
        protected virtual ICurrentWindowService CurrentWindowService { get { return null; } }
        protected virtual ISplashScreenService SplashScreenService { get { return this.GetService<ISplashScreenService>(); } }

        /// <summary>
        /// 登录人名称
        /// </summary>
        public virtual string LoginName { get; set; }

        private DispatcherTimer clockTimer = new DispatcherTimer();

        public virtual string TimeDateText { get; set; }


        public MainViewModel()
        {
            List<ModuleInfo> modules = new List<ModuleInfo>()
            {
                 ViewModelSource.Create(()=>new ModuleInfo("SettingPortView",this,"串口设置")).SetIcon("setting"),                 
                 ViewModelSource.Create(()=>new ModuleInfo("SettingLinkPortView",this,"模块设置")).SetIcon("menu_mode"),
                 ViewModelSource.Create(()=>new ModuleInfo("SettingStationView",this,"工位设置")).SetIcon("menu_station"),
                 ViewModelSource.Create(()=>new ModuleInfo("AssignView",this,"待检车辆")).SetIcon("menu_wait"),
                 ViewModelSource.Create(()=>new ModuleInfo("MasterView",this,"主控检测")).SetIcon("menu_master"),
                 //ViewModelSource.Create(()=>new ModuleInfo("QueryCarView",this,"车辆查询")).SetIcon("car"),
                 //ViewModelSource.Create(()=>new ModuleInfo("PrintView",this,"导出/打印")).SetIcon("car"),
                 //ViewModelSource.Create(()=>new ModuleInfo("TestyView",this,"实验1")).SetIcon("car"),
                 //ViewModelSource.Create(()=>new ModuleInfo("TestView",this,"实验")).SetIcon("car"),
                 ViewModelSource.Create(()=>new ModuleInfo("SettingBaseView",this,"基本信息")).SetIcon("setting"),
                 ViewModelSource.Create(()=>new ModuleInfo("SettingTimeView",this,"时间参数")).SetIcon("time"),
                 ViewModelSource.Create(()=>new ModuleInfo("QueryTestLineView",this,"查询")).SetIcon("port"),
                 ViewModelSource.Create(()=>new ModuleInfo("CarInfoView",this,"车籍信息")).SetIcon("line"),
            };
            ModuleGroups = new ModuleGroup[] {
                new ModuleGroup("功能",modules)
            };

            LoginName = Core.Core.User.Name;
            clockTimer.Interval = new TimeSpan(0, 0, 1);
            clockTimer.IsEnabled = true;
            clockTimer.Tick += ClockTimer_Tick;
            clockTimer.Start();

        }

        private void ClockTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimeDate();
        }

        private void UpdateTimeDate()
        {
            string timeDateString = "";
            DateTime now = DateTime.Now;
            timeDateString = string.Format("{0}年{1}月{2}日 {3}:{4}:{5}",
                now.Year,
                now.Month.ToString("00"),
                now.Day.ToString("00"),
                now.Hour.ToString("00"),
                now.Minute.ToString("00"),
                now.Second.ToString("00"));

            TimeDateText = timeDateString;
        }


        public void OnModulesLoaded()
        {
            if (SelectedModuleInfo == null)
            {
                SelectedModuleInfo = ModuleGroups.First().ModuleInfos.First();
                SelectedModuleInfo.IsSelected = true;
                SelectedModuleInfo.Show();
            }

        }

    }



    /// <summary>
    /// 模块
    /// </summary>
    public class ModuleInfo
    {
        ISupportServices parent;

        public ModuleInfo(string _type, object parent, string _title)
        {
            Type = _type;
            this.parent = (ISupportServices)parent;
            Title = _title;
        }

        public string Type { get; private set; }

        public virtual bool IsSelected { get; set; }

        public string Title { get; private set; }

        public virtual Uri Icon { get; set; }

        public ModuleInfo SetIcon(string icon)
        {
            this.Icon = AssemblyHelper.GetResourceUri(typeof(ModuleInfo).Assembly, string.Format("Img/{0}.png", icon));
            return this;
        }

        public void ClearNavigation()
        {
            INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
            navigationService.ClearNavigationHistory();
        }

        public void Show(object parameter = null)
        {
            try
            {
                //if (IsSelected)
                if (true)
                {
                    INavigationService navigationService = parent.ServiceContainer.GetRequiredService<INavigationService>();
                    navigationService.Navigate(Type, parameter, parent);
                    IsSelected = false;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }

    /// <summary>
    /// 模块组
    /// </summary>
    public class ModuleGroup
    {
        public ModuleGroup(string _title, IEnumerable<ModuleInfo> _moduleInfos)
        {
            Title = _title;
            ModuleInfos = _moduleInfos;
        }
        public string Title { get; private set; }
        public IEnumerable<ModuleInfo> ModuleInfos { get; private set; }
    }
}