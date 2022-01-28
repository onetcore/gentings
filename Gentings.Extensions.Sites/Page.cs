using Gentings.AspNetCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gentings.Extensions.Sites
{
    /// <summary>
    /// 页面类型。
    /// </summary>
    [Table("site_Pages")]
    public class Page : SourceEntityBase
    {
        /// <summary>
        /// 唯一键，主要用于URL。
        /// </summary>
        [Size(64)]
        public string? Key { get; set; }

        /// <summary>
        /// 标题。
        /// </summary>
        [Size(256)]
        public string? Title { get; set; }

        /// <summary>
        /// 激活的菜单Id。
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// Body样式名称。
        /// </summary>
        [NotMapped]
        public string ClassName { get => this[nameof(ClassName)]; set => this[nameof(ClassName)] = value; }

        /// <summary>
        /// 是否宽屏。
        /// </summary>
        [NotMapped]
        public bool? IsFluid { get => GetBoolean(nameof(IsFluid)); set => SetBoolean(nameof(IsFluid), value); }

        /// <summary>
        /// 关键词。
        /// </summary>
        [NotMapped]
        public string Keyword { get => this[nameof(Keyword)]; set => this[nameof(Keyword)] = value; }

        /// <summary>
        /// 描述。
        /// </summary>
        [NotMapped]
        public string Description { get => this[nameof(Description)]; set => this[nameof(Description)] = value; }

        /// <summary>
        /// 头部标签。
        /// </summary>
        [NotMapped]
        public string Header { get => this[nameof(Header)]; set => this[nameof(Header)] = value; }

        /// <summary>
        /// 尾部标签。
        /// </summary>
        [NotMapped]
        public string Footer { get => this[nameof(Footer)]; set => this[nameof(Footer)] = value; }

        /// <summary>
        /// 引入脚本库。
        /// </summary>
        [NotMapped]
        public ImportLibrary ImportLibraries { get => GetEnum<ImportLibrary>(nameof(ImportLibraries)) ?? ImportLibrary.None; set => SetEnum(nameof(ImportLibraries), value); }

        private ImportLibrary[]? _importLibraries;
        /// <summary>
        /// 引入库列表，用于checkboxlist。
        /// </summary>
        [NotMapped]
        public ImportLibrary[] FormImportLibraries
        {
            get
            {
                if (_importLibraries == null)
                {
                    var libraries = new List<ImportLibrary>();
                    foreach (ImportLibrary value in Enum.GetValues(typeof(ImportLibrary)))
                    {
                        if ((value & ImportLibraries) == value)
                            libraries.Add(value);
                    }

                    _importLibraries = libraries.ToArray();
                }

                return _importLibraries;
            }
            set
            {
                _importLibraries = value;
                ImportLibraries = ImportLibrary.None;
                if (_importLibraries != null)
                {
                    foreach (var library in _importLibraries)
                    {
                        ImportLibraries |= library;
                    }
                }
            }
        }

        /// <summary>
        /// 模板Id。
        /// </summary>
        [Size(64)]
        public string? TemplateName { get; set; }

        /// <summary>
        /// 访问地址。
        /// </summary>
        public string Url => Key == "/" ? "/" : $"/pages/{Key}";
    }
}
