-- Şifre sıfırlama scripti
-- Yeni oluşturulan hash'i buraya yapıştırın
-- Örnek: Bu hash "Test123!" şifresi için oluşturulmuştur
DECLARE @NewPasswordHash NVARCHAR(MAX) = 'AQAAAAIAAYagAAAAEF7pNm2H8fOCQrCCXzB8MsJFOXVuADQTzrZwp2XyE61PtBxCWJGt07lY02I9Tw2t4w==';

-- Admin kullanıcısı için şifre güncelleme
UPDATE [dbo].[AspNetUsers]
SET [PasswordHash] = @NewPasswordHash
WHERE [Email] = 'EPOSTA';


PRINT 'Şifreler başarıyla güncellendi. Yeni şifre: "Test123!"'; 