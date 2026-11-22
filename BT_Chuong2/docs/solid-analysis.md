# Phân tích vi phạm SOLID trong `ClubManager`

File phân tích: `Ch02.Refactor/ClubManager.cs` (bản gốc, trước khi refactor).

`ClubManager` là một "God class": cùng lúc quản lý CLB, thành viên, sự kiện, ghi log, gửi thông báo và in báo cáo. Bảng dưới liệt kê 8 vi phạm (yêu cầu tối thiểu là 4), kèm dòng code, nguyên tắc bị vi phạm, hậu quả và cách đã khắc phục.

| # | Dòng | Nguyên tắc | Vi phạm | Hậu quả | Cách khắc phục |
|---|------|-----------|---------|---------|----------------|
| 1 | 35–64 (`CreateClub`) | **SRP** | Một method làm 4 việc: validate, lưu, ghi log, gửi thông báo. | Đổi quy tắc validate, đổi nơi lưu, đổi cách log hay đổi cách thông báo đều phải sửa cùng một method. Khó test riêng từng việc. | Validate chuyển vào `Club.Create`. Lưu qua `IClubRepository`. Log qua `IAppLogger`. Thông báo qua `INotificationService`. Điều phối trong `ClubService.CreateClub`. |
| 2 | 52–63 (chuỗi `if/else` theo đuôi email) | **OCP** | Chọn cách gửi thông báo bằng `if (email.Contains("@gmail.com")) ... else if ("@yahoo.com") ...`. | Muốn thêm SMS, Slack hay nhà cung cấp email khác phải **sửa** method đang chạy tốt, dễ sinh lỗi mới. Logic gửi lại bị chọn theo đuôi email chứ không theo kênh thật. | **Factory**: `INotificationServiceFactory.Create(NotificationChannel)` trả về `Email/Sms/SlackNotificationService`. Thêm kênh mới = thêm class và một nhánh trong factory, không đụng `ClubService`. |
| 3 | 48, 79, 94, 111 (`File.AppendAllText`) | **DIP** | Module cấp cao (nghiệp vụ CLB) phụ thuộc trực tiếp vào chi tiết cấp thấp (`File`, đường dẫn `"club-log.txt"` ở dòng 32). | Không thể test mà không tạo file thật. Muốn log ra console hay database phải sửa 4 chỗ. Lặp lại cùng đoạn code ghi log. | Phụ thuộc vào `IAppLogger`. Có `ConsoleLogger`, `FileLogger`, inject qua constructor. |
| 4 | 30–31 (`new List<Club>()`, `new List<Member>()`) và 116 (`GetAllClubs`) | **DIP** + đóng gói | Lưu dữ liệu bằng list cụ thể nằm ngay trong class, `GetAllClubs()` còn trả về chính list nội bộ cho bên ngoài. | Không thể thay bằng database. Bên ngoài có thể `Add/Remove` trực tiếp, bỏ qua mọi quy tắc nghiệp vụ. | `IClubRepository`, `IMemberRepository`, `IEventRepository` (hiện thực `InMemory*Repository`). Repository chỉ trả `IReadOnlyList`. |
| 5 | 82–83 (`AddMemberToClub`) | **OCP** + **SRP** | Cứng 2 dòng thông báo: một email, một Slack. | Muốn bỏ Slack hay thêm SMS phải sửa method. Nghiệp vụ "thêm thành viên" bị trộn với "gửi thông báo". | `ClubService.AddMember` chỉ gọi `INotificationService.Send`. Kênh nào do factory quyết định. |
| 6 | 88–99 (`CreateEvent`, đặc biệt 96–97) | **SRP** + **OCP** | Tạo sự kiện và tự gửi email cho từng thành viên trong cùng method. | Muốn thêm một hành động khi có sự kiện mới (log, Slack, push) phải sửa `CreateEvent`. | **Observer**: `IEventObserver.OnEventCreated`. `EventService` chỉ thông báo cho observer đã `Subscribe` (`EmailNotifier`, `LogNotifier`). Thêm hành động = thêm observer mới. |
| 7 | 103–113 (`PrintClubReport`) | **SRP** + **OCP** + **DIP** | Vừa tính số liệu, vừa in ra `Console`, vừa ghi file log (dòng 111). | Muốn xuất CSV phải sửa hoặc nhân bản method. Không kiểm thử được vì kết quả chỉ nằm trên màn hình, không trả về giá trị. | **Strategy**: `IReportStrategy` (`ConsoleReportStrategy`, `CsvReportStrategy`). `ReportService.Generate(strategy)` tính số liệu rồi trả về chuỗi. |
| 8 | 40–41, 72–73 (`100`, `50`) và 44, 75, 93 (`new Club`, `new Member`, `new Event`) | **SRP** (Domain bị bỏ trống) | Quy tắc nghiệp vụ (tên ≤ 100 ký tự, tối đa 50 thành viên) viết như magic number trong class điều phối; các entity chỉ là túi dữ liệu. | Quy tắc nằm sai chỗ nên không test được riêng. Nếu nơi khác cũng tạo `Club` thì phải chép lại quy tắc. | Chuyển vào Domain: `Club.Create`, `Club.HasRoom`, `Club.MaxMembers`, `Club.MaxNameLength`, `Event.Register`, `Event.CheckIn`. |

## Bonus: lỗi nghiệp vụ thấy khi đọc code

- Dòng 93–98: `CreateEvent` tạo `ev` rồi trả về mà **không lưu vào đâu cả**. Sự kiện biến mất ngay sau khi method kết thúc.
- Dòng 88–93: `CreateEvent` không kiểm tra `title` rỗng. Bản refactor (`EventService.CreateEvent`) đã thêm kiểm tra.

## Về LSP và ISP

Code gốc hầu như không có kế thừa hay interface nên chưa thể vi phạm trực tiếp LSP và ISP. Cái "vi phạm" ở đây là **thiếu** abstraction, và đây là cách bản refactor tuân thủ hai nguyên tắc này:

- **ISP:** các interface nhỏ, mỗi cái một vai trò (`IAppLogger` chỉ có `Info`; `INotificationService` chỉ có `Send`; `IEventObserver` chỉ có `OnEventCreated`). Class chỉ phụ thuộc vào đúng thứ nó dùng, ví dụ `LogNotifier` chỉ biết `IAppLogger`.
- **LSP:** mọi hiện thực thay thế được cho nhau mà code gọi không phải sửa. Ví dụ `LoggingClubRepository` thay `InMemoryClubRepository` ở bất kỳ chỗ nào cần `IClubRepository`, và `Program.cs` hoán đổi `Email/Sms/Slack` mà vẫn chạy.
