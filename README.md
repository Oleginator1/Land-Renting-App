# 🌾 Sistem de Evidență a Fermierilor, Terenurilor și Contractelor de Arendă

## Descriere
Aplicație desktop Windows Forms (C# .NET 8) pentru gestionarea
fermierilor înregistrați, terenurilor agricole disponibile
și contractelor de arendă, cu raport financiar detaliat.

## Cerințe de sistem
- Windows 10/11
- .NET 8 Runtime (Windows Desktop)
- SQL Server 2019+ sau SQL Server Express
- Visual Studio 2022 (pentru development)

## Configurare baza de date
1. Deschide **SQL Server Management Studio**
2. Execută scriptul `Database/CreateDatabase.sql`
3. Execută scriptul `Database/SeedData.sql`

## Configurare conexiune
Modifică stringul de conexiune în `Config/AppSettings.cs`:
```csharp
"Server=NUMELE_TĂU\\SQLEXPRESS;Database=ArendaDB;
 Trusted_Connection=True;TrustServerCertificate=True;"
```

## Rulare proiect
```bash
git clone https://github.com/username/ArendaManagement.git
cd ArendaManagement
dotnet restore
dotnet run --project ArendaManagement
```

## Structura proiectului