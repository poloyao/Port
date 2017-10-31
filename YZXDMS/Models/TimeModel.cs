using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("DBSystemTime")]
    public class DBSystemTime
    {
        public long Id { get; set; }

        public string TimeType { get; set; }
        public string Name { get; set; }

        public decimal Value { get; set; }


    }

    /// <summary>
    /// 时间设置
    /// </summary>
    public class SystemTime
    {
        public LatticeScreenTime LatticeScreenTime { get; set; } = new LatticeScreenTime();

        public BrakeTime BrakeTime { get; set; } = new BrakeTime();

        public SmokeDegreeTime SmokeDegreeTime { get; set; } = new SmokeDegreeTime();

        public SpeedTime SpeedTime { get; set; } = new SpeedTime();

        public LightTime LightTime { get; set; } = new LightTime();
    }

    /// <summary>
    /// 点阵屛的时间参数
    /// </summary>
    public class LatticeScreenTime
    {
        /// <summary>
        /// 刷新时间
        /// </summary>
        public decimal Refresh { get; set; }
        /// <summary>
        /// 刷新结果显示时间
        /// </summary>
        public decimal Result { get; set; }
        /// <summary>
        /// 牌照前时间
        /// </summary>
        public decimal Before { get; set; }
        /// <summary>
        /// 牌照后时间
        /// </summary>
        public decimal After { get; set; }
    }

    /// <summary>
    /// 制动时间参数
    /// </summary>
    public class BrakeTime
    {
        /// <summary>
        /// 光电入场时间
        /// </summary>
        public decimal PVC { get; set; }
        /// <summary>
        /// 检测时间
        /// </summary>
        public decimal Test { get; set; }
        /// <summary>
        /// 拖滞时间
        /// </summary>
        public decimal Dragging { get; set; }
        /// <summary>
        /// 支架升
        /// </summary>
        public decimal StentUp { get; set; }
        /// <summary>
        /// 支架降
        /// </summary>
        public decimal StentDown { get; set; }
        /// <summary>
        /// 台体升
        /// </summary>
        public decimal StationUp { get; set; }
        /// <summary>
        /// 台体降
        /// </summary>
        public decimal StationDown { get; set; }
    }

    /// <summary>
    /// 烟度检测时间参数
    /// </summary>
    public class SmokeDegreeTime
    {
        /// <summary>
        /// 踏油门时间
        /// </summary>
        public decimal AcceleratorIn { get; set; }
        /// <summary>
        /// 放油门时间
        /// </summary>
        public decimal AcceleratorOut { get; set; }
        /// <summary>
        /// 废气检测时间
        /// </summary>
        public decimal ExhaustGas { get; set; }
        /// <summary>
        /// 烟度测试时间
        /// </summary>
        public decimal Test { get; set; }
        /// <summary>
        /// 烟度插入探针时间
        /// </summary>
        public decimal Probe { get; set; }
    }

    /// <summary>
    /// 速度时间参数
    /// </summary>
    public class SpeedTime
    {
        /// <summary>
        /// 支架升
        /// </summary>
        public decimal StentUp { get; set; }
        /// <summary>
        /// 支架降
        /// </summary>
        public decimal StentDown { get; set; }
    }

    /// <summary>
    /// 大灯检测时间
    /// </summary>
    public class LightTime
    {
        /// <summary>
        /// 光电入场时间
        /// </summary>
        public decimal PVC { get; set; }
        /// <summary>
        /// 最大检测时间
        /// </summary>
        public decimal Max { get; set; }
        /// <summary>
        /// 二次运行时间
        /// </summary>
        public decimal Second { get; set; }
    }







}
