# Sơ đồ lớp sơ bộ – EduHub

```mermaid
classDiagram
    direction LR

    class Member {
        +Guid Id  «PK»
        +string Name
        +string Email
        +string StudentCode
        +DateTime JoinedAt
        +Create(name, email, studentCode)$ Member
        +IsActive() bool
        +Deactivate() void
    }

    class Club {
        +Guid Id  «PK»
        +string Name
        +string Description
        +DateTime CreatedAt
        +IReadOnlyList~Member~ Members
        +Create(name, description)$ Club
        +AddMember(member) void
        +MemberCount() int
    }

    class Event {
        +Guid Id  «PK»
        +Guid ClubId  «FK → Club.Id»
        +string Title
        +DateTime Date
        +int Capacity
        +bool IsOnline
        +Create(clubId, title, date, capacity, isOnline)$ Event
        +HasCapacity(registeredCount) bool
    }

    class Registration {
        +Guid Id  «PK»
        +Guid MemberId  «FK → Member.Id»
        +Guid EventId  «FK → Event.Id»
        +DateTime RegisteredAt
        +RegistrationStatus Status
        +Create(memberId, eventId)$ Registration
        +Confirm() void
        +Cancel() void
    }

    class CheckIn {
        +Guid Id  «PK»
        +Guid RegistrationId  «FK → Registration.Id»
        +DateTime CheckedInAt
        +CheckInMethod Method
        +Create(registration, method)$ CheckIn
        +Summary() string
    }

    Member "*" -- "*" Club : tham gia
    Club "1" --> "*" Event : tổ chức
    Member "1" --> "*" Registration : đăng ký
    Event "1" --> "*" Registration : có
    Registration "1" --> "0..1" CheckIn : ghi nhận
```

## Giải thích quan hệ

| Quan hệ | Bội số | Ghi chú |
|---|---|---|
| Member – Club | * – * | Một sinh viên tham gia nhiều CLB, một CLB có nhiều thành viên (`Club.Members`). |
| Club – Event | 1 – * | Event giữ khoá ngoại `ClubId`. |
| Member – Registration | 1 – * | Registration giữ khoá ngoại `MemberId`. |
| Event – Registration | 1 – * | Registration giữ khoá ngoại `EventId`. |
| Registration – CheckIn | 1 – 0..1 | CheckIn giữ khoá ngoại `RegistrationId`; chưa check-in thì chưa có bản ghi. |

`Registration` là bảng nối giữa `Member` và `Event` (quan hệ nhiều–nhiều). `RegistrationStatus` (Pending/Confirmed/Cancelled) và `CheckInMethod` (Manual/QrCode/SelfService) là enum.
