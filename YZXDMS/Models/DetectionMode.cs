using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.Models
{
    /// <summary>
    /// 检测模式
    /// </summary>
    public enum DetectionMode
    {
        /// <summary>
        /// 综安检同时
        /// </summary>
        ALL,
        /// <summary>
        /// 综合检测
        /// </summary>
        CPD,
        /// <summary>
        /// 安全检测
        /// </summary>
        SPD
    }
}
