# User Stories – EduHub

Hệ thống quản lý Câu lạc bộ & Sự kiện sinh viên – Trường Đại học Đồng Tháp.

## Actor: Sinh viên

### US-01 – Đăng ký tham gia CLB
**As a** sinh viên, **I want** đăng ký tham gia một câu lạc bộ, **so that** tôi trở thành thành viên và nhận thông tin hoạt động của CLB.

Acceptance criteria:
- **Given** tôi chưa là thành viên CLB, **When** tôi gửi đăng ký với tên, email (có `@`) và MSSV hợp lệ, **Then** yêu cầu được ghi nhận và chờ Admin phê duyệt.
- **Given** tôi nhập email thiếu `@` hoặc để trống tên, **When** tôi gửi đăng ký, **Then** hệ thống từ chối và hiển thị lỗi cụ thể.
- **Given** tôi đã là thành viên của CLB, **When** tôi đăng ký lại, **Then** hệ thống báo trùng và không tạo bản ghi mới.

### US-02 – Đăng ký tham dự sự kiện
**As a** sinh viên, **I want** đăng ký tham dự một sự kiện của CLB, **so that** tôi giữ được chỗ và được ban tổ chức xác nhận.

Acceptance criteria:
- **Given** sự kiện còn chỗ, **When** tôi đăng ký, **Then** phiếu đăng ký được tạo với trạng thái `Pending`.
- **Given** số đăng ký đã đạt sức chứa (`Capacity`), **When** tôi đăng ký, **Then** hệ thống từ chối vì hết chỗ.
- **Given** tôi có phiếu `Pending` hoặc `Confirmed`, **When** tôi huỷ, **Then** phiếu chuyển sang `Cancelled`.

### US-03 – Check-in tại sự kiện
**As a** sinh viên, **I want** check-in khi đến sự kiện, **so that** sự có mặt của tôi được ghi nhận.

Acceptance criteria:
- **Given** phiếu đăng ký của tôi đã `Confirmed`, **When** tôi check-in (QR hoặc thủ công), **Then** hệ thống tạo bản ghi `CheckIn` có thời điểm và hình thức check-in.
- **Given** phiếu của tôi đang `Pending` hoặc `Cancelled`, **When** tôi check-in, **Then** hệ thống từ chối.

## Actor: Ban CLB

### US-04 – Tạo sự kiện
**As a** thành viên Ban CLB, **I want** tạo sự kiện cho CLB, **so that** sinh viên có thể đăng ký tham dự.

Acceptance criteria:
- **Given** tôi nhập tiêu đề, ngày, hình thức (online/offline) và sức chứa > 0, **When** tôi lưu, **Then** sự kiện được tạo và gắn với CLB của tôi.
- **Given** sức chứa ≤ 0 hoặc tiêu đề trống, **When** tôi lưu, **Then** hệ thống từ chối và nêu rõ trường sai.

### US-05 – Xem danh sách thành viên
**As a** thành viên Ban CLB, **I want** xem danh sách thành viên của CLB, **so that** tôi nắm được nhân sự để tổ chức hoạt động.

Acceptance criteria:
- Danh sách hiển thị tên, MSSV, email, ngày tham gia và trạng thái hoạt động của từng thành viên.
- Hiển thị tổng số thành viên của CLB.

### US-06 – Xuất báo cáo tham dự
**As a** thành viên Ban CLB, **I want** xuất báo cáo tham dự của một sự kiện, **so that** tôi đánh giá mức độ tham gia và lưu minh chứng.

Acceptance criteria:
- Báo cáo nêu số đăng ký, số check-in và danh sách tên người tham dự.
- Phiếu `Cancelled` không được tính vào số tham dự.

## Actor: Admin

### US-07 – Quản lý CLB
**As an** Admin, **I want** tạo và quản lý các câu lạc bộ, **so that** hệ thống phản ánh đúng các CLB đang hoạt động của trường.

Acceptance criteria:
- **Given** tôi nhập tên CLB không trống, **When** tôi lưu, **Then** CLB được tạo với mô tả và thời điểm tạo.
- **Given** tên CLB trống, **When** tôi lưu, **Then** hệ thống từ chối.

### US-08 – Phê duyệt thành viên
**As an** Admin, **I want** phê duyệt hoặc từ chối yêu cầu gia nhập CLB, **so that** chỉ sinh viên hợp lệ mới trở thành thành viên.

Acceptance criteria:
- **Given** có yêu cầu gia nhập đang chờ, **When** tôi phê duyệt, **Then** sinh viên được thêm vào danh sách thành viên của CLB.
- **Given** có yêu cầu gia nhập đang chờ, **When** tôi từ chối, **Then** sinh viên không được thêm vào CLB.
