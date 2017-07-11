using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace YZXDMS.ViewModels
{
    [POCOViewModel]
    public class QueryTestLineViewModel
    {
        public List<QueryTestLineModel> Items { get; set; } = new List<QueryTestLineModel>();
        public QueryTestLineViewModel()
        {
            Items.Add(new QueryTestLineModel() { LineNo = "123",CarNo = "asdas"});
        }
    }


    public class QueryTestLineModel
    {
        /// <summary>
        /// 线号
        /// </summary>
        [Display(Name = "线号")]
        public string LineNo { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        [Display(Name = "车牌号码")]
        public string CarNo { get; set; }
        /// <summary>
        /// 车牌类型
        /// </summary>
        [Display(Name = "车牌类型")]
        public string CardType { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        [Display(Name = "车辆类型")]
        public string CarType { get; set; }
        /// <summary>
        /// 检测次数
        /// </summary>
        [Display(Name = "检测次数")]
        public string TestNo { get; set; }
        /// <summary>
        /// 车辆所有人
        /// </summary>
        [Display(Name = "车辆所有人")]
        public string CarOwner { get; set; }
        /// <summary>
        /// 检测时间
        /// </summary>
        [Display(Name = "检测时间")]
        public string TestDate { get; set; }
        /// <summary>
        /// 流水号
        /// </summary>
        [Display(Name = "流水号")]
        public string SerialNo { get; set; }
        /// <summary>
        /// 是否通过
        /// </summary>
        [Display(Name = "是否通过")]
        public string IsPass { get; set; }
    }
}