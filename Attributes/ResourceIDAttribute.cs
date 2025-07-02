namespace Ntreev.Library.Psd.Attributes;


/// <summary>
/// 标记一个资源的ID和显示名称。(由于资源ID通常不具有可读性)
/// </summary>
/// <param name="resourceID"></param>
[AttributeUsage(AttributeTargets.Class)]
internal class ResourceIDAttribute(params string[] resourceID) : Attribute
    {
    public string[] ID { get; } = resourceID;

    /// <summary>
    /// 可读名称, 如果没有设置则使用 <see cref="ID"/>的第一个元素。
    /// </summary>
    public string DisplayName
        {
        get => string.IsNullOrEmpty(field) == true ? ID.First() : field; set;
        }
    }

