CREATE DATABASE  IF NOT EXISTS `caece` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */;
USE `caece`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: caece
-- ------------------------------------------------------
-- Server version	8.0.11

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `alumno`
--

DROP TABLE IF EXISTS `alumno`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `alumno` (
  `idAlumno` int(11) NOT NULL,
  `Matricula` varchar(45) DEFAULT NULL,
  `Contraseña` varchar(45) DEFAULT NULL,
  `PlanDeCarrera_idPlanDeCarrera` int(11) NOT NULL,
  PRIMARY KEY (`idAlumno`,`PlanDeCarrera_idPlanDeCarrera`),
  KEY `fk_Alumno_PlanDeCarrera1_idx` (`PlanDeCarrera_idPlanDeCarrera`),
  CONSTRAINT `fk_Alumno_PlanDeCarrera1` FOREIGN KEY (`PlanDeCarrera_idPlanDeCarrera`) REFERENCES `plandecarrera` (`idplandecarrera`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `alumno`
--

LOCK TABLES `alumno` WRITE;
/*!40000 ALTER TABLE `alumno` DISABLE KEYS */;
/*!40000 ALTER TABLE `alumno` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `correlativas`
--

DROP TABLE IF EXISTS `correlativas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `correlativas` (
  `Materia_has_PlanDeCarrera_Materia_idMateria` int(11) NOT NULL,
  `Materia_has_PlanDeCarrera_PlanDeCarrera_idPlanDeCarrera` int(11) NOT NULL,
  `Materia_has_PlanDeCarrera_Materia_idMateria1` int(11) NOT NULL,
  `Materia_has_PlanDeCarrera_PlanDeCarrera_idPlanDeCarrera1` int(11) NOT NULL,
  `ParaCursar` char(1) DEFAULT NULL,
  `ParaAprobar` char(1) DEFAULT NULL,
  PRIMARY KEY (`Materia_has_PlanDeCarrera_Materia_idMateria`,`Materia_has_PlanDeCarrera_PlanDeCarrera_idPlanDeCarrera`,`Materia_has_PlanDeCarrera_Materia_idMateria1`,`Materia_has_PlanDeCarrera_PlanDeCarrera_idPlanDeCarrera1`),
  KEY `fk_Materia_has_PlanDeCarrera_has_Materia_has_PlanDeCarrera__idx` (`Materia_has_PlanDeCarrera_Materia_idMateria1`,`Materia_has_PlanDeCarrera_PlanDeCarrera_idPlanDeCarrera1`),
  KEY `fk_Materia_has_PlanDeCarrera_has_Materia_has_PlanDeCarrera__idx1` (`Materia_has_PlanDeCarrera_Materia_idMateria`,`Materia_has_PlanDeCarrera_PlanDeCarrera_idPlanDeCarrera`),
  CONSTRAINT `fk_Materia_has_PlanDeCarrera_has_Materia_has_PlanDeCarrera_Ma1` FOREIGN KEY (`Materia_has_PlanDeCarrera_Materia_idMateria`, `Materia_has_PlanDeCarrera_PlanDeCarrera_idPlanDeCarrera`) REFERENCES `materia_has_plandecarrera` (`materia_idmateria`, `plandecarrera_idplandecarrera`),
  CONSTRAINT `fk_Materia_has_PlanDeCarrera_has_Materia_has_PlanDeCarrera_Ma2` FOREIGN KEY (`Materia_has_PlanDeCarrera_Materia_idMateria1`, `Materia_has_PlanDeCarrera_PlanDeCarrera_idPlanDeCarrera1`) REFERENCES `materia_has_plandecarrera` (`materia_idmateria`, `plandecarrera_idplandecarrera`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `correlativas`
--

LOCK TABLES `correlativas` WRITE;
/*!40000 ALTER TABLE `correlativas` DISABLE KEYS */;
/*!40000 ALTER TABLE `correlativas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `materia`
--

DROP TABLE IF EXISTS `materia`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `materia` (
  `idMateria` int(11) NOT NULL,
  `nombre` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idMateria`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `materia`
--

LOCK TABLES `materia` WRITE;
/*!40000 ALTER TABLE `materia` DISABLE KEYS */;
/*!40000 ALTER TABLE `materia` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `materia_has_plandecarrera`
--

DROP TABLE IF EXISTS `materia_has_plandecarrera`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `materia_has_plandecarrera` (
  `Materia_idMateria` int(11) NOT NULL,
  `PlanDeCarrera_idPlanDeCarrera` int(11) NOT NULL,
  PRIMARY KEY (`Materia_idMateria`,`PlanDeCarrera_idPlanDeCarrera`),
  KEY `fk_Materia_has_PlanDeCarrera_PlanDeCarrera1_idx` (`PlanDeCarrera_idPlanDeCarrera`),
  KEY `fk_Materia_has_PlanDeCarrera_Materia_idx` (`Materia_idMateria`),
  CONSTRAINT `fk_Materia_has_PlanDeCarrera_Materia` FOREIGN KEY (`Materia_idMateria`) REFERENCES `materia` (`idmateria`),
  CONSTRAINT `fk_Materia_has_PlanDeCarrera_PlanDeCarrera1` FOREIGN KEY (`PlanDeCarrera_idPlanDeCarrera`) REFERENCES `plandecarrera` (`idplandecarrera`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `materia_has_plandecarrera`
--

LOCK TABLES `materia_has_plandecarrera` WRITE;
/*!40000 ALTER TABLE `materia_has_plandecarrera` DISABLE KEYS */;
/*!40000 ALTER TABLE `materia_has_plandecarrera` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `plandecarrera`
--

DROP TABLE IF EXISTS `plandecarrera`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `plandecarrera` (
  `idPlanDeCarrera` int(11) NOT NULL,
  `nombre` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`idPlanDeCarrera`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `plandecarrera`
--

LOCK TABLES `plandecarrera` WRITE;
/*!40000 ALTER TABLE `plandecarrera` DISABLE KEYS */;
/*!40000 ALTER TABLE `plandecarrera` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'caece'
--

--
-- Dumping routines for database 'caece'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-08-11 16:16:47
