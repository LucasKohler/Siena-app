#Requires -Version 5.1
<#
.SYNOPSIS
  Cria usuário/banco PostgreSQL e aplica migrations EF do Siena.

  Preferir o stack completo: docker compose up --build (ver README.md).

.PARAMETER PostgresAdminPassword
  Senha do usuário postgres (superuser) instalado localmente.

.EXAMPLE
  .\scripts\setup-database.ps1 -PostgresAdminPassword 'sua-senha-postgres'
#>
param(
    [Parameter(Mandatory = $true)]
    [string] $PostgresAdminPassword
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$psql = 'C:\Program Files\PostgreSQL\17\bin\psql.exe'

if (-not (Test-Path $psql)) {
    $found = Get-ChildItem 'C:\Program Files\PostgreSQL\*\bin\psql.exe' -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($found) { $psql = $found.FullName }
    else { throw 'psql não encontrado. Instale PostgreSQL ou ajuste o caminho no script.' }
}

$env:PGPASSWORD = $PostgresAdminPassword
$sqlFile = Join-Path $PSScriptRoot 'init-postgres.sql'

Write-Host 'Criando role e database siena...'
& $psql -U postgres -h localhost -p 5432 -d postgres -f $sqlFile
if ($LASTEXITCODE -ne 0) { throw "psql falhou com código $LASTEXITCODE" }

Remove-Item Env:PGPASSWORD -ErrorAction SilentlyContinue

Write-Host 'Aplicando migrations EF...'
Push-Location (Join-Path $repoRoot 'apps\api\src\Siena.Infrastructure')
try {
    dotnet ef database update --startup-project ..\Siena.Api\Siena.Api.csproj
    if ($LASTEXITCODE -ne 0) { throw "dotnet ef database update falhou com código $LASTEXITCODE" }
}
finally {
    Pop-Location
}

Write-Host 'Banco pronto: Host=localhost;Port=5432;Database=siena;Username=siena;Password=siena_dev'
