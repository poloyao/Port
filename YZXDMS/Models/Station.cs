﻿using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.Models
{
    /// <summary>
    /// 工位
    /// </summary>
    public class StationModel : ViewModelBase
    {
        public int Id { get; set; }
        /// <summary>
        /// 工位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工位顺序
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        //public List<AssistDeviceOrder> AssistItems { get; set; }


        public ObservableCollection<DetectionOrder> DetectionItems { get; set; }


        [Command(true)]
        public void Save()
        {
            //需要处理项目顺序
            if (DetectionItems == null)
            {
                DetectionItems = new ObservableCollection<DetectionOrder>();
            }
            DetectionItems.Add(new DetectionOrder());
        }
    }
}
