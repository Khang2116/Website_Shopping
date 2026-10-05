# WebShopping

## Đồ án học tập

- **Tác giả:** Lê Khang
- **Lớp:** CĐ CNTT 24WEBD
- **Mã sinh viên:** 0306241452

## Công nghệ và yêu cầu phiên bản

- ASP.NET Core MVC, .NET 10 (`net10.0`)
- Entity Framework Core (Code First) và SQL Server provider
- SQL Server
- .NET SDK 10.0
- Công cụ EF Core CLI `dotnet-ef`

(Phiên bản chính xác của từng gói NuGet xem trong file `WebShopping.csproj`.)

## Chức năng chính

**Phía khách hàng (Client):**
- Xem sản phẩm mới nhất tại trang chủ
- Xem danh sách sản phẩm, lọc theo danh mục
- Xem chi tiết sản phẩm kèm sản phẩm liên quan
- Giỏ hàng (lưu bằng Session): thêm, sửa số lượng, xóa sản phẩm
- Đặt hàng (Checkout), lưu vào bảng Order và OrderDetail

**Phía quản trị (Admin):**
- Đăng nhập / đăng xuất bằng Cookie Authentication, mật khẩu được băm SHA-256
- Quản lý Danh mục (thêm, sửa, xóa)
- Quản lý Sản phẩm (thêm, sửa, xóa, kèm upload ảnh)
- Quản lý Đơn hàng (xem danh sách, xem chi tiết, cập nhật trạng thái)

## Cài đặt

1. Mở thư mục dự án chứa `WebShopping.csproj`.
2. Cấu hình chuỗi kết nối `ConnectionStrings:DefaultConnection` trong `appsettings.json` để trỏ đến SQL Server của bạn.
3. Khôi phục các gói NuGet:

```powershell
   dotnet restore
```

4. Nếu chưa cài EF Core CLI, cài đặt:

```powershell
   dotnet tool install --global dotnet-ef
```

5. Tạo cơ sở dữ liệu bằng Migration:

```powershell
   dotnet ef database update
```

   Hoặc chạy trực tiếp file script `Database/db_WebShopping.sql` (đính kèm trong đồ án) bằng SQL Server Management Studio để tạo database kèm sẵn dữ liệu mẫu.

6. **Tạo tài khoản quản trị đầu tiên** (migration chỉ tạo cấu trúc bảng, chưa có sẵn tài khoản nào) — chạy câu lệnh sau trong SQL Server:

```sql
   INSERT INTO AdminUsers (Username, PasswordHash)
   VALUES (N'admin', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', '123456'), 2));
```

7. Chạy ứng dụng:

```powershell
   dotnet run
```

8. Truy cập trang chủ: `http://localhost:5081`
9. Truy cập trang quản trị: `http://localhost:5081/Admin/Account/Login`
   - Tài khoản: `admin`
   - Mật khẩu: `123456`

## Cấu trúc thư mục

- `Controllers/`, `Views/` — phía Client
- `Areas/Admin/` — phía quản trị (Controllers, Views, Models riêng)
- `Models/` — các lớp dữ liệu (`Category`, `Product`, `Order`, `OrderDetail`, `AdminUser`, `CartItem`)
- `Data/` — `ApplicationDbContext`
- `Migrations/` — lịch sử thay đổi cấu trúc database
- `wwwroot/img/products/` — ảnh sản phẩm do Admin upload

## Thiết kế cơ sở dữ liệu

- `Category` — danh mục sản phẩm
- `Product` — sản phẩm, khóa ngoại `CategoryId` tham chiếu `Category`
- `Order` — đơn hàng
- `OrderDetail` — chi tiết đơn hàng, khóa ngoại `OrderId`, `ProductId`
- `AdminUser` — tài khoản quản trị

## Ghi chú

- Đặt hàng theo hình thức khách vãng lai (guest checkout), không có hệ thống đăng ký/đăng nhập cho khách hàng.
- Chưa tích hợp cổng thanh toán trực tuyến, đơn hàng mặc định ở trạng thái "Chờ xử lý" sau khi đặt.