-- OnlineAppointmentSystem Default Veri Ekleme Scripti
-- Bu script veritabanı oluşturulduktan sonra çalıştırılmalıdır.

USE [OnlineAppointmentSystemDB]
GO

-- Set QUOTED_IDENTIFIER ON to fix the related error
SET QUOTED_IDENTIFIER ON
GO

-- Veritabanının zaten var olup olmadığını kontrol et
IF EXISTS (SELECT 1 FROM [dbo].[AspNetRoles] WHERE [Id] = N'03112A28-192F-4AE3-A08C-C3D29BB5E671')
BEGIN
    PRINT 'Roller zaten mevcut. Rol ekleme işlemi atlanıyor.'
END
ELSE
BEGIN
    -- Rol verilerini ekle
    INSERT INTO [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) 
    VALUES 
        (N'03112A28-192F-4AE3-A08C-C3D29BB5E671', N'Admin', N'ADMIN', N'463063EA-7D44-4A76-96EE-B0CF3C8A6B1B'),
        (N'7849D1AC-A061-44D7-AF0D-AF1B8A12BF09', N'Employee', N'EMPLOYEE', N'9AA9E89B-6061-4E70-851A-054B808A2D5E'),
        (N'F4278966-9D6D-4912-858C-C2D501A564FE', N'Customer', N'CUSTOMER', N'3EFB32FF-6C15-4933-916A-5C6427047555');
    PRINT 'Rol verileri eklendi.'
END
GO

-- Admin kullanıcısını kontrol et
IF EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE [Id] = N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5')
BEGIN
    PRINT 'Admin kullanıcısı zaten mevcut. Admin kullanıcı ekleme işlemi atlanıyor.'
END
ELSE
BEGIN
    BEGIN TRY
        -- Admin kullanıcısı ekle (Şifre: Admin123!)
        INSERT INTO [dbo].[AspNetUsers] 
            ([Id], [FirstName], [LastName], [Address], [CreatedDate], [IsActive], 
            [UserName], [NormalizedUserName], [Email], [NormalizedEmail], 
            [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], 
            [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
        VALUES
            (N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'Bahadır Alp', N'Er', N'Piripaşa Mahallesi / Beyoğlu', CAST(N'2025-05-07T17:33:17.2130000' AS DateTime2), 1, 
            N'bahadiralper03@gmail.com', N'BAHADIRALPER03@GMAIL.COM', N'bahadiralper03@gmail.com', N'BAHADIRALPER03@GMAIL.COM', 
            1, N'AQAAAAIAAYagAAAAECPK2t12sz/5SxB8NiXRwb+2N8zsVsOS9/iYdEhASCOIGW+gzJWTY2a53uS0OaJdew==', N'ZKPJ5PSVDPWXI5KJY4YNZHKZ5XKPBT6Y', N'37285a73-07ea-4899-84f9-54e82ee3cfc1', 
            N'+905342822401', 0, 0, NULL, 1, 0);

        -- Admin rolünü kullanıcıya ata
        INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
        VALUES (N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'03112A28-192F-4AE3-A08C-C3D29BB5E671');

        -- Admin kullanıcı claim'lerini ekle
        INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue])
        VALUES 
            (N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'FirstName', N'Bahadır Alp'),
            (N'd7c5d09f-d62d-4127-9e71-fa2aff2a31c5', N'LastName', N'Er');
        
        PRINT 'Admin kullanıcısı eklendi.'
    END TRY
    BEGIN CATCH
        PRINT 'Error adding Admin user: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- Örnek çalışan kullanıcısını kontrol et
IF EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE [Id] = N'3cb7dba1-1b06-4d63-b7ba-225528dbb452')
BEGIN
    PRINT 'Çalışan kullanıcısı zaten mevcut. Çalışan kullanıcı ekleme işlemi atlanıyor.'
END
ELSE
BEGIN
    BEGIN TRY
        -- Örnek bir çalışan kullanıcısı ekle (Şifre: Employee123!)
        INSERT INTO [dbo].[AspNetUsers] 
            ([Id], [FirstName], [LastName], [Address], [CreatedDate], [IsActive], 
            [UserName], [NormalizedUserName], [Email], [NormalizedEmail], 
            [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], 
            [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
        VALUES
            (N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'Hüseyin Cenk', N'Karatopcu', N'Beylikdüzü', CAST(N'2025-05-13T01:53:00.3784454' AS DateTime2), 1, 
            N'cenkkrtpc@hastane.com', N'CENKKRTPC@GMAIL.COM', N'cenkkrtpc@hastane.com', N'CENKKRTPC@GMAIL.COM', 
            1, N'AQAAAAIAAYagAAAAEPlg/ExAF5/k2dW2CYimG2i8JBjXHaOo66T2T1+VcBXJcbqhJK5o/qSa+xEMaph2eQ==', N'T22UBB3TZTWNB6D6ILNQ367KK64TJBBX', N'caa69190-ec2b-4d2b-987d-064ee4fc24bd', 
            N'+905384777725', 0, 0, NULL, 1, 0);


        -- Çalışan rolünü kullanıcıya ata
        INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
        VALUES (N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'7849D1AC-A061-44D7-AF0D-AF1B8A12BF09');

        -- Çalışan kullanıcı claim'lerini ekle
        INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue])
        VALUES 
            (N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'FirstName', N'Hüseyin Cenk'),
            (N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'LastName', N'Karatopcu');

        -- Çalışan kaydını ekle
        INSERT INTO [dbo].[Employees] ([UserId], [Title], [Department], [IsActive], [CreatedDate], [UpdatedDate])
        VALUES (N'3cb7dba1-1b06-4d63-b7ba-225528dbb452', N'Op.Dr', N'Beyin ve Sinir Cerrahisi', 1, 
        CAST(N'2025-05-13T01:53:00.7546403' AS DateTime2), NULL);
        
        PRINT 'Çalışan kullanıcısı eklendi.'
    END TRY
    BEGIN CATCH
        PRINT 'Error adding Employee user: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- Örnek müşteri kullanıcısını kontrol et
IF EXISTS (SELECT 1 FROM [dbo].[AspNetUsers] WHERE [Id] = N'5b1c5b06-4357-4745-bfb5-6246a86a8d75')
BEGIN
    PRINT 'Müşteri kullanıcısı zaten mevcut. Müşteri kullanıcı ekleme işlemi atlanıyor.'
END
ELSE
BEGIN
    BEGIN TRY
        -- Örnek bir müşteri kullanıcısı ekle (Şifre: Customer123!)
        INSERT INTO [dbo].[AspNetUsers] 
            ([Id], [FirstName], [LastName], [Address], [CreatedDate], [IsActive], 
            [UserName], [NormalizedUserName], [Email], [NormalizedEmail], 
            [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], 
            [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount])
        VALUES
            (N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', N'Mehmet Emin', N'Kök', N'Ümraniye', CAST(N'2025-05-13T01:26:09.4371843' AS DateTime2), 1, 
            N'bahadir.ermon@outlook.com', N'BAHADIR.ERMON@OUTLOOK.COM', N'bahadir.ermon@outlook.com', N'BAHADIR.ERMON@OUTLOOK.COM',
            1, N'AQAAAAIAAYagAAAAEI3jlfoQRp1DorxGVa3Uq0hDMNcFPBEBbf/eqVjMl+wtR58ZZQ1BP0CarqFVUuuzlw==', N'B4ZL5JV757XT6HSBLJDZSXIOI5L5RLAI', N'9743ffb0-a69e-4a5d-96a4-55e34ee5b8db', 
            N'+905522920355', 0, 0, NULL, 1, 0);

        -- Müşteri rolünü kullanıcıya ata
        INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId])
        VALUES (N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', N'F4278966-9D6D-4912-858C-C2D501A564FE');

        -- Müşteri kullanıcı claim'lerini ekle
        INSERT INTO [dbo].[AspNetUserClaims] ([UserId], [ClaimType], [ClaimValue])
        VALUES 
            (N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', N'FirstName', N'Hasta'),
            (N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', N'LastName', N'Örnek');

        -- Müşteri kaydını ekle
        INSERT INTO [dbo].[Customers] ([UserId], [DateOfBirth], [Gender], [CreatedDate], [UpdatedDate], [IsActive])
        VALUES (N'5b1c5b06-4357-4745-bfb5-6246a86a8d75', NULL, NULL, CAST(N'2025-05-13T01:26:09.8282393' AS DateTime2), NULL, 1);
        
        PRINT 'Müşteri kullanıcısı eklendi.'
    END TRY
    BEGIN CATCH
        PRINT 'Error adding Customer user: ' + ERROR_MESSAGE();
    END CATCH
END
GO

-- Hizmetleri kontrol et
IF EXISTS (SELECT 1 FROM [dbo].[Services] WHERE [ServiceName] = N'Kardiyoloji')
BEGIN
    PRINT 'Hizmetler zaten mevcut. Hizmet ekleme işlemi atlanıyor.'
END
ELSE
BEGIN
    -- Hizmetleri ekle
    INSERT INTO [dbo].[Services] ([ServiceName], [Description], [Duration], [Price], [IsActive], [CreatedDate], [UpdatedDate])
    VALUES
        (N'Kardiyoloji', N'Kalp ve damar hastalıklarının tanı ve tedavisi.', 15, CAST(500.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), NULL),
        (N'Dermatoloji', N'Cilt hastalıklarının teşhis ve tedavisi.', 15, CAST(400.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), NULL),
        (N'Nöroloji', N'Beyin ve sinir sistemi rahatsızlıkları.', 15, CAST(550.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), NULL),
        (N'Ortopedi', N'Kemik, eklem ve kas sistemi hastalıkları.', 15, CAST(450.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), NULL),
        (N'Göz Hastalıkları', N'Görme ve göz hastalıklarının tedavisi.', 15, CAST(350.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), NULL),
        (N'Kulak Burun Boğaz', N'KBB rahatsızlıklarının teşhis ve tedavisi.', 15, CAST(400.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), NULL),
        (N'Üroloji', N'İdrar yolları ve üreme sistemi hastalıkları.', 15, CAST(500.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), NULL),
        (N'Kadın Doğum', N'Kadın sağlığı ve gebelik takibi.', 15, CAST(500.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), NULL),
        (N'Dahiliye', N'İç hastalıklarının genel kontrolü.', 15, CAST(400.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), NULL),
        (N'Psikiyatri', N'Ruh sağlığı ve hastalıkları tedavisi.', 15, CAST(600.00 AS Decimal(18, 2)), 1, CAST(N'2025-05-07T19:04:41.7530000' AS DateTime2), NULL);
    
    PRINT 'Hizmetler eklendi.'
END
GO

-- Çalışan-Hizmet ilişkilerini ve çalışma saatlerini kontrol et
DECLARE @EmployeeId INT
SELECT @EmployeeId = EmployeeId FROM [dbo].[Employees] WHERE UserId = N'3cb7dba1-1b06-4d63-b7ba-225528dbb452'

-- Ensure we have a valid EmployeeId
IF @EmployeeId IS NOT NULL
BEGIN
    -- Doktor-Hizmet ilişkisini kontrol et
    IF NOT EXISTS (SELECT 1 FROM [dbo].[EmployeeServices] WHERE [EmployeeId] = @EmployeeId AND [ServiceId] = 3)
    BEGIN
        -- Doktor-Hizmet ilişkilerini ekle
        INSERT INTO [dbo].[EmployeeServices] ([EmployeeId], [ServiceId])
        VALUES (@EmployeeId, 3);
        
        PRINT 'Doktor-Hizmet ilişkisi eklendi.'
    END
    ELSE
    BEGIN
        PRINT 'Doktor-Hizmet ilişkisi zaten mevcut.'
    END
    
    -- Çalışma saatlerini kontrol et
    IF NOT EXISTS (SELECT 1 FROM [dbo].[WorkingHours] WHERE [EmployeeId] = @EmployeeId)
    BEGIN
        -- Çalışma saatlerini ekle
        INSERT INTO [dbo].[WorkingHours] ([EmployeeId], [DayOfWeek], [StartTime], [EndTime], [IsActive])
        VALUES
            (@EmployeeId, 1, CAST(N'08:00:00' AS Time), CAST(N'17:00:00' AS Time), 1),
            (@EmployeeId, 2, CAST(N'08:00:00' AS Time), CAST(N'17:00:00' AS Time), 1),
            (@EmployeeId, 3, CAST(N'08:00:00' AS Time), CAST(N'17:00:00' AS Time), 1),
            (@EmployeeId, 4, CAST(N'08:00:00' AS Time), CAST(N'17:00:00' AS Time), 1),
            (@EmployeeId, 5, CAST(N'08:00:00' AS Time), CAST(N'17:00:00' AS Time), 1);
        
        PRINT 'Çalışma saatleri eklendi.'
    END
    ELSE
    BEGIN
        PRINT 'Çalışma saatleri zaten mevcut.'
    END
END
ELSE
BEGIN
    PRINT 'Employee record not found for UserId: 3cb7dba1-1b06-4d63-b7ba-225528dbb452';
END
GO

PRINT 'Veri ekleme/kontrol işlemi tamamlandı.'
GO