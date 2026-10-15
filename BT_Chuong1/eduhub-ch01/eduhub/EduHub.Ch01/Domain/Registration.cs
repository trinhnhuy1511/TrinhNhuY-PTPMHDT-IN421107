namespace EduHub.Ch01.Domain;

/// <summary>
/// Trạng thái của phiếu đăng ký.
/// </summary>
public enum RegistrationStatus
{
    /// <summary>Đã đăng ký, chờ xác nhận.</summary>
    Pending,

    /// <summary>Đã được xác nhận.</summary>
    Confirmed,

    /// <summary>Đã huỷ.</summary>
    Cancelled
}

/// <summary>
/// Phiếu đăng ký của một thành viên tham dự một sự kiện (bảng nối Member ↔ Event).
/// </summary>
public sealed class Registration
{
    /// <summary>Khoá chính của phiếu đăng ký.</summary>
    public Guid Id { get; private set; }

    /// <summary>Khoá ngoại tới <see cref="Member"/>.</summary>
    public Guid MemberId { get; private set; }

    /// <summary>Khoá ngoại tới <see cref="Event"/>.</summary>
    public Guid EventId { get; private set; }

    /// <summary>Thời điểm đăng ký.</summary>
    public DateTime RegisteredAt { get; private set; }

    /// <summary>Trạng thái hiện tại.</summary>
    public RegistrationStatus Status { get; private set; }

    private Registration(Guid id, Guid memberId, Guid eventId, DateTime registeredAt)
    {
        Id = id;
        MemberId = memberId;
        EventId = eventId;
        RegisteredAt = registeredAt;
        Status = RegistrationStatus.Pending;
    }

    /// <summary>
    /// Tạo phiếu đăng ký mới với trạng thái <see cref="RegistrationStatus.Pending"/>.
    /// </summary>
    /// <exception cref="ArgumentException">MemberId hoặc EventId rỗng.</exception>
    public static Registration Create(Guid memberId, Guid eventId)
    {
        if (memberId == Guid.Empty)
            throw new ArgumentException("MemberId không hợp lệ.", nameof(memberId));
        if (eventId == Guid.Empty)
            throw new ArgumentException("EventId không hợp lệ.", nameof(eventId));

        return new Registration(Guid.NewGuid(), memberId, eventId, DateTime.UtcNow);
    }

    /// <summary>Xác nhận phiếu đăng ký (chỉ từ trạng thái Pending).</summary>
    /// <exception cref="InvalidOperationException">Phiếu không ở trạng thái Pending.</exception>
    public void Confirm()
    {
        if (Status != RegistrationStatus.Pending)
            throw new InvalidOperationException($"Không thể xác nhận phiếu đang ở trạng thái {Status}.");

        Status = RegistrationStatus.Confirmed;
    }

    /// <summary>Huỷ phiếu đăng ký (từ Pending hoặc Confirmed).</summary>
    /// <exception cref="InvalidOperationException">Phiếu đã huỷ trước đó.</exception>
    public void Cancel()
    {
        if (Status == RegistrationStatus.Cancelled)
            throw new InvalidOperationException("Phiếu đăng ký đã được huỷ trước đó.");

        Status = RegistrationStatus.Cancelled;
    }
}
