using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YZXDMS.Models
{
    /// <summary>
    /// 灯光检测模式
    /// </summary>
    public enum LightDetectionMode
    {
        /// <summary>
        /// 双灯同检，
        /// 一侧灯检测失败后，另一侧灯继续替换检测。
        /// </summary>
        Double,
        /// <summary>
        /// 单灯右进
        /// </summary>
        SingleRight,
        /// <summary>
        /// 单灯左进
        /// </summary>
        SingleLeft
    }

    

}
