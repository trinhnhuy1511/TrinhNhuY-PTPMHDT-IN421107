namespace EduHub.Ch01.Domain;

/// <summary>
/// Thành viên sinh viên của câu lạc bộ.
/// </summary>
public sealed class Member
{
    private bool _isActive = true;

    /// <summary>Khoá chính của thành viên.</summary>
    public Guid Id { get; private set; }

    /// <summary>Họ và tên.</summary>
    public string Name { get; private set; }

    /// <summary>Email liên hệ.</summary>
    public string Email { get; private set; }

    /// <summary>Mã số sinh viên.</summary>
    public string StudentCode { get; private set; }

    /// <summary>Thời điểm tham gia hệ thống.</summary>
    public DateTime JoinedAt { get; private set; }

    private Member(Guid id, string name, string email, string studentCode, DateTime joinedAt)
    {
        Id = id;
        Name = name;
        Email = email;
        StudentCode = studentCode;
        JoinedAt = joinedAt;
    }

    /// <summary>
    /// Tạo thành viên mới sau khi kiểm tra dữ liệu đầu vào.
    /// </summary>
    /// <exception cref="ArgumentException">Tên/MSSV trống hoặc email không hợp lệ.</exception>
    public static Member Create(string name, string email, string studentCode)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Tên thành viên không được để trống.", nameof(name));
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("Email phải chứa ký tự '@'.", nameof(email));
        if (string.IsNullOrWhiteSpace(studentCode))
            throw new ArgumentException("Mã số sinh viên không được để trống.", nameof(studentCode));

        return new Member(Guid.NewGuid(), name.Trim(), email.Trim(), studentCode.Trim(), DateTime.UtcNow);
    }

    /// <summary>Thành viên còn hoạt động hay không.</summary>
    public bool IsActive() => _isActive;

    /// <summary>Ngừng hoạt động của thành viên.</summary>
    public void Deactivate() => _isActive = false;
}
