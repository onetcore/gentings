using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Gentings.Storages.Avatars
{
    /// <summary>
    /// 头像管理类。
    /// </summary>
    public class AvatarManager : IAvatarManager
    {
        private readonly IStorageDirectory _storageDirectory;
        /// <summary>
        /// 初始化类<see cref="AvatarManager"/>。
        /// </summary>
        /// <param name="storageDirectory">存储文件夹操作接口。</param>
        public AvatarManager(IStorageDirectory storageDirectory)
        {
            _storageDirectory = storageDirectory;
        }

        private const int Size = 320;

        /// <summary>
        /// 获取头像文件实例。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="size">宽度或者高度。</param>
        /// <returns>返回文件实例。</returns>
        public IStorageFile GetFile(int userId, int size)
        {
            var path = GetPath(userId);
            var defaultFile = _storageDirectory.GetFile(path + "default.png");
            if (size <= 0 || size == Size)
            {
                return defaultFile;
            }

            var currentFile = _storageDirectory.GetFile(path + $"{size}.png");
            if (!currentFile.Exists)
            {
                currentFile.Resize(size, size).MoveTo($"{size}.png");
            }

            return currentFile;

        }

        /// <summary>
        /// 上传用户头像。
        /// </summary>
        /// <param name="userId">用户Id。</param>
        /// <param name="file">当前用户实例。</param>
        /// <returns>返回上传任务。</returns>
        public async Task<string> UploadAsync(int userId, IFormFile file)
        {
            var tempFile = await _storageDirectory.SaveToTempAsync(file);
            tempFile = tempFile.Resize(Size, Size);
            //上传头像新文件，把老文件删除
            var path = _storageDirectory.GetPhysicalPath(GetPath(userId));
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            path = Path.Combine(path, "default.png").MakeDirectory();
            tempFile.MoveTo(path);
            return $"/s-avatars/{userId}.png";
        }

        private string GetPath(int userId)
        {
            return $"avatars/{userId}/";
        }
    }
}
