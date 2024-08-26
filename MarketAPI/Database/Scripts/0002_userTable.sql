CREATE TABLE 
    IF NOT EXISTS user (
      id text PRIMARY KEY,
      name varchar(255) NOT NULL,
      email varchar(255) NOT NULL,
      password varchar(255) NOT NULL,
      role varchar(255) NOT NULL
    );