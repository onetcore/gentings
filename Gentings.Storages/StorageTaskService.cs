using System.Threading.Tasks;
using Gentings.Tasks;

namespace Gentings.Storages
{
    /// <summary>
    /// 存储文件夹清理服务。
    /// </summary>
    [Suppress(typeof(ClearTaskService))]
    public class StorageTaskService : ClearTaskService
    {
        private readonly IMediaDirectory _mediaDirectory;
        /// <summary>
        /// 初始化类<see cref="StorageTaskService"/>。
        /// </summary>
        /// <param name="storageDirectory">存储文件夹接口。</param>
        /// <param name="storageDirectory">媒体文件夹接口。</param>
        public StorageTaskService(IStorageDirectory storageDirectory, IMediaDirectory mediaDirectory)
            : base(storageDirectory)
        {
            _mediaDirectory = mediaDirectory;
        }

        /// <summary>
        /// 执行方法。
        /// </summary>
        /// <param name="argument">参数。</param>
        public override async Task ExecuteAsync(Argument argument)
        {
            await _mediaDirectory.ClearDeletedPhysicalFilesAsync();
            await base.ExecuteAsync(argument);
        }
    }
}