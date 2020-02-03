namespace Gentings.Data.Migrations
{
    /// <summary>
    /// 迁移服务。
    /// </summary>
    public class MigrationService
    {
        private MigrationService() { }
        private MigrationStatus _status;
        private string _message;
        private static readonly MigrationService _instance = new MigrationService();
        /// <summary>
        /// 当前状态。
        /// </summary>
        public static MigrationStatus Status
        {
            get => _instance._status;
            set
            {
                if (value != MigrationStatus.Error)
                    Message = null;
                _instance._status = value;
            }
        }
        /// <summary>
        /// 消息。
        /// </summary>
        public static string Message
        {
            get => _instance._message;
            set => _instance._message = value;
        }
    }
}