CREATE DATABASE DOGA;
USE DATABASE DOGA;

CREATE TABLE user (

   id int(11) NOT NULL AUTO_INCREMENT,

   name varchar(45) DEFAULT NULL,

   password varchar(45) DEFAULT NULL,

   email varchar(45) DEFAULT NULL,

   flag varchar(45) DEFAULT NULL,

   PRIMARY KEY (id)
);