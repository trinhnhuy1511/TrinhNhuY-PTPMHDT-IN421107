namespace EduHub.Ch01.Domain;

/// <summary>
/// Câu lạc bộ sinh viên.
/// </summary>
public sealed class Club
{
    private readonly List<Member> _members = new();

    /// <summary>Khoá chính của CLB.</summary>
    public Guid Id { get; private set; }

    /// <summary>Tên CLB.</summary>
    public string Name { get; private set; }

    /// <summary>Mô tả CLB.</summary>
    public string Description { get; private set; }

    /// <summary>Thời điểm tạo CLB.</summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>Danh sách thành viên (chỉ đọc, thêm qua <see cref="AddMember"/>).</summary>
    public IReadOnlyList<Member> Members => _members;

    private Club(Guid id, string name, string description, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Description = description;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// Tạo CLB mới sau khi kiểm tra dữ liệu đầu vào.
    /// </summary>
    /// <exception cref="ArgumentException">Tên CLB trống.</exception>
    public static Club Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên CLB không được để trống.", nameof(name));

        return new Club(Guid.NewGuid(), name.Trim(), description?.Trim() ?? string.Empty, DateTime.UtcNow);
    }

    /// <summary>Thêm thành viên vào CLB.</summary>
    /// <exception cref="ArgumentNullException">Thành viên null.</exception>
    /// <exception cref="InvalidOperationException">Thành viên đã có trong CLB.</exception>
    public void AddMember(Member member)
    {
        ArgumentNullException.ThrowIfNull(member);
        if (_members.Any(m => m.Id == member.Id))
            throw new InvalidOperationException($"Thành viên '{member.Name}' đã thuộc CLB '{Name}'.");

        _members.Add(member);
    }

    /// <summary>Số lượng thành viên hiện có.</summary>
    public int MemberCount() => _members.Count;
}
