namespace Gentings.Extensions.Notifications
{
    /// <summary>
    /// 通知配置。
    /// </summary>
    public class NotificationSettings
    {
        /// <summary>
        /// 每个用户保留通知个数。
        /// </summary>
        public int MaxSize { get; set; } = 20;

        /// <summary>
        /// 判断通知重复时间。
        /// </summary>
        public double DuplicateHours = 24.0;
    }
}