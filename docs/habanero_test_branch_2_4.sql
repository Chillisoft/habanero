-- MySQL Administrator dump 1.4
--
-- ------------------------------------------------------
-- Server version	5.0.45-community-nt


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


--
-- Create schema habanero_test_branch_2_4
--

CREATE DATABASE /*!32312 IF NOT EXISTS*/ habanero_test_branch_2_4;
USE habanero_test_branch_2_4;

--
-- Table structure for table `habanero_test_branch_2_4`.`another_number_generator`
--

DROP TABLE IF EXISTS `another_number_generator`;
CREATE TABLE `another_number_generator` (
  `SequenceNumber` int(10) unsigned NOT NULL default '0',
  `NumberType` varchar(45) NOT NULL default '',
  `UserLocked` varchar(45) default NULL,
  `Locked` tinyint(1) default NULL,
  `MachineLocked` varchar(45) default NULL,
  `OperatingSystemUserLocked` varchar(45) default NULL,
  `DateTimeLocked` datetime default NULL,
  PRIMARY KEY  (`NumberType`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`another_number_generator`
--

/*!40000 ALTER TABLE `another_number_generator` DISABLE KEYS */;
INSERT INTO `another_number_generator` (`SequenceNumber`,`NumberType`,`UserLocked`,`Locked`,`MachineLocked`,`OperatingSystemUserLocked`,`DateTimeLocked`) VALUES 
 (1,'tmp',NULL,NULL,NULL,NULL,NULL);
/*!40000 ALTER TABLE `another_number_generator` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`bowithintid`
--

DROP TABLE IF EXISTS `bowithintid`;
CREATE TABLE `bowithintid` (
  `IntID` int(10) unsigned NOT NULL auto_increment,
  `TestField` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`IntID`)
) ENGINE=InnoDB AUTO_INCREMENT=56 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`bowithintid`
--

/*!40000 ALTER TABLE `bowithintid` DISABLE KEYS */;
INSERT INTO `bowithintid` (`IntID`,`TestField`) VALUES 
 (55,'PropValue');
/*!40000 ALTER TABLE `bowithintid` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`car_table`
--

DROP TABLE IF EXISTS `car_table`;
CREATE TABLE `car_table` (
  `Car_ID` varchar(38) default NULL,
  `Owner_Id` varchar(38) default NULL,
  `Car_Reg_No` varchar(50) default NULL,
  `Driver_FK1` varchar(50) default NULL,
  `Driver_FK2` varchar(50) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`car_table`
--

/*!40000 ALTER TABLE `car_table` DISABLE KEYS */;
INSERT INTO `car_table` (`Car_ID`,`Owner_Id`,`Car_Reg_No`,`Driver_FK1`,`Driver_FK2`) VALUES 
 ('{3C6E0B0C-9AD7-4773-B96D-3B62C054D547}',NULL,'NP32459',NULL,NULL);
/*!40000 ALTER TABLE `car_table` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`circle_concrete`
--

DROP TABLE IF EXISTS `circle_concrete`;
CREATE TABLE `circle_concrete` (
  `CircleID_field` char(38) NOT NULL default '',
  `Radius` int(10) unsigned default NULL,
  `ShapeID_field` char(38) default NULL,
  `ShapeName` varchar(255) default NULL,
  PRIMARY KEY  (`CircleID_field`),
  KEY `Index_2` (`ShapeID_field`),
  CONSTRAINT `FK_circle_concrete_1` FOREIGN KEY (`ShapeID_field`) REFERENCES `shape_table` (`ShapeID_field`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`circle_concrete`
--

/*!40000 ALTER TABLE `circle_concrete` DISABLE KEYS */;
/*!40000 ALTER TABLE `circle_concrete` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`circle_table`
--

DROP TABLE IF EXISTS `circle_table`;
CREATE TABLE `circle_table` (
  `CircleID_field` char(38) default NULL,
  `Radius` int(10) unsigned NOT NULL default '0',
  `ShapeID_field` char(38) default NULL,
  `Colour` int(10) unsigned NOT NULL default '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`circle_table`
--

/*!40000 ALTER TABLE `circle_table` DISABLE KEYS */;
/*!40000 ALTER TABLE `circle_table` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`contact_person`
--

DROP TABLE IF EXISTS `contact_person`;
CREATE TABLE `contact_person` (
  `ContactPersonID` char(38) NOT NULL default '',
  `Surname_field` varchar(255) default NULL,
  `FirstName_field` varchar(255) default NULL,
  `EmailAddress` varchar(255) default NULL,
  `PhoneNumber` varchar(255) default NULL,
  `CellNumber` varchar(255) default NULL,
  `DateOfBirth` datetime default NULL,
  `DateLastUpdated` datetime default NULL,
  `UserLastUpdated` varchar(255) default NULL,
  `MachineLastUpdated` varchar(255) default NULL,
  `VersionNumber` int(11) default NULL,
  `PK2_Prop1` varchar(255) default NULL,
  `PK2_Prop2` varchar(255) default NULL,
  `PK3_Prop` varchar(255) default NULL,
  `OrganisationID` char(38) default NULL,
  `UserLocked` varchar(45) default NULL,
  `DateTimeLocked` datetime default NULL,
  `MachineLocked` varchar(45) default NULL,
  `OperatingSystemUserLocked` varchar(45) default NULL,
  `Locked` tinyint(1) default NULL,
  `IntegerProperty` int(11) default NULL,
  PRIMARY KEY  (`ContactPersonID`),
  UNIQUE KEY `Index_2` (`Surname_field`,`FirstName_field`),
  KEY `FK_contact_person_1` (`OrganisationID`),
  CONSTRAINT `FK_contact_person_1` FOREIGN KEY (`OrganisationID`) REFERENCES `organisation` (`OrganisationID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`contact_person`
--

/*!40000 ALTER TABLE `contact_person` DISABLE KEYS */;
/*!40000 ALTER TABLE `contact_person` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`contact_person_address`
--

DROP TABLE IF EXISTS `contact_person_address`;
CREATE TABLE `contact_person_address` (
  `AddressID` char(38) NOT NULL default '',
  `ContactPersonID` char(38) NOT NULL default '',
  `AddressLine1` varchar(255) default NULL,
  `AddressLine2` varchar(255) default NULL,
  `AddressLine3` varchar(255) default NULL,
  `AddressLine4` varchar(255) default NULL,
  `OrganisationID` char(38) default NULL,
  PRIMARY KEY  (`AddressID`),
  KEY `FK_contact_person_address_1` (`ContactPersonID`),
  CONSTRAINT `FK_contact_person_address_1` FOREIGN KEY (`ContactPersonID`) REFERENCES `contact_person` (`ContactPersonID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`contact_person_address`
--

/*!40000 ALTER TABLE `contact_person_address` DISABLE KEYS */;
/*!40000 ALTER TABLE `contact_person_address` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`contactpersoncompositekey`
--

DROP TABLE IF EXISTS `contactpersoncompositekey`;
CREATE TABLE `contactpersoncompositekey` (
  `PK1_Prop1` varchar(50) default NULL,
  `PK1_Prop2` varchar(50) default NULL,
  `Surname` varchar(50) default NULL,
  `FirstName` varchar(50) default NULL,
  `DateOfBirth` datetime default NULL,
  `DateLastUpdated` datetime default NULL,
  `UserLastUpdated` varchar(50) default NULL,
  `MachineLastUpdated` varchar(50) default NULL,
  `VersionNumber` int(11) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`contactpersoncompositekey`
--

/*!40000 ALTER TABLE `contactpersoncompositekey` DISABLE KEYS */;
INSERT INTO `contactpersoncompositekey` (`PK1_Prop1`,`PK1_Prop2`,`Surname`,`FirstName`,`DateOfBirth`,`DateLastUpdated`,`UserLastUpdated`,`MachineLastUpdated`,`VersionNumber`) VALUES 
 ('{173D954C-9434-41DB-8BE5-76F40D192E7B}','{794A0F77-BC4A-4FAB-8B46-3FD385A6E08D}','Surname','NewFirstName','1969-01-29 00:00:00','2009-01-08 14:47:48',NULL,NULL,1),
 ('{8A26B532-ACBF-449B-A0C8-38D833971571}','{86C1CDE1-2E1C-4212-B765-ED995F422D51}','Vincent','Brad','1980-01-22 00:00:00','2009-01-08 14:47:48',NULL,NULL,1),
 ('9669ebf0-59aa-44a6-b908-c8862fb055711','a1437138-84ba-4c5d-b1c9-cb73d93a0650','Vincent',NULL,NULL,'2009-01-08 14:47:48',NULL,NULL,1),
 ('9669ebf0-59aa-44a6-b908-c8862fb05571','a1437138-84ba-4c5d-b1c9-cb73d93a0650','Vincent 2',NULL,NULL,'2009-01-08 14:47:48',NULL,NULL,1),
 ('{CEBE8FD8-12FC-41F3-8A93-1724D17425D3}','{E638001C-1B48-4233-8E49-61C3B9ECAFFC}','Vincent','Brad','1980-01-22 00:00:00','2009-01-08 14:47:48',NULL,NULL,1),
 ('{83D74710-CC18-455B-BA1B-AEEE7D873E93}','{27D2A271-F03D-45D6-8DA0-053156441CF4}','Vincent','Brad','1980-01-22 00:00:00','2009-01-08 14:47:48',NULL,NULL,1),
 ('{656FCC9C-CCB9-4864-9923-FB92EB1FE16B}','{B96376A0-782B-4C7A-9E7F-A31C2FEBFB8D}','Vincent','Brad','1980-01-22 00:00:00','2009-01-08 14:47:48',NULL,NULL,1);
INSERT INTO `contactpersoncompositekey` (`PK1_Prop1`,`PK1_Prop2`,`Surname`,`FirstName`,`DateOfBirth`,`DateLastUpdated`,`UserLastUpdated`,`MachineLastUpdated`,`VersionNumber`) VALUES 
 ('{8B7FE295-85FF-4898-AA52-29CEB12E0507}','{1CEFF608-E00D-43B0-AF95-EBFC9A3A775A}','Vincent','Brad','1980-01-22 00:00:00','2009-01-08 14:47:48',NULL,NULL,1);
/*!40000 ALTER TABLE `contactpersoncompositekey` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`database_lookup_guid`
--

DROP TABLE IF EXISTS `database_lookup_guid`;
CREATE TABLE `database_lookup_guid` (
  `LookupID` varchar(38) NOT NULL default '',
  `LookupValue` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`LookupID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`database_lookup_guid`
--

/*!40000 ALTER TABLE `database_lookup_guid` DISABLE KEYS */;
INSERT INTO `database_lookup_guid` (`LookupID`,`LookupValue`) VALUES 
 ('{6EAE79DD-11A8-4f31-8AF5-A08F22FE556E}','test2'),
 ('{831B3C35-5842-484b-BEC9-CE24CCE05AC2}','test1');
/*!40000 ALTER TABLE `database_lookup_guid` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`database_lookup_int`
--

DROP TABLE IF EXISTS `database_lookup_int`;
CREATE TABLE `database_lookup_int` (
  `LookupID` int(10) unsigned NOT NULL default '0',
  `LookupValue` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`LookupID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`database_lookup_int`
--

/*!40000 ALTER TABLE `database_lookup_int` DISABLE KEYS */;
INSERT INTO `database_lookup_int` (`LookupID`,`LookupValue`) VALUES 
 (1,'TestInt1'),
 (7,'TestInt7');
/*!40000 ALTER TABLE `database_lookup_int` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`filledcircle_concrete`
--

DROP TABLE IF EXISTS `filledcircle_concrete`;
CREATE TABLE `filledcircle_concrete` (
  `FilledCircleID_field` char(38) NOT NULL default '',
  `Colour` int(10) unsigned default NULL,
  `ShapeID_field` char(38) default NULL,
  `CircleID_field` char(38) default NULL,
  `Radius` int(10) unsigned default NULL,
  `ShapeName` varchar(255) default NULL,
  PRIMARY KEY  (`FilledCircleID_field`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `habanero_test_branch_2_4`.`filledcircle_concrete`
--

/*!40000 ALTER TABLE `filledcircle_concrete` DISABLE KEYS */;
/*!40000 ALTER TABLE `filledcircle_concrete` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`filledcircle_table`
--

DROP TABLE IF EXISTS `filledcircle_table`;
CREATE TABLE `filledcircle_table` (
  `FilledCircleID_field` char(38) default NULL,
  `Colour` int(10) unsigned NOT NULL default '0',
  `ShapeID_field` char(38) default NULL,
  `CircleID_field` char(38) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`filledcircle_table`
--

/*!40000 ALTER TABLE `filledcircle_table` DISABLE KEYS */;
/*!40000 ALTER TABLE `filledcircle_table` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`mockbo`
--

DROP TABLE IF EXISTS `mockbo`;
CREATE TABLE `mockbo` (
  `MockBOID` varchar(38) default NULL,
  `MockBOProp1` varchar(38) default NULL,
  `MockBOProp2` varchar(50) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`mockbo`
--

/*!40000 ALTER TABLE `mockbo` DISABLE KEYS */;
/*!40000 ALTER TABLE `mockbo` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`mybo`
--

DROP TABLE IF EXISTS `mybo`;
CREATE TABLE `mybo` (
  `MyBoID` varchar(255) NOT NULL default '',
  `TestProp` varchar(45) default NULL,
  `TestProp2` varchar(45) default NULL,
  `ShapeID` char(38) default NULL,
  PRIMARY KEY  (`MyBoID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`mybo`
--

/*!40000 ALTER TABLE `mybo` DISABLE KEYS */;
/*!40000 ALTER TABLE `mybo` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`numbergenerator`
--

DROP TABLE IF EXISTS `numbergenerator`;
CREATE TABLE `numbergenerator` (
  `SequenceNumber` int(10) unsigned NOT NULL default '0',
  `NumberType` varchar(45) NOT NULL default '',
  `UserLocked` varchar(45) default NULL,
  `Locked` tinyint(1) default NULL,
  `MachineLocked` varchar(45) default NULL,
  `OperatingSystemUserLocked` varchar(45) default NULL,
  `DateTimeLocked` datetime default NULL,
  PRIMARY KEY  (`NumberType`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`numbergenerator`
--

/*!40000 ALTER TABLE `numbergenerator` DISABLE KEYS */;
INSERT INTO `numbergenerator` (`SequenceNumber`,`NumberType`,`UserLocked`,`Locked`,`MachineLocked`,`OperatingSystemUserLocked`,`DateTimeLocked`) VALUES 
 (0,'GeneratedNumber',NULL,NULL,NULL,NULL,NULL),
 (0,'tmp','CHILLI\\brett',0,'BRETT','CHILLI\\brett','2009-01-08 14:46:25');
/*!40000 ALTER TABLE `numbergenerator` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`organisation`
--

DROP TABLE IF EXISTS `organisation`;
CREATE TABLE `organisation` (
  `OrganisationID` char(38) NOT NULL default '',
  `Name` varchar(255) default NULL,
  PRIMARY KEY  (`OrganisationID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`organisation`
--

/*!40000 ALTER TABLE `organisation` DISABLE KEYS */;
INSERT INTO `organisation` (`OrganisationID`,`Name`) VALUES 
 ('{145CCE8A-6AB0-490D-87D5-B40FE073B4D5}',NULL),
 ('{2893AACC-EE98-420C-8956-61DAAA482CD4}',NULL),
 ('{586E2A1F-FFB9-4DE3-8BA2-5DAF9BE1EAA7}',NULL),
 ('{6CC1B216-8601-4266-A64D-FD2D50783B22}',NULL),
 ('{7F0DC506-2768-4A9E-ACBA-14B49ADA5A1B}',NULL),
 ('{9EF4B703-91BA-4C41-8B8C-1DD43625541D}',NULL),
 ('{B137CC98-6972-4516-88AF-16EE03DEB164}',NULL),
 ('{B4E14EA8-ECC5-4306-AD99-0368CD43933B}',NULL),
 ('{C32EE8AB-3A1C-49DB-9210-BC61BFA86B77}',NULL),
 ('{CC02B5C9-4395-4C6E-BB73-46F6DB10FB90}',NULL),
 ('{D97CB3F7-38B0-4B2E-957E-6D1BA1FE0A5D}',NULL),
 ('{E3C2221B-0D78-4D7C-9237-0CA3FF63AC2B}',NULL),
 ('{FD93D3BC-4842-40E2-99DD-9BFCE726DFB2}',NULL);
/*!40000 ALTER TABLE `organisation` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`shape_table`
--

DROP TABLE IF EXISTS `shape_table`;
CREATE TABLE `shape_table` (
  `ShapeID_field` char(38) NOT NULL default '',
  `ShapeName` varchar(45) default NULL,
  `ShapeType_field` varchar(45) default NULL,
  `Radius` int(10) unsigned NOT NULL default '0',
  `Colour` int(10) unsigned NOT NULL default '0',
  `CircleType_field` varchar(45) default NULL,
  PRIMARY KEY  (`ShapeID_field`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`shape_table`
--

/*!40000 ALTER TABLE `shape_table` DISABLE KEYS */;
/*!40000 ALTER TABLE `shape_table` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`stubdatabasetransaction`
--

DROP TABLE IF EXISTS `stubdatabasetransaction`;
CREATE TABLE `stubdatabasetransaction` (
  `id` varchar(255) default NULL,
  `name` varchar(255) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`stubdatabasetransaction`
--

/*!40000 ALTER TABLE `stubdatabasetransaction` DISABLE KEYS */;
/*!40000 ALTER TABLE `stubdatabasetransaction` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`table_class_car`
--

DROP TABLE IF EXISTS `table_class_car`;
CREATE TABLE `table_class_car` (
  `field_Car_ID` char(38) NOT NULL default '',
  `field_Registration_No` varchar(50) default NULL,
  `field_Length` float default NULL,
  `field_Is_Convertible` tinyint(1) default NULL,
  `field_Driver_ID` char(38) default NULL,
  PRIMARY KEY  (`field_Car_ID`),
  KEY `table_class_Car_Driver_FK` (`field_Driver_ID`),
  CONSTRAINT `table_class_Car_Driver_FK` FOREIGN KEY (`field_Driver_ID`) REFERENCES `table_class_person` (`field_Person_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`table_class_car`
--

/*!40000 ALTER TABLE `table_class_car` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_class_car` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`table_class_engine`
--

DROP TABLE IF EXISTS `table_class_engine`;
CREATE TABLE `table_class_engine` (
  `field_Engine_ID` char(38) NOT NULL,
  `field_Engine_No` varchar(50) default NULL,
  `field_Date_Manufactured` datetime default NULL,
  `field_Horse_Power` varchar(50) default NULL,
  `field_Fue_lInjected` tinyint(1) default NULL,
  `field_Car_ID` char(38) default NULL,
  PRIMARY KEY  (`field_Engine_ID`),
  KEY `table_class_Engine_Car_FK` (`field_Car_ID`),
  CONSTRAINT `table_class_Engine_Car_FK` FOREIGN KEY (`field_Car_ID`) REFERENCES `table_class_car` (`field_Car_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`table_class_engine`
--

/*!40000 ALTER TABLE `table_class_engine` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_class_engine` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`table_class_legalentity`
--

DROP TABLE IF EXISTS `table_class_legalentity`;
CREATE TABLE `table_class_legalentity` (
  `field_Legal_Entity_ID` char(38) NOT NULL default '',
  `field_Legal_Entity_Type` varchar(50) default NULL,
  PRIMARY KEY  (`field_Legal_Entity_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`table_class_legalentity`
--

/*!40000 ALTER TABLE `table_class_legalentity` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_class_legalentity` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`table_class_organisation`
--

DROP TABLE IF EXISTS `table_class_organisation`;
CREATE TABLE `table_class_organisation` (
  `field_Name` varchar(50) default NULL,
  `field_Date_Formed` varchar(50) default NULL,
  `field_Organisation_ID` char(38) NOT NULL,
  PRIMARY KEY  (`field_Organisation_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`table_class_organisation`
--

/*!40000 ALTER TABLE `table_class_organisation` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_class_organisation` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`table_class_part`
--

DROP TABLE IF EXISTS `table_class_part`;
CREATE TABLE `table_class_part` (
  `field_Part_ID` char(38) NOT NULL,
  `field_Model_No` varchar(50) default NULL,
  `field_Part_Type` varchar(50) default NULL,
  PRIMARY KEY  (`field_Part_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`table_class_part`
--

/*!40000 ALTER TABLE `table_class_part` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_class_part` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`table_class_person`
--

DROP TABLE IF EXISTS `table_class_person`;
CREATE TABLE `table_class_person` (
  `field_ID_Number` varchar(50) default NULL,
  `field_First_Name` varchar(50) default NULL,
  `field_Last_Name` varchar(50) default NULL,
  `field_Person_ID` varchar(50) NOT NULL,
  PRIMARY KEY  (`field_Person_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`table_class_person`
--

/*!40000 ALTER TABLE `table_class_person` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_class_person` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`table_class_vehicle`
--

DROP TABLE IF EXISTS `table_class_vehicle`;
CREATE TABLE `table_class_vehicle` (
  `field_Vehicle_ID` char(38) NOT NULL default '',
  `field_Vehicle_Type` varchar(50) default NULL,
  `field_Date_Assembled` datetime default NULL,
  `field_Owner_ID` char(38) default NULL,
  PRIMARY KEY  (`field_Vehicle_ID`),
  KEY `table_class_Vehicle_Owner_FK` (`field_Owner_ID`),
  CONSTRAINT `table_class_Vehicle_Owner_FK` FOREIGN KEY (`field_Owner_ID`) REFERENCES `table_class_legalentity` (`field_Legal_Entity_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`table_class_vehicle`
--

/*!40000 ALTER TABLE `table_class_vehicle` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_class_vehicle` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`table_engine`
--

DROP TABLE IF EXISTS `table_engine`;
CREATE TABLE `table_engine` (
  `ENGINE_ID` varchar(38) default NULL,
  `CAR_ID` varchar(38) default NULL,
  `ENGINE_NO` varchar(50) default NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`table_engine`
--

/*!40000 ALTER TABLE `table_engine` DISABLE KEYS */;
INSERT INTO `table_engine` (`ENGINE_ID`,`CAR_ID`,`ENGINE_NO`) VALUES 
 ('{AA17061E-B7D2-44D7-B7B4-D578F809005C}','{3C6E0B0C-9AD7-4773-B96D-3B62C054D547}','NO111');
/*!40000 ALTER TABLE `table_engine` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`table_entity`
--

DROP TABLE IF EXISTS `table_entity`;
CREATE TABLE `table_entity` (
  `field_Entity_ID` char(38) NOT NULL,
  `field_Entity_Type` varchar(50) default NULL,
  PRIMARY KEY  (`field_Entity_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`table_entity`
--

/*!40000 ALTER TABLE `table_entity` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_entity` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`table_organisationperson`
--

DROP TABLE IF EXISTS `table_organisationperson`;
CREATE TABLE `table_organisationperson` (
  `field_Organisatiion_ID` char(38) NOT NULL default '',
  `field_Person_ID` char(38) NOT NULL default '',
  `field_Relationship` varchar(50) default NULL,
  PRIMARY KEY  (`field_Organisatiion_ID`,`field_Person_ID`),
  KEY `table_OrganisationPerson_Person_FK` (`field_Person_ID`),
  CONSTRAINT `table_OrganisationPerson_Organisation_FK` FOREIGN KEY (`field_Organisatiion_ID`) REFERENCES `table_class_organisation` (`field_Organisation_ID`),
  CONSTRAINT `table_OrganisationPerson_Person_FK` FOREIGN KEY (`field_Person_ID`) REFERENCES `table_class_person` (`field_Person_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`table_organisationperson`
--

/*!40000 ALTER TABLE `table_organisationperson` DISABLE KEYS */;
/*!40000 ALTER TABLE `table_organisationperson` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`testautoinc`
--

DROP TABLE IF EXISTS `testautoinc`;
CREATE TABLE `testautoinc` (
  `testautoincid` int(10) unsigned NOT NULL auto_increment,
  `testfield` varchar(45) NOT NULL default '',
  PRIMARY KEY  (`testautoincid`)
) ENGINE=InnoDB AUTO_INCREMENT=820 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`testautoinc`
--

/*!40000 ALTER TABLE `testautoinc` DISABLE KEYS */;
/*!40000 ALTER TABLE `testautoinc` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`testtableread`
--

DROP TABLE IF EXISTS `testtableread`;
CREATE TABLE `testtableread` (
  `TestTableReadData` varchar(50) default NULL,
  `TestTableReadID` varchar(38) default NULL,
  UNIQUE KEY `Index_1` (`TestTableReadData`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`testtableread`
--

/*!40000 ALTER TABLE `testtableread` DISABLE KEYS */;
INSERT INTO `testtableread` (`TestTableReadData`,`TestTableReadID`) VALUES 
 ('Test',NULL),
 ('Test2',NULL);
/*!40000 ALTER TABLE `testtableread` ENABLE KEYS */;


--
-- Table structure for table `habanero_test_branch_2_4`.`transactionlog`
--

DROP TABLE IF EXISTS `transactionlog`;
CREATE TABLE `transactionlog` (
  `TransactionSequenceNo` int(11) NOT NULL auto_increment,
  `DateTimeUpdated` datetime default NULL,
  `WindowsUser` varchar(50) default NULL,
  `LogonUser` varchar(50) default NULL,
  `MachineName` varchar(50) default NULL,
  `BusinessObjectTypeName` varchar(50) default NULL,
  `CRUDAction` varchar(50) default NULL,
  `DirtyXML` mediumtext,
  `BusinessObjectToString` varchar(255) default NULL,
  PRIMARY KEY  (`TransactionSequenceNo`)
) ENGINE=InnoDB AUTO_INCREMENT=2491 DEFAULT CHARSET=latin1;

--
-- Dumping data for table `habanero_test_branch_2_4`.`transactionlog`
--

/*!40000 ALTER TABLE `transactionlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `transactionlog` ENABLE KEYS */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
