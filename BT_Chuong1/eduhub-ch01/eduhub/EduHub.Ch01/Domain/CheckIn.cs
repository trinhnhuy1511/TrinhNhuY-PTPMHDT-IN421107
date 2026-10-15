namespace EduHub.Ch01.Domain;

/// <summary>
/// Hình thức điểm danh.
/// </summary>
public enum CheckInMethod
{
    /// <summary>Ban tổ chức điểm danh thủ công.</summary>
    Manual,

    /// <summary>Quét mã QR.</summary>
    QrCode,

    /// <summary>Sinh viên tự check-in.</summary>
    SelfService
}

/// <summary>
/// Bản ghi điểm danh, thuộc đúng một <see cref="Registration"/>.
/// </summary>
public sealed class CheckIn
{
    /// <summary>Khoá chính của bản ghi điểm danh.</summary>
    public Guid Id { get; private set; }

    /// <summary>Khoá ngoại tới <see cref="Registration"/>.</summary>
    public Guid RegistrationId { get; private set; }

    /// <summary>Thời điểm điểm danh.</summary>
    public DateTime CheckedInAt { get; private set; }

    /// <summary>Hình thức điểm danh.</summary>
    public CheckInMethod Method { get; private set; }

    private CheckIn(Guid id, Guid registrationId, DateTime checkedInAt, CheckInMethod method)
    {
        Id = id;
        RegistrationId = registrationId;
        CheckedInAt = checkedInAt;
        Method = method;
    }

    /// <summary>
    /// Tạo bản ghi điểm danh; chỉ phiếu đã xác nhận mới được check-in.
    /// </summary>
    /// <exception cref="ArgumentNullException">Phiếu đăng ký null.</exception>
    /// <exception cref="InvalidOperationException">Phiếu chưa ở trạng thái Confirmed.</exception>
    public static CheckIn Create(Registration registration, CheckInMethod method)
    {
        ArgumentNullException.ThrowIfNull(registration);
        if (registration.Status != RegistrationStatus.Confirmed)
            throw new InvalidOperationException(
                $"Chỉ phiếu đã xác nhận mới được check-in (hiện tại: {registration.Status}).");

        return new CheckIn(Guid.NewGuid(), registration.Id, DateTime.UtcNow, method);
    }

    /// <summary>Chuỗi mô tả ngắn gọn bản ghi điểm danh.</summary>
    public string Summary() =>
        $"CheckIn {Id.ToString()[..8]} | phiếu {RegistrationId.ToString()[..8]} | {Method} | {CheckedInAt:HH:mm:ss}";
}
