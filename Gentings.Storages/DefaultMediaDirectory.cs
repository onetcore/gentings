using Gentings.Data;

namespace Gentings.Storages
{
    internal class DefaultMediaDirectory : MediaDirectory
    {
        public DefaultMediaDirectory(IStorageDirectory directory, IDbContext<MediaFile> mfdb, IDbContext<StoredFile> sfdb) : base(directory, mfdb, sfdb)
        {
        }
    }
}