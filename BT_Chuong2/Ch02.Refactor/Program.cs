using Ch02.Refactor;
using Ch02.Refactor.Logging;
using Ch02.Refactor.Notification;
using Ch02.Refactor.Patterns.Observers;
using Ch02.Refactor.Patterns.Strategies;
using Ch02.Refactor.Repositories;
using Ch02.Refactor.Services;

// ═══════════════════════════════════════════════════════════════
//  EduHub Ch02 — Refactoring với SOLID và Design Patterns
//  ClubManager.cs chứa code vi phạm SOLID (đọc các TODO ở đầu file đó).
// ═══════════════════════════════════════════════════════════════

// Demo code cũ (vi phạm SOLID) — chạy được nhưng thiết kế rất xấu:
//var manager = new ClubManager();
//manager.CreateClub("Tech Club", "Câu lạc bộ công nghệ", "president@gmail.com");
//manager.AddMemberToClub(manager.GetAllClubs()[0].Id, "Nguyễn Văn A", "a@student.dthu.edu.vn");
//manager.CreateEvent(manager.GetAllClubs()[0].Id, "Workshop SOLID", new DateTime(2026, 11, 20));
//manager.PrintClubReport();

//Console.WriteLine("\n--- TODO: refactor code trên (xem checklist trong de-bai.html) ---");
//Console.WriteLine("1. Phân tích vi phạm SOLID  ->  docs/solid-analysis.md");
//Console.WriteLine("2. Tách ClubManager thành các class nhỏ, phụ thuộc qua interface (SRP, OCP, DIP)");
//Console.WriteLine("3. Áp dụng Factory, Decorator, Strategy, Observer");
//Console.WriteLine("4. Hoàn thiện Domain và viết unit test AAA (dotnet test)");
//Console.WriteLine("5. Nhánh feature/solid-refactor, Conventional Commits, rebase, Pull Request");
IAppLogger logger = new ConsoleLogger();

INotificationServiceFactory notificationFactory = new NotificationServiceFactory();
INotificationService notifier = notificationFactory.Create(NotificationChannel.Email);

IClubRepository clubRepo = new LoggingClubRepository(new InMemoryClubRepository(), logger);
IMemberRepository memberRepo = new InMemoryMemberRepository();
IEventRepository eventRepo = new InMemoryEventRepository();

IClubService clubService = new ClubService(clubRepo, memberRepo, notifier, logger);
IEventService eventService = new EventService(clubRepo, memberRepo, eventRepo);
IReportService reportService = new ReportService(clubRepo, memberRepo, eventRepo);

Console.WriteLine("== 1. Tạo CLB và thêm thành viên ==");
var club = clubService.CreateClub("Tech Club", "Câu lạc bộ công nghệ", "president@gmail.com");
var a = clubService.AddMember(club.Id, "Nguyễn Văn A", "a@student.dthu.edu.vn");
var b = clubService.AddMember(club.Id, "Trần Thị B", "b@student.dthu.edu.vn");

Console.WriteLine("\n== 2. Observer: tạo sự kiện -> 2 observer được báo ==");
eventService.Subscribe(new LogNotifier(logger));
eventService.Subscribe(new EmailNotifier(memberRepo, notifier));
var ev = eventService.CreateEvent(club.Id, "Workshop SOLID", new DateTime(2026, 11, 20));
eventService.Register(ev.Id, a.Id);
eventService.Register(ev.Id, b.Id);
eventService.CheckIn(ev.Id, a.Id);

Console.WriteLine("\n== 3. Strategy: báo cáo Console ==");
IReportStrategy console = new ConsoleReportStrategy();
Console.Write(reportService.Generate(console));

Console.WriteLine("== 3. Strategy: báo cáo CSV ==");
IReportStrategy csv = new CsvReportStrategy();
Console.Write(reportService.Generate(csv));

Console.WriteLine("\n== 4. Factory: tạo kênh thông báo theo yêu cầu ==");
foreach (var channel in Enum.GetValues<NotificationChannel>())
    notificationFactory.Create(channel).Send("a@student.dthu.edu.vn", $"Thử kênh {channel}");

Console.WriteLine("\n== 5. Kiểm tra validate ==");
try { clubService.CreateClub("", "x", "p@gmail.com"); }
catch (ArgumentException ex) { Console.WriteLine($"  Lỗi đúng dự kiến: {ex.Message}"); }
