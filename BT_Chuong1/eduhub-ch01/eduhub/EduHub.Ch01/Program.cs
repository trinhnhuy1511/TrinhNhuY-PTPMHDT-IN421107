using EduHub.Ch01.Domain;

const int SeparatorWidth = 60;
const int WorkshopCapacity = 50;
const string ClubName = "Tech Club";
const string ClubDescription = "Câu lạc bộ công nghệ: chia sẻ kiến thức lập trình, AI và dự án thực tế.";
const string EventTitle = "Workshop AI";

Console.OutputEncoding = System.Text.Encoding.UTF8;

static void PrintStep(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', SeparatorWidth));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', SeparatorWidth));
}

// Bước 1: tạo CLB
PrintStep($"Bước 1: Tạo CLB \"{ClubName}\"");
var club = Club.Create(ClubName, ClubDescription);
Console.WriteLine($"Đã tạo CLB: {club.Name} (Id = {club.Id})");
Console.WriteLine($"Mô tả: {club.Description}");

// Bước 2: thêm 3 thành viên
PrintStep("Bước 2: Thêm 3 thành viên vào CLB");
var members = new[]
{
    Member.Create("Nguyễn Văn An", "an.nv@student.dthu.edu.vn", "SV001"),
    Member.Create("Trần Thị Bình", "binh.tt@student.dthu.edu.vn", "SV002"),
    Member.Create("Lê Hoàng Cường", "cuong.lh@student.dthu.edu.vn", "SV003"),
};
foreach (var member in members)
{
    club.AddMember(member);
    Console.WriteLine($"+ {member.Name} ({member.StudentCode}) - active: {member.IsActive()}");
}
Console.WriteLine($"Tổng số thành viên: {club.MemberCount()}");

// Bước 3: tạo sự kiện
PrintStep($"Bước 3: Tạo sự kiện \"{EventTitle}\"");
var workshop = Event.Create(club.Id, EventTitle, DateTime.Today.AddDays(7), WorkshopCapacity, isOnline: false);
Console.WriteLine($"Sự kiện: {workshop.Title} | Ngày: {workshop.Date:dd/MM/yyyy} | Sức chứa: {workshop.Capacity}");

// Bước 4: 2 thành viên đăng ký và được xác nhận
PrintStep("Bước 4: Đăng ký tham dự và xác nhận");
var registrations = new List<Registration>();
foreach (var member in members.Take(2))
{
    if (!workshop.HasCapacity(registrations.Count))
    {
        Console.WriteLine("Sự kiện đã hết chỗ.");
        break;
    }

    var registration = Registration.Create(member.Id, workshop.Id);
    Console.WriteLine($"{member.Name}: đăng ký -> {registration.Status}");
    registration.Confirm();
    Console.WriteLine($"{member.Name}: xác nhận -> {registration.Status}");
    registrations.Add(registration);
}

// Bước 5: 1 thành viên check-in
PrintStep("Bước 5: Check-in");
var checkIns = new List<CheckIn>();
var checkedInRegistration = registrations[0];
var checkIn = CheckIn.Create(checkedInRegistration, CheckInMethod.QrCode);
checkIns.Add(checkIn);
Console.WriteLine(checkIn.Summary());

// Bước 6: báo cáo tham dự
PrintStep("Bước 6: Báo cáo tham dự");
var memberById = members.ToDictionary(m => m.Id);
var registrationById = registrations.ToDictionary(r => r.Id);
Console.WriteLine($"Sự kiện: {workshop.Title}");
Console.WriteLine($"Số đăng ký: {registrations.Count}/{workshop.Capacity}");
Console.WriteLine($"Số check-in: {checkIns.Count}");
Console.WriteLine("Danh sách đăng ký:");
foreach (var r in registrations)
    Console.WriteLine($"  - {memberById[r.MemberId].Name} [{r.Status}]");
Console.WriteLine("Danh sách đã check-in:");
foreach (var c in checkIns)
    Console.WriteLine($"  - {memberById[registrationById[c.RegistrationId].MemberId].Name} lúc {c.CheckedInAt:HH:mm:ss}");

// Bước 7: vi phạm luật nghiệp vụ
PrintStep("Bước 7: Thử thao tác vi phạm luật nghiệp vụ");
var cancelled = registrations[1];
cancelled.Cancel();
Console.WriteLine($"Đã huỷ phiếu của {memberById[cancelled.MemberId].Name} -> {cancelled.Status}");
try
{
    cancelled.Confirm();
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Bắt được InvalidOperationException: {ex.Message}");
}
try
{
    CheckIn.Create(cancelled, CheckInMethod.Manual);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Bắt được InvalidOperationException: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine(new string('=', SeparatorWidth));
Console.WriteLine("Hoàn tất demo.");
