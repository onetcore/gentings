using Gentings.Data;

namespace Gentings.Storages.Media
{
    internal class DefaultMediaDirectory : MediaDirectory
    {
        public DefaultMediaDirectory(IStorageDirectory directory, IDbContext<MediaFile> mfdb, IDbContext<StoredFile> sfdb) : base(directory, mfdb, sfdb)
        {
        }
    }
}