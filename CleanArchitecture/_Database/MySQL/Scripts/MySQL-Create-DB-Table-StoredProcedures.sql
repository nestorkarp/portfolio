CREATE DATABASE  IF NOT EXISTS `demohr` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `demohr`;
-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: demohr
-- ------------------------------------------------------
-- Server version	8.0.46

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `employee`
--

DROP TABLE IF EXISTS employee;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE employee (
  ele_id int NOT NULL AUTO_INCREMENT,
  ele_identityCard varchar(11) NOT NULL,
  ele_firstName varchar(30) NOT NULL,
  ele_lastName varchar(30) NOT NULL,
  ele_birthdate date NOT NULL,
  ele_active tinyint NOT NULL,
  ele_audit_datetime timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (ele_id),
  UNIQUE KEY ELE_IDENTITYCARD_UNIQUE (ele_identityCard)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping routines for database 'demohr'
--
/*!50003 DROP PROCEDURE IF EXISTS SP_Employee_C */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=root@localhost PROCEDURE SP_Employee_C(
    IN p_IdentityCard VARCHAR(11),
    IN p_Firstname VARCHAR(30),
    IN p_Lastname VARCHAR(30),
    IN p_Birthdate DATE,
    IN p_Active TINYINT,
    OUT p_Id INT,
    OUT p_ErrorSource VARCHAR(10),
    OUT p_ErrorMessage VARCHAR(255)
)
BEGIN
    -- Manejo de error para clave duplicada (Validación)
    DECLARE EXIT HANDLER FOR 1062
    BEGIN
        SET p_Id = NULL;
        SET p_ErrorSource = 'VALIDATION';
        SET p_ErrorMessage = 'Registro Existente';
    END;

    -- Manejo de errores genéricos (Técnicos)
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_Id = NULL;
        SET p_ErrorSource = 'TECHNICAL';
        SET p_ErrorMessage = 'Error interno al intentar insertar el registro';
    END;

    -- Inicialización de variables de salida
    SET p_ErrorSource = NULL;
    SET p_ErrorMessage = NULL;

    -- Ejecución de la inserción
    INSERT INTO EMPLOYEE (ele_identityCard, ele_firstname, ele_lastname, ele_birthdate, ele_active)
    VALUES (p_IdentityCard, p_Firstname, p_Lastname, p_Birthdate, p_Active);

    -- Recuperación del último Id generado
    SET p_Id = LAST_INSERT_Id();
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS SP_Employee_R */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=root@localhost PROCEDURE SP_Employee_R(
    IN p_Id INT,
    IN p_IdentityCard VARCHAR(11),
    IN p_Active TINYINT,
    OUT p_ErrorSource VARCHAR(10),
    OUT p_ErrorMessage VARCHAR(255)
)
BEGIN
    -- Manejo de errores genéricos (Técnicos)
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_ErrorSource = 'TECHNICAL';
        SET p_ErrorMessage = 'Error interno al intentar leer los registros';
    END;

    -- Inicialización de variables de salIda
    SET p_ErrorSource = NULL;
    SET p_ErrorMessage = NULL;

    -- Consulta con lógica para parámetros dinámicos
    SELECT 
        ele_id AS EmployeeId,
        ele_identityCard AS IdentityCard,
        ele_firstname AS Firstname,
        ele_lastname AS Lastname,
        date_format(ele_birthdate, '%d/%m/%Y') AS Birthdate,
        ele_active AS Active,
        ele_audit_datetime AS Audit_DateTime
    FROM EMPLOYEE
   WHERE ((p_Id = 0) OR (ELE_ID = p_Id))
       AND (  ( (p_IdentityCard IS NULL) OR (LENGTH(TRIM(p_IdentityCard))= 0))  OR (ELE_IDENTITYCARD = p_IdentityCard))
       AND ((p_Active IS NULL) OR (ELE_ACTIVE = p_Active));
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP PROCEDURE IF EXISTS SP_Employee_U */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=root@localhost PROCEDURE SP_Employee_U(
    IN p_Id INT,
    IN p_Firstname VARCHAR(30),
    IN p_Lastname VARCHAR(30),
    IN p_Birthdate DATE,
    IN p_Active TINYINT,
    OUT p_ErrorSource VARCHAR(10),
    OUT p_ErrorMessage VARCHAR(255)
)
BEGIN
    -- Manejo de errores genéricos (Técnicos)
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        SET p_ErrorSource= 'TECHNICAL';
        SET p_ErrorMessage = 'Error interno al intentar actualizar el registro';
    END;

    -- Inicialización de variables de salIda
    SET p_ErrorSource = NULL;
    SET p_ErrorMessage = NULL;

    -- Ejecución de la actualización
    UPDATE EMPLOYEE 
    SET ele_firstname = p_Firstname,
        ele_lastname = p_Lastname,
        ele_birthdate = p_Birthdate,
        ele_active = p_Active
    WHERE (ele_Id = p_Id);

    -- ValIdación si el registro no existía
    IF ROW_COUNT() = 0 THEN
        SET p_ErrorSource = 'VALIDATION';
        SET p_ErrorMessage = 'Registro Inexistente';
    END IF;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-28 13:48:23
