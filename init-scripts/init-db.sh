#!/bin/bash
set -e

echo "Waiting for SQL Server to be ready..."
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Appt@Sys0nlin3DB!2024 -Q "SELECT 1" -b -o /dev/null
while [ $? -ne 0 ]
do
    sleep 5
    echo "Waiting for SQL Server to be ready..."
    /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Appt@Sys0nlin3DB!2024 -Q "SELECT 1" -b -o /dev/null
done

echo "SQL Server is ready."

echo "OnlineAppointmentSystemDB veritabanı oluşturuluyor..."
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Appt@Sys0nlin3DB!2024 -Q "IF DB_ID('OnlineAppointmentSystemDB') IS NULL CREATE DATABASE OnlineAppointmentSystemDB"

echo "Tablo ve kısıtlamalar oluşturuluyor..."
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Appt@Sys0nlin3DB!2024 -d OnlineAppointmentSystemDB -i /init-scripts/init-db.sql

echo "Veritabanı oluşturma işlemi tamamlandı."

# Varsayılan verileri ekleme
echo "Varsayılan veriler ekleniyor..."
/opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P Appt@Sys0nlin3DB!2024 -d OnlineAppointmentSystemDB -i /init-scripts/insert-default-data.sql

if [ $? -eq 0 ]
then
    echo "Varsayılan veriler başarıyla eklendi."
else
    echo "Varsayılan verileri eklerken hata oluştu."
fi 