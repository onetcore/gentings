using Gentings.Security.Scores.Properties;

namespace Gentings.Security.Scores
{
    /// <summary>
    /// 积分配置。
    /// </summary>
    public class ScoreSettings
    {
        /// <summary>
        /// 积分名称。
        /// </summary>
        public string ScoreName { get; set; } = Resources.DefaultScoreName;

        /// <summary>
        /// 积分单位。
        /// </summary>
        public string ScoreUnit { get; set; }
    }
}