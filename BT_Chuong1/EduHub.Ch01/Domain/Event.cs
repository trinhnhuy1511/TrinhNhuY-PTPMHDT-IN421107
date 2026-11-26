namespace EduHub.Ch01.Domain;

/// <summary>
/// Sự kiện do một CLB tổ chức.
/// </summary>
public sealed class Event
{
    /// <summary>Khoá chính của sự kiện.</summary>
    public Guid Id { get; private set; }

    /// <summary>Khoá ngoại tới <see cref="Club"/> tổ chức sự kiện.</summary>
    public Guid ClubId { get; private set; }

    /// <summary>Tiêu đề sự kiện.</summary>
    public string Title { get; private set; }

    /// <summary>Ngày diễn ra.</summary>
    public DateTime Date { get; private set; }

    /// <summary>Số chỗ tối đa.</summary>
    public int Capacity { get; private set; }

    /// <summary>Sự kiện trực tuyến hay không.</summary>
    public bool IsOnline { get; private set; }

    private Event(Guid id, Guid clubId, string title, DateTime date, int capacity, bool isOnline)
    {
        Id = id;
        ClubId = clubId;
        Title = title;
        Date = date;
        Capacity = capacity;
        IsOnline = isOnline;
    }

    /// <summary>
    /// Tạo sự kiện mới sau khi kiểm tra dữ liệu đầu vào.
    /// </summary>
    /// <exception cref="ArgumentException">ClubId rỗng, tiêu đề trống hoặc sức chứa ≤ 0.</exception>
    public static Event Create(Guid clubId, string title, DateTime date, int capacity, bool isOnline)
    {
        if (clubId == Guid.Empty)
            throw new ArgumentException("ClubId không hợp lệ.", nameof(clubId));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Tiêu đề sự kiện không được để trống.", nameof(title));
        if (capacity <= 0)
            throw new ArgumentException("Sức chứa phải lớn hơn 0.", nameof(capacity));

        return new Event(Guid.NewGuid(), clubId, title.Trim(), date, capacity, isOnline);
    }

    /// <summary>Kiểm tra sự kiện còn chỗ với số lượng đã đăng ký hiện tại.</summary>
    public bool HasCapacity(int registeredCount) => registeredCount < Capacity;
}
