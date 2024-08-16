#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
	-- CREATE USER docker WITH PASSWORD 'docker';
  
	CREATE DATABASE decks;
  \c decks;
  CREATE EXTENSION IF NOT EXISTS "pgcrypto";
  
	CREATE DATABASE users;
  \c users;
  CREATE EXTENSION IF NOT EXISTS "pgcrypto";
  
	CREATE DATABASE auth;
  \c auth;
  CREATE EXTENSION IF NOT EXISTS "pgcrypto";
  
	-- GRANT ALL PRIVILEGES ON DATABASE decks TO docker;
	-- GRANT ALL PRIVILEGES ON DATABASE users TO docker;
	-- GRANT ALL PRIVILEGES ON DATABASE auth TO docker;
EOSQL
