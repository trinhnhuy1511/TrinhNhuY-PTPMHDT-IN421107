# EduHub – Ch01: Domain Model

Hệ thống quản lý Câu lạc bộ & Sự kiện sinh viên – Trường Đại học Đồng Tháp.
Bài tập Ch01: phân tích domain, vẽ sơ đồ lớp và cài đặt 5 domain class bằng C# (.NET 10).

## Thiết kế domain

| Class | Vai trò | Hành vi chính |
|---|---|---|
| `Member` | Thành viên sinh viên | `IsActive()`, `Deactivate()` |
| `Club` | Câu lạc bộ, chứa danh sách thành viên | `AddMember()`, `MemberCount()` |
| `Event` | Sự kiện của CLB (FK `ClubId`) | `HasCapacity()` |
| `Registration` | Phiếu đăng ký Member ↔ Event (FK `MemberId`, `EventId`) | `Confirm()`, `Cancel()` |
| `CheckIn` | Điểm danh, thuộc 1 Registration (FK `RegistrationId`, 0..1) | `Summary()` |

Nguyên tắc thiết kế:
- **Encapsulation**: constructor `private`, tạo đối tượng qua `static Create(...)` có kiểm tra đầu vào; property chỉ có `private set`; `Club.Members` là `IReadOnlyList<Member>`.
- **Không magic string**: trạng thái dùng enum `RegistrationStatus`, hình thức check-in dùng enum `CheckInMethod`.
- **Luật nghiệp vụ**: chỉ phiếu `Pending` mới `Confirm()` được; phiếu `Cancelled` không thể huỷ/xác nhận lại; chỉ phiếu `Confirmed` mới được check-in (vi phạm ném `InvalidOperationException`).
- **Nullable** bật trong `.csproj`; XML doc comment cho mọi public member.

Tài liệu: [User Stories](docs/user-stories.md) · [Class Diagram](docs/class-diagram.md)

## Cách chạy

Yêu cầu: .NET 10 SDK.

```bash
dotnet run
```

## Kết quả chạy

Output đầy đủ của lần chạy mẫu: [docs/run-output.txt](docs/run-output.txt).

<!-- TODO: chèn ảnh chụp màn hình terminal, ví dụ: ![Kết quả chạy](docs/run-screenshot.png) -->

## Cấu trúc

```
EduHub.Ch01/
├── Domain/ (Member, Club, Event, Registration, CheckIn)
├── docs/   (user-stories.md, class-diagram.md, run-output.txt)
├── Program.cs
└── EduHub.Ch01.csproj
```
