using Ch02.Refactor;
using Ch02.Refactor.Domain;

namespace Ch02.Refactor.Tests;

// Mẫu AAA: Arrange (chuẩn bị) - Act (thực hiện) - Assert (kiểm tra).
// Mỗi test chỉ kiểm tra MỘT hành vi. Đặt tên: TênMethod_TìnhHuống_KếtQuảMongĐợi.
public class DomainTests
{
    // Test mẫu đã viết sẵn: chạy được ngay trên ClubManager hiện tại.
    [Fact]
    public void CreateClub_WithEmptyName_ThrowsArgumentException()
    {
        // Arrange
        var manager = new ClubManager();

        // Act
        var act = () => manager.CreateClub("", "mô tả", "p@gmail.com");

        // Assert
        Assert.Throws<ArgumentException>(act);
    }

    // TODO (Nhiệm vụ 4): bỏ Skip và viết thân test cho từng trường hợp dưới đây.
    // Cần ≥ 6 test AAA cho lớp Domain (Club, Event), trong đó có cả trường hợp lỗi.

    [Fact]
    public void Club_Create_WithValidName_ReturnsClub()
    {
        // Arrange
        var name = "CLB Công Nghệ";
        var description = "Chia sẻ kiến thức lập trình";

        // Act
        var club = Club.Create(name, description);

        // Assert
        Assert.NotNull(club);
        Assert.Equal(name, club.Name);
        Assert.Equal(description, club.Description);
    }

    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Club_Create_WithInvalidName_ThrowsArgumentException(string invalidName)
    {
        // Arrange
        var tooLongName = new string('A', 101);

        // Act and Assert
        Assert.Throws<ArgumentException>(() => Club.Create(invalidName, "Mô tả"));
        Assert.Throws<ArgumentException>(() => Club.Create(tooLongName, "Mô tả"));


    }

    [Fact]
    public void Club_HasRoom_AtMaxMembers_ReturnsFalse()
    {
        // Arrange
        var club = Club.Create("CLB Cờ Vua", "Sinh hoạt cuối tuần");

        // Act
        var hasRoomAt49 = club.HasRoom(Club.MaxMembers - 1); // 49 thành viên
        var hasRoomAt50 = club.HasRoom(Club.MaxMembers);     // 50 thành viên

        // Assert
        Assert.True(hasRoomAt49);
        Assert.False(hasRoomAt50);
    }

    [Fact]
    public void Event_Register_NewMember_ReturnsTrue()
    {
        // Arrange
        var ev = new Event { Id = Guid.NewGuid(), Title = "Workshop .NET", Date = DateTime.UtcNow.AddDays(3) };
        var memberId = Guid.NewGuid();

        // Act
        var registered = ev.Register(memberId);

        // Assert
        Assert.True(registered);
    }

    [Fact]
    public void Event_Register_SameMemberTwice_ReturnsFalse()
    {
        // Arrange
        var ev = new Event { Id = Guid.NewGuid(), Title = "Workshop .NET", Date = DateTime.UtcNow.AddDays(3) };
        var memberId = Guid.NewGuid();
        ev.Register(memberId);

        // Act
        var secondRegister = ev.Register(memberId);

        // Assert
        Assert.False(secondRegister);
    }

    [Fact]
    public void Event_CheckIn_UnregisteredMember_ThrowsInvalidOperationException()
    {
        // Arrange
        var ev = new Event { Id = Guid.NewGuid(), Title = "Workshop .NET", Date = DateTime.UtcNow.AddDays(3) };
        var unregisteredMemberId = Guid.NewGuid();
        // Act
        var act = () => ev.CheckIn(unregisteredMemberId);
        // Assert
        Assert.Throws<InvalidOperationException>(act);
    }
}
