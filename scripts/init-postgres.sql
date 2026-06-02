-- Bootstrap do banco Siena (executar como superuser postgres).
-- Credenciais alinhadas a appsettings.Development.json e .env.example

DO $$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_roles WHERE rolname = 'siena') THEN
        CREATE ROLE siena LOGIN PASSWORD 'siena_dev';
    ELSE
        ALTER ROLE siena WITH LOGIN PASSWORD 'siena_dev';
    END IF;
END
$$;

DO $$
BEGIN
    IF NOT EXISTS (SELECT FROM pg_database WHERE datname = 'siena') THEN
        CREATE DATABASE siena OWNER siena;
    END IF;
END
$$;

GRANT ALL PRIVILEGES ON DATABASE siena TO siena;
